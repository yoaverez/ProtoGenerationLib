using System;
using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.IntermediateRepresentations
{
    /// <summary>
    /// Represents a Type with only methods i.e contract types.
    /// </summary>
    public interface IContractTypeMetadata
    {
        /// <summary>
        /// The type of the contract object.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The methods that the contract defines.
        /// </summary>
        IEnumerable<IMethodMetadata> Methods { get; }
    }
}
