using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using System;
using System.Reflection;

namespace ProtoGenerator.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IFieldMetadata"/>
    public class FieldMetadata : IFieldMetadata
    {
        /// <inheritdoc/>
        public MemberInfo MemberInfo { get; set; }

        /// <inheritdoc/>
        public Type FieldType { get; set; }

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
        /// Create new instance of the <see cref="FieldMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public FieldMetadata(IFieldMetadata other)
        {
            MemberInfo = other.MemberInfo;
            FieldType = other.FieldType;
            DeclaringType = other.DeclaringType;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as FieldMetadata;
            return other != null
                   && MemberInfo.Equals(other.MemberInfo)
                   && FieldType.Equals(other.FieldType)
                   && DeclaringType.Equals(other.DeclaringType);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (MemberInfo,
                    FieldType,
                    DeclaringType).GetHashCode();
        }

        #endregion Object Overrides
    }
}
