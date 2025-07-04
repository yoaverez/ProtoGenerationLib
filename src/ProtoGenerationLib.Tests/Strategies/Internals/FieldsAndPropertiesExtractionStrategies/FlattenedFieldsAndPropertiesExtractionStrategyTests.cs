using Moq;
using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Strategies.Internals.FieldsAndPropertiesExtractionStrategies;
using ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes;
using static ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.TypesExtractorsUtils;

namespace ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies
{
    [TestClass]
    public class FlattenedFieldsAndPropertiesExtractionStrategyTests
    {
        private static FlattenedFieldsAndPropertiesExtractionStrategy strategy;

        private Mock<IDocumentationExtractionStrategy> mockDocumentationExtractionStrategy;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            strategy = new FlattenedFieldsAndPropertiesExtractionStrategy();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            mockDocumentationExtractionStrategy = new Mock<IDocumentationExtractionStrategy>();
        }

        #region TypeContainsEmptyMembers Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsOnlyEmptyMembersAndShouldRemoveEmptyTypes_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsOnlyEmptyMembers);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            analysisOptions.RemoveEmptyMembers = true;

            var expectedMembers = new List<IFieldMetadata>();

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsOnlyEmptyMembersAndShouldNotRemoveEmptyTypes_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsOnlyEmptyMembers);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            analysisOptions.RemoveEmptyMembers = false;

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(TypeContainsOnlyEmptyMembers.I1), nameof(TypeContainsOnlyEmptyMembers.I1Prop), type),
                CreateFieldMetadata(typeof(TypeContainsOnlyEmptyMembers.C1), nameof(TypeContainsOnlyEmptyMembers.C1Prop), type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsSomeImpostersEmptyMembers_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsImpostersEmptyMembers);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "Prop1", type),
                CreateFieldMetadata(typeof(Enum1), "Prop2", type),
                CreateFieldMetadata(typeof(IEnumerable<string>), "Prop3", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion TypeContainsEmptyMembers Tests

        #region TypeContainsASecondDegreeMembers Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsASecondDegreeMembers_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeContainsASecondDegreeMembers);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(bool), "A", type),
                CreateFieldMetadata(typeof(string), "Str", type),
                CreateFieldMetadata(typeof(string), "Name", type),
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
                CreateFieldMetadata(typeof(bool), "A", type),
                CreateFieldMetadata(typeof(string), "Str", type),
                CreateFieldMetadata(typeof(string), "Name", type),
                CreateFieldMetadata(typeof(int), "publicField", type),
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
                CreateFieldMetadata(typeof(bool), "A", type),
                CreateFieldMetadata(typeof(string), "Str", type),
                CreateFieldMetadata(typeof(string), "Name", type),
                CreateFieldMetadata(typeof(int), "PublicStaticProp", type),
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
                CreateFieldMetadata(typeof(bool), "A", type),
                CreateFieldMetadata(typeof(string), "Str", type),
                CreateFieldMetadata(typeof(string), "Name", type),
                CreateFieldMetadata(typeof(int), "PrivateProp", type),
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
                CreateFieldMetadata(typeof(bool), "A", type),
                CreateFieldMetadata(typeof(string), "Str", type),
                CreateFieldMetadata(typeof(string), "Name", type),
                CreateFieldMetadata(typeof(int), "publicField", type),
                CreateFieldMetadata(typeof(int), "PublicStaticProp", type),
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
                CreateFieldMetadata(typeof(bool), "A", type),
                CreateFieldMetadata(typeof(string), "Str", type),
                CreateFieldMetadata(typeof(string), "Name", type),
                CreateFieldMetadata(typeof(int), "publicField", type),
                CreateFieldMetadata(typeof(int), "PrivateProp", type),
                CreateFieldMetadata(typeof(int), "privateField", type),
                CreateFieldMetadata(typeof(int), "protectedField", type),
                CreateFieldMetadata(typeof(int), "_publicStaticProp", type),
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
                CreateFieldMetadata(typeof(bool), "A", type),
                CreateFieldMetadata(typeof(string), "Str", type),
                CreateFieldMetadata(typeof(string), "Name", type),
                CreateFieldMetadata(typeof(int), "PrivateProp", type),
                CreateFieldMetadata(typeof(int), "PublicStaticProp", type),
                CreateFieldMetadata(typeof(int), "PrivateStaticProp", type),
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
                CreateFieldMetadata(typeof(bool), "A", type),
                CreateFieldMetadata(typeof(string), "Str", type),
                CreateFieldMetadata(typeof(string), "Name", type),
                CreateFieldMetadata(typeof(int), "PrivateProp", type),
                CreateFieldMetadata(typeof(int), "PublicStaticProp", type),
                CreateFieldMetadata(typeof(int), "PrivateStaticProp", type),
                CreateFieldMetadata(typeof(int), "publicField", type),
                CreateFieldMetadata(typeof(int), "privateField", type),
                CreateFieldMetadata(typeof(int), "protectedField", type),
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
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
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
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
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
                CreateFieldMetadata(typeof(bool), "d", type),
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
                CreateFieldMetadata(typeof(int), "Value", type),
                CreateFieldMetadata(typeof(RecursiveType), "Next", type),
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
                CreateFieldMetadata(typeof(int), "Prop1", type, new List<Attribute> { new OptionalDataMemberAttribute() }),
                CreateFieldMetadata(typeof(int), "field1", type, new List<Attribute> { new OptionalDataMemberAttribute() }),
                CreateFieldMetadata(typeof(int), "field2", type),
                CreateFieldMetadata(typeof(int), "Prop2", type, new List<Attribute> { new OptionalDataMemberAttribute() }),
                CreateFieldMetadata(typeof(int), "field3", type, new List<Attribute> { new OptionalDataMemberAttribute() }),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion TypeHasAttributedMembers Tests

        #region Edge Cases

        /// <summary>
        /// This test check that when the removal of empty members happens,
        /// There will be no exception due to adding a new value in the
        /// alreadyCheckedIsEmpty dictionary that already exists in the dictionary.
        /// This may happen due to the recursion manner of the RemoveAllEmptyMembers
        /// that may call the ExtractFieldsAndProperties that calls RemoveAllEmptyMembers
        /// again.
        /// </summary>
        [TestMethod]
        public void ExtractFieldsAndProperties_TypeContainsFieldThatContainsOtherFieldType_ExtractedCorrectFieldsAndPropertiesAndDoesNotThrowException()
        {
            // Arrange
            var type = typeof(TypeContainsFieldThatContainsOtherFieldType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(TypeContainsFieldThatContainsOtherFieldType.NestedClass1), "Prop1", type),
                CreateFieldMetadata(typeof(TypeContainsFieldThatContainsOtherFieldType.NestedClass2), "Prop2", type),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #endregion Edge Cases

        #region Type Contains Documentation Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_ThereAreFieldsWithDocumentationFromProvider_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeWithPropertiesAndFields);
            var analysisOptions = CreateAnalysisOptions(true, false, false);

            var providerPropDocumentation = "provider prop docs";
            var providerFieldDocumentation = "provider field docs";
            analysisOptions.DocumentationProviderAndAdder.AddFieldDocumentation<TypeWithPropertiesAndFields>(nameof(TypeWithPropertiesAndFields.Prop), providerPropDocumentation);
            analysisOptions.DocumentationProviderAndAdder.AddFieldDocumentation<TypeWithPropertiesAndFields>(nameof(TypeWithPropertiesAndFields.field), providerFieldDocumentation);

            var extractorPropDocumentation = "extractor prop docs";
            var extractorFieldDocumentation = "extractor field docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetPropertyDocumentation(typeof(TypeWithPropertiesAndFields).GetProperty(nameof(TypeWithPropertiesAndFields.Prop)), out extractorPropDocumentation))
                                               .Returns(false);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetFieldDocumentation(typeof(TypeWithPropertiesAndFields).GetField(nameof(TypeWithPropertiesAndFields.field)), out extractorFieldDocumentation))
                                               .Returns(false);

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "Prop", type, documentation: providerPropDocumentation),
                CreateFieldMetadata(typeof(bool), "field", type, documentation: providerFieldDocumentation),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_ThereAreFieldsWithDocumentationFromProviderAndExtractor_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeWithPropertiesAndFields);
            var analysisOptions = CreateAnalysisOptions(true, false, false);

            var providerPropDocumentation = "provider prop docs";
            var providerFieldDocumentation = "provider field docs";
            analysisOptions.DocumentationProviderAndAdder.AddFieldDocumentation<TypeWithPropertiesAndFields>(nameof(TypeWithPropertiesAndFields.Prop), providerPropDocumentation);
            analysisOptions.DocumentationProviderAndAdder.AddFieldDocumentation<TypeWithPropertiesAndFields>(nameof(TypeWithPropertiesAndFields.field), providerFieldDocumentation);

            var extractorPropDocumentation = "extractor prop docs";
            var extractorFieldDocumentation = "extractor field docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetPropertyDocumentation(typeof(TypeWithPropertiesAndFields).GetProperty(nameof(TypeWithPropertiesAndFields.Prop)), out extractorPropDocumentation))
                                               .Returns(true);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetFieldDocumentation(typeof(TypeWithPropertiesAndFields).GetField(nameof(TypeWithPropertiesAndFields.field)), out extractorFieldDocumentation))
                                               .Returns(true);

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "Prop", type, documentation: providerPropDocumentation),
                CreateFieldMetadata(typeof(bool), "field", type, documentation: providerFieldDocumentation),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_ThereAreFieldsWithDocumentationFromExtractor_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeWithPropertiesAndFields);
            var analysisOptions = CreateAnalysisOptions(true, false, false);

            var extractorPropDocumentation = "extractor prop docs";
            var extractorFieldDocumentation = "extractor field docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetPropertyDocumentation(typeof(TypeWithPropertiesAndFields).GetProperty(nameof(TypeWithPropertiesAndFields.Prop)), out extractorPropDocumentation))
                                               .Returns(true);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetFieldDocumentation(typeof(TypeWithPropertiesAndFields).GetField(nameof(TypeWithPropertiesAndFields.field)), out extractorFieldDocumentation))
                                               .Returns(true);

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "Prop", type, documentation: extractorPropDocumentation),
                CreateFieldMetadata(typeof(bool), "field", type, documentation: extractorFieldDocumentation),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithConstructorAttributeWithDocumentationFromProvider_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeWithConstructorAttribute);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var providerParam1Documentation = "provider param1 docs";
            var providerParam2Documentation = "provider param2 docs";
            analysisOptions.DocumentationProviderAndAdder.AddFieldDocumentation<TypeWithConstructorAttribute>("a", providerParam1Documentation);
            analysisOptions.DocumentationProviderAndAdder.AddFieldDocumentation<TypeWithConstructorAttribute>("b", providerParam2Documentation);

            var extractorParam1Documentation = "extractor param1 docs";
            var extractorParam2Documentation = "extractor param2 docs";
            var ctor = typeof(TypeWithConstructorAttribute).GetConstructors().Single();
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetMethodParameterDocumentation(ctor, "a", out extractorParam1Documentation))
                                               .Returns(false);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetMethodParameterDocumentation(ctor, "b", out extractorParam2Documentation))
                                               .Returns(false);

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type, documentation: providerParam1Documentation),
                CreateFieldMetadata(typeof(bool), "b", type, documentation: providerParam2Documentation),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithConstructorAttributeWithDocumentationFromProviderAndExtractor_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeWithConstructorAttribute);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var providerParam1Documentation = "provider param1 docs";
            var providerParam2Documentation = "provider param2 docs";
            analysisOptions.DocumentationProviderAndAdder.AddFieldDocumentation<TypeWithConstructorAttribute>("a", providerParam1Documentation);
            analysisOptions.DocumentationProviderAndAdder.AddFieldDocumentation<TypeWithConstructorAttribute>("b", providerParam2Documentation);

            var extractorParam1Documentation = "extractor param1 docs";
            var extractorParam2Documentation = "extractor param2 docs";
            var ctor = typeof(TypeWithConstructorAttribute).GetConstructors().Single();
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetMethodParameterDocumentation(ctor, "a", out extractorParam1Documentation))
                                               .Returns(true);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetMethodParameterDocumentation(ctor, "b", out extractorParam2Documentation))
                                               .Returns(true);

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type, documentation: providerParam1Documentation),
                CreateFieldMetadata(typeof(bool), "b", type, documentation: providerParam2Documentation),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithConstructorAttributeWithDocumentationFromExtractor_ExtractedCorrectFieldsAndProperties()
        {
            // Arrange
            var type = typeof(TypeWithConstructorAttribute);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var extractorParam1Documentation = "extractor param1 docs";
            var extractorParam2Documentation = "extractor param2 docs";
            var ctor = typeof(TypeWithConstructorAttribute).GetConstructors().Single();
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetMethodParameterDocumentation(ctor, "a", out extractorParam1Documentation))
                                               .Returns(true);
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetMethodParameterDocumentation(ctor, "b", out extractorParam2Documentation))
                                               .Returns(true);

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type, documentation: extractorParam1Documentation),
                CreateFieldMetadata(typeof(bool), "b", type, documentation: extractorParam2Documentation),
            };

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        #endregion Type Contains Documentation Tests

        private void ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(Type type, AnalysisOptions analysisOptions, List<IFieldMetadata> expectedMembers, IDocumentationExtractionStrategy? documentationExtractionStrategy = null)
        {
            // Act
            var actualMembers = strategy.ExtractFieldsAndProperties(type, analysisOptions, documentationExtractionStrategy).ToList();

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
