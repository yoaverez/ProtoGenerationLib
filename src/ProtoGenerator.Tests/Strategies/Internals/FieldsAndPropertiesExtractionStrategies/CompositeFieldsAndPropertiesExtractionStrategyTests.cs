using Moq;
using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Strategies.Internals.FieldsAndPropertiesExtractionStrategies;
using ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes;

namespace ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b")
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
            var expectedMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b")
            };

            // Mock the base type.
            mockFlattenedStrategy.Setup(flattenStrategy => flattenStrategy.ExtractFieldsAndProperties(It.Is<Type>(type => type.Equals(typeof(TypeWithoutBaseType))), It.IsAny<IAnalysisOptions>()))
                                 .Returns(new List<(Type, string)>());

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

            var baseMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b")
            };

            var allMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b"),
                (typeof(int), "c"), (typeof(bool), "d")
            };

            var expectedMembers = new List<(Type, string)>
            {
                (typeof(TypeWithoutBaseType), nameof(TypeWithoutBaseType)), (typeof(int), "c"), (typeof(bool), "d")
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

            var baseMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b")
            };

            var allMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b"),
                (typeof(int), "c"), (typeof(bool), "d")
            };

            var expectedMembers = new List<(Type, string)>
            {
                (typeof(TypeWithBaseType), nameof(TypeWithBaseType)), (typeof(int), "c"), (typeof(bool), "d")
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

            var baseMembers = new List<(Type, string)>
            {
                (typeof(int), "d"), (typeof(bool), "e")
            };

            var allMembers = new List<(Type, string)>
            {
                (typeof(int), "C"), (typeof(int), "garbage"),
            };

            var expectedMembers = new List<(Type, string)>
            {
                (typeof(int), "a"), (typeof(bool), "b")
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

        #endregion Auxiliary Methods
    }
}
