using System;
using System.Collections.Generic;
using System.Reflection;

namespace ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations
{
    /// <summary>
    /// Represents a meta data of single method.
    /// </summary>
    public interface IMethodMetadata
    {
        /// <summary>
        /// The method info from which the method meta data was created.
        /// </summary>
        MethodInfo MethodInfo { get; }

        /// <summary>
        /// The return type of the method.
        /// </summary>
        Type ReturnType { get; }

        /// <summary>
        /// The parameters that this method receives.
        /// </summary>
        IEnumerable<IMethodParameterMetadata> Parameters { get; }
    }
}
