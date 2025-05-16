using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors
{
    [TestClass]
    public class TupleTypesExtractorTests
    {
        private TupleTypesExtractor extractor;

        private ITypeExtractionOptions extractionOptions;

        private Mock<INewTypeNamingStrategy> mockINewTypeNamingStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            extractionOptions = new ProtoGeneratorConfiguration()
            {
                NewTypeNamingStrategiesOptions = new NewTypeNamingStrategiesOptions
                {
                    NewTypeNamingStrategy = "a"
                }
            };

            mockINewTypeNamingStrategy = new Mock<INewTypeNamingStrategy>();

            var mockINewTypeNamingStrategiesProvider = new Mock<INewTypeNamingStrategiesProvider>();
            mockINewTypeNamingStrategiesProvider.Setup(provider => provider.GetNewTypeNamingStrategy(It.IsAny<string>()))
                                                .Returns(mockINewTypeNamingStrategy.Object);
            extractor = new TupleTypesExtractor(mockINewTypeNamingStrategiesProvider.Object);
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
            var testClassName = nameof(TupleTypesExtractorTests);
            return new List<object[]>
            {
                new object[] { typeof((bool a, string b)), new List<string> { $"{testClassName}1", typeof(bool).Name, typeof(string).Name } },
                new object[] { typeof((int, object)), new List<string> { $"{testClassName}2", typeof(int).Name, typeof(object).Name } },
                new object[] { typeof(ValueTuple<char, bool, string>), new List<string> { $"{testClassName}3", typeof(char).Name, typeof(bool).Name, typeof(string).Name } },
                new object[] { typeof(Tuple<char, bool, object>), new List<string> { $"{testClassName}4", typeof(char).Name, typeof(bool).Name, typeof(object).Name } },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            var typeAndResult = GetTypesThatCanBeHandledAndTheirUsedTypes();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
