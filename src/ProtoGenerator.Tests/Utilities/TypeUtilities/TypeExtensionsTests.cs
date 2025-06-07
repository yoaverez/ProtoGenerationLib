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

        #region IsSingleDimensionalArray Tests

        [DataRow(typeof(int))]
        [DataRow(typeof(List<bool>))]
        [DataRow(typeof(int[][]))]
        [DataRow(typeof(int[][][][][]))]
        [DataRow(typeof(int[,]))]
        [DataRow(typeof(int[,,,,,,,,,]))]
        [TestMethod]
        public void IsSingleDimensionalArray_TypeIsNotASingleDimensionalArray_ReturnFalse(Type type)
        {
            // Act
            var returnValue = type.IsSingleDimensionalArray();

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(int[]))]
        [DataRow(typeof(bool[]))]
        [DataRow(typeof(object[]))]
        [DataRow(typeof(IEnumerable<Type>[]))]
        [TestMethod]
        public void IsSingleDimensionalArray_TypeIsASingleDimensionalArray_ReturnTrue(Type type)
        {
            // Act
            var returnValue = type.IsSingleDimensionalArray();

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion IsSingleDimensionalArray Tests

        #region IsMultiDimensionalOrJaggedArray Tests

        [DataRow(typeof(int[]))]
        [DataRow(typeof(bool[]))]
        [DataRow(typeof(object[]))]
        [DataRow(typeof(IEnumerable<Type>[]))]
        [TestMethod]
        public void IsMultiDimensionalOrJaggedArray_TypeIsNotAMultiDimensionalArrayNorJaggedArray_ReturnFalse(Type type)
        {
            // Act
            var returnValue = type.IsMultiDimensionalOrJaggedArray();

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(int[][]))]
        [DataRow(typeof(char[][][][][]))]
        [DataRow(typeof(bool[,]))]
        [DataRow(typeof(string[,,,,,,,,,]))]
        [TestMethod]
        public void IsMultiDimensionalOrJaggedArray_TypeIsAMultiDimensionalArrayOrJaggedArray_ReturnTrue(Type type)
        {
            // Act
            var returnValue = type.IsMultiDimensionalOrJaggedArray();

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion IsMultiDimensionalOrJaggedArray Tests

        #region GetArrayElementType Tests

        [DataRow(typeof(int))]
        [DataRow(typeof(IEnumerable<object>))]
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GetArrayElementType_TypeIsNotAnArray_ArgumentExceptionIsThrown(Type type)
        {
            // Act
            type.GetArrayElementType();

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [DataRow(typeof(int[]), typeof(int))]
        [DataRow(typeof(IEnumerable<object>[]), typeof(IEnumerable<object>))]
        [DataRow(typeof(int[][][]), typeof(int))]
        [DataRow(typeof(IEnumerable<object>[][][][]), typeof(IEnumerable<object>))]
        [DataRow(typeof(int[,,,]), typeof(int))]
        [DataRow(typeof(IEnumerable<object>[,,,,,]), typeof(IEnumerable<object>))]
        [TestMethod]
        public void GetArrayElementType_TypeAnArray_ReturnsTheElementTypeOfTheArray(Type type, Type expectedElementType)
        {
            // Act
            var actualElementType = type.GetArrayElementType();

            // Assert
            Assert.AreEqual(expectedElementType, actualElementType);
        }

        #endregion GetArrayElementType Tests

        #region IsNullable Tests

        [DataRow(typeof(int[]))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(object))]
        [DataRow(typeof(IEnumerable<string>))]
        [TestMethod]
        public void IsNullable_TypeIsNotNullable_ReturnFalse(Type type)
        {
            // Act
            var returnValue = type.IsNullable();

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(int?))]
        [DataRow(typeof(Nullable<bool>))]
        [TestMethod]
        public void IsNullable_TypeIsNullable_ReturnTrue(Type type)
        {
            // Act
            var returnValue = type.IsNullable();

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion IsNullable Tests

        #region TryGetElementOfNullableType Tests

        [DataRow(typeof(int[]))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(object))]
        [DataRow(typeof(IEnumerable<string>))]
        [TestMethod]
        public void TryGetElementOfNullableType_TypeIsNotNullable_ReturnsFalse(Type type)
        {
            // Act
            var returnValue = type.TryGetElementOfNullableType(out _);

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(int?), typeof(int))]
        [DataRow(typeof(Nullable<bool>), typeof(bool))]
        [TestMethod]
        public void TryGetElementOfNullableType_TypeIsNullable_ReturnTrueAndElementType(Type type, Type expectedElementType)
        {
            // Act
            var returnValue = type.TryGetElementOfNullableType(out var actualElementType);

            // Assert
            Assert.IsTrue(returnValue);
            Assert.AreEqual(expectedElementType, actualElementType);
        }

        #endregion TryGetElementOfNullableType Tests

        #region IsEnumerableType Tests

        [DataRow(typeof(int))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(object))]
        [DataRow(typeof(Type))]
        [TestMethod]
        public void IsEnumerableType_TypeIsNotEnumerable_ReturnFalse(Type type)
        {
            // Act
            var returnValue = type.IsEnumerableType();

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(int[][]))]
        [DataRow(typeof(IEnumerable<object>))]
        [DataRow(typeof(List<object>))]
        [DataRow(typeof(Dictionary<int, object>))]
        [TestMethod]
        public void IsEnumerableType_TypeIsEnumerable_ReturnTrue(Type type)
        {
            // Act
            var returnValue = type.IsEnumerableType();

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion IsEnumerableType Tests

        #region TryGetElementOfEnumerableType Tests

        [DataRow(typeof(int))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(object))]
        [DataRow(typeof(Type))]
        [DataRow(typeof(string))]
        [TestMethod]
        public void TryGetElementOfEnumerableType_TypeIsNotEnumerable_ReturnsFalse(Type type)
        {
            // Act
            var returnValue = type.TryGetElementOfEnumerableType(out _);

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(int[][]), typeof(int[]))]
        [DataRow(typeof(IEnumerable<object>), typeof(object))]
        [DataRow(typeof(List<object>), typeof(object))]
        [DataRow(typeof(Dictionary<int, object>), typeof(KeyValuePair<int, object>))]
        [TestMethod]
        public void TryGetElementOfEnumerableType_TypeIsEnumerable_ReturnTrueAndElementType(Type type, Type expectedElementType)
        {
            // Act
            var returnValue = type.TryGetElementOfEnumerableType(out var actualElementType);

            // Assert
            Assert.IsTrue(returnValue);
            Assert.AreEqual(expectedElementType, actualElementType);
        }

        #endregion TryGetElementOfEnumerableType Tests

        #region IsKeyValuePairEnumerableType Tests

        [DataRow(typeof(int))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(object))]
        [DataRow(typeof(Type))]
        [DataRow(typeof(IEnumerable<int>))]
        [DataRow(typeof(int[]))]
        [TestMethod]
        public void IsKeyValuePairEnumerableType_TypeIsNotKeyValuePairEnumerable_ReturnFalse(Type type)
        {
            // Act
            var returnValue = type.IsKeyValuePairEnumerableType();

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(IEnumerable<KeyValuePair<int, object>>))]
        [DataRow(typeof(Dictionary<int, string>))]
        [DataRow(typeof(IDictionary<int, bool>))]
        [TestMethod]
        public void IsKeyValuePairEnumerableType_TypeIsKeyValuePairEnumerable_ReturnTrue(Type type)
        {
            // Act
            var returnValue = type.IsKeyValuePairEnumerableType();

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion IsKeyValuePairEnumerableType Tests

        #region TryGetElementsOfKeyValuePairEnumerableType Tests

        [DataRow(typeof(int))]
        [DataRow(typeof(bool))]
        [DataRow(typeof(object))]
        [DataRow(typeof(Type))]
        [DataRow(typeof(IEnumerable<int>))]
        [DataRow(typeof(int[]))]
        [TestMethod]
        public void TryGetElementsOfKeyValuePairEnumerableType_TypeIsNotKeyValuePairEnumerable_ReturnFalse(Type type)
        {
            // Act
            var returnValue = type.TryGetElementsOfKeyValuePairEnumerableType(out _, out _);

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(IEnumerable<KeyValuePair<int, object>>), typeof(int), typeof(object))]
        [DataRow(typeof(Dictionary<double, string>), typeof(double), typeof(string))]
        [DataRow(typeof(IDictionary<int, bool>), typeof(int), typeof(bool))]
        [TestMethod]
        public void TryGetElementsOfKeyValuePairEnumerableType_TypeIsKeyValuePairEnumerable_ReturnTrue(Type type, Type expectedKeyType, Type expectedValueType)
        {
            // Act
            var returnValue = type.TryGetElementsOfKeyValuePairEnumerableType(out var actualKeyType, out var actualValueType);

            // Assert
            Assert.IsTrue(returnValue);
            Assert.AreEqual(expectedKeyType, actualKeyType);
            Assert.AreEqual(expectedValueType, actualValueType);
        }

        #endregion TryGetElementsOfKeyValuePairEnumerableType Tests

        #region IsValueTuple Tests

        [DataRow(typeof(int))]
        [DataRow(typeof(IEnumerable<bool>))]
        [DataRow(typeof(Tuple<int, string, object>))]
        [TestMethod]
        public void IsValueTuple_TypeIsNotValueTuple_ReturnFalse(Type type)
        {
            // Act
            var returnValue = type.IsValueTuple();

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof((int a, string b, object c)))]
        [DataRow(typeof(ValueTuple<int, string, object>))]
        [DataRow(typeof((object, char)))]
        [DataRow(typeof(ValueTuple<object, char>))]
        [TestMethod]
        public void IsValueTuple_TypeIsValueTuple_ReturnTrue(Type type)
        {
            // Act
            var returnValue = type.IsValueTuple();

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion IsValueTuple Tests

        #region IsTuple Tests

        [DataRow(typeof(int))]
        [DataRow(typeof(IEnumerable<bool>))]
        [DataRow(typeof((int a, string b, object c)))]
        [DataRow(typeof(ValueTuple<int, string, object>))]
        [DataRow(typeof((object, char)))]
        [TestMethod]
        public void IsTuple_TypeIsNotTuple_ReturnFalse(Type type)
        {
            // Act
            var returnValue = type.IsTuple();

            // Assert
            Assert.IsFalse(returnValue);
        }

        [DataRow(typeof(Tuple<int, string, object>))]
        [DataRow(typeof(Tuple<object, char>))]
        [TestMethod]
        public void IsTuple_TypeIsTuple_ReturnTrue(Type type)
        {
            // Act
            var returnValue = type.IsTuple();

            // Assert
            Assert.IsTrue(returnValue);
        }

        #endregion IsTuple Tests

        #region GetTypeNameWithoutGenerics Tests

        [DataRow(typeof(int), "Int32")]
        [DataRow(typeof(bool), "Boolean")]
        [DataRow(typeof(IEnumerable<object>), "IEnumerable")]
        [DataRow(typeof(Tuple<object, int, bool>), "Tuple")]
        [TestMethod]
        public void GetTypeNameWithoutGenericsTest(Type type, string expectedName)
        {
            // Act
            var actualName = type.GetTypeNameWithoutGenerics();

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        #endregion GetTypeNameWithoutGenerics Tests

        #region DoesTypeHaveProtobufWrapperType Tests

        [DataRow(typeof(bool))]
        [DataRow(typeof(byte))]
        [DataRow(typeof(sbyte))]
        [DataRow(typeof(short))]
        [DataRow(typeof(ushort))]
        [DataRow(typeof(int))]
        [DataRow(typeof(uint))]
        [DataRow(typeof(long))]
        [DataRow(typeof(ulong))]
        [DataRow(typeof(float))]
        [DataRow(typeof(double))]
        [DataRow(typeof(decimal))]
        [DataRow(typeof(byte[]))]
        [DataRow(typeof(char))]
        [DataRow(typeof(string))]
        [TestMethod]
        public void DoesTypeHaveProtobufWrapperType_TypeHaveWrapperType_ReturnTrue(Type type)
        {
            // Act
            var actualResult = type.DoesTypeHaveProtobufWrapperType();

            // Assert
            Assert.IsTrue(actualResult);
        }

        [DataRow(typeof(object))]
        [DataRow(typeof(IntPtr))]
        [DataRow(typeof(TypeExtensionsTests))]
        [DataRow(typeof(TimeSpan))]
        [TestMethod]
        public void DoesTypeHaveProtobufWrapperType_TypeDoesNotHaveWrapperType_ReturnFalse(Type type)
        {
            // Act
            var actualResult = type.DoesTypeHaveProtobufWrapperType();

            // Assert
            Assert.IsFalse(actualResult);
        }

        #endregion DoesTypeHaveProtobufWrapperType Tests
    }
}
