using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IProtoTypeMetadata"/>
    public class ProtoTypeMetadata : ProtoTypeBaseMetadata, IProtoTypeMetadata
    {
        /// <inheritdoc/>
        public string FullName { get; set; }

        /// <inheritdoc/>
        public bool IsNested { get; set; }

        /// <inheritdoc/>
        public ISet<Type> NestedTypes { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypeMetadata"/> class.
        /// </summary>
        public ProtoTypeMetadata() : base()
        {
            IsNested = false;
            NestedTypes = new HashSet<Type>();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypeMetadata"/> class.
        /// </summary>
        /// <param name="fullName"><inheritdoc cref="FullName" path="/node()"/></param>
        /// <param name="isNested"><inheritdoc cref="IsNested" path="/node()"/></param>
        /// <param name="nestedTypes"><inheritdoc cref="NestedTypes" path="/node()"/></param>
        /// <inheritdoc cref="ProtoTypeBaseMetadata(string, string, string, bool)" path="/param"/>
        public ProtoTypeMetadata(string name,
                                 string package,
                                 string fullName,
                                 string filePath,
                                 bool shouldCreateProtoType = true,
                                 bool isNested = false,
                                 ISet<Type>? nestedTypes = null) : base(name, package, filePath, shouldCreateProtoType)
        {
            FullName = fullName;
            IsNested = isNested;
            NestedTypes = nestedTypes ?? new HashSet<Type>();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypeMetadata"/> class.
        /// </summary>
        /// <param name="fullName"><inheritdoc cref="FullName" path="/node()"/></param>
        /// <param name="isNested"><inheritdoc cref="IsNested" path="/node()"/></param>
        /// <param name="nestedTypes"><inheritdoc cref="NestedTypes" path="/node()"/></param>
        /// <param name="protoTypeBaseMetadata">The base meta data.</param>
        public ProtoTypeMetadata(IProtoTypeBaseMetadata protoTypeBaseMetadata,
                                 string fullName,
                                 bool isNested = false,
                                 ISet<Type>? nestedTypes = null) : base(protoTypeBaseMetadata)
        {
            FullName = fullName;
            IsNested = isNested;
            NestedTypes = nestedTypes ?? new HashSet<Type>();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypeMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public ProtoTypeMetadata(IProtoTypeMetadata other) : base(other)
        {
            FullName = other.FullName;
            IsNested = other.IsNested;
            NestedTypes = other.NestedTypes.ToHashSet();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as ProtoTypeMetadata;
            return other != null
                   && base.Equals(other)
                   && FullName.Equals(other.FullName)
                   && IsNested.Equals(other.IsNested)
                   && NestedTypes.SetEquals(other.NestedTypes);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (base.GetHashCode(),
                    FullName,
                    IsNested,
                    NestedTypes.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
