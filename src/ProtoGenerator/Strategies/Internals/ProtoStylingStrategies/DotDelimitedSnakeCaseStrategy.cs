using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Utilities;
using System;
using System.Linq;

namespace ProtoGenerator.Strategies.Internals.ProtoStylingStrategies
{
    /// <summary>
    /// Format string as a dot delimited snake case string.
    /// e.g if the words are: ILikeApple, MeToo then the result will be
    /// i_like_apple.me_too
    /// </summary>
    public class DotDelimitedSnakeCaseStrategy : IProtoStylingStrategy
    {
        /// <inheritdoc/>
        public string ToProtoStyle(string word)
        {
            return ToProtoStyle(word.SplitToAlphaNumericWords());
        }

        /// <inheritdoc/>
        public string ToProtoStyle(string[] words)
        {
            return string.Join(".", words.Select(word => word.ToSnakeCase()));
        }
    }
}
