using ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition;

namespace ProtoGenerationLib.Tests.Converters.Internals.IntermediateToProtoDefinition
{
    [TestClass]
    public class IntermediateToProtoDefinitionUtilsTests
    {
        [DataRow("", "a", ".", "")]
        [DataRow("a", "a", ".", "")]
        [DataRow("a.b", "a", ".", "b")]
        [DataRow("a.b.c", "a", ".", "b.c")]
        [DataRow("a.b.c", "a.d.e", ".", "b.c")]
        [DataRow("a.b.c", "a.d.e", "_", "a.b.c")]
        [TestMethod]
        public void GetTypeShortName_ShortNameIsCorrect(string typeFullName,
                                                        string filePackage,
                                                        string packageComponentsSeparator,
                                                        string expectedResult)
        {
            // Act
            var actualResult = IntermediateToProtoDefinitionUtils.GetTypeShortName(typeFullName, filePackage, packageComponentsSeparator);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
