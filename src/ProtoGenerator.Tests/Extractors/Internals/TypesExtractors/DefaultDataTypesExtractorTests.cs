using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Extractors.Internals.TypesExtractors;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;
using static ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.TypesExtractorsUtils;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors
{
    [TestClass]
    public class DefaultDataTypesExtractorTests
    {
        private DefaultDataTypesExtractor extractor;

        private IProtoGenerationOptions generationOptions;

        private Mock<IFieldsAndPropertiesExtractionStrategy> mockIFieldsAndPropertiesExtractionStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGenerationOptions()
            {
                AnalysisOptions = new AnalysisOptions()
                {
                    FieldsAndPropertiesExtractionStrategy = "a",
                }
            };

            mockIFieldsAndPropertiesExtractionStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>();
            mockIFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(It.IsAny<Type>(), It.IsAny<IAnalysisOptions>()))
                                                      .Returns((Type type, IAnalysisOptions analysisOptions) => new List<IFieldMetadata>
                                                      {
                                                          CreateFieldMetaData(type, "a", type),
                                                      });

            var mockIExtractionStrategiesProvider = new Mock<IExtractionStrategiesProvider>();
            mockIExtractionStrategiesProvider.Setup(provider => provider.GetFieldsAndPropertiesExtractionStrategy(It.IsAny<string>()))
                                             .Returns(mockIFieldsAndPropertiesExtractionStrategy.Object);

            var mockIFieldsTypesExtractor = new Mock<IFieldsTypesExtractor>();
            mockIFieldsTypesExtractor.Setup(fieldExtractor => fieldExtractor.ExtractUsedTypesFromFields(It.IsAny<IEnumerable<Type>>(), It.IsAny<IProtoGenerationOptions>()))
                                     .Returns<IEnumerable<Type>, IProtoGenerationOptions>((fieldTypes, _) => fieldTypes.Append(typeof(DefaultDataTypesExtractorTests)).ToList());

            extractor = new DefaultDataTypesExtractor(mockIExtractionStrategiesProvider.Object, mockIFieldsTypesExtractor.Object);
        }

        #region CanHandle Tests

        [DynamicData(nameof(GetTypesThatCanBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanBeHandled_ReturnTrue(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanBeHandled_ReturnTrue(extractor, type, generationOptions);
        }

        #endregion CanHandle Tests

        #region ExtractUsedTypes Tests

        [DynamicData(nameof(GetTypesThatCanBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(Type type)
        {
            // Arrange
            var expectedUsedTypes = new List<Type>
            {
                type, typeof(DefaultDataTypesExtractorTests)
            };

            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, type, generationOptions, expectedUsedTypes);
        }

        #endregion ExtractUsedTypes Tests

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            return new List<object[]>
            {
                new object[] { typeof(int) },
                new object[] { typeof(bool) },
                new object[] { typeof(char) },
                new object[] { typeof(object) },
            };
        }
    }
}
