using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Strategies.Internals.EnumValueNumberingStrategies
{
    /// <summary>
    /// Enum value numbering strategy that assigns the index of the enum value as the enum value.
    /// </summary>
    public class SequentialEnumValueNumberingStrategy : IEnumValueNumberingStrategy
    {
        /// <inheritdoc/>
        public int GetEnumValueNumber(IEnumTypeMetadata enumMetadata, IEnumValueMetadata enumValue, int valueIndex, int numOfValues)
        {
            return valueIndex;
        }
    }
}
