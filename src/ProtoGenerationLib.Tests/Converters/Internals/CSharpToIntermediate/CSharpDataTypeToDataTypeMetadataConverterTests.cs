using Moq;
using ProtoGenerationLib.Configurations.Internals;
using static ProtoGenerationLib.Tests.Converters.Internals.ConvertersTestsUtils;
using ProtoGenerationLib.Tests.Converters.Internals.DummyTypes;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Converters.Internals.CSharpToIntermediate;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Customizations.Abstracts;

namespace ProtoGenerationLib.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpDataTypeToDataTypeMetadataConverterTests
    {
        private static IEnumTypeMetadata enumTypeMetadata;

        private ProtoGenerationOptions generationOptions;

        private CSharpDataTypeToDataTypeMetadataConverter converter;

        private Mock<IFieldsAndPropertiesExtractionStrategy> mockFieldsAndPropertiesExtractionStrategy;

        private Mock<IDocumentationExtractionStrategy> mockDocumentationExtractionStrategy;

        private IList<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> customConverters;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            enumTypeMetadata = CreateEnumTypeMetadata(typeof(Enum1), new List<IEnumValueMetadata>
            {
                CreateEnumValueMetadata("Value1", 5),
                CreateEnumValueMetadata("Value2", 2),
                CreateEnumValueMetadata("Value3", -4),
            });
        }

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGenerationOptions
            {
                AnalysisOptions = new AnalysisOptions
                {
                    FieldsAndPropertiesExtractionStrategy = "a",
                    DocumentationExtractionStrategy = "b",
                }
            };

            customConverters = generationOptions.DataTypeCustomConverters;

            mockFieldsAndPropertiesExtractionStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>();
            mockDocumentationExtractionStrategy = new Mock<IDocumentationExtractionStrategy>();

            var mockProvider = new Mock<IProvider>();
            mockProvider.Setup(provider => provider.GetFieldsAndPropertiesExtractionStrategy(It.IsAny<string>()))
                        .Returns(mockFieldsAndPropertiesExtractionStrategy.Object);
            mockProvider.Setup(provider => provider.GetDocumentationExtractionStrategy(It.IsAny<string>()))
                        .Returns(mockDocumentationExtractionStrategy.Object);

            var mockEnumConverter = new Mock<ICSharpToIntermediateConverter<IEnumTypeMetadata>>();
            mockEnumConverter.Setup(enumConverter => enumConverter.ConvertTypeToIntermediateRepresentation(It.IsAny<Type>(), It.IsAny<IProtoGenerationOptions>()))
                             .Returns(enumTypeMetadata);

            converter = new CSharpDataTypeToDataTypeMetadataConverter(mockProvider.Object, mockEnumConverter.Object);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsNotDataType_ArgumentExceptionIsThrown()
        {
            // Arrange
            var type = typeof(Enum1);

            // Act
            converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            // Noting to do. The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsDataType_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(DataType1);

            var dataType2Metadata = CreateDataTypeMetadata(typeof(DataType1.DataType2), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "b", typeof(int)),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>());

            var dataType3Metadata = CreateDataTypeMetadata(typeof(DataType1.DataType3), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "c", typeof(int)),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>());

            var expectedMetadata = CreateDataTypeMetadata(type, new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", typeof(int)),
            }, new List<IDataTypeMetadata>
            {
                dataType2Metadata, dataType3Metadata,
            }, new List<IEnumTypeMetadata>
            {
                enumTypeMetadata, enumTypeMetadata,
            });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "a", typeof(int)) });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType2), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "b", typeof(int)) });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType3), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "c", typeof(int)) });

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsDataTypeAndCouldBeHandledByCustomConverter_MetadataIsTheCustomConverterResult(int suitableCustomConverterIndex)
        {
            // Arrange
            var type = typeof(DataType1);
            var expectedMetadata = CreateDataTypeMetadata(typeof(DataType1.DataType2), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "b", typeof(int)),
                CreateFieldMetadata(typeof(object), "c", typeof(double)),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>());

            for (int i = 0; i < 3; i++)
            {
                var mockConverter = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
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
        public void ConvertTypeToIntermediateRepresentation_TypeHasDocumentationFromProvider_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(DataType1);

            var providerTypeDocumentation = "provider type docs";
            generationOptions.AddDocumentation<DataType1>(providerTypeDocumentation);
            generationOptions.AddDocumentation<DataType1.DataType2>(providerTypeDocumentation);

            var extractorTypeDocumentation = "extractor type docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(typeof(DataType1), out extractorTypeDocumentation))
                                               .Returns(false);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(typeof(DataType1.DataType2), out extractorTypeDocumentation))
                                               .Returns(false);

            var fieldDocumentation = "field docs";

            var dataType2Metadata = CreateDataTypeMetadata(typeof(DataType1.DataType2), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "b", typeof(int)),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>(), providerTypeDocumentation);

            var dataType3Metadata = CreateDataTypeMetadata(typeof(DataType1.DataType3), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "c", typeof(int), documentation: fieldDocumentation),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>());

            var expectedMetadata = CreateDataTypeMetadata(type, new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", typeof(int)),
            }, new List<IDataTypeMetadata>
            {
                dataType2Metadata, dataType3Metadata,
            }, new List<IEnumTypeMetadata>
            {
                enumTypeMetadata, enumTypeMetadata,
            }, providerTypeDocumentation);

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "a", typeof(int)) });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType2), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "b", typeof(int)) });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType3), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "c", typeof(int), documentation: fieldDocumentation) });

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeHasDocumentationFromProviderAndExtractor_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(DataType1);

            var providerTypeDocumentation = "provider type docs";
            generationOptions.AddDocumentation<DataType1>(providerTypeDocumentation);
            generationOptions.AddDocumentation<DataType1.DataType2>(providerTypeDocumentation);

            var extractorTypeDocumentation = "extractor type docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(typeof(DataType1), out extractorTypeDocumentation))
                                               .Returns(true);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(typeof(DataType1.DataType2), out extractorTypeDocumentation))
                                               .Returns(true);

            var fieldDocumentation = "field docs";

            var dataType2Metadata = CreateDataTypeMetadata(typeof(DataType1.DataType2), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "b", typeof(int)),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>(), providerTypeDocumentation);

            var dataType3Metadata = CreateDataTypeMetadata(typeof(DataType1.DataType3), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "c", typeof(int), documentation: fieldDocumentation),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>());

            var expectedMetadata = CreateDataTypeMetadata(type, new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", typeof(int)),
            }, new List<IDataTypeMetadata>
            {
                dataType2Metadata, dataType3Metadata,
            }, new List<IEnumTypeMetadata>
            {
                enumTypeMetadata, enumTypeMetadata,
            }, providerTypeDocumentation);

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "a", typeof(int)) });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType2), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "b", typeof(int)) });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType3), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "c", typeof(int), documentation: fieldDocumentation) });

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeHasDocumentationFromExtractor_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(DataType1);

            var extractorTypeDocumentation = "extractor type docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(typeof(DataType1), out extractorTypeDocumentation))
                                               .Returns(true);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetTypeDocumentation(typeof(DataType1.DataType2), out extractorTypeDocumentation))
                                               .Returns(true);

            var fieldDocumentation = "field docs";

            var dataType2Metadata = CreateDataTypeMetadata(typeof(DataType1.DataType2), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "b", typeof(int)),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>(), extractorTypeDocumentation);

            var dataType3Metadata = CreateDataTypeMetadata(typeof(DataType1.DataType3), new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "c", typeof(int), documentation: fieldDocumentation),
            }, new List<IDataTypeMetadata>(), new List<IEnumTypeMetadata>());

            var expectedMetadata = CreateDataTypeMetadata(type, new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", typeof(int)),
            }, new List<IDataTypeMetadata>
            {
                dataType2Metadata, dataType3Metadata,
            }, new List<IEnumTypeMetadata>
            {
                enumTypeMetadata, enumTypeMetadata,
            }, extractorTypeDocumentation);

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "a", typeof(int)) });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType2), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "b", typeof(int)) });

            mockFieldsAndPropertiesExtractionStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType3), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "c", typeof(int), documentation: fieldDocumentation) });

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }
    }
}
