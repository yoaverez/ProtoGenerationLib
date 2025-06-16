using HarmonyLib;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.CustomConverters;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;

namespace ProtoGenerationLib.Tests.Converters.CustomConverters
{
    [TestClass]
    public class CSharpDataTypeToDataTypeMetadataCustomConverterTests
    {
        public TestContext TestContext { get; set; }

        private class CustomConverter : CSharpDataTypeToDataTypeMetadataCustomConverter
        {
            public Func<Type, IProtoGenerationOptions, bool> CanHandleTypeIProtoGenerationOptions { get; set; }
            public Func<Type, IProtoGenerationOptions, IDataTypeMetadata> BaseConvertTypeToIntermediateRepresentationTypeIProtoGenerationOptions { get; set; }

            public CustomConverter()
            {
                CanHandleTypeIProtoGenerationOptions = (a, b) => default;
                BaseConvertTypeToIntermediateRepresentationTypeIProtoGenerationOptions = (a, b) => default;
            }

            public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
            {
                return CanHandleTypeIProtoGenerationOptions(type, generationOptions);
            }

            protected override IDataTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
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

            var expectedMetadata = CreateDataTypeMetadata(new List<Type>
            {
                typeof(int), typeof(bool), typeof(string)
            });
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

            var typesInMetadata = new List<Type>
            {
                typeof(int), typeof(bool), typeof(string)
            };
            var expectedMetadata = CreateDataTypeMetadata(typesInMetadata);
            customConverter.BaseConvertTypeToIntermediateRepresentationTypeIProtoGenerationOptions = (a, b) =>
            {
                return expectedMetadata;
            };

            var type = typeof(int);
            var expectedUsedTypes = typesInMetadata.Append(typeof(void)).ToList();

            var harmony = new Harmony($"{GetType().Name}.{TestContext.TestName}");
            var originMethod = typeof(FieldsTypesExtractor).GetMethod(nameof(FieldsTypesExtractor.ExtractUsedTypesFromFields));
            static bool Prefix(ref IEnumerable<Type> __result, IEnumerable<Type> fieldTypes, IProtoGenerationOptions generationOptions)
            {
                __result = fieldTypes.Append(typeof(void)).ToList();
                return false;
            }
            harmony.Patch(originMethod, new HarmonyMethod(Prefix));

            // Act
            var actualUsedTypes = customConverter.ExtractUsedTypes(type, generationOptions);

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
