using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Abstracts
{
    /// <summary>
    /// A contract for the extraction of needed proto types from fields.
    /// </summary>
    internal interface IFieldsTypesExtractor
    {
        /// <summary>
        /// Extract the needed types for the proto generation
        /// from the given <paramref name="fieldTypes"/>.
        /// </summary>
        /// <param name="fieldTypes">The field types from which to extract the needed types.</param>
        /// <returns>
        /// All the needed types for the proto generation
        /// from the given <paramref name="fieldTypes"/>.
        /// </returns>
        IEnumerable<Type> ExtractUsedTypesFromFields(IEnumerable<Type> fieldTypes);
    }
}
