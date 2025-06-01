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

        private List<string> actualFlow;

        private const string canHandleFormattedString = $"{{0}} extractor - {nameof(ITypesExtractor.CanHandle)} - return {{1}}";

        private const string extractUsedTypesFormattedString = $"{{0}} extractor - {nameof(ITypesExtractor.ExtractUsedTypes)}";

        private const string extractFieldsAndPropertiesString = $"{nameof(IFieldsAndPropertiesExtractionStrategy.ExtractFieldsAndProperties)}";

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

            actualFlow = new List<string>();

            var wrapperElementTypesExtractors = new List<ITypesExtractor>
            {
                CreateExtractorsMock(typeof(int), actualFlow),
                CreateExtractorsMock(typeof(bool), actualFlow),
                CreateExtractorsMock(typeof(char), actualFlow),
            };

            mockIFieldsAndPropertiesExtractionStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>();
            mockIFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(It.IsAny<Type>(), It.IsAny<IAnalysisOptions>()))
                                                      .Callback(() => actualFlow.Add(extractFieldsAndPropertiesString))
                                                      .Returns((Type type, IAnalysisOptions analysisOptions) => new List<IFieldMetadata>
                                                      {
                                                          CreateFieldMetaData(type, "a", type),
                                                          CreateFieldMetaData(typeof(DefaultDataTypesExtractor), "b", type)
                                                      });

            var mockIExtractionStrategiesProvider = new Mock<IExtractionStrategiesProvider>();
            mockIExtractionStrategiesProvider.Setup(provider => provider.GetFieldsAndPropertiesExtractionStrategy(It.IsAny<string>()))
                                             .Returns(mockIFieldsAndPropertiesExtractionStrategy.Object);
            extractor = new DefaultDataTypesExtractor(mockIExtractionStrategiesProvider.Object, wrapperElementTypesExtractors);
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

        [DynamicData(nameof(GetTypesThatCanBeHandledAndTheirUsedTypes), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(Type type, IEnumerable<Type> expectedUsedTypes)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, type, generationOptions, expectedUsedTypes);
        }

        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandledByFirstExtractor_FlowIsCorrect()
        {
            // Arrange
            var testedType = typeof(int);
            var expectedFlow = new List<string>
            {
                extractFieldsAndPropertiesString,

                string.Format(canHandleFormattedString, testedType.Name, true),
                string.Format(extractUsedTypesFormattedString, testedType.Name),

                // For the DefaultDataTypesExtractor property.
                string.Format(canHandleFormattedString, typeof(int).Name, false),
                string.Format(canHandleFormattedString, typeof(bool).Name, false),
                string.Format(canHandleFormattedString, typeof(char).Name, false),
            };

            // Act
            extractor.ExtractUsedTypes(testedType, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }

        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandledByMiddleExtractor_FlowIsCorrect()
        {
            // Arrange
            var testedType = typeof(bool);
            var expectedFlow = new List<string>
            {
                extractFieldsAndPropertiesString,

                string.Format(canHandleFormattedString,typeof(int).Name, false),
                string.Format(canHandleFormattedString, testedType.Name, true),
                string.Format(extractUsedTypesFormattedString, testedType.Name),

                // For the DefaultDataTypesExtractor property.
                string.Format(canHandleFormattedString, typeof(int).Name, false),
                string.Format(canHandleFormattedString, typeof(bool).Name, false),
                string.Format(canHandleFormattedString, typeof(char).Name, false),
            };

            // Act
            extractor.ExtractUsedTypes(testedType, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }

        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandledByLastExtractor_FlowIsCorrect()
        {
            // Arrange
            var testedType = typeof(char);
            var expectedFlow = new List<string>
            {
                extractFieldsAndPropertiesString,

                string.Format(canHandleFormattedString,typeof(int).Name, false),
                string.Format(canHandleFormattedString,typeof(bool).Name, false),
                string.Format(canHandleFormattedString, testedType.Name, true),
                string.Format(extractUsedTypesFormattedString, testedType.Name),

                // For the DefaultDataTypesExtractor property.
                string.Format(canHandleFormattedString, typeof(int).Name, false),
                string.Format(canHandleFormattedString, typeof(bool).Name, false),
                string.Format(canHandleFormattedString, typeof(char).Name, false),
            };

            // Act
            extractor.ExtractUsedTypes(testedType, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }

        [TestMethod]
        public void ExtractUsedTypes_TypeCanNotBeHandledByAnyExtractor_ArgumentExceptionIsThrown()
        {
            // Arrange
            var testedType = typeof(object);
            var expectedFlow = new List<string>
            {
                extractFieldsAndPropertiesString,

                // For the testedType.
                string.Format(canHandleFormattedString, typeof(int).Name, false),
                string.Format(canHandleFormattedString, typeof(bool).Name, false),
                string.Format(canHandleFormattedString, typeof(char).Name, false),

                // For the DefaultDataTypesExtractor property.
                string.Format(canHandleFormattedString, typeof(int).Name, false),
                string.Format(canHandleFormattedString, typeof(bool).Name, false),
                string.Format(canHandleFormattedString, typeof(char).Name, false),
            };

            // Act
            extractor.ExtractUsedTypes(testedType, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }

        #endregion ExtractUsedTypes Tests

        private static IEnumerable<object[]> GetTypesThatCanBeHandledAndTheirUsedTypes()
        {
            return new List<object[]>
            {
                new object[] { typeof(int), new List<Type> { typeof(DefaultDataTypesExtractor), typeof(IEnumerable<int>), typeof(int[]) } },
                new object[] { typeof(bool), new List<Type> { typeof(DefaultDataTypesExtractor), typeof(IEnumerable<bool>), typeof(bool[]) } },
                new object[] { typeof(char), new List<Type> { typeof(DefaultDataTypesExtractor), typeof(IEnumerable<char>), typeof(char[]) } },
                new object[] { typeof(object), new List<Type> { typeof(DefaultDataTypesExtractor), typeof(object) } },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            var typeAndResult = GetTypesThatCanBeHandledAndTheirUsedTypes();
            return typeAndResult.Select(x => new object[] { x[0] });
        }

        private ITypesExtractor CreateExtractorsMock(Type canHandleType, List<string> actualFlow)
        {
            var mockExtractor = new Mock<ITypesExtractor>();

            // Setup the CanHandle method.
            mockExtractor.Setup(x => x.CanHandle(It.Is<Type>(type => type.Equals(canHandleType)), generationOptions))
                         .Callback(() => actualFlow.Add(string.Format(canHandleFormattedString, canHandleType.Name, true)))
                         .Returns(true);
            mockExtractor.Setup(x => x.CanHandle(It.Is<Type>(type => !type.Equals(canHandleType)), generationOptions))
                         .Callback(() => actualFlow.Add(string.Format(canHandleFormattedString, canHandleType.Name, false)))
                         .Returns(false);

            // Setup the CanHandle method.
            mockExtractor.Setup(x => x.ExtractUsedTypes(It.IsAny<Type>(), generationOptions))
                         .Callback(() => actualFlow.Add(string.Format(extractUsedTypesFormattedString, canHandleType.Name)))
                         .Returns(new Type[] { typeof(IEnumerable<>).MakeGenericType(canHandleType), canHandleType.MakeArrayType() });

            return mockExtractor.Object;
        }
    }
}
