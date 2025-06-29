using Moq;
using ProtoGenerationLib.Converters.Internals.CSharpToIntermediate;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpToIntermediateUtilsTests
    {
        #region TryConvertWithCustomConverters Tests

        [TestMethod]
        public void TryConvertWithCustomConverters_ZeroConverters_ReturnFalse()
        {
            // Arrange
            var type = typeof(int);
            var customConverters = new List<ICSharpToIntermediateCustomConverter<string>>();

            // Act
            var actualResult = CSharpToIntermediateUtils.TryConvertWithCustomConverters(type, customConverters, out _);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryConvertWithCustomConverters_MultipleConvertersThatCanNotHandleTheType_ReturnFalse()
        {
            // Arrange
            var type = typeof(int);
            var customConverters = new List<ICSharpToIntermediateCustomConverter<string>>();

            for (int i = 0; i < 3; i++)
            {
                var mockConverter = new Mock<ICSharpToIntermediateCustomConverter<string>>();
                mockConverter.Setup(converter => converter.CanHandle(It.IsAny<Type>()))
                             .Returns(false);
            }

            // Act
            var actualResult = CSharpToIntermediateUtils.TryConvertWithCustomConverters(type, customConverters, out _);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void TryConvertWithCustomConverters_MultipleConvertersAtLeastOneThatCanHandleTheType_ReturnTrue(int suitableCustomConverterIndex)
        {
            // Arrange
            var type = typeof(int);
            var customConverters = new List<ICSharpToIntermediateCustomConverter<int>>();

            for (int i = 0; i < 3; i++)
            {
                var mockConverter = new Mock<ICSharpToIntermediateCustomConverter<int>>();
                if (i < suitableCustomConverterIndex)
                {
                    mockConverter.Setup(converter => converter.CanHandle(It.IsAny<Type>()))
                                 .Returns(false);
                }
                else
                {
                    mockConverter.Setup(converter => converter.CanHandle(It.IsAny<Type>()))
                                 .Returns(true);
                    mockConverter.Setup(converter => converter.ConvertTypeToIntermediateRepresentation(It.IsAny<Type>()))
                                 .Returns(i);
                }
                customConverters.Add(mockConverter.Object);
            }

            var expectedConvertedObject = suitableCustomConverterIndex;

            // Act
            var actualResult = CSharpToIntermediateUtils.TryConvertWithCustomConverters(type, customConverters, out var actualConvertedObject);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedConvertedObject, actualConvertedObject);
        }

        #endregion TryConvertWithCustomConverters Tests

        #region TryGetTypeDocumentation Tests

        [TestMethod]
        public void TryGetTypeDocumentation_TypeDoesNotHaveDocumentation_ReturnFalse()
        {
            // Arrange
            var type = typeof(int);
            var providerDocumentation = "";
            var mockDocumentationProvider = new Mock<IDocumentationProvider>();
            mockDocumentationProvider.Setup(provider => provider.TryGetTypeDocumentation(type, out providerDocumentation))
                                     .Returns(false);

            var extractorDocumentation = "";
            var mockDocumentationExtractor = new Mock<IDocumentationExtractionStrategy>();
            mockDocumentationExtractor.Setup(extractor => extractor.TryGetTypeDocumentation(type, out extractorDocumentation))
                                      .Returns(false);

            // Act
            var result = CSharpToIntermediateUtils.TryGetTypeDocumentation(type,
                                                                           mockDocumentationProvider.Object,
                                                                           mockDocumentationExtractor.Object,
                                                                           out var actualDocumentation);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryGetTypeDocumentation_TypeHaveDocumentationFromProvider_ReturnTrueAndCorrectDocumentation()
        {
            // Arrange
            var type = typeof(int);
            var providerDocumentation = "docs";
            var mockDocumentationProvider = new Mock<IDocumentationProvider>();
            mockDocumentationProvider.Setup(provider => provider.TryGetTypeDocumentation(type, out providerDocumentation))
                                     .Returns(true);

            var extractorDocumentation = "";
            var mockDocumentationExtractor = new Mock<IDocumentationExtractionStrategy>();
            mockDocumentationExtractor.Setup(extractor => extractor.TryGetTypeDocumentation(type, out extractorDocumentation))
                                      .Returns(false);

            // Act
            var result = CSharpToIntermediateUtils.TryGetTypeDocumentation(type,
                                                                           mockDocumentationProvider.Object,
                                                                           mockDocumentationExtractor.Object,
                                                                           out var actualDocumentation);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(providerDocumentation, actualDocumentation);
        }

        [TestMethod]
        public void TryGetTypeDocumentation_TypeHaveDocumentationFromExtractor_ReturnTrueAndCorrectDocumentation()
        {
            // Arrange
            var type = typeof(int);
            var providerDocumentation = "";
            var mockDocumentationProvider = new Mock<IDocumentationProvider>();
            mockDocumentationProvider.Setup(provider => provider.TryGetTypeDocumentation(type, out providerDocumentation))
                                     .Returns(false);

            var extractorDocumentation = "docs";
            var mockDocumentationExtractor = new Mock<IDocumentationExtractionStrategy>();
            mockDocumentationExtractor.Setup(extractor => extractor.TryGetTypeDocumentation(type, out extractorDocumentation))
                                      .Returns(true);

            // Act
            var result = CSharpToIntermediateUtils.TryGetTypeDocumentation(type,
                                                                           mockDocumentationProvider.Object,
                                                                           mockDocumentationExtractor.Object,
                                                                           out var actualDocumentation);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(extractorDocumentation, actualDocumentation);
        }

        [TestMethod]
        public void TryGetTypeDocumentation_TypeHaveDocumentationFromProviderExtractor_ReturnTrueAndProviderDocumentation()
        {
            // Arrange
            var type = typeof(int);
            var providerDocumentation = "docs1";
            var mockDocumentationProvider = new Mock<IDocumentationProvider>();
            mockDocumentationProvider.Setup(provider => provider.TryGetTypeDocumentation(type, out providerDocumentation))
                                     .Returns(true);

            var extractorDocumentation = "docs";
            var mockDocumentationExtractor = new Mock<IDocumentationExtractionStrategy>();
            mockDocumentationExtractor.Setup(extractor => extractor.TryGetTypeDocumentation(type, out extractorDocumentation))
                                      .Returns(true);

            // Act
            var result = CSharpToIntermediateUtils.TryGetTypeDocumentation(type,
                                                                           mockDocumentationProvider.Object,
                                                                           mockDocumentationExtractor.Object,
                                                                           out var actualDocumentation);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(providerDocumentation, actualDocumentation);
        }

        #endregion TryGetTypeDocumentation Tests
    }
}
