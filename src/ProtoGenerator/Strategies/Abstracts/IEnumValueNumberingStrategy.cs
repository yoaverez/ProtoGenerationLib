using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;

namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// Numbering strategy enum values.
    /// </summary>
    public interface IEnumValueNumberingStrategy
    {
        /// <summary>
        /// Gets the number that will represents the given <paramref name="enumValue"/>.
        /// </summary>
        /// <param name="enumMetadata">The meta data of the enum that contains the <paramref name="enumValue"/>.</param>
        /// <param name="enumValue">The original enum value.</param>
        /// <param name="valueIndex">The index of the enum value out of all the values. Starts at 0.</param>
        /// <param name="numOfValues">The total number of enum values.</param>
        /// <returns>The protobuf enum value that will represent the given <paramref name="enumValue"/>.</returns>
        int GetEnumValueNumber(IEnumTypeMetadata enumMetadata, IEnumValueMetadata enumValue, int valueIndex, int numOfValues);
    }
}
