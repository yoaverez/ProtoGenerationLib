using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities;
using System.Linq;

namespace ProtoGenerationLib.Strategies.Internals.ProtoStylingStrategies
{
    /// <summary>
    /// Format string as a dot delimited snake case string.
    /// e.g if the words are: ILikeApple, MeToo then the result will be
    /// i_like_apple.me_too
    /// </summary>
    public class DotDelimitedSnakeCaseStrategy : IPackageStylingStrategy
    {
        /// <inheritdoc/>
        public string PackageComponentsSeparator => ".";

        /// <inheritdoc/>
        public string ToProtoStyle(string word)
        {
            return ToProtoStyle(word.SplitToAlphaNumericWords());
        }

        /// <inheritdoc/>
        public string ToProtoStyle(string[] words)
        {
            return string.Join(PackageComponentsSeparator, words.Select(word => word.ToSnakeCase()));
        }
    }
}
