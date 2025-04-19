using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IEnumMetaData"/>
    public class EnumMetadata : IEnumMetadata
    {
        /// <inheritdoc/>
        public Type Type { get; set; }

        /// <inheritdoc cref="IEnumMetadata.Values"/>
        public List<IEnumValueMetadata> Values { get; set; }
        IEnumerable<IEnumValueMetadata> IEnumMetadata.Values => Values;

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="EnumMetadata"/> class.
        /// </summary>
        public EnumMetadata()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public EnumMetadata(IEnumMetadata other)
        {
            Type = other.Type;
            Values = other.Values.Select(value => new EnumValueMetadata(value)).Cast<IEnumValueMetadata>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as EnumMetadata;
            return other != null
                   && Type.Equals(other.Type)
                   && Values.SequenceEqual(other.Values);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Type,
                    Values.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
