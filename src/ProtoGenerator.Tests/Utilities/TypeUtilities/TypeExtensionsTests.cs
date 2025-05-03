using ProtoGenerator.Tests.Utilities.TypeUtilities.DummyTypes;
using ProtoGenerator.Utilities.TypeUtilities;

namespace ProtoGenerator.Tests.Utilities.TypeUtilities
{
    [TestClass]
    public class TypeExtensionsTests
    {
        #region TryGetBase Tests

        [TestMethod]
        public void TryGetBase_ObjectDoesNotHaveBase_ReturnFalse()
        {
            // Arrange
            var type = typeof(object);

            // Act
            var actualResult = type.TryGetBase(out var _);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryGetBase_NullDoesNotHaveBase_ReturnFalse()
        {
            // Arrange
            Type type = null;

            // Act
            var actualResult = type.TryGetBase(out var _);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryGetBase_TypeWithoutBaseType_ReturnFalse()
        {
            // Arrange
            var type = typeof(TypeWithoutBaseType);

            // Act
            var actualResult = type.TryGetBase(out var _);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryGetBase_TypeWithBaseType_ReturnTrueWithCorrectBaseType()
        {
            // Arrange
            var type = typeof(TypeWithBaseType);
            var expectedBaseType = typeof(TypeWithoutBaseType);

            // Act
            var actualResult = type.TryGetBase(out var actualBaseType);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedBaseType, actualBaseType);
        }

        #endregion TryGetBase Tests

        #region GetAllImplementedInterfaces Tests

        [TestMethod]
        public void GetAllImplementedInterfaces_TypeWithoutImplementedInterfaces_ReturnEmptyArray()
        {
            // Arrange
            var type = typeof(TypeWithoutImplementedInterfaces);
            var expectedInterfaces = new List<Type>();

            // Act
            var actualInterfaces = type.GetAllImplementedInterfaces();

            // Assert
            CollectionAssert.AreEquivalent(expectedInterfaces, actualInterfaces);
        }

        [TestMethod]
        public void GetAllImplementedInterfaces_TypeWithImplementedInterfaces_ReturnEmptyArray()
        {
            // Arrange
            var type = typeof(TypeWithImplementedInterfaces);
            var expectedInterfaces = new List<Type>
            {
                typeof(I1), typeof(I2), typeof(I3), typeof(I4),
            };

            // Act
            var actualInterfaces = type.GetAllImplementedInterfaces();

            // Assert
            CollectionAssert.AreEquivalent(expectedInterfaces, actualInterfaces);
        }

        #endregion GetAllImplementedInterfaces Tests

        #region IsAttributeInherited Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void IsAttributeInherited_TypeIsNotAttribute_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(int);

            // Act
            var result = type.IsAttributeInherited();

            // Assert
            // Noting to do. The ExpectedException will do the assert.
        }

        [TestMethod]
        public void IsAttributeInherited_AttributeIsNotInherited_ReturnFalse()
        {
            // Arrange
            var type = typeof(NotInheritedAttribute);

            // Act
            var result = type.IsAttributeInherited();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAttributeInherited_AttributeIsInherited_ReturnTrue()
        {
            // Arrange
            var type = typeof(InheritedAttribute);

            // Act
            var result = type.IsAttributeInherited();

            // Assert
            Assert.IsTrue(result);
        }

        #endregion IsAttributeInherited Tests
    }
}
