using Moq;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Replacers.Abstracts;
using ProtoGenerator.Replacers.Internals;
using ProtoGenerator.Replacers.Internals.TypeReplacers;

namespace ProtoGenerator.Tests.Replacers.Internals
{
    [TestClass]
    public class DefaultTypeReplacersCreatorTests
    {
        private static Mock<IProvider> mockIProvider;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            mockIProvider = new Mock<IProvider>();
        }

        #region CreateDefaultTypeReplacers Tests

        [TestMethod]
        public void CreateDefaultTypeReplacers_NumberOfReplacersIsCorrect()
        {
            // Arrange
            var expectedNumberOfReplacers = 5;

            // Act
            var actualReplacers = DefaultTypeReplacersCreator.CreateDefaultTypeReplacers(mockIProvider.Object);

            // Assert
            var actualNumberOfReplacers = actualReplacers.Count();
            Assert.AreEqual(expectedNumberOfReplacers, actualNumberOfReplacers);
        }

        [DataRow(typeof(ArrayTypeReplacer), typeof(EnumerableTypeReplacer))]
        [DataRow(typeof(DictionaryTypeReplacer), typeof(EnumerableTypeReplacer))]
        [TestMethod]
        public void CreateDefaultTypeReplacers_Replacer1IsBeforeReplacer2(Type replacer1Type, Type replacer2Type)
        {
            // Act
            var actualReplacers = DefaultTypeReplacersCreator.CreateDefaultTypeReplacers(mockIProvider.Object);

            // Assert
            var replacer1Location = GetReplacerIndex(actualReplacers, replacer1Type);
            var replacer2Location = GetReplacerIndex(actualReplacers, replacer2Type);
            Assert.IsTrue(replacer1Location < replacer2Location);
        }

        #endregion CreateDefaultTypeReplacers Tests

        #region Auxiliary Methods

        private int GetReplacerIndex(IEnumerable<ITypeReplacer> replacers, Type replacerType)
        {
            return replacers.TakeWhile(extractor => !extractor.GetType().Equals(replacerType)).Count();
        }

        #endregion Auxiliary Methods
    }
}
