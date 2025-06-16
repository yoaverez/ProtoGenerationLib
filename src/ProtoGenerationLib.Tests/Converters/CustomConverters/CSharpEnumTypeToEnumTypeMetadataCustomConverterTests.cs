using HarmonyLib;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.CustomConverters;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;

namespace ProtoGenerationLib.Tests.Converters.CustomConverters
{
    [TestClass]
    public class CSharpEnumTypeToEnumTypeMetadataCustomConverterTests
    {
        private class CustomConverter : CSharpEnumTypeToEnumTypeMetadataCustomConverter
        {
            public Func<Type, IProtoGenerationOptions, bool> CanHandleTypeIProtoGenerationOptions { get; set; }
            public Func<Type, IProtoGenerationOptions, IEnumTypeMetadata> BaseConvertTypeToIntermediateRepresentationTypeIProtoGenerationOptions { get; set; }

            public CustomConverter()
            {
                CanHandleTypeIProtoGenerationOptions = (a, b) => default;
                BaseConvertTypeToIntermediateRepresentationTypeIProtoGenerationOptions = (a, b) => default;
            }

            public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
            {
                return CanHandleTypeIProtoGenerationOptions(type, generationOptions);
            }

            protected override IEnumTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
            {
                return BaseConvertTypeToIntermediateRepresentationTypeIProtoGenerationOptions(type, generationOptions);
            }
        }

        private static IProtoGenerationOptions generationOptions;

        private CustomConverter customConverter;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            generationOptions = new ProtoGenerationOptions();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            customConverter = new CustomConverter();
        }

        #region ConvertTypeToIntermediateRepresentation Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_CanNotHandleType_ThrowsArgumentException()
        {
            // Arrange
            customConverter.CanHandleTypeIProtoGenerationOptions = (a, b) => false;
            var type = typeof(int);

            // Act
            customConverter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_CanHandleType_ReturnSameMetadataAsBaseMethod()
        {
            // Arrange
            customConverter.CanHandleTypeIProtoGenerationOptions = (a, b) => true;

            var expectedMetadata = CreateEnumTypeMetadata(4);
            customConverter.BaseConvertTypeToIntermediateRepresentationTypeIProtoGenerationOptions = (a, b) =>
            {
                return expectedMetadata;
            };
            var type = typeof(int);

            // Act
            var actualMetadata = customConverter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreSame(expectedMetadata, actualMetadata);
        }

        #endregion ConvertTypeToIntermediateRepresentation Tests

        #region ExtractUsedTypes Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExtractUsedTypes_CanNotHandleType_ThrowsArgumentException()
        {
            // Arrange
            customConverter.CanHandleTypeIProtoGenerationOptions = (a, b) => false;
            var type = typeof(int);

            // Act
            customConverter.ExtractUsedTypes(type, generationOptions);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ExtractUsedTypes_CanHandleType_ReturnAllTheUsedTypes()
        {
            // Arrange
            customConverter.CanHandleTypeIProtoGenerationOptions = (a, b) => true;

            customConverter.BaseConvertTypeToIntermediateRepresentationTypeIProtoGenerationOptions = (a, b) =>
            {
                return CreateEnumTypeMetadata(4);
            };

            var type = typeof(int);
            var expectedUsedTypes = new List<Type>();

            // Act
            var actualUsedTypes = customConverter.ExtractUsedTypes(type, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedUsedTypes, actualUsedTypes.ToList());
        }

        #endregion ExtractUsedTypes Tests

        #region Auxiliary Functions

        private static EnumTypeMetadata CreateEnumTypeMetadata(int numOfValues)
        {
            var enumValues = new List<IEnumValueMetadata>();
            for (int i = 0; i < numOfValues; i++)
            {
                enumValues.Add(new EnumValueMetadata(i.ToString(), i));
            }
            return new EnumTypeMetadata { Values = enumValues };
        }

        #endregion Auxiliary Functions
    }
}
