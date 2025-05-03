using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Utilities;

namespace ProtoGenerator.Strategies.Internals.ProtoStylingStrategies
{
    /// <summary>
    /// Format string as a UpperCamelCase string.
    /// </summary>
    public class UpperCamelCaseStrategy : IProtoStylingStrategy
    {
        /// <inheritdoc/>
        public string ToProtoStyle(string word)
        {
            return word.ToUpperCamelCase();
        }

        /// <inheritdoc/>
        public string ToProtoStyle(string[] words)
        {
            return string.Join(" ", words).ToUpperCamelCase();
        }
    }
}
