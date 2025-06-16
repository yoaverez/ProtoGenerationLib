using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations
{
    /// <summary>
    /// Represents a field meta data.
    /// </summary>
    public interface IFieldMetadata
    {
        /// <summary>
        /// The type of the field.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The name of the field.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The attributes of the field.
        /// </summary>
        IEnumerable<Attribute> Attributes { get; }

        /// <summary>
        /// The type that declares this field.
        /// </summary>
        Type DeclaringType { get; }
    }
}
