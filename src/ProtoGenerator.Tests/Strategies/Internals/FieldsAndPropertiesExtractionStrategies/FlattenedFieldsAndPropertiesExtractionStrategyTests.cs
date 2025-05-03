using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Strategies.Internals.FieldsAndPropertiesExtractionStrategies;
using ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes;

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
            var expectedMembers = new List<(Type, string)>();

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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "A"), (typeof(string), "Str"), (typeof(string), "Name")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "A"), (typeof(string), "Str"), (typeof(string), "Name"), (typeof(int), "publicField")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "A"), (typeof(string), "Str"), (typeof(string), "Name"), (typeof(int), "PublicStaticProp")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "A"), (typeof(string), "Str"), (typeof(string), "Name"), (typeof(int), "PrivateProp")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "A"), (typeof(string), "Str"), (typeof(string), "Name"), (typeof(int), "publicField"), (typeof(int), "PublicStaticProp")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "A"), (typeof(string), "Str"), (typeof(string), "Name"), (typeof(int), "publicField"),
                (typeof(int), "PrivateProp"), (typeof(int), "privateField"), (typeof(int), "protectedField"), (typeof(int), "_publicStaticProp")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "A"), (typeof(string), "Str"), (typeof(string), "Name"),
                (typeof(int), "PrivateProp"), (typeof(int), "PublicStaticProp"), (typeof(int), "PrivateStaticProp")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "A"), (typeof(string), "Str"), (typeof(string), "Name"),
                (typeof(int), "PrivateProp"), (typeof(int), "PublicStaticProp"), (typeof(int), "PrivateStaticProp"),
                (typeof(int), "publicField"), (typeof(int), "privateField"), (typeof(int), "protectedField")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b"),
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b"),
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(bool), "d"),
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(int), "Value"),
                (typeof(RecursiveType), "Next"),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion RecursiveType Tests

        private void ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(Type type, AnalysisOptions analysisOptions, List<(Type, string)> expectedMembers)
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
