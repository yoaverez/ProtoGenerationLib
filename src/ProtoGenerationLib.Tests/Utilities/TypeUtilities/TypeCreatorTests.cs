using ProtoGenerationLib.Utilities.TypeUtilities;

namespace ProtoGenerationLib.Tests.Utilities.TypeUtilities
{
    [TestClass]
    public class TypeCreatorTests
    {
        #region CreateDataType Tests

        [TestMethod]
        public void CreateDataType_TypeWithSameNameAlreadyExists_ReturnTheExistingType()
        {
            // Arrange
            var props1 = new List<(Type, string)>
            {
                (typeof(IEnumerable<object>), "prop1"),
                (typeof(int), "prop2"),
            };

            var typeName = "CreateDataType_TypeWithSameNameAlreadyExists_ReturnTheExistingType";
            var newType1 = TypeCreator.CreateDataType(typeName, props1);

            var props2 = new List<(Type, string)>
            {
                (typeof(bool), "prop1"),
            };

            // Act
            var newType2 = TypeCreator.CreateDataType(typeName, props2);

            // Assert
            Assert.AreSame(newType1, newType2);
        }

        [TestMethod]
        public void CreateDataType_TypeDoesNotExists_NewTypeIsCreatedWithTheGivenProperties()
        {
            // Arrange
            var expectedProps = new List<(Type, string)>
            {
                (typeof(IEnumerable<object>), "prop1"),
                (typeof(int), "prop2"),
            };

            var typeName = "CreateDataType_TypeDoesNotExists_NewTypeIsCreatedWithTheGivenProperties";

            // Act
            var newType = TypeCreator.CreateDataType(typeName, expectedProps);
            var actualProps = newType.GetProperties().Select(p => (p.PropertyType, p.Name)).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedProps, actualProps);
        }

        #endregion CreateDataType Tests

        #region CreateProtoArrayType Tests

        [DataRow(typeof(IEnumerable<int[]>))]
        [DataRow(typeof(string))]
        [DataRow(typeof(object))]
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void CreateProtoArrayType_ElementTypeIsNotAnArray_ArgumentExceptionIsThrown(Type noneArrayType)
        {
            // Act
            TypeCreator.CreateProtoArrayType(noneArrayType, t => "a");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void CreateProtoArrayType_DataTypeWithSameNameAlreadyExits_ReturnTheExistingArray()
        {
            // Arrange
            var props1 = new List<(Type, string)>
            {
                (typeof(IEnumerable<object>), "prop1"),
                (typeof(int), "prop2"),
            };

            var typeName = nameof(CreateProtoArrayType_DataTypeWithSameNameAlreadyExits_ReturnTheExistingArray);
            var newType1 = TypeCreator.CreateDataType(typeName, props1);

            // Act
            var newType2 = TypeCreator.CreateProtoArrayType(typeof(int[]), t => typeName);

            // Assert
            Assert.AreSame(newType1, newType2);
        }

        [TestMethod]
        public void CreateProtoArrayType_ArrayTypeWithSameNameAlreadyExits_ReturnTheExistingArray()
        {
            // Arrange
            var arrayType = typeof(int[]);
            var typeName = nameof(CreateProtoArrayType_ArrayTypeWithSameNameAlreadyExits_ReturnTheExistingArray);

            var newType1 = TypeCreator.CreateProtoArrayType(arrayType, t => typeName);

            // Act
            var newType2 = TypeCreator.CreateProtoArrayType(arrayType, t => typeName);

            // Assert
            Assert.AreSame(newType1, newType2);
        }

        [TestMethod]
        public void CreateProtoArrayType_TypeIsMultiDimensionalArray_NewArrayTypeIsBuiltCorrectly()
        {
            // Arrange
            var arrayType = typeof(int[,,]);
            var typeName = nameof(CreateProtoArrayType_TypeIsMultiDimensionalArray_NewArrayTypeIsBuiltCorrectly);

            var expectedPropsTypes = new List<Type>
            {
                arrayType.GetArrayElementType().MakeArrayType(), // For the elements.
                typeof(int[]),                                   // For the dimensions.
            };

            // Act
            var newType = TypeCreator.CreateProtoArrayType(arrayType, t => typeName);
            var actualPropsTypes = newType.GetProperties().Select(p => p.PropertyType).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedPropsTypes, actualPropsTypes);
        }

