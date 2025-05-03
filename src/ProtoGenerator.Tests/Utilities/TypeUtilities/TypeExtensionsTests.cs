using ProtoGenerator.Attributes;
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

        #region ExtractMethods Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExtractMethods_AttributeTypeIsNotAnAttribute_ThrowsArgumentException()
        {
            // Arrange
            var type = typeof(Service1);

            // Act
            var result = type.ExtractMethods(typeof(int));

            // Assert
            // Noting to do. The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ExtractMethods_TypeIsInterface_GetAllImplementedInterfacesMethods()
        {
            // Arrange
            var type = typeof(IService4);
            var expectedMethodsNames = new List<string>
            {
                nameof(IService4.IService1Method1),
                nameof(IService4.IService2Method1),
                nameof(IService4.IService3Method1),
                nameof(IService4.IService4Method1),
            };

            // Act
            var methods = type.ExtractMethods(typeof(ProtoRpcAttribute));
            var actualMethodsNames = methods.Select(method => method.Name).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedMethodsNames, actualMethodsNames);
        }

        [TestMethod]
        public void ExtractMethods_TypeIsDataType_GetAllMethods()
        {
            // Arrange
            var type = typeof(Service2);
            var expectedMethodsNames = new List<string>
            {
                nameof(Service2.IService1Method1),
                nameof(Service1.Foo),
                nameof(AbstractService.Method1),
                nameof(AbstractService.Method2),
            };

            // Act
            var methods = type.ExtractMethods(typeof(ProtoRpcAttribute));
            var actualMethodsNames = methods.Select(method => method.Name).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedMethodsNames, actualMethodsNames);
        }

        #endregion ExtractMethods Tests
    }
}
