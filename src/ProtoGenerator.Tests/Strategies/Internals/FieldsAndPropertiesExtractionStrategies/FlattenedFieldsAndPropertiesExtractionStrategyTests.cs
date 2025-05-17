using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Strategies.Internals.FieldsAndPropertiesExtractionStrategies;
using ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes;
using static ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.TypesExtractorsUtils;

namespace ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies
{
    [TestClass]
    public class FlattenedFieldsAndPropertiesExtractionStrategyTests
    {
        private static FlattenedFieldsAndPropertiesExtractionStrategy strategy;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            strategy = new FlattenedFieldsAndPropertiesExtractionStrategy();
        }

        #region TypeContainsOnlyEmptyMembers Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsOnlyEmptyMembers_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsOnlyEmptyMembers);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>();

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion TypeContainsOnlyEmptyMembers Tests

        #region TypeContainsASecondDegreeMembers Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembers_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "A", type),
                CreateFieldMetaData(typeof(string), "Str", type),
                CreateFieldMetaData(typeof(string), "Name", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembersIncludeFields_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(true, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "A", type),
                CreateFieldMetaData(typeof(string), "Str", type),
                CreateFieldMetaData(typeof(string), "Name", type),
                CreateFieldMetaData(typeof(int), "publicField", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembersIncludeStatics_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(false, true, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "A", type),
                CreateFieldMetaData(typeof(string), "Str", type),
                CreateFieldMetaData(typeof(string), "Name", type),
                CreateFieldMetaData(typeof(int), "PublicStaticProp", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembersIncludePrivates_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(false, false, true);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "A", type),
                CreateFieldMetaData(typeof(string), "Str", type),
                CreateFieldMetaData(typeof(string), "Name", type),
                CreateFieldMetaData(typeof(int), "PrivateProp", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembersIncludeFieldsIncludeStatics_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(true, true, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "A", type),
                CreateFieldMetaData(typeof(string), "Str", type),
                CreateFieldMetaData(typeof(string), "Name", type),
                CreateFieldMetaData(typeof(int), "publicField", type),
                CreateFieldMetaData(typeof(int), "PublicStaticProp", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembersIncludeFieldsIncludePrivates_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(true, false, true);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "A", type),
                CreateFieldMetaData(typeof(string), "Str", type),
                CreateFieldMetaData(typeof(string), "Name", type),
                CreateFieldMetaData(typeof(int), "publicField", type),
                CreateFieldMetaData(typeof(int), "PrivateProp", type),
                CreateFieldMetaData(typeof(int), "privateField", type),
                CreateFieldMetaData(typeof(int), "protectedField", type),
                CreateFieldMetaData(typeof(int), "_publicStaticProp", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembersIncludeStaticsIncludePrivates_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(false, true, true);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "A", type),
                CreateFieldMetaData(typeof(string), "Str", type),
                CreateFieldMetaData(typeof(string), "Name", type),
                CreateFieldMetaData(typeof(int), "PrivateProp", type),
                CreateFieldMetaData(typeof(int), "PublicStaticProp", type),
                CreateFieldMetaData(typeof(int), "PrivateStaticProp", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembersIncludeFieldsIncludeStaticsIncludePrivates_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(true, true, true);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "A", type),
                CreateFieldMetaData(typeof(string), "Str", type),
                CreateFieldMetaData(typeof(string), "Name", type),
                CreateFieldMetaData(typeof(int), "PrivateProp", type),
                CreateFieldMetaData(typeof(int), "PublicStaticProp", type),
                CreateFieldMetaData(typeof(int), "PrivateStaticProp", type),
                CreateFieldMetaData(typeof(int), "publicField", type),
                CreateFieldMetaData(typeof(int), "privateField", type),
                CreateFieldMetaData(typeof(int), "protectedField", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion TypeContainsASecondDegreeMembers Tests

        #region TypeWithConstructorAttribute Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithConstructorAttribute_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeWithConstructorAttribute);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithConstructorAttributeIncludePrivates_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeWithConstructorAttribute2);
            var analysisOptions = CreateAnalysisOptions(false, false, true);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion TypeWithConstructorAttribute Tests

        #region TypeContainsIgnoreMembers Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsIgnoreMembers_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsIgnoreMembers);
            var analysisOptions = CreateAnalysisOptions(true, true, true);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(bool), "d", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion TypeContainsIgnoreMembers Tests

        #region RecursiveType Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_RecursiveType_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(RecursiveType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "Value", type),
                CreateFieldMetaData(typeof(RecursiveType), "Next", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion RecursiveType Tests

        #region TypeHasAttributedMembers Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeHasAttributedMembers_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeHasAttributedMembers2);
            var analysisOptions = CreateAnalysisOptions(true, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "Prop1", type, new List<Attribute> { new OptionalDataMemberAttribute() }),
                CreateFieldMetaData(typeof(int), "field1", type, new List<Attribute> { new OptionalDataMemberAttribute() }),
                CreateFieldMetaData(typeof(int), "field2", type),
                CreateFieldMetaData(typeof(int), "Prop2", type, new List<Attribute> { new OptionalDataMemberAttribute() }),
                CreateFieldMetaData(typeof(int), "field3", type, new List<Attribute> { new OptionalDataMemberAttribute() }),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion TypeHasAttributedMembers Tests

        private void ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(Type type, AnalysisOptions analysisOptions, List<IFieldMetadata> expectedMembers)
        {
            // Act
            var actualMembers = strategy.ExtractFieldsAndProperties(type, analysisOptions).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedMembers, actualMembers);
        }

        private static AnalysisOptions CreateAnalysisOptions(bool includeFields, bool includeStatics, bool includePrivates)
        {
            return new AnalysisOptions
            {
                IncludeFields = includeFields,
                IncludeStatics = includeStatics,
                IncludePrivates = includePrivates,
                DataTypeConstructorAttribute = typeof(ProtoMessageConstructorAttribute),
                IgnoreFieldOrPropertyAttribute = typeof(ProtoIgnoreAttribute),
            };
        }
    }
}
