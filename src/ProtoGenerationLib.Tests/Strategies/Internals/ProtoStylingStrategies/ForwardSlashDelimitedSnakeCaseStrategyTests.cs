using ProtoGenerationLib.Strategies.Internals.ProtoStylingStrategies;

namespace ProtoGenerationLib.Tests.Strategies.Internals.ProtoStylingStrategies
{
    [TestClass]
    public class ForwardSlashDelimitedSnakeCaseStrategyTests
    {
        private static ForwardSlashDelimitedSnakeCaseStrategy filePathStylingStrategy;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            filePathStylingStrategy = new ForwardSlashDelimitedSnakeCaseStrategy();
        }

        [DataRow(new string[] {"A1bBc.proto"}, "a1b_bc.proto")]
        [DataRow(new string[] {"A", "b2", "C.proto"}, "a/b2/c.proto")]
        [TestMethod]
        public void ToProtoStyle_StylingIsCorrect(string[] ustyledFilePathComponents, string expectedStyledFilePath)
        {
            // Act
            var actualStyledFilePath = filePathStylingStrategy.ToProtoStyle(ustyledFilePathComponents);

            // Assert
            Assert.AreEqual(expectedStyledFilePath, actualStyledFilePath);
        }

        [ExpectedException(typeof(IndexOutOfRangeException))]
        [TestMethod]
        public void ToProtoStyle_GivenEmptyArray_ThrowsIndexOutOfRangeException()
        {
            // Act
            var ustyledFilePathComponents = new string[0];
            filePathStylingStrategy.ToProtoStyle(ustyledFilePathComponents);

            // Assert
            // Noting to do the expected exception will do the assert.
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ToProtoStyle_GivenEmptyArray_ThrowsArgumentOutOfRangeException()
        {
            // Act
            var ustyledFilePathComponents = new string[] { "AbB" };
            filePathStylingStrategy.ToProtoStyle(ustyledFilePathComponents);

            // Assert
            // Noting to do the expected exception will do the assert.
        }
    }
}
