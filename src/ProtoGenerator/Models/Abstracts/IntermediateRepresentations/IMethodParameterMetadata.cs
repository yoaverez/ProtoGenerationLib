using System;

namespace ProtoGenerator.Models.Abstracts.IntermediateRepresentations
{
    /// <summary>
    /// Represents meta data of a single method parameter.
    /// </summary>
    public interface IMethodParameterMetadata
    {
        /// <summary>
        /// The type of the parameter.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        /// <remarks>The name can be null in a method declaration.</remarks>
        string? Name { get; }
    }
}
