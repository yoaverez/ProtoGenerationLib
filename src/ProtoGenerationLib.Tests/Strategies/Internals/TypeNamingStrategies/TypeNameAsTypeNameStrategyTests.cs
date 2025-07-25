﻿using ProtoGenerationLib.Strategies.Internals.TypeNamingStrategies;

namespace ProtoGenerationLib.Tests.Strategies.Internals.TypeNamingStrategies
{
    [TestClass]
    public class TypeNameAsTypeNameStrategyTests
    {
        [DataRow(typeof(int[]), "ArrayOfInt32")]
        [DataRow(typeof(bool[][][]), "ArrayOfArrayOfArrayOfBoolean")]
        [DataRow(typeof(string[,,]), "MultiDimensionalArrayOfString")]
        [DataRow(typeof(object[,,][]), "MultiDimensionalArrayOfArrayOfObject")]
        [DataRow(typeof(object[][,,]), "ArrayOfMultiDimensionalArrayOfObject")]
        [DataRow(typeof(int?), "NullableOfInt32")]
        [DataRow(typeof(IEnumerable<object>), "IEnumerableOfObject")]
        [DataRow(typeof(IEnumerable<object[]>), "IEnumerableOfArrayOfObject")]
        [DataRow(typeof(Dictionary<object, string>), "DictionaryOfObjectString")]
        [DataRow(typeof(Tuple<object, string, IEnumerable<int>>), "TupleOfObjectStringIEnumerableOfInt32")]
        [DataRow(typeof(TypeNameAsTypeNameStrategyTests), nameof(TypeNameAsTypeNameStrategyTests))]
        [TestMethod]
        public void GetTypeName_NameIsCorrect(Type type, string expectedName)
        {
            // Arrange
            var strategy = new TypeNameAsAlphaNumericTypeNameStrategy();

            // Act
            var actualName = strategy.GetTypeName(type);

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }
    }
}
