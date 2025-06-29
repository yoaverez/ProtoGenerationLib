using Moq;
using static ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.TypesExtractorsUtils;
using ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Internals.FieldsAndPropertiesExtractionStrategies;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies
{
    [TestClass]
    public class CompositeFieldsAndPropertiesExtractionStrategyTests
    {
        private CompositeFieldsAndPropertiesExtractionStrategy strategy;

        private Mock<IFieldsAndPropertiesExtractionStrategy> mockFlattenedStrategy;

        private Mock<IDocumentationExtractionStrategy> mockDocumentationExtractionStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            mockFlattenedStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>();
            mockDocumentationExtractionStrategy = new Mock<IDocumentationExtractionStrategy>();
            strategy = new CompositeFieldsAndPropertiesExtractionStrategy(mockFlattenedStrategy.Object);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithoutBaseType_ReturnSameAsFlattenedStrategy()
        {
            // Arrange
            var type = typeof(TypeWithoutBaseType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.IsAny<Type>(), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(expectedMembers.ToList());


            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithEmptyBaseTypeAndShouldRemoveEmptyTypes_ReturnSameAsFlattenedStrategy()
        {
            // Arrange
            var type = typeof(TypeWithBaseType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            analysisOptions.RemoveEmptyMembers = true;

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(new List<IFieldMetadata>());

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(expectedMembers.ToList());


            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithEmptyBaseTypeAndShouldNotRemoveEmptyTypes_ReturnSameAsFlattenedStrategyWithTheAdditionOfBaseType()
        {
            // Arrange
            var type = typeof(TypeWithBaseType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            analysisOptions.RemoveEmptyMembers = false;

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(TypeWithoutBaseType), "TypeWithoutBaseType", type),
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(new List<IFieldMetadata>());

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(expectedMembers.ToList());


            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithNoneEmptyBaseType_ReturnBaseTypeAndUniqueMembersFromFlattenedStrategy()
        {
            // Arrange
            var type = typeof(TypeWithBaseType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var baseMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(TypeWithoutBaseType), nameof(TypeWithoutBaseType), type),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(allMembers);


            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithBaseTypeChain_ReturnBaseTypeAndUniqueMembersFromFlattenedStrategy()
        {
            // Arrange
            var type = typeof(TypeWithBaseTypeChain);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var baseMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(TypeWithBaseType), nameof(TypeWithBaseType), type),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(allMembers);


            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithConstructorAttribute_ReturnAllTheFieldsFromConstructor()
        {
            // Arrange
            var type = typeof(TypeWithConstructorAttribute);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var baseMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "d", type),
                CreateFieldMetadata(typeof(bool), "e", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "C", type),
                CreateFieldMetadata(typeof(bool), "garbage", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(ClassWithThings2))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(ClassWithThings2))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(allMembers);


            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #region Type Contains Documentation Tests

        [TestMethod]
        public void ExtractFieldsAndProperties_BaseTypeHasDocumentationFromProvider_ReturnBaseTypeAndUniqueMembersFromFlattenedStrategy()
        {
            // Arrange
            var type = typeof(TypeWithBaseType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var providerBaseTypeFieldDocumentation = "provider base type field docs";
            analysisOptions.DocumentationProviderAndAdder.AddDocumentation<TypeWithBaseType>(nameof(TypeWithoutBaseType), providerBaseTypeFieldDocumentation);

            var extractorBaseTypeFieldDocumentation = "extractor base type field docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetBaseTypeFieldDocumentation(typeof(TypeWithBaseType), typeof(TypeWithoutBaseType), out extractorBaseTypeFieldDocumentation))
                                               .Returns(false);

            var baseMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(TypeWithoutBaseType), nameof(TypeWithoutBaseType), type, documentation: providerBaseTypeFieldDocumentation),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(allMembers);

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_BaseTypeHasDocumentationFromProviderAndExtractor_ReturnBaseTypeAndUniqueMembersFromFlattenedStrategy()
        {
            // Arrange
            var type = typeof(TypeWithBaseType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var providerBaseTypeFieldDocumentation = "provider base type field docs";
            analysisOptions.DocumentationProviderAndAdder.AddDocumentation<TypeWithBaseType>(nameof(TypeWithoutBaseType), providerBaseTypeFieldDocumentation);

            var extractorBaseTypeFieldDocumentation = "extractor base type field docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetBaseTypeFieldDocumentation(typeof(TypeWithBaseType), typeof(TypeWithoutBaseType), out extractorBaseTypeFieldDocumentation))
                                               .Returns(true);

            var baseMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(TypeWithoutBaseType), nameof(TypeWithoutBaseType), type, documentation: providerBaseTypeFieldDocumentation),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(allMembers);

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_BaseTypeHasDocumentationFromExtractor_ReturnBaseTypeAndUniqueMembersFromFlattenedStrategy()
        {
            // Arrange
            var type = typeof(TypeWithBaseType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);

            var extractorBaseTypeFieldDocumentation = "extractor base type field docs";
            mockDocumentationExtractionStrategy.Setup(extractor => extractor.TryGetBaseTypeFieldDocumentation(typeof(TypeWithBaseType), typeof(TypeWithoutBaseType), out extractorBaseTypeFieldDocumentation))
                                               .Returns(true);

            var baseMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(int), "a", type),
                CreateFieldMetadata(typeof(bool), "b", type),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetadata(typeof(TypeWithoutBaseType), nameof(TypeWithoutBaseType), type, documentation: extractorBaseTypeFieldDocumentation),
                CreateFieldMetadata(typeof(int), "c", type),
                CreateFieldMetadata(typeof(bool), "d", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>(), It.IsAny<IDocumentationExtractionStrategy>()))
                                 .Returns(allMembers);

            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers, mockDocumentationExtractionStrategy.Object);
        }

        #endregion Type Contains Documentation Tests

        #region Auxiliary Methods

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

        #endregion Auxiliary Methods
    }
}
