using Moq;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Converters.Internals.CSharpToIntermediate;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Tests.Converters.Internals.DummyTypes;
using static ProtoGenerationLib.Tests.Converters.Internals.ConvertersTestsUtils;

namespace ProtoGenerationLib.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpEnumTypeToEnumTypeMetadataConverterTests
    {
        private ProtoGenerationOptions generationOptions;

        private CSharpEnumTypeToEnumTypeMetadataConverter converter;

        private IList<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> customConverters;

        private Mock<IDocumentationExtractionStrategy> mockDocumentationExtractionStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGenerationOptions();
            customConverters = generationOptions.EnumTypeCustomConverters;

            mockDocumentationExtractionStrategy = new Mock<IDocumentationExtractionStrategy>();

            var mockIProvider = new Mock<IProvider>();
            mockIProvider.Setup(provider => provider.GetDocumentationExtractionStrategy(It.IsAny<string>()))
                         .Returns(mockDocumentationExtractionStrategy.Object);

            converter = new CSharpEnumTypeToEnumTypeMetadataConverter(mockIProvider.Object);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsNotEnum_ArgumentExceptionIsThrown()
        {
            // Arrange
            var type = typeof(CSharpEnumTypeToEnumTypeMetadataConverterTests);

            // Act
            converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            // Noting to do. The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsEnum_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(Enum1);
            var expectedMetadata = CreateEnumTypeMetadata(type, new List<IEnumValueMetadata>
            {
                CreateEnumValueMetadata("Value1", 5),
                CreateEnumValueMetadata("Value2", 2),
                CreateEnumValueMetadata("Value3", -4),
            });

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsEnumAndCouldBeHandledByCustomConverter_MetadataIsTheCustomConverterResult(int suitableCustomConverterIndex)
        {
            // Arrange
            var type = typeof(Enum1);
            var expectedMetadata = CreateEnumTypeMetadata(type, new List<IEnumValueMetadata>
            {
                CreateEnumValueMetadata("Value1", 5),
                CreateEnumValueMetadata("Value2", 2),
                CreateEnumValueMetadata("Value3", -4),
                CreateEnumValueMetadata("Value4", -8),
                CreateEnumValueMetadata("Value5", 24),
            });

            for (int i = 0; i < 3; i++)
            {
                var mockConverter = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
                if (i != suitableCustomConverterIndex)
                {
                    mockConverter.Setup(customConverter => customConverter.CanHandle(It.Is<Type>((t) => t.Equals(type))))
                                 .Returns(false);
                }
                else
                {
                    mockConverter.Setup(customConverter => customConverter.CanHandle(It.Is<Type>((t) => t.Equals(type))))
                                 .Returns(true);
                    mockConverter.Setup(customConverter => customConverter.ConvertTypeToIntermediateRepresentation(It.Is<Type>((t) => t.Equals(type))))
                                 .Returns(expectedMetadata);
                }
                customConverters.Add(mockConverter.Object);
            }

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreSame(expectedMetadata, actualMetadata);
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_EnumHasDocumentationFromProvider_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(Enum1);

            var providerTypeDocs = "provider type docs";
            var providerEnumValueDocs = "provider enum value docs";
            generationOptions.AddDocumentation<Enum1>(providerTypeDocs);
            generationOptions.AddDocumentation<Enum1>(2, providerEnumValueDocs);

            var extractorTypeDocs = "extractor type docs";
            var extractorEnumValueDocs = "extractor enum value docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(type, out extractorTypeDocs))
                                               .Returns(false);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetEnumValueDocumentation(type, 2, out extractorEnumValueDocs))
                                               .Returns(false);

            var expectedMetadata = CreateEnumTypeMetadata(type, new List<IEnumValueMetadata>
            {
                CreateEnumValueMetadata("Value1", 5),
                CreateEnumValueMetadata("Value2", 2, providerEnumValueDocs),
                CreateEnumValueMetadata("Value3", -4),
            }, providerTypeDocs);

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_EnumHasDocumentationFromProviderAndExtractor_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(Enum1);

            var providerTypeDocs = "provider type docs";
            var providerEnumValueDocs = "provider enum value docs";
            generationOptions.AddDocumentation<Enum1>(providerTypeDocs);
            generationOptions.AddDocumentation<Enum1>(2, providerEnumValueDocs);

            var extractorTypeDocs = "extractor type docs";
            var extractorEnumValueDocs = "extractor enum value docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(type, out extractorTypeDocs))
                                               .Returns(true);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetEnumValueDocumentation(type, 2, out extractorEnumValueDocs))
                                               .Returns(true);

            var expectedMetadata = CreateEnumTypeMetadata(type, new List<IEnumValueMetadata>
            {
                CreateEnumValueMetadata("Value1", 5),
                CreateEnumValueMetadata("Value2", 2, providerEnumValueDocs),
                CreateEnumValueMetadata("Value3", -4),
            }, providerTypeDocs);

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_EnumHasDocumentationFromExtractor_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(Enum1);

            var extractorTypeDocs = "extractor type docs";
            var extractorEnumValueDocs = "extractor enum value docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(type, out extractorTypeDocs))
                                               .Returns(true);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetEnumValueDocumentation(type, 2, out extractorEnumValueDocs))
                                               .Returns(true);

            var expectedMetadata = CreateEnumTypeMetadata(type, new List<IEnumValueMetadata>
            {
                CreateEnumValueMetadata("Value1", 5),
                CreateEnumValueMetadata("Value2", 2, extractorEnumValueDocs),
                CreateEnumValueMetadata("Value3", -4),
            }, extractorTypeDocs);

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }
    }
}
