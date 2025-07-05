using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.Strategies.Internals.EnumValueNumberingStrategies
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
