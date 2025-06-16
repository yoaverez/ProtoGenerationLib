using ProtoGenerationLib.Configurations.Abstracts;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Abstracts
{
    /// <summary>
    /// A contract for the extraction of needed proto types from fields.
    /// </summary>
    public interface IFieldsTypesExtractor
    {
        /// <summary>
        /// Extract the needed types for the proto generation
        /// from the given <paramref name="fieldTypes"/>.
        /// </summary>
        /// <param name="fieldTypes">The field types from which to extract the needed types.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>
        /// All the needed types for the proto generation
        /// from the given <paramref name="fieldTypes"/>.
        /// </returns>
        IEnumerable<Type> ExtractUsedTypesFromFields(IEnumerable<Type> fieldTypes, IProtoGenerationOptions generationOptions);
    }
}
