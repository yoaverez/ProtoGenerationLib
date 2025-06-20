using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProtoGenerationLib.Utilities
{
    /// <summary>
    /// Utility extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to camelCase.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>The camelCase string that represents the <paramref name="input"/>.</returns>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Split to alpha numeric words.
            string[] words = SplitToAlphaNumericSingleWords(input);

            // I can use Substring(1) since the empty entries
            // was removed in the SplitToAlphaNumericWords method.
            // So if the word is with length of 1 than Substring(1) return empty string.
            var firstWord = words[0].ToLowerInvariant();
            var noneFirstWords = words.Skip(1).Select(word => $"{char.ToUpperInvariant(word[0])}{word.Substring(1).ToLowerInvariant()}");
            var camelCaseWords = new[] { firstWord }.Concat(noneFirstWords).ToList();
            return string.Join("", camelCaseWords);
        }

        /// <summary>
        /// Converts a string to UpperCamelCase.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>The UpperCamelCase string that represents the <paramref name="input"/>.</returns>
        public static string ToUpperCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Split to alpha numeric words.
            string[] words = SplitToAlphaNumericSingleWords(input);

            // I can use Substring(1) since the empty entries
            // was removed in the SplitToAlphaNumericWords method.
            // So if the word is with length of 1 than Substring(1) return empty string.
            var upperCamelCaseWords = words.Select(word => $"{char.ToUpperInvariant(word[0])}{word.Substring(1).ToLowerInvariant()}");
            return string.Join("", upperCamelCaseWords);
        }

        /// <summary>
        /// Converts a string to snake_case.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>The snake_case string that represents the <paramref name="input"/>.</returns>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Split to alpha numeric words.
            string[] words = SplitToAlphaNumericSingleWords(input);

            var snakeCaseWords = words.Select(word => word.ToLowerInvariant());
            return string.Join("_", snakeCaseWords);
        }

        /// <summary>
        /// Converts a string to UPPER_SNAKE_CASE.
        /// </summary>
        /// <param name="input">The input string to convert</param>
        /// <returns>The UPPER_SNAKE_CASE string that represents the <paramref name="input"/>.</returns>
        public static string ToUpperSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.ToSnakeCase().ToUpperInvariant();
        }

        /// <summary>
        /// Split the given string to alpha numeric single words.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <returns>An array of alpha numeric words.</returns>
        /// <remarks>
        /// The returned array does not contains empty strings.
        /// i.e. each word length is at least 1.
        /// </remarks>
        public static string[] SplitToAlphaNumericWords(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return new string[0];

            var separator = "_";

            // Replace all none alpha numeric with separator.
            var onlyAlphaNumericAndSeparator = Regex.Replace(str, @"[^a-zA-Z0-9]", separator);

            // Split by the separator.
            string[] words = onlyAlphaNumericAndSeparator.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }

        /// <summary>
        /// Split the given string to alpha numeric single words.
        /// i.e TryHarder become: Try, Harder.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <returns>An array of alpha numeric words.</returns>
        /// <remarks>
        /// The returned array does not contains empty strings.
        /// i.e. each word length is at least 1.
        /// </remarks>
        private static string[] SplitToAlphaNumericSingleWords(string str)
        {
            var separator = "_";

            // Replace all none alpha numeric with separator.
            var onlyAlphaNumericAndSeparator = Regex.Replace(str, @"[^a-zA-Z0-9]", separator);

            // Separate none capital letter from its following capital letter.
            var allWordsAreSeparated = Regex.Replace(onlyAlphaNumericAndSeparator, @"([^A-Z])([A-Z])", $"$1{separator}$2");

            // Separate capital letter from its following capital letter that is followed by small
            // letters.
            allWordsAreSeparated = Regex.Replace(allWordsAreSeparated, @"([A-Z])([A-Z][a-z])", $"$1{separator}$2");

            // Split by the separator.
            string[] words = allWordsAreSeparated.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
    }
}
