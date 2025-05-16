using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Extractors.Internals.TypesExtractors;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.DummyTypes;
using ProtoGenerator.Utilities.TypeUtilities;
using System.Reflection;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors
{
    [TestClass]
    public class ContractTypesExtractorTests
    {
        private static ContractTypesExtractor extractor;

        private static ITypeExtractionOptions extractionOptions;

        private static Mock<INewTypeNamingStrategiesProvider> mockINewTypeNamingStrategiesProvider;

        private static Type newType;
        private const string NEW_TYPE_NAME = "NewType";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var props = new List<(Type, string)>
            {
                (typeof(int), "prop1"),
                (typeof((object, bool, string)), "prop2"),
                (typeof(IEnumerable<bool>), "prop3"),
            };
            newType = TypeCreator.CreateDataType(NEW_TYPE_NAME, props);

            var mockStrategy = new Mock<IParameterListNamingStrategy>();
            mockStrategy.Setup(strategy => strategy.GetNewParametersListTypeName(It.IsAny<MethodInfo>()))
                        .Returns(NEW_TYPE_NAME);

            mockINewTypeNamingStrategiesProvider = new Mock<INewTypeNamingStrategiesProvider>();
            mockINewTypeNamingStrategiesProvider.Setup(provider => provider.GetParameterListNamingStrategy(It.IsAny<string>()))
                                                .Returns(mockStrategy.Object);

            extractor = new ContractTypesExtractor(mockINewTypeNamingStrategiesProvider.Object);
            extractionOptions = new ProtoGeneratorConfiguration
            {
                NewTypeNamingStrategiesOptions = new NewTypeNamingStrategiesOptions(),
            };
        }

        #region CanHandle Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanNotBeHandled_ReturnFalse(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanNotBeHandled_ReturnFalse(extractor, type, extractionOptions);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanBeHandled_ReturnTrue(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanBeHandled_ReturnTrue(extractor, type, extractionOptions);
        }

        #endregion CanHandle Tests

        #region ExtractUsedTypes Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(extractor, type, extractionOptions);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandledAndTheirUsedTypes), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(Type type, IEnumerable<Type> expectedUsedTypes)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, type, extractionOptions, expectedUsedTypes);
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
                new object[] { typeof(DummyService1), new List<Type> { newType, typeof(int), typeof(void), typeof(Type) } },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            var typeAndResult = GetTypesThatCanBeHandledAndTheirUsedTypes();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
