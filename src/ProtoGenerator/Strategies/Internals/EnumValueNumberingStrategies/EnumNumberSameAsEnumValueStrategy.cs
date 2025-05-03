using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Strategies.Internals.EnumValueNumberingStrategies
{
    /// <summary>
    /// Enum value numbering strategy that assigns the same value of the enum value as the enum value.
    /// </summary>
    public class EnumNumberSameAsEnumValueStrategy : IEnumValueNumberingStrategy
    {
        /// <inheritdoc/>
        public int GetEnumValueNumber(IEnumTypeMetadata enumMetadata, IEnumValueMetadata enumValue, int valueIndex, int numOfValues)
        {
            return enumValue.Value;
        }
    }
}
