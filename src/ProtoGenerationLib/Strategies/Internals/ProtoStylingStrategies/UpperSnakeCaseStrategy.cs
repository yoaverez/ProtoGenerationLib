using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities;

namespace ProtoGenerationLib.Strategies.Internals.ProtoStylingStrategies
{
    /// <summary>
    /// Format string as a UPPER_SNAKE_CASE string.
    /// </summary>
    public class UpperSnakeCaseStrategy : IProtoStylingStrategy
    {
        /// <inheritdoc/>
        public string ToProtoStyle(string word)
        {
            return word.ToUpperSnakeCase();
        }

        /// <inheritdoc/>
        public string ToProtoStyle(string[] words)
        {
            return string.Join(" ", words).ToUpperSnakeCase();
        }
    }
}