        [TestMethod]
        public void CreateProtoArrayType_TypeIsJaggedArray_NewArrayTypeIsBuiltCorrectly()
        {
            // Arrange
            var arrayType = typeof(int[][][]);
            var arrayElementType = arrayType.GetArrayElementType();
            var typeNamePrefix = nameof(CreateProtoArrayType_TypeIsJaggedArray_NewArrayTypeIsBuiltCorrectly);

            Func<Type, string> namingFunction = (t) =>
            {
                if (t.Equals(typeof(int[][][])))
                    return $"{typeNamePrefix}_Int3JaggedArray";

                if (t.Equals(typeof(int[][])))
                    return $"{typeNamePrefix}_Int2JaggedArray";

                if (t.Equals(typeof(int[])))
                    return $"{typeNamePrefix}_ArrayOfInt";

                return typeNamePrefix;
            };

            var expectedArrayOfIntPropsTypesNames = new List<string>()
            {
                arrayElementType.MakeArrayType().Name,
            };

            var expectedInt2JaggedArrayPropsTypesNames = new List<string>()
            {
                $"{typeNamePrefix}_ArrayOfInt[]",
            };

            var expectedInt3JaggedArrayPropsTypesNames = new List<string>()
            {
                $"{typeNamePrefix}_Int2JaggedArray[]",
            };

            // Act
            var newType = TypeCreator.CreateProtoArrayType(arrayType, namingFunction);

            // Assert
            var actualInt3JaggedArrayPropsTypes = newType.GetProperties().Select(p => p.PropertyType).ToList();
            var actualInt3JaggedArrayPropsTypesNames = actualInt3JaggedArrayPropsTypes.Select(t => t.Name).ToList();
            CollectionAssert.AreEquivalent(expectedInt3JaggedArrayPropsTypesNames, actualInt3JaggedArrayPropsTypesNames);

            var actualInt2JaggedArrayPropsTypes = actualInt3JaggedArrayPropsTypes.Select(t => t.GetElementType()).SelectMany(t => t.GetProperties()).Select(p => p.PropertyType).ToList();
            var actualInt2JaggedArrayPropsTypesNames = actualInt2JaggedArrayPropsTypes.Select(t => t.Name).ToList();
            CollectionAssert.AreEquivalent(expectedInt2JaggedArrayPropsTypesNames, actualInt2JaggedArrayPropsTypesNames);

            var actualArrayOfIntPropsTypes = actualInt2JaggedArrayPropsTypes.Select(t => t.GetElementType()).SelectMany(t => t.GetProperties()).Select(p => p.PropertyType).ToList();
            var actualArrayOfIntPropsTypesNames = actualArrayOfIntPropsTypes.Select(t => t.Name).ToList();
            CollectionAssert.AreEquivalent(expectedArrayOfIntPropsTypesNames, actualArrayOfIntPropsTypesNames);
        }

        [TestMethod]
        public void CreateProtoArrayType_TypeIsArrayOfMultiDimensionalArrays_NewArrayTypeIsBuiltCorrectly()
        {
            // Arrange
            var arrayType = typeof(string[][,]);
            var arrayElementType = arrayType.GetArrayElementType();
            var typeNamePrefix = nameof(CreateProtoArrayType_TypeIsArrayOfMultiDimensionalArrays_NewArrayTypeIsBuiltCorrectly);

            Func<Type, string> namingFunction = (t) =>
            {
                if (t.Equals(typeof(string[][,])))
                    return $"{typeNamePrefix}_2";

                if (t.Equals(typeof(string[,])))
                    return $"{typeNamePrefix}_1";

                return typeNamePrefix;
            };

            var expectedArrayOfMultidimensionalArraysPropsTypesNames = new List<string>()
            {
                $"{typeNamePrefix}_1[]",
            };

            var expectedMultiDimensionalArrayPropsTypesNames = new List<string>()
            {
                arrayElementType.MakeArrayType().Name,
                typeof(int[]).Name,
            };

            // Act
            var newType = TypeCreator.CreateProtoArrayType(arrayType, namingFunction);

            // Assert
            var actualArrayOfMultidimensionalArraysPropsTypes = newType.GetProperties().Select(p => p.PropertyType).ToList();
            var actualArrayOfMultidimensionalArraysPropsTypesNames = actualArrayOfMultidimensionalArraysPropsTypes.Select(t => t.Name).ToList();
            CollectionAssert.AreEquivalent(expectedArrayOfMultidimensionalArraysPropsTypesNames, actualArrayOfMultidimensionalArraysPropsTypesNames);

            var actualMultiDimensionalArrayPropsTypes = actualArrayOfMultidimensionalArraysPropsTypes.Select(t => t.GetElementType()).SelectMany(t => t.GetProperties()).Select(p => p.PropertyType).ToList();
            var actualMultiDimensionalArrayPropsTypesNames = actualMultiDimensionalArrayPropsTypes.Select(t => t.Name).ToList();
            CollectionAssert.AreEquivalent(expectedMultiDimensionalArrayPropsTypesNames, actualMultiDimensionalArrayPropsTypesNames);
        }

