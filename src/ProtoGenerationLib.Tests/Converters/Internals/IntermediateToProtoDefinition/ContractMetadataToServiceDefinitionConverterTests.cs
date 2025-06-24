using Moq;
using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Tests.Converters.Internals.DummyTypes;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System.Reflection;

namespace ProtoGenerationLib.Tests.Converters.Internals.IntermediateToProtoDefinition
{
    [TestClass]
    public class ContractMetadataToServiceDefinitionConverterTests
    {
        private Mock<IProvider> mockIProvider;

        private Mock<IPackageStylingStrategy> mockIPackageStylingStrategy;

        private Mock<IProtoStylingStrategy> mockIProtoStylingStrategy;

        private Mock<IParameterListNamingStrategy> mockIParameterListNamingStrategy;

        private Mock<INewTypeNamingStrategy> mockINewTypeNamingStrategy;

        private ContractMetadataToServiceDefinitionConverter converter;

        private Dictionary<Type, IProtoTypeMetadata> primitiveTypesWrappers;

        private ProtoGenerationOptions generationOptions;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGenerationOptions
            {
                NewTypeNamingStrategiesOptions = new NewTypeNamingStrategiesOptions
                {
                    ParameterListNamingStrategy = "1",
                    NewTypeNamingStrategy = "2",
                },
                ProtoStylingConventionsStrategiesOptions = new ProtoStylingConventionsStrategiesOptions
                {
                    PackageStylingStrategy = "3",
                    RpcStylingStrategy = "4",
                },
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoRpcAttribute = typeof(ProtoRpcAttribute),
                    TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) => { rpcType = ProtoRpcType.Unary; return false; },
                }
            };

            mockIPackageStylingStrategy = new Mock<IPackageStylingStrategy>();
            mockIPackageStylingStrategy.Setup(strategy => strategy.PackageComponentsSeparator)
                                       .Returns(".");

            mockIProtoStylingStrategy = new Mock<IProtoStylingStrategy>();
            mockIProtoStylingStrategy.Setup(strategy => strategy.ToProtoStyle(It.IsAny<string>()))
                                     .Returns<string>((name) => name.ToUpperInvariant());

            mockIParameterListNamingStrategy = new Mock<IParameterListNamingStrategy>();
            mockIParameterListNamingStrategy.Setup(strategy => strategy.GetNewParametersListTypeName(It.IsAny<MethodInfo>()))
                .Returns<MethodInfo>(info => $"{TestContext.TestName}_{info.Name}");

            mockINewTypeNamingStrategy = new Mock<INewTypeNamingStrategy>();
            mockINewTypeNamingStrategy.Setup(strategy => strategy.GetNewTypeName(It.IsAny<Type>()))
                                      .Returns<Type>(type => $"{TestContext.TestName}_{type.Name}Wrapper");

            mockIProvider = new Mock<IProvider>();
            mockIProvider.Setup(provider => provider.GetParameterListNamingStrategy("1"))
                         .Returns(mockIParameterListNamingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetNewTypeNamingStrategy("2"))
                         .Returns(mockINewTypeNamingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetPackageStylingStrategy("3"))
                         .Returns(mockIPackageStylingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetProtoStylingStrategy("4"))
                         .Returns(mockIProtoStylingStrategy.Object);

            primitiveTypesWrappers = new Dictionary<Type, IProtoTypeMetadata>();
            converter = new ContractMetadataToServiceDefinitionConverter(mockIProvider.Object, primitiveTypesWrappers);
        }

        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_TypeContainsAllMethodOptions_ServiceDefinitionIsCorrect()
        {
            // Arrange
            var contractType = typeof(IContractType2);

            generationOptions.AnalysisOptions.TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) =>
            {
                rpcType = ProtoRpcType.ServerStreaming;

                if (method.Name.Equals(nameof(IContractType2.Method4)))
                {
                    rpcType = ProtoRpcType.ClientStreaming;
                    return true;
                }

                return false;
            };

            var contractMetadata = new ContractTypeMetadata(contractType,
                                                            contractType.GetMethods()
                                                                        .Where(m => !m.Name.Equals(nameof(IContractType2.Method5)))
                                                                        .Select(x => new MethodMetadata(x))
                                                                        .Cast<IMethodMetadata>()
                                                                        .ToList());

            var newParameterListType1 = TypeCreator.CreateDataType($"{TestContext.TestName}_{nameof(IContractType2.Method3)}",
                                                                  new List<(Type, string)> { (typeof(int), "a"), (typeof(bool), "b") });

            var newParameterListType2 = TypeCreator.CreateDataType($"{TestContext.TestName}_{nameof(IContractType2.Method4)}",
                                                                  new List<(Type, string)> { (typeof(int), "a"), (typeof(bool), "b") });

            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(void)] = new ProtoTypeMetadata("void", "", "void", "path1"),
                [typeof(int)] = new ProtoTypeMetadata("int", "int.pac", "int.pac.int", "path2"),
                [typeof(bool)] = new ProtoTypeMetadata("bool", "pac", "pac.bool", "path2"),
                [typeof(double)] = new ProtoTypeMetadata("double", "pac", "pac.double", "path2"),
                [typeof(IContractType2)] = new ProtoTypeMetadata("contract", "pac.pac2", "pac.pac2.contract", "path3"),
                [newParameterListType1] = new ProtoTypeMetadata(newParameterListType1.Name, newParameterListType1.Namespace, $"{newParameterListType1.Namespace}.{newParameterListType1.Name}", "path4"),
                [newParameterListType2] = new ProtoTypeMetadata(newParameterListType2.Name, newParameterListType2.Namespace, $"{newParameterListType2.Namespace}.{newParameterListType2.Name}", "path4"),
            };

            primitiveTypesWrappers.Add(typeof(int), new ProtoTypeMetadata("PrimitiveInt", "int.pac", "int.pac.PrimitiveInt", "protobuf.primitives"));
            primitiveTypesWrappers.Add(typeof(double), new ProtoTypeMetadata("PrimitiveDouble", "pac", "pac.PrimitiveDouble", "protobuf.primitives"));

            var rpcs = new List<IRpcDefinition>
            {
                new RpcDefinition(nameof(IContractType2.Method1).ToUpperInvariant(), "void", "void", ProtoRpcType.Unary),
                new RpcDefinition(nameof(IContractType2.Method2).ToUpperInvariant(), "void", "int.pac.PrimitiveInt", ProtoRpcType.ClientStreaming),
                new RpcDefinition(nameof(IContractType2.Method3).ToUpperInvariant(), "PrimitiveDouble", $"{newParameterListType1.Namespace}.{newParameterListType1.Name}", ProtoRpcType.BidirectionalStreaming),
                new RpcDefinition(nameof(IContractType2.Method4).ToUpperInvariant(), "PrimitiveDouble", $"{newParameterListType2.Namespace}.{newParameterListType2.Name}", ProtoRpcType.ClientStreaming),
            };
            var imports = new HashSet<string>
            {
                "path1", "path4", "protobuf.primitives"
            };
            var expectedDefinition = new ServiceDefinition("contract", "pac.pac2", imports, rpcs);

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(contractMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedDefinition, actualDefinition);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_ParameterListWasNotCreated_ThrowsException()
        {
            // Arrange
            var contractType = typeof(IContractType2);
            var contractMetadata = new ContractTypeMetadata(contractType,
                                                            contractType.GetMethods()
                                                                        .Select(x => new MethodMetadata(x))
                                                                        .Cast<IMethodMetadata>()
                                                                        .ToList());

            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(void)] = new ProtoTypeMetadata("void", "", "void", "path1"),
                [typeof(int)] = new ProtoTypeMetadata("int", "int.pac", "int.pac.int", "path2"),
                [typeof(bool)] = new ProtoTypeMetadata("bool", "pac", "pac.bool", "path2"),
                [typeof(double)] = new ProtoTypeMetadata("double", "pac", "pac.double", "path2"),
                [typeof(IContractType2)] = new ProtoTypeMetadata("contract", "pac.pac2", "pac.pac2.contract", "path3"),
            };

            primitiveTypesWrappers.Add(typeof(int), new ProtoTypeMetadata("PrimitiveInt", "int.pac", "int.pac.PrimitiveInt", "protobuf.primitives"));
            primitiveTypesWrappers.Add(typeof(double), new ProtoTypeMetadata("PrimitiveDouble", "pac", "pac.PrimitiveDouble", "protobuf.primitives"));

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(contractMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_ParameterAndResultTypeIsEnum_ServiceDefinitionIsCorrect()
        {
            // Arrange
            var contractType = typeof(IContractType3);
            var contractMetadata = new ContractTypeMetadata(contractType,
                                                            contractType.GetMethods()
                                                                        .Select(x => new MethodMetadata(x))
                                                                        .Cast<IMethodMetadata>()
                                                                        .ToList());

            // Make sure that there is new type that wraps the enum.
            var enumWrapper = TypeCreator.CreateDataType($"{TestContext.TestName}_{typeof(Enum1).Name}Wrapper", new List<(Type, string)>());

            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(Enum1)] = new ProtoTypeMetadata("a", "pac", "pac.a", "path1"),
                [enumWrapper] = new ProtoTypeMetadata("b", "pac", "pac.b", "path1"),
                [typeof(IContractType3)] = new ProtoTypeMetadata("contract", "pac.pac2", "pac.pac2.contract", "path3"),
            };

            var rpcs = new List<IRpcDefinition>
            {
                new RpcDefinition(nameof(IContractType3.Method1).ToUpperInvariant(), "b", "b", ProtoRpcType.Unary),
            };
            var imports = new HashSet<string>
            {
                "path1",
            };
            var expectedDefinition = new ServiceDefinition("contract", "pac.pac2", imports, rpcs);

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(contractMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedDefinition, actualDefinition);
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_ParameterIsEnum_ThrowsException()
        {
            // Arrange
            var contractType = typeof(IContractType3);
            var contractMetadata = new ContractTypeMetadata(contractType,
                                                            contractType.GetMethods()
                                                                        .Select(x => new MethodMetadata(x))
                                                                        .Cast<IMethodMetadata>()
                                                                        .ToList());

            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(Enum1)] = new ProtoTypeMetadata("a", "pac", "pac.a", "path1"),
                [typeof(IContractType3)] = new ProtoTypeMetadata("contract", "pac.pac2", "pac.pac2.contract", "path3"),
            };

            primitiveTypesWrappers.Add(typeof(int), new ProtoTypeMetadata("PrimitiveInt", "int.pac", "int.pac.PrimitiveInt", "protobuf.primitives"));
            primitiveTypesWrappers.Add(typeof(double), new ProtoTypeMetadata("PrimitiveDouble", "pac", "pac.PrimitiveDouble", "protobuf.primitives"));

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(contractMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }
    }
}
