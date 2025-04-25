using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;

namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// Numbering strategy for numbering protobuf fields.
    /// </summary>
    public interface IFieldNumberingStrategy
    {
        /// <summary>
        /// Gets the number of the <paramref name="field"/> in the protobuf message.
        /// </summary>
        /// <param name="field">The field meta data.</param>
        /// <param name="fieldIndex">The index of the field in the message fields. Starts from 0.</param>
        /// <param name="numOfFields">The total number of fields in the message.</param>
        /// <returns>The number of the <paramref name="field"/> in the protobuf message.</returns>
        uint GetFieldNumber(IFieldMetadata field, int fieldIndex, int numOfFields);
    }
}
