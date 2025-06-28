using HarmonyLib;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Customizations.Abstracts.CustomConverters;

namespace ProtoGenerationLib.Tests.Converters.CustomConverters
{
    [TestClass]
    public class CSharpDataTypeToDataTypeMetadataCustomConverterTests
    {
        public TestContext TestContext { get; set; }

        private class CustomConverter : CSharpDataTypeToDataTypeMetadataCustomConverter
        {
            public Func<Type, bool> CanHandleType { get; set; }
            public Func<Type, IDataTypeMetadata> BaseConvertTypeToIntermediateRepresentationType { get; set; }

            public CustomConverter()
            {
                CanHandleType = (a) => default;
                BaseConvertTypeToIntermediateRepresentationType = (a) => default;
            }

            public override bool CanHandle(Type type)
            {
                return CanHandleType(type);
            }

            protected override IDataTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type)
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

            var expectedMetadata = CreateDataTypeMetadata(new List<Type>
            {
                typeof(int), typeof(bool), typeof(string)
            });
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

            var typesInMetadata = new List<Type>
            {
                typeof(int), typeof(bool), typeof(string)
            };
            var expectedMetadata = CreateDataTypeMetadata(typesInMetadata);
            customConverter.BaseConvertTypeToIntermediateRepresentationType = (a) =>
            {
                return expectedMetadata;
            };

            var type = typeof(int);
            var expectedUsedTypes = typesInMetadata.Append(typeof(void)).ToList();

            var harmony = new Harmony($"{GetType().Name}.{TestContext.TestName}");
            var originMethod = typeof(FieldsTypesExtractor).GetMethod(nameof(FieldsTypesExtractor.ExtractUsedTypesFromFields));
            static bool Prefix(ref IEnumerable<Type> __result, IEnumerable<Type> fieldTypes)
            {
                __result = fieldTypes.Append(typeof(void)).ToList();
                return false;
            }
            harmony.Patch(originMethod, new HarmonyMethod(Prefix));

            // Act
            var actualUsedTypes = customConverter.ExtractUsedTypes(type);

            // Clean harmony
            harmony.UnpatchAll();

            // Assert
            CollectionAssert.AreEquivalent(expectedUsedTypes, actualUsedTypes.ToList());
        }

        #endregion ExtractUsedTypes Tests

        #region Auxiliary Functions

        private static DataTypeMetadata CreateDataTypeMetadata(IEnumerable<Type> fieldTypes)
        {
            return new DataTypeMetadata
            {
                Fields = fieldTypes.Select(x => new FieldMetadata() { Type = x }).Cast<IFieldMetadata>().ToList(),
            };
        }

        #endregion Auxiliary Functions
    }
}
