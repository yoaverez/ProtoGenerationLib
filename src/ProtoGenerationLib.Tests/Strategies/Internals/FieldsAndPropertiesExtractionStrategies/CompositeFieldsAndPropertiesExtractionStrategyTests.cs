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

        [TestInitialize]
        public void TestInitialize()
        {
            mockFlattenedStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>();
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
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
            };

            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.IsAny<Type>(), It.IsAny<IAnalysisOptions>()))
                                 .Returns(expectedMembers.ToList());


            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        [TestMethod]
        public void ExtractFieldsAndProperties_TypeWithEmptyBaseType_ReturnSameAsFlattenedStrategy()
        {
            // Arrange
            var type = typeof(TypeWithBaseType);
            var analysisOptions = CreateAnalysisOptions(false, false, false);
            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>()))
                                 .Returns(new List<IFieldMetadata>());

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>()))
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
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
                CreateFieldMetaData(typeof(int), "c", type),
                CreateFieldMetaData(typeof(bool), "d", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(TypeWithoutBaseType), nameof(TypeWithoutBaseType), type),
                CreateFieldMetaData(typeof(int), "c", type),
                CreateFieldMetaData(typeof(bool), "d", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>()))
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
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
                CreateFieldMetaData(typeof(int), "c", type),
                CreateFieldMetaData(typeof(bool), "d", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(TypeWithBaseType), nameof(TypeWithBaseType), type),
                CreateFieldMetaData(typeof(int), "c", type),
                CreateFieldMetaData(typeof(bool), "d", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithBaseType))), It.IsAny<IAnalysisOptions>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(TypeWithBaseType))), It.IsAny<IAnalysisOptions>()))
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
                CreateFieldMetaData(typeof(int), "d", type),
                CreateFieldMetaData(typeof(bool), "e", type),
            };

            var allMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "C", type),
                CreateFieldMetaData(typeof(bool), "garbage", type),
            };

            var expectedMembers = new List<IFieldMetadata>
            {
                CreateFieldMetaData(typeof(int), "a", type),
                CreateFieldMetaData(typeof(bool), "b", type),
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(ClassWithThings2))), It.IsAny<IAnalysisOptions>()))
                                 .Returns(baseMembers);

            // Mock the rest of the types.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => !type.Equals(typeof(ClassWithThings2))), It.IsAny<IAnalysisOptions>()))
                                 .Returns(allMembers);


            // Act + Assert
            ExtractFieldsAndProperties_ExtractedCorrectFieldsAndProperties(type, analysisOptions, expectedMembers);
        }

        #region Auxiliary Methods

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

        #endregion Auxiliary Methods
    }
}
