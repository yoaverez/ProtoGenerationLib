using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;
using ProtoGenerationLib.Utilities.TypeUtilities;

namespace ProtoGenerationLib.Tests.Strategies.Internals.FileNamingStrategies
{
    [TestClass]
    public class NameSpaceAndTypeNameAsFileNameStrategyTests
    {
        private static NameSpaceAndTypeNameAsFileNameStrategy fileNamingStrategy;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            fileNamingStrategy = new NameSpaceAndTypeNameAsFileNameStrategy();
        }

        [TestMethod]
        public void GetFilePath_CheckThatRelativeProtoPathIsCorrect()
        {
            // Arrange
            var testedTypeNameSpace = "A123.B1B.C";
            var testedType = TypeCreator.CreateDataType(nameof(GetFilePath_CheckThatRelativeProtoPathIsCorrect), Array.Empty<(Type, string)>(), testedTypeNameSpace);

            var expectedFilePath = $"A123/B1B/C/{nameof(GetFilePath_CheckThatRelativeProtoPathIsCorrect)}.proto";

            // Act
            var actualFilePath = fileNamingStrategy.GetFilePath(testedType);

            // Assert
            Assert.AreEqual(expectedFilePath, actualFilePath);
        }
    }
}
