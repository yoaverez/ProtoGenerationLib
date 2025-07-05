using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using System;

namespace ProtoGenerationLib.Strategies.Internals.FieldNumberingStrategies
{
    /// <summary>
    /// Field numbering strategy that assigns the index of the field as the field number.
    /// </summary>
    public class SequentialFieldNumberingStrategy : IFieldNumberingStrategy
    {
        /// <inheritdoc/>
        public uint GetFieldNumber(IFieldMetadata field, int fieldIndex, int numOfFields)
        {
            // Its +1 since field number are positive numbers (i.e. greater than 0)
            // and the given index starts from 0.
            return Convert.ToUInt32(fieldIndex + 1);
        }
    }
}
