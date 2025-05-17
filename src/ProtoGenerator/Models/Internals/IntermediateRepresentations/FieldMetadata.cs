using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IFieldMetadata"/>
    public class FieldMetadata : IFieldMetadata
    {
        /// <inheritdoc/>
        public Type Type { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc cref="IFieldMetadata.Attributes"/>
        public List<Attribute> Attributes { get; set; }
        IEnumerable<Attribute> IFieldMetadata.Attributes => Attributes;

        /// <inheritdoc/>
        public Type DeclaringType { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="FieldMetadata"/> class.
        /// </summary>
        public FieldMetadata()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="FieldMetadata"/> class.
        /// </summary>
        /// <param name="type"><inheritdoc cref="Type" path="/node()"/></param>
        /// <param name="name"><inheritdoc cref="Name" path="/node()"/></param>
        /// <param name="attributes"><inheritdoc cref="Attributes" path="/node()"/></param>
        /// <param name="declaringType"><inheritdoc cref="DeclaringType" path="/node()"/></param>
        public FieldMetadata(Type type, string name, IEnumerable<Attribute> attributes, Type declaringType)
        {
            Type = type;
            Name = name;
            Attributes = attributes.ToList();
            DeclaringType = declaringType;
        }

        /// <summary>
        /// Create new instance of the <see cref="FieldMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public FieldMetadata(IFieldMetadata other)
        {
            Type = other.Type;
            Name = other.Name;
            Attributes = other.Attributes.ToList();
            DeclaringType = other.DeclaringType;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as FieldMetadata;
            return other != null
                   && Type.Equals(other.Type)
                   && Name.Equals(other.Name)
                   && Attributes.SequenceEqual(other.Attributes)
                   && DeclaringType.Equals(other.DeclaringType);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Type,
                    Name,
                    Attributes.CalcHashCode(),
                    DeclaringType).GetHashCode();
        }

        #endregion Object Overrides
    }
}