        [TestMethod]
        public void CreateProtoArrayType_TypeIsMultiDimensionalArrayOfArrays_NewArrayTypeIsBuiltCorrectly()
        {
            // Arrange
            var arrayType = typeof(string[,][]);
            var arrayElementType = arrayType.GetArrayElementType();
            var typeNamePrefix = nameof(CreateProtoArrayType_TypeIsMultiDimensionalArrayOfArrays_NewArrayTypeIsBuiltCorrectly);

            Func<Type, string> namingFunction = (t) =>
            {
                if (t.Equals(typeof(string[,][])))
                    return $"{typeNamePrefix}_2";

                if (t.Equals(typeof(string[])))
                    return $"{typeNamePrefix}_1";

                return typeNamePrefix;
            };

            var expectedMultidimensionalArrayOfArraysPropsTypesNames = new List<string>()
            {
                $"{typeNamePrefix}_1[]",
                typeof(int[]).Name,
            };

            var expectedArrayPropsTypesNames = new List<string>()
            {
                arrayElementType.MakeArrayType().Name,
            };

            // Act
            var newType = TypeCreator.CreateProtoArrayType(arrayType, namingFunction);

            // Assert
            var actualMultidimensionalArrayOfArraysPropsTypes = newType.GetProperties().Select(p => p.PropertyType).ToList();
            var actualMultidimensionalArrayOfArraysPropsTypesNames = actualMultidimensionalArrayOfArraysPropsTypes.Select(t => t.Name).ToList();
            CollectionAssert.AreEquivalent(expectedMultidimensionalArrayOfArraysPropsTypesNames, actualMultidimensionalArrayOfArraysPropsTypesNames);

            var actualArrayPropsTypes = actualMultidimensionalArrayOfArraysPropsTypes.Select(t => t.GetElementType()).SelectMany(t => t.GetProperties()).Select(p => p.PropertyType).ToList();
            var actualArrayPropsTypesNames = actualArrayPropsTypes.Select(t => t.Name).ToList();
            CollectionAssert.AreEquivalent(expectedArrayPropsTypesNames, actualArrayPropsTypesNames);
        }

        #endregion CreateProtoArrayType Tests

        #region TryGetCreatedType Tests

        [TestMethod]
        public void TryGetCreatedType_TypeWithGivenNameHasNotBeenCreated_ReturnFalse()
        {
            // Act
            var actualResult = TypeCreator.TryGetCreatedType("a", out var type);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryGetCreatedType_TypeWithGivenNameHasBeenCreated_ReturnTrue()
        {
            // Arrange
            var dataTypeName = $"{nameof(TryGetCreatedType_TypeWithGivenNameHasBeenCreated_ReturnTrue)}-DataType";
            var arrayTypeName = $"{nameof(TryGetCreatedType_TypeWithGivenNameHasBeenCreated_ReturnTrue)}-ArrayType";
            var newDataType = TypeCreator.CreateDataType(dataTypeName, Array.Empty<(Type, string)>());
            var newArrayType = TypeCreator.CreateProtoArrayType(typeof(int[]), t => arrayTypeName);

            // Act
            var actualDataTypeResult = TypeCreator.TryGetCreatedType(dataTypeName, out var actualDataType);
            var actualArrayTypeResult = TypeCreator.TryGetCreatedType(arrayTypeName, out var actualArrayType);

            // Assert
            Assert.IsTrue(actualDataTypeResult);
            Assert.AreSame(newDataType, actualDataType);

            Assert.IsTrue(actualArrayTypeResult);
            Assert.AreSame(newArrayType, actualArrayType);
        }

        #endregion TryGetCreatedType Tests
    }
}
