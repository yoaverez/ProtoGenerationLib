using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Utilities;

namespace ProtoGenerator.Strategies.Internals.ProtoStylingStrategies
{
    /// <summary>
    /// Format string as a camelCase string.
    /// </summary>
    public class CamelCaseStrategy : IProtoStylingStrategy
    {
        /// <inheritdoc/>
        public string ToProtoStyle(string word)
        {
            return word.ToCamelCase();
        }

        /// <inheritdoc/>
        public string ToProtoStyle(string[] words)
        {
            return string.Join(" ", words).ToCamelCase();
        }
    }
}
