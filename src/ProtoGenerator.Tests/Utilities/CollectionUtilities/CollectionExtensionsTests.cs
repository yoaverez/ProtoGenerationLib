using ProtoGenerator.Utilities.CollectionUtilities;

namespace ProtoGenerator.Tests.Utilities.CollectionUtilities
{
    [TestClass]
    public class CollectionExtensionsTests
    {
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
