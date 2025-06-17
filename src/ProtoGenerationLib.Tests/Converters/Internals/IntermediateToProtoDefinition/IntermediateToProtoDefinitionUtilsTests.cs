using ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition;

namespace ProtoGenerationLib.Tests.Converters.Internals.IntermediateToProtoDefinition
{
    [TestClass]
    public class IntermediateToProtoDefinitionUtilsTests
    {
        [DataRow("", "a", ".", "")]
        [DataRow("a", "a", ".", "a")]
        [DataRow("a.b", "a", ".", "b")]
        [DataRow("a.b.c", "a", ".", "b.c")]
        [DataRow("a.b.c", "a.d.e", ".", "b.c")]
        [DataRow("a.b.c", "a.d.e", "_", "a.b.c")]
        [TestMethod]
        public void GetTypeShortName_ShortNameIsCorrect(string innerTypeFullName,
                                                        string outerTypeFullName,
                                                        string packageComponentsSeparator,
                                                        string expectedResult)
        {
            // Act
            var actualResult = IntermediateToProtoDefinitionUtils.GetTypeShortName(innerTypeFullName, outerTypeFullName, packageComponentsSeparator);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
