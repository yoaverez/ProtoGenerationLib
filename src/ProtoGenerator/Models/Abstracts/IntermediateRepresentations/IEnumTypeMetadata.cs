using System;
using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.IntermediateRepresentations
{
    /// <summary>
    /// Represents an enum type meta data.
    /// </summary>
    public interface IEnumTypeMetadata
    {
        /// <summary>
        /// The type of the enum.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The enum values.
        /// </summary>
        IEnumerable<IEnumValueMetadata> Values { get; }
    }
}
