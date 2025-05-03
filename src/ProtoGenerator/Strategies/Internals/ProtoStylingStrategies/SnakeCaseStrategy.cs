using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Utilities;

namespace ProtoGenerator.Strategies.Internals.ProtoStylingStrategies
{
    /// <summary>
    /// Format string as a snake_case string.
    /// </summary>
    public class SnakeCaseStrategy : IProtoStylingStrategy
    {
        /// <inheritdoc/>
        public string ToProtoStyle(string word)
        {
            return word.ToSnakeCase();
        }

        /// <inheritdoc/>
        public string ToProtoStyle(string[] words)
        {
            return string.Join(" ", words).ToSnakeCase();
        }
    }
}
