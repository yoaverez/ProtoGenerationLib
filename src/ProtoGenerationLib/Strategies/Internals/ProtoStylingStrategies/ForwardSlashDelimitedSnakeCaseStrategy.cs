using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities;
using System;
using System.Linq;

namespace ProtoGenerationLib.Strategies.Internals.ProtoStylingStrategies
{
    /// <summary>
    /// Format string as a forward slash delimited snake case string.
    /// e.g if the words are: ILikeApple, MeToo then the result will be
    /// i_like_apple/me_too
    /// </summary>
    public class ForwardSlashDelimitedSnakeCaseStrategy : IFilePathStylingStrategy
    {
        /// <summary>
        /// The extension of a proto file.
        /// </summary>
        private const string PROTO_FILE_NAME_EXTENSION = ".proto";

        /// <inheritdoc/>
        public string ToProtoStyle(string[] words)
        {
            // Remove the extension of the proto file.
            var fileNameAndExtension = words[words.Length - 1];

            if (!fileNameAndExtension.Contains(PROTO_FILE_NAME_EXTENSION))
                throw new ArgumentException($"Given {nameof(words)} are not a proto file path. The last word does not end with \"{PROTO_FILE_NAME_EXTENSION}\"");

            var fileName = fileNameAndExtension.Substring(0, fileNameAndExtension.LastIndexOf(PROTO_FILE_NAME_EXTENSION));
            words[words.Length - 1] = fileName;

            var styledFilePathWithoutExtension = string.Join("/", words.Select(word => word.ToSnakeCase()));
            return $"{styledFilePathWithoutExtension}{PROTO_FILE_NAME_EXTENSION}";
        }
    }
}
