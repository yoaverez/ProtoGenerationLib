using ProtoGenerator.Configurations.Abstracts;
using System.Collections.Generic;
using System;

namespace ProtoGenerator.Extractors.Abstracts
{
    /// <summary>
    /// Extractor for extracting all the needed types for the proto generation.
    /// </summary>
    public interface IProtoTypesExtractor
    {
        /// <summary>
        /// Extract all the types that are needed for the proto generation of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to from which to extract types that are needed for proto generation.</param>
        /// <param name="typeExtractionOptions">The options for the extraction.</param>
        /// <returns>All the types that are used by the given <paramref name="type"/>.</returns>
        IEnumerable<Type> ExtractProtoTypes(Type type, ITypeExtractionOptions typeExtractionOptions);
    }
}
