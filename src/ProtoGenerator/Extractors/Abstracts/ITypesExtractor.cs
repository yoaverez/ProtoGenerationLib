using ProtoGenerator.Configurations.Abstracts;
using System.Collections.Generic;
using System;

namespace ProtoGenerator.Extractors.Abstracts
{
    /// <summary>
    /// Extractor for used types.
    /// </summary>
    public interface ITypesExtractor
    {
        /// <summary>
        /// Check whether or not the given <paramref name="type"/> can be handled
        /// by the this extractor.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/>
        /// can be handled by this extractor otherwise <see langword="false"/>.
        /// </returns>
        bool CanHandle(Type type, IProtoGenerationOptions generationOptions);

        /// <summary>
        /// Extract all the types that are used by the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to extract used types from.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>All the types that are used by the given <paramref name="type"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="type"/>
        /// can not be handled by this extractor.
        /// </exception>
        IEnumerable<Type> ExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions);
    }
}
