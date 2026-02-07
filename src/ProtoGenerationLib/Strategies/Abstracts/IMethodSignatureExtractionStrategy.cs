using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ProtoGenerationLib.Strategies.Abstracts
{
    /// <summary>
    /// Contract for extracting method signature.
    /// </summary>
    public interface IMethodSignatureExtractionStrategy
    {
        /// <summary>
        /// Extract the given <paramref name="method"/> signature
        /// i.e. it return type and parameters.
        /// </summary>
        /// <param name="method">The method whose signature is requested.</param>
        /// <param name="parameterIgnoreAttribute">The type of the attribute that tells which parameters to ignore.</param>
        /// <returns>
        /// The given <paramref name="method"/> return type and parameters.
        /// </returns>
        (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) ExtractMethodSignature(MethodInfo method, Type parameterIgnoreAttribute);
    }
}
