using System.Collections.Generic;
using System;
using ProtoGenerationLib.Configurations.Abstracts;

namespace ProtoGenerationLib.Extractors.Abstracts
{
    /// <summary>
    /// Extractor for extracting all the needed types for the proto generation.
    /// </summary>
    public interface IProtoTypesExtractor
    {
        /// <summary>
        /// Extract all the types that are needed for the proto generation of the given <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The types to from which to extract types that are needed for proto generation.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>All the types that are used by the given <paramref name="types"/>.</returns>
        IEnumerable<Type> ExtractProtoTypes(IEnumerable<Type> types, IProtoGenerationOptions generationOptions, out IReadOnlyDictionary<Type, Type> originTypeToNewTypeMapping);

        /// <summary>
        /// Extract all the types that are needed for the proto generation of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to from which to extract types that are needed for proto generation.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>All the types that are used by the given <paramref name="type"/>.</returns>
        IEnumerable<Type> ExtractProtoTypes(Type type, IProtoGenerationOptions generationOptions, out IReadOnlyDictionary<Type, Type> originTypeToNewTypeMapping);
    }
}
