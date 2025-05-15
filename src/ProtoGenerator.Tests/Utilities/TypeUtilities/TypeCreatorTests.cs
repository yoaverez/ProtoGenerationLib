using ProtoGenerator.Utilities.TypeUtilities;

namespace ProtoGenerator.Tests.Utilities.TypeUtilities
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

        #region CreateArrayType Tests

        [DataRow(typeof(int[]))]
        [DataRow(typeof(string[][][]))]
        [DataRow(typeof(object[,,,,,]))]
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void CreateArrayType_ElementTypeIsAnArray_ArgumentExceptionIsThrown(Type elementType)
        {
            // Act
            TypeCreator.CreateArrayType(elementType, "a");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void CreateArrayType_DataTypeWithSameNameAlreadyExits_ReturnTheExistingArray()
        {
            // Arrange
            var props1 = new List<(Type, string)>
            {
                (typeof(IEnumerable<object>), "prop1"),
                (typeof(int), "prop2"),
            };

            var typeName = "CreateArrayType_DataTypeWithSameNameAlreadyExits_ReturnTheExistingArray";
            var newType1 = TypeCreator.CreateDataType(typeName, props1);

            // Act
            var newType2 = TypeCreator.CreateArrayType(typeof(int), typeName);

            // Assert
            Assert.AreSame(newType1, newType2);
        }

        [TestMethod]
        public void CreateArrayType_ArrayTypeWithSameNameAlreadyExits_ReturnTheExistingArray()
        {
            // Arrange
            var elementType = typeof(int);
            var typeName = "CreateArrayType_ArrayTypeWithSameNameAlreadyExits_ReturnTheExistingArray";

            var newType1 = TypeCreator.CreateArrayType(elementType, typeName);

            // Act
            var newType2 = TypeCreator.CreateArrayType(elementType, typeName);

            // Assert
            Assert.AreSame(newType1, newType2);
        }

        [TestMethod]
        public void CreateArrayType_TypeDoesNotAlreadyExists_NewArrayTypeIsBuiltCorrectly()
        {
            // Arrange
            var elementType = typeof(int);
            var typeName = "CreateArrayType_TypeDoesNotAlreadyExists_NewArrayTypeIsBuiltCorrectly";

            var expectedPropsTypes = new List<Type>
            {
                elementType.MakeArrayType(),    // For the elements.
                typeof(int[]),                  // For the dimensions.
            };

            // Act
            var newType = TypeCreator.CreateArrayType(elementType, typeName);
            var actualPropsTypes = newType.GetProperties().Select(p => p.PropertyType).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedPropsTypes, actualPropsTypes);
        }

        #endregion CreateArrayType Tests
    }
}
