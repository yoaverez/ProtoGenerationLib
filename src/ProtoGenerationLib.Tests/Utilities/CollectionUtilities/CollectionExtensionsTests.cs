using ProtoGenerationLib.Utilities.CollectionUtilities;

namespace ProtoGenerationLib.Tests.Utilities.CollectionUtilities
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        #region AddRange Tests

        [TestMethod]
        public void SetAddRange_ReturnAsExpected()
        {
            // Arrange
            var initialSet = new HashSet<int> { 1, 3, 6 };
            var collectionToAdd = new List<int> { 5, 3, 4, 1, 2 };
            var expectedSet = new HashSet<int> { 1, 2, 3, 4, 5, 6 };
            var actualSet = new HashSet<int>(initialSet);

            // Act
            actualSet.AddRange(collectionToAdd);

            // Assert
            CollectionAssert.AreEquivalent(expectedSet.ToArray(), actualSet.ToArray());
        }

        [TestMethod]
        public void DictionaryAddRange_ReturnAsExpected()
        {
            // Arrange
            var initialDictionary = new Dictionary<int, int>
            {
                [1] = 1,
                [3] = 3,
                [6] = 6
            };
            var collectionToAdd = new List<KeyValuePair<int, int>>
            {
                new KeyValuePair<int, int>(5, 5),
                new KeyValuePair<int, int>(3, 3),
                new KeyValuePair<int, int>(4, 4),
                new KeyValuePair<int, int>(1, 1),
                new KeyValuePair<int, int>(2, 2),
            };

            var expectedDictionary = new Dictionary<int, int>
            {
                [1] = 1,
                [2] = 2,
                [3] = 3,
                [4] = 4,
                [5] = 5,
                [6] = 6,
            };
            var actualDictionary = new Dictionary<int, int>(initialDictionary);

            // Act
            actualDictionary.AddRange(collectionToAdd);

            // Assert
            CollectionAssert.AreEquivalent(expectedDictionary, actualDictionary);
        }

        #endregion AddRange Tests

        #region SequenceEquivalence Tests

        [TestMethod]
        public void SequenceEquivalence_CollectionsAreBothNull_ReturnTrue()
        {
            // Arrange
            List<int> collection1 = null;
            List<int> collection2 = null;

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SequenceEquivalence_OnlyFirstCollectionIsNull_ReturnFalse()
        {
            // Arrange
            List<int> collection1 = null;
            var collection2 = new List<int> { 1, 4, -9 };

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SequenceEquivalence_OnlySecondCollectionIsNull_ReturnFalse()
        {
            // Arrange
            var collection1 = new List<int> { 1, 4, -9 };
            List<int> collection2 = null;

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SequenceEquivalence_CollectionsAreSame_ReturnTrue()
        {
            // Arrange
            var collection1 = new List<int> { 1, 4, -9 };
            var collection2 = collection1;

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SequenceEquivalence_FirstCollectionHasMoreElements_ReturnFalse()
        {
            // Arrange
            var collection1 = new List<int> { 1, 4, -9, 10 };
            var collection2 = new List<int> { 1, 4, -9 };

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SequenceEquivalence_SecondCollectionHasMoreElements_ReturnFalse()
        {
            // Arrange
            var collection1 = new List<int> { 1, 4, -9 };
            var collection2 = new List<int> { 1, 4, -9, 10 };

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SequenceEquivalence_CollectionsAreEqualsAlsoInOrder_ReturnTrue()
        {
            // Arrange
            var collection1 = new List<int> { 1, 4, -9 };
            var collection2 = new List<int> { 1, 4, -9 };

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SequenceEquivalence_CollectionsAreEqualsNotInOrder_ReturnTrue()
        {
            // Arrange
            var collection1 = new List<int> { 1, 4, -9 };
            var collection2 = new List<int> { -9, 1, 4 };

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SequenceEquivalence_CollectionsHaveOneDifferentItem_ReturnFalse()
        {
            // Arrange
            var collection1 = new List<int> { 1, 4, -9 };
            var collection2 = new List<int> { 1, 3, -9 };

            // Act
            var result = collection1.SequenceEquivalence(collection2);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion SequenceEquivalence Tests
    }
}
