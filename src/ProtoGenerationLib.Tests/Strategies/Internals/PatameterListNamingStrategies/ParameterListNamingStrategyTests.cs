using ProtoGenerationLib.Strategies.Internals.PatameterListNamingStrategies;
using System.Reflection;

namespace ProtoGenerationLib.Tests.Strategies.Internals.PatameterListNamingStrategies
{
    [TestClass]
    public class ParameterListNamingStrategyTests
    {
        [TestMethod]
        public void GetTypeName_NameIsCorrect()
        {
            // Arrange
            var strategy = new ParameterListNamingStrategy();
            var method = GetType().GetMethod(nameof(Method1), BindingFlags.NonPublic | BindingFlags.Static);
            var expectedName = "Method1ArrayOfInt32ArrayOfArrayOfArrayOfBooleanMultiDimensionalArrayOfString" +
                               "NullableOfInt32IEnumerableOfArrayOfObjectDictionaryOfObjectString" +
                               "TupleOfObjectStringIEnumerableOfInt32ParameterListNamingStrategyTests";

            // Act
            var actualName = strategy.GetNewParametersListTypeName(method);

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        private static void Method1(int[] a,
                                    bool[][][] b,
                                    string[,,] c,
                                    int? d,
                                    IEnumerable<object[]> e,
                                    Dictionary<object, string> f,
                                    Tuple<object, string, IEnumerable<int>> g,
                                    ParameterListNamingStrategyTests h)
        {
            // Noting to do.
        }
    }
}
