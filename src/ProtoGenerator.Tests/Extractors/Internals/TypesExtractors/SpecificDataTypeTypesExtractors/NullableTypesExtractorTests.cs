using Moq;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors
{
    [TestClass]
    public class NullableTypesExtractorTests
    {
        private NullableTypesExtractor extractor;

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
            extractor = new NullableTypesExtractor(mockINewTypeNamingStrategiesProvider.Object);
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
                typeof(int),
                typeof(object),
                typeof(IEnumerable<int>),
            };

            return typesThatCanNotBeHandled.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandledAndTheirUsedTypes()
        {
            var testClassName = nameof(NullableTypesExtractorTests);
            return new List<object[]>
            {
                new object[] { typeof(bool?), new List<string> { $"{testClassName}1", typeof(bool).Name } },
                new object[] { typeof(Nullable<char>), new List<string> { $"{testClassName}2", typeof(char).Name } },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            var typeAndResult = GetTypesThatCanBeHandledAndTheirUsedTypes();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
