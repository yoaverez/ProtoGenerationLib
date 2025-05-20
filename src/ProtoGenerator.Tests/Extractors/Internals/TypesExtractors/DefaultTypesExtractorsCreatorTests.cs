using Moq;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Extractors.Internals.TypesExtractors;
using ProtoGenerator.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Replacers.Internals.TypeReplacers;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors
{
    [TestClass]
    public class DefaultTypesExtractorsCreatorTests
    {
        private static Mock<IProvider> mockIProvider;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            mockIProvider = new Mock<IProvider>();
        }

        #region CreateStructuralTypesExtractors Tests

        [TestMethod]
        public void CreateStructuralTypesExtractors_NumberOfExtractorsIsCorrect()
        {
            // Arrange
            var expectedNumberOfExtractors = 3;

            // Act
            var actualExtractors = DefaultTypesExtractorsCreator.CreateStructuralTypesExtractors(mockIProvider.Object);

            // Assert
            var actualNumberOfExtractors = actualExtractors.Count();
            Assert.AreEqual(expectedNumberOfExtractors, actualNumberOfExtractors);
        }

        [DataRow(typeof(ContractTypesExtractor), typeof(DataTypesExtractor))]
        [TestMethod]
        public void CreateStructuralTypesExtractors_Extractor1IsBeforeExtractor2(Type extractor1Type, Type extractor2Type)
        {
            // Act
            var actualExtractors = DefaultTypesExtractorsCreator.CreateStructuralTypesExtractors(mockIProvider.Object);

            // Assert
            var extractor1Location = GetExtractorIndex(actualExtractors, extractor1Type);
            var extractor2Location = GetExtractorIndex(actualExtractors, extractor2Type);
            Assert.IsTrue(extractor1Location < extractor2Location);
        }

        #endregion CreateStructuralTypesExtractors Tests

        #region CreateDefaultDataTypeTypesExtractors Tests

        [TestMethod]
        public void CreateDefaultDataTypeTypesExtractors_NumberOfExtractorsIsCorrect()
        {
            // Arrange
            var expectedNumberOfExtractors = 1;

            // Act
            var actualExtractors = DefaultTypesExtractorsCreator.CreateDefaultDataTypeTypesExtractors(mockIProvider.Object);

            // Assert
            var actualNumberOfExtractors = actualExtractors.Count();
            Assert.AreEqual(expectedNumberOfExtractors, actualNumberOfExtractors);
        }

        [TestMethod]
        public void CreateDefaultDataTypeTypesExtractors_DefaultDataTypesExtractorShouldBeTheLastExtractor()
        {
            // Arrange
            var expectedLastExtractorType = typeof(DefaultDataTypesExtractor);

            // Act
            var actualExtractors = DefaultTypesExtractorsCreator.CreateDefaultDataTypeTypesExtractors(mockIProvider.Object);

            // Assert
            var actualLastExtractorType = actualExtractors.Last().GetType();
            Assert.AreEqual(expectedLastExtractorType, actualLastExtractorType);
        }

        #endregion CreateDefaultDataTypeTypesExtractors Tests

        #region CreateDefaultWrapperElementTypesExtractors Tests

        [TestMethod]
        public void CreateDefaultWrapperElementTypesExtractors_NumberOfExtractorsIsCorrect()
        {
            // Arrange
            var expectedNumberOfExtractors = 4;

            // Act
            var actualExtractors = DefaultTypesExtractorsCreator.CreateDefaultWrapperElementTypesExtractors();

            // Assert
            var actualNumberOfExtractors = actualExtractors.Count();
            Assert.AreEqual(expectedNumberOfExtractors, actualNumberOfExtractors);
        }

        [DataRow(typeof(ArrayElementTypeExtractor), typeof(EnumerableElementTypeExtractor))]
        [DataRow(typeof(DictionaryElementTypesExtractor), typeof(EnumerableElementTypeExtractor))]
        [TestMethod]
        public void CreateDefaultWrapperElementTypesExtractors_Extractor1IsBeforeExtractor2(Type extractor1Type, Type extractor2Type)
        {
            // Act
            var actualExtractors = DefaultTypesExtractorsCreator.CreateDefaultWrapperElementTypesExtractors();

            // Assert
            var extractor1Location = GetExtractorIndex(actualExtractors, extractor1Type);
            var extractor2Location = GetExtractorIndex(actualExtractors, extractor2Type);
            Assert.IsTrue(extractor1Location < extractor2Location);
        }

        #endregion CreateDefaultWrapperElementTypesExtractors Tests

        #region Auxiliary Methods

        private int GetExtractorIndex(IEnumerable<ITypesExtractor> extractors, Type extractorType)
        {
            return extractors.TakeWhile(extractor => !extractor.GetType().Equals(extractorType)).Count();
        }

        #endregion Auxiliary Methods
    }
}
