using System;
using System.Reflection;

namespace ProtoGenerator.Models.Abstracts.IntermediateRepresentations
{
    /// <summary>
    /// Represents a field meta data.
    /// </summary>
    public interface IFieldMetadata
    {
        /// <summary>
        /// The info of the field or property.
        /// </summary>
        MemberInfo MemberInfo { get; }

        /// <summary>
        /// The type of the field.
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// The type that declares this field.
        /// </summary>
        Type DeclaringType { get; }
    }
}
