using HarmonyLib;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Customizations.Abstracts.CustomConverters;

namespace ProtoGenerationLib.Tests.Converters.CustomConverters
{
    [TestClass]
    public class CSharpEnumTypeToEnumTypeMetadataCustomConverterTests
    {
        private class CustomConverter : CSharpEnumTypeToEnumTypeMetadataCustomConverter
        {
            public Func<Type, bool> CanHandleType { get; set; }
            public Func<Type, IEnumTypeMetadata> BaseConvertTypeToIntermediateRepresentationType { get; set; }

            public CustomConverter()
            {
                CanHandleType = (a) => default;
                BaseConvertTypeToIntermediateRepresentationType = (a) => default;
            }

            public override bool CanHandle(Type type)
            {
                return CanHandleType(type);
            }

            protected override IEnumTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type)
            {
                return BaseConvertTypeToIntermediateRepresentationType(type);
            }
        }

        private CustomConverter customConverter;

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
            customConverter.CanHandleType = (a) => false;
            var type = typeof(int);

            // Act
            customConverter.ConvertTypeToIntermediateRepresentation(type);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_CanHandleType_ReturnSameMetadataAsBaseMethod()
        {
            // Arrange
            customConverter.CanHandleType = (a) => true;

            var expectedMetadata = CreateEnumTypeMetadata(4);
            customConverter.BaseConvertTypeToIntermediateRepresentationType = (a) =>
            {
                return expectedMetadata;
            };
            var type = typeof(int);

            // Act
            var actualMetadata = customConverter.ConvertTypeToIntermediateRepresentation(type);

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
            customConverter.CanHandleType = (a) => false;
            var type = typeof(int);

            // Act
            customConverter.ExtractUsedTypes(type);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ExtractUsedTypes_CanHandleType_ReturnAllTheUsedTypes()
        {
            // Arrange
            customConverter.CanHandleType = (a) => true;

            customConverter.BaseConvertTypeToIntermediateRepresentationType = (a) =>
            {
                return CreateEnumTypeMetadata(4);
            };

            var type = typeof(int);
            var expectedUsedTypes = new List<Type>();

            // Act
            var actualUsedTypes = customConverter.ExtractUsedTypes(type);

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
