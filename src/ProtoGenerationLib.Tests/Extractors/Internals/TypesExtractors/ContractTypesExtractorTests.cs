using Moq;
using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.DummyTypes;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System.Reflection;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors
{
    [TestClass]
    public class ContractTypesExtractorTests
    {
        private static ContractTypesExtractor extractor;

        private static ProtoGenerationOptions generationOptions;

        private static Mock<IProvider> mockIProvider;

        private static Type newParameterListType;

        private static Type newType;

        private const string NEW_PARAMETER_LIST_TYPE_NAME = "NewParameterListType";

        private const string NEW_TYPE_NAME = "NewType";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var props1 = new List<(Type, string)>
            {
                (typeof(int), "prop1"),
                (typeof((object, bool, string)), "prop2"),
                (typeof(IEnumerable<bool>), "prop3"),
            };
            newParameterListType = TypeCreator.CreateDataType(NEW_PARAMETER_LIST_TYPE_NAME, props1);

            var props2 = new List<(Type, string)>
            {
                (typeof(DummyEnum1), "Value"),
            };
            newType = TypeCreator.CreateDataType(NEW_TYPE_NAME, props2);

            var mockParameterListStrategy = new Mock<IParameterListNamingStrategy>();
            mockParameterListStrategy.Setup(strategy => strategy.GetNewParametersListTypeName(It.IsAny<MethodInfo>()))
                                     .Returns(NEW_PARAMETER_LIST_TYPE_NAME);

            var mockNewTypeNamingStrategy = new Mock<INewTypeNamingStrategy>();
            mockNewTypeNamingStrategy.Setup(strategy => strategy.GetNewTypeName(It.IsAny<Type>()))
                                     .Returns(NEW_TYPE_NAME);

            var mockMethodSignatureExtractionStrategy = new Mock<IMethodSignatureExtractionStrategy>();
            mockMethodSignatureExtractionStrategy.Setup(strategy => strategy.ExtractMethodSignature(It.IsAny<MethodInfo>(), It.IsAny<Type>()))
                                                 .Returns<MethodInfo, Type>((method, ignoreAttribute) => (method.ReturnType, method.GetParameters().Select(x => new MethodParameterMetadata(x.ParameterType, x.Name))));

            mockIProvider = new Mock<IProvider>();
            mockIProvider.Setup(provider => provider.GetParameterListNamingStrategy(It.IsAny<string>()))
                         .Returns(mockParameterListStrategy.Object);
            mockIProvider.Setup(provider => provider.GetNewTypeNamingStrategy(It.IsAny<string>()))
                         .Returns(mockNewTypeNamingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetMethodSignatureExtractionStrategy(It.IsAny<string>()))
                         .Returns(mockMethodSignatureExtractionStrategy.Object);

            extractor = new ContractTypesExtractor(mockIProvider.Object);

            generationOptions = new ProtoGenerationOptions
            {
                NewTypeNamingStrategiesOptions = new NewTypeNamingStrategiesOptions(),
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoServiceAttribute = typeof(ProtoServiceAttribute),
                    IsProtoServiceDelegate = (type) => type.Equals(typeof(DummyService2)),
                    ProtoRpcAttribute = typeof(ProtoRpcAttribute),
                    TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) =>
                    {
                        rpcType = ProtoRpcType.ServerStreaming;

                        if (declaringType.Equals(typeof(DummyService2)) && method.Name.Equals(nameof(DummyService2.Method2)))
                        {
                            rpcType = ProtoRpcType.ClientStreaming;
                            return true;
                        }

                        return false;
                    },
                }
            };
        }

        #region CanHandle Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanNotBeHandled_ReturnFalse(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanNotBeHandled_ReturnFalse(extractor, type, generationOptions);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanBeHandled_ReturnTrue(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanBeHandled_ReturnTrue(extractor, type, generationOptions);
        }

        #endregion CanHandle Tests

        #region ExtractUsedTypes Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(extractor, type, generationOptions);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandledAndTheirUsedTypes), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(Type type, IEnumerable<Type> expectedUsedTypes)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, type, generationOptions, expectedUsedTypes);
        }

        #endregion ExtractUsedTypes Tests

        private static IEnumerable<object[]> GetTypesThatCanNotBeHandled()
        {
            var typesThatCanNotBeHandled = new Type[]
            {
                typeof(IEnumerable<>),
                typeof(int),
                typeof(object),
            };

            return typesThatCanNotBeHandled.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandledAndTheirUsedTypes()
        {
            return new List<object[]>
            {
                new object[] { typeof(DummyService1), new List<Type> { newParameterListType, typeof(int), typeof(void), typeof(Type) } },
                new object[] { typeof(DummyService2), new List<Type> { typeof(int), typeof(bool), typeof(string) } },
                new object[] { typeof(IDummyService3), new List<Type> { newType } },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            var typeAndResult = GetTypesThatCanBeHandledAndTheirUsedTypes();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
