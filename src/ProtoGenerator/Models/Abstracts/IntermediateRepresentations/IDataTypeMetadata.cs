using System;
using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.IntermediateRepresentations
{
    /// <summary>
    /// Represents a meta data of a type.
    /// </summary>
    public interface IDataTypeMetadata
    {
        /// <summary>
        /// The type whose meta data this it.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The fields meta data of the type.
        /// This could also contain property meta data.
        /// (there is no word for describing both field and property and not a method)
        /// </summary>
        IEnumerable<IFieldMetadata> Fields { get; }

        /// <summary>
        /// The data types meta data that are nested in this type.
        /// </summary>
        IEnumerable<IDataTypeMetadata> NestedDataTypes { get; }

        /// <summary>
        /// The enum types meta data that are nested in this type.
        /// </summary>
        IEnumerable<IEnumMetadata> NestedEnumTypes { get; }
    }
}
