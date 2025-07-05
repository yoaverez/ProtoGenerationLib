using Moq;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Configurations.Internals;
using static ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.TypesExtractorsUtils;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors
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
            mockIFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(It.IsAny<Type>(), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                                      .Returns((Type type, IAnalysisOptions analysisOptions, IDocumentationExtractionStrategy strategy) => new List<IFieldMetadata>
                                                      {
                                                          CreateFieldMetadata(type, "a", type),
                                                      });

            var mockIExtractionStrategiesProvider = new Mock<IExtractionStrategiesProvider>();
            mockIExtractionStrategiesProvider.Setup(provider => provider.GetFieldsAndPropertiesExtractionStrategy(It.IsAny<string>()))
                                             .Returns(mockIFieldsAndPropertiesExtractionStrategy.Object);

            var mockIFieldsTypesExtractor = new Mock<IFieldsTypesExtractor>();
            mockIFieldsTypesExtractor.Setup(fieldExtractor => fieldExtractor.ExtractUsedTypesFromFields(It.IsAny<IEnumerable<Type>>()))
                                     .Returns<IEnumerable<Type>>((fieldTypes) => fieldTypes.Append(typeof(DefaultDataTypesExtractorTests)).ToList());

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
