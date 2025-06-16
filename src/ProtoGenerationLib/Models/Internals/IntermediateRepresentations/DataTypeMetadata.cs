using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Utilities.CollectionUtilities;

namespace ProtoGenerationLib.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IDataTypeMetadata"/>
    public class DataTypeMetadata : IDataTypeMetadata
    {
        /// <inheritdoc/>
        public Type Type { get; set; }

        /// <inheritdoc cref="IDataTypeMetadata.Fields"/>
        public List<IFieldMetadata> Fields { get; set; }
        IEnumerable<IFieldMetadata> IDataTypeMetadata.Fields => Fields;

        /// <inheritdoc cref="IDataTypeMetadata.NestedDataTypes"/>
        public List<IDataTypeMetadata> NestedDataTypes { get; set; }
        IEnumerable<IDataTypeMetadata> IDataTypeMetadata.NestedDataTypes => NestedDataTypes;

        /// <inheritdoc cref="IDataTypeMetadata.NestedEnumTypes"/>
        public List<IEnumTypeMetadata> NestedEnumTypes { get; set; }
        IEnumerable<IEnumTypeMetadata> IDataTypeMetadata.NestedEnumTypes => NestedEnumTypes;

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="DataTypeMetadata"/> class.
        /// </summary>
        public DataTypeMetadata()
        {
            Fields = new List<IFieldMetadata>();
            NestedDataTypes = new List<IDataTypeMetadata>();
            NestedEnumTypes = new List<IEnumTypeMetadata>();
        }

        /// <summary>
        /// Create new instance of the <see cref="DataTypeMetadata"/> class.
        /// </summary>
        /// <param name="type"><inheritdoc cref="Type" path="/node()"/></param>
        /// <param name="fields"><inheritdoc cref="Fields" path="/node()"/></param>
        /// <param name="nestedDataTypes"><inheritdoc cref="NestedDataTypes" path="/node()"/></param>
        /// <param name="nestedEnumTypes"><inheritdoc cref="NestedEnumTypes" path="/node()"/></param>
        public DataTypeMetadata(Type type, IEnumerable<IFieldMetadata> fields, IEnumerable<IDataTypeMetadata> nestedDataTypes, IEnumerable<IEnumTypeMetadata> nestedEnumTypes)
        {
            Type = type;
            Fields = fields.ToList();
            NestedDataTypes = nestedDataTypes.ToList();
            NestedEnumTypes = nestedEnumTypes.ToList();
        }



        /// <summary>
        /// Create new instance of the <see cref="DataTypeMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public DataTypeMetadata(IDataTypeMetadata other)
        {
            Type = other.Type;
            Fields = other.Fields.Select(field => new FieldMetadata(field)).Cast<IFieldMetadata>().ToList();
            NestedDataTypes = other.NestedDataTypes.Select(typeMetadata => new DataTypeMetadata(typeMetadata)).Cast<IDataTypeMetadata>().ToList();
            NestedEnumTypes = other.NestedEnumTypes.Select(enumMetadata => new EnumTypeMetadata(enumMetadata)).Cast<IEnumTypeMetadata>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as DataTypeMetadata;
            return other != null
                   && Type.Equals(other.Type)
                   && Fields.SequenceEqual(other.Fields)
                   && NestedDataTypes.SequenceEqual(other.NestedDataTypes)
                   && NestedEnumTypes.SequenceEqual(other.NestedEnumTypes);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Type,
                    Fields.CalcHashCode(),
                    NestedDataTypes.CalcHashCode(),
                    NestedEnumTypes.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
