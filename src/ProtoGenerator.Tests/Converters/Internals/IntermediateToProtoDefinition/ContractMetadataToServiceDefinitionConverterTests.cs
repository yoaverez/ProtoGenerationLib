using Moq;
using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Converters.Internals.IntermediateToProtoDefinition;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Tests.Converters.Internals.DummyTypes;
using ProtoGenerator.Utilities.TypeUtilities;
using System.Reflection;

namespace ProtoGenerator.Tests.Converters.Internals.IntermediateToProtoDefinition
{
    [TestClass]
    public class ContractMetadataToServiceDefinitionConverterTests
    {
        private Mock<IProvider> mockIProvider;

        private Mock<IPackageStylingStrategy> mockIPackageStylingStrategy;

        private Mock<IProtoStylingStrategy> mockIProtoStylingStrategy;

        private Mock<IParameterListNamingStrategy> mockIParameterListNamingStrategy;

        private ContractMetadataToServiceDefinitionConverter converter;

        private static ProtoGenerationOptions generationOptions;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            generationOptions = new ProtoGenerationOptions
            {
                NewTypeNamingStrategiesOptions = new NewTypeNamingStrategiesOptions
                {
                    ParameterListNamingStrategy = "1",
                },
                ProtoStylingConventionsStrategiesOptions = new ProtoStylingConventionsStrategiesOptions
                {
                    PackageStylingStrategy = "2",
                    RpcStylingStrategy = "3",
                },
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoRpcAttribute = typeof(ProtoRpcAttribute),
                }
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            mockIPackageStylingStrategy = new Mock<IPackageStylingStrategy>();
            mockIPackageStylingStrategy.Setup(strategy => strategy.PackageComponentsSeparator)
                                       .Returns(".");

            mockIProtoStylingStrategy = new Mock<IProtoStylingStrategy>();
            mockIProtoStylingStrategy.Setup(strategy => strategy.ToProtoStyle(It.IsAny<string>()))
                                     .Returns<string>((name) => name.ToUpperInvariant());

            mockIParameterListNamingStrategy = new Mock<IParameterListNamingStrategy>();
            mockIParameterListNamingStrategy.Setup(strategy => strategy.GetNewParametersListTypeName(It.IsAny<MethodInfo>()))
                .Returns<MethodInfo>(info => info.Name);

            mockIProvider = new Mock<IProvider>();
            mockIProvider.Setup(provider => provider.GetParameterListNamingStrategy("1"))
                         .Returns(mockIParameterListNamingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetPackageStylingStrategy("2"))
                         .Returns(mockIPackageStylingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetProtoStylingStrategy("3"))
                         .Returns(mockIProtoStylingStrategy.Object);

            converter = new ContractMetadataToServiceDefinitionConverter(mockIProvider.Object);
        }

        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_TypeContainsAllMethodOptions_ServiceDefinitionIsCorrect()
        {
            // Arrange
            var contractType = typeof(IContractType2);
            var contractMetadata = new ContractTypeMetadata(contractType,
                                                            contractType.GetMethods()
                                                                        .Select(x => new MethodMetadata(x))
                                                                        .Cast<IMethodMetadata>()
                                                                        .ToList());

            var newParameterListType = TypeCreator.CreateDataType(nameof(IContractType2.Method3),
                                                                  new List<(Type, string)> { (typeof(int), "a"), (typeof(bool), "b") });
            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(void)] = new ProtoTypeMetadata("void", "", "void", "path1"),
                [typeof(int)] = new ProtoTypeMetadata("int", "int.pac", "int.pac.int", "path2"),
                [typeof(bool)] = new ProtoTypeMetadata("bool", "pac", "pac.bool", "path2"),
                [typeof(double)] = new ProtoTypeMetadata("double", "pac", "pac.double", "path2"),
                [typeof(IContractType2)] = new ProtoTypeMetadata("contract", "pac.pac2", "pac.pac2.contract", "path3"),
                [newParameterListType] = new ProtoTypeMetadata(newParameterListType.Name, newParameterListType.Namespace, $"{newParameterListType.Namespace}.{newParameterListType.Name}", "path4"),
            };

            var rpcs = new List<IRpcDefinition>
            {
                new RpcDefinition(nameof(IContractType2.Method1).ToUpperInvariant(), "void", "void", ProtoRpcType.Unary),
                new RpcDefinition(nameof(IContractType2.Method2).ToUpperInvariant(), "void", "int.pac.int", ProtoRpcType.ClientStreaming),
                new RpcDefinition(nameof(IContractType2.Method3).ToUpperInvariant(), "double", $"{newParameterListType.Namespace}.{newParameterListType.Name}", ProtoRpcType.BidirectionalStreaming),
            };
            var imports = new HashSet<string>
            {
                "path1", "path2", "path4",
            };
            var expectedDefinition = new ServiceDefinition(rpcs, "contract", "pac.pac2", imports);

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

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(contractMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }
    }
}
