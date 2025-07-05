using ProtoGenerationLib.Strategies.Internals.NewTypeNamingStrategy;
using ProtoGenerationLib.Strategies.Internals.TypeNamingStrategies;
using ProtoGenerationLib.Tests.Strategies.Internals.TypeNamingStrategies;
using System.Reflection;

namespace ProtoGenerationLib.Tests.Strategies.Internals.NewTypeNamingStrategies
{
    [TestClass]
    public class NewTypeNamingStrategyTests
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
        [DataRow(typeof(BindingFlags), $"{nameof(BindingFlags)}Wrapper")]
        [TestMethod]
        public void GetNewTypeName_NameIsCorrect(Type type, string expectedName)
        {
            // Arrange
            var strategy = new NewTypeNamingStrategy();

            // Act
            var actualName = strategy.GetNewTypeName(type);

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }
    }
}
