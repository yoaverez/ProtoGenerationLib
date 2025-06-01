using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Converters.Internals.CSharpToIntermediate;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Tests.Converters.Internals.DummyTypes;
using static ProtoGenerator.Tests.Converters.Internals.ConvertersTestsUtils;

namespace ProtoGenerator.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpDataTypeToDataTypeMetadataConverterTests
    {
        private static IProtoGeneratorConfiguration generationOptions;

        private static IEnumTypeMetadata enumTypeMetadata;

        private CSharpDataTypeToDataTypeMetadataConverter converter;

        private Mock<IFieldsAndPropertiesExtractionStrategy> mockStrategy;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            generationOptions = new ProtoGeneratorConfiguration
            {
                AnalysisOptions = new AnalysisOptions
                {
                    FieldsAndPropertiesExtractionStrategy = "a"
                }
            };

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
            mockStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>();

            var mockProvider = new Mock<IProvider>();
            mockProvider.Setup(provider => provider.GetFieldsAndPropertiesExtractionStrategy(It.IsAny<string>()))
                        .Returns(mockStrategy.Object);

            var mockEnumConverter = new Mock<ICSharpToIntermediateConverter<IEnumTypeMetadata>>();
            mockEnumConverter.Setup(enumConverter => enumConverter.ConvertTypeToIntermediateRepresentation(It.IsAny<Type>(), It.IsAny<IProtoGeneratorConfiguration>()))
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

            mockStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1), It.IsAny<IAnalysisOptions>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "a", typeof(int)) });

            mockStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType2), It.IsAny<IAnalysisOptions>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "b", typeof(int)) });

            mockStrategy.Setup(strategy => strategy.ExtractFieldsAndProperties(typeof(DataType1.DataType3), It.IsAny<IAnalysisOptions>()))
                        .Returns(new List<IFieldMetadata> { CreateFieldMetadata(typeof(int), "c", typeof(int)) });

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }
    }
}
