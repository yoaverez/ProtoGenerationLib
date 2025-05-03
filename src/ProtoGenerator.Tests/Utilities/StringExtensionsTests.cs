using ProtoGenerator.Utilities;

namespace ProtoGenerator.Tests.Utilities
{
    [TestClass]
    public class StringExtensionsTests
    {
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("k", "k")]
        [DataRow("G", "g")]
        [DataRow("acc DOOR", "accDoor")]
        [DataRow("l.j", "lJ")]
        [DataRow("camelCase", "camelCase")]
        [DataRow("UpperCamelCase", "upperCamelCase")]
        [DataRow("snake_case", "snakeCase")]
        [DataRow("UPPER_SNAKE_CASE", "upperSnakeCase")]
        [DataRow("kabab-case", "kababCase")]
        [DataRow("av1M", "av1M")]
        [DataRow("av1m", "av1m")]
        [TestMethod]
        public void ToCamelCaseTests(string input, string expectedOutput)
        {
            // Act
            var actualOutput = input.ToCamelCase();

            // Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("k", "K")]
        [DataRow("G", "G")]
        [DataRow("acc DOOR", "AccDoor")]
        [DataRow("l.j", "LJ")]
        [DataRow("camelCase", "CamelCase")]
        [DataRow("UpperCamelCase", "UpperCamelCase")]
        [DataRow("snake_case", "SnakeCase")]
        [DataRow("UPPER_SNAKE_CASE", "UpperSnakeCase")]
        [DataRow("kabab-case", "KababCase")]
        [DataRow("av1M", "Av1M")]
        [DataRow("av1m", "Av1m")]
        [TestMethod]
        public void ToUpperCamelCaseTests(string input, string expectedOutput)
        {
            // Act
            var actualOutput = input.ToUpperCamelCase();

            // Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("k", "k")]
        [DataRow("G", "g")]
        [DataRow("acc DOOR", "acc_door")]
        [DataRow("l.j", "l_j")]
        [DataRow("camelCase", "camel_case")]
        [DataRow("UpperCamelCase", "upper_camel_case")]
        [DataRow("snake_case", "snake_case")]
        [DataRow("UPPER_SNAKE_CASE", "upper_snake_case")]
        [DataRow("kabab-case", "kabab_case")]
        [DataRow("av1M", "av1_m")]
        [DataRow("av1m", "av1m")]
        [TestMethod]
        public void ToSnakeCaseTests(string input, string expectedOutput)
        {
            // Act
            var actualOutput = input.ToSnakeCase();

            // Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("k", "K")]
        [DataRow("G", "G")]
        [DataRow("acc DOOR", "ACC_DOOR")]
        [DataRow("l.j", "L_J")]
        [DataRow("camelCase", "CAMEL_CASE")]
        [DataRow("UpperCamelCase", "UPPER_CAMEL_CASE")]
        [DataRow("snake_case", "SNAKE_CASE")]
        [DataRow("UPPER_SNAKE_CASE", "UPPER_SNAKE_CASE")]
        [DataRow("kabab-case", "KABAB_CASE")]
        [DataRow("av1M", "AV1_M")]
        [DataRow("av1m", "AV1M")]
        [TestMethod]
        public void ToUpperSnakeCaseTests(string input, string expectedOutput)
        {
            // Act
            var actualOutput = input.ToUpperSnakeCase();

            // Assert
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [DataRow(null)]
        [DataRow("")]
        [DataRow("k", "k")]
        [DataRow("G", "G")]
        [DataRow("acc DOOR", "acc", "DOOR")]
        [DataRow("l.j", "l", "j")]
        [DataRow("camelCase", "camelCase")]
        [DataRow("UpperCamelCase", "UpperCamelCase")]
        [DataRow("snake_case", "snake", "case")]
        [DataRow("UPPER_SNAKE_CASE", "UPPER", "SNAKE", "CASE")]
        [DataRow("kabab-case", "kabab", "case")]
        [DataRow("av1M", "av1M")]
        [DataRow("av1m", "av1m")]
        [TestMethod]
        public void SplitToAlphaNumericWordsTests(string input, params string[] expectedOutput)
        {
            // Act
            var actualOutput = input.SplitToAlphaNumericWords();

            // Assert
            CollectionAssert.AreEqual(expectedOutput, actualOutput);
        }
    }
}
