using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Extractors.Internals.TypesExtractors;
using ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.DummyTypes;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors
{
    [TestClass]
    public class DataTypesExtractorTests
    {
        private DataTypesExtractor extractor;

        private ITypeExtractionOptions extractionOptions;

        private List<string> actualFlow;

        private const string canHandleFormattedString = $"{{0}} extractor - {nameof(ITypesExtractor.CanHandle)} - return {{1}}";

        private const string extractUsedTypesFormattedString = $"{{0}} extractor - {nameof(ITypesExtractor.ExtractUsedTypes)}";

        [TestInitialize]
        public void TestInitialize()
        {
            actualFlow = new List<string>();

            var dataTypesExtractors = new List<ITypesExtractor>
            {
                CreateExtractorsMock(typeof(int), actualFlow),
                CreateExtractorsMock(typeof(bool), actualFlow),
                CreateExtractorsMock(typeof(char), actualFlow),
            };
            extractor = new DataTypesExtractor(dataTypesExtractors);
            extractionOptions = new ProtoGeneratorConfiguration();
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

        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandledByFirstExtractor_FlowIsCorrect()
        {
            // Arrange
            var testedType = typeof(int);
            var expectedFlow = new List<string>
            {
                string.Format(canHandleFormattedString, testedType.Name, true),
                string.Format(extractUsedTypesFormattedString, testedType.Name),
            };

            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, testedType, extractionOptions, new Type[] { testedType });
            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }

        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandledByMiddleExtractor_FlowIsCorrect()
        {
            // Arrange
            var testedType = typeof(bool);
            var expectedFlow = new List<string>
            {
                string.Format(canHandleFormattedString,typeof(int).Name, false),
                string.Format(canHandleFormattedString, testedType.Name, true),
                string.Format(extractUsedTypesFormattedString, testedType.Name),
            };

            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, testedType, extractionOptions, new Type[] { testedType });
            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }

        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandledByLastExtractor_FlowIsCorrect()
        {
            // Arrange
            var testedType = typeof(char);
            var expectedFlow = new List<string>
            {
                string.Format(canHandleFormattedString,typeof(int).Name, false),
                string.Format(canHandleFormattedString,typeof(bool).Name, false),
                string.Format(canHandleFormattedString, testedType.Name, true),
                string.Format(extractUsedTypesFormattedString, testedType.Name),
            };

            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, testedType, extractionOptions, new Type[] { testedType });
            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }

        [TestMethod]
        public void ExtractUsedTypes_TypeCanNotBeHandledByAnyExtractor_ArgumentExceptionIsThrown()
        {
            // Arrange
            var testedType = typeof(object);

            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(extractor, testedType, extractionOptions);
        }

        #endregion ExtractUsedTypes Tests

        private static IEnumerable<object[]> GetTypesThatCanNotBeHandled()
        {
            var typesThatCanNotBeHandled = new Type[]
            {
                typeof(DummyEnum1),
            };

            return typesThatCanNotBeHandled.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandledAndTheirUsedTypes()
        {
            return new List<object[]>
            {
                new object[] { typeof(int), new List<Type> { typeof(int) } },
                new object[] { typeof(bool), new List<Type> { typeof(bool) } },
                new object[] { typeof(char), new List<Type> { typeof(char) } },
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
            mockExtractor.Setup(x => x.CanHandle(It.Is<Type>(type => type.Equals(canHandleType)), It.IsAny<ITypeExtractionOptions>()))
                         .Callback(() => actualFlow.Add(string.Format(canHandleFormattedString, canHandleType.Name, true)))
                         .Returns(true);
            mockExtractor.Setup(x => x.CanHandle(It.Is<Type>(type => !type.Equals(canHandleType)), It.IsAny<ITypeExtractionOptions>()))
                         .Callback(() => actualFlow.Add(string.Format(canHandleFormattedString, canHandleType.Name, false)))
                         .Returns(false);

            // Setup the CanHandle method.
            mockExtractor.Setup(x => x.ExtractUsedTypes(It.IsAny<Type>(), It.IsAny<ITypeExtractionOptions>()))
                         .Callback(() => actualFlow.Add(string.Format(extractUsedTypesFormattedString, canHandleType.Name)))
                         .Returns(new Type[] { canHandleType });

            return mockExtractor.Object;
        }
    }
}
