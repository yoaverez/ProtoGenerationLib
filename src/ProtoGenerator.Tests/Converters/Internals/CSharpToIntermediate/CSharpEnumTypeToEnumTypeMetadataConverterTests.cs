using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Converters.Internals.CSharpToIntermediate;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Tests.Converters.Internals.DummyTypes;
using static ProtoGenerator.Tests.Converters.Internals.ConvertersTestsUtils;

namespace ProtoGenerator.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpEnumTypeToEnumTypeMetadataConverterTests
    {
        private static IProtoGeneratorConfiguration generationOptions;

        private static CSharpEnumTypeToEnumTypeMetadataConverter converter;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            generationOptions = new ProtoGeneratorConfiguration();
            converter = new CSharpEnumTypeToEnumTypeMetadataConverter();
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
    }
}
