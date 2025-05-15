using Moq;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors
{
    [TestClass]
    public class DictionaryTypesExtractorTests
    {
        private DictionaryTypesExtractor extractor;

        private TypeExtractionOptions extractionOptions;

        private Mock<INewTypeNamingStrategy> mockINewTypeNamingStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            extractionOptions = new TypeExtractionOptions()
            {
                NewTypeNamingStrategy = "a"
            };

            mockINewTypeNamingStrategy = new Mock<INewTypeNamingStrategy>();

            var mockINewTypeNamingStrategiesProvider = new Mock<INewTypeNamingStrategiesProvider>();
            mockINewTypeNamingStrategiesProvider.Setup(provider => provider.GetNewTypeNamingStrategy(It.IsAny<string>()))
                                                .Returns(mockINewTypeNamingStrategy.Object);
            extractor = new DictionaryTypesExtractor(mockINewTypeNamingStrategiesProvider.Object);
        }

        #region CanHandle Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanNotBeHandled_ReturnFalse(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanNotBeHandled_ReturnFalse(extractor, type);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanBeHandled_ReturnTrue(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanBeHandled_ReturnTrue(extractor, type);
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
        public void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(Type type, IEnumerable<string> expectedUsedTypes)
        {
            // Arrange
            var newTypeName = expectedUsedTypes.First();
            mockINewTypeNamingStrategy.Setup(x => x.GetNewTypeName(It.Is<Type>(t => t.Equals(type))))
                                      .Returns(newTypeName);

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
                typeof(bool[]),
            };

            return typesThatCanNotBeHandled.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandledAndTheirUsedTypes()
        {
            var testClassName = nameof(DictionaryTypesExtractorTests);
            return new List<object[]>
            {
                new object[] { typeof(Dictionary<int, string>), new List<string> { $"{testClassName}1", typeof(int).Name, typeof(string).Name } },
                new object[] { typeof(IDictionary<char, bool>), new List<string> { $"{testClassName}2", typeof(char).Name, typeof(bool).Name } },
                new object[] { typeof(IEnumerable<KeyValuePair<double, object>>), new List<string> { $"{testClassName}3", typeof(double).Name, typeof(object).Name } },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            var typeAndResult = GetTypesThatCanBeHandledAndTheirUsedTypes();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
