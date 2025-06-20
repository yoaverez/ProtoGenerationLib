using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IEnumTypeMetadata"/>
    public class EnumTypeMetadata : IEnumTypeMetadata
    {
        /// <inheritdoc/>
        public Type Type { get; set; }

        /// <inheritdoc cref="IEnumTypeMetadata.Values"/>
        public List<IEnumValueMetadata> Values { get; set; }
        IEnumerable<IEnumValueMetadata> IEnumTypeMetadata.Values => Values;

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="EnumTypeMetadata"/> class.
        /// </summary>
        public EnumTypeMetadata()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumTypeMetadata"/> class.
        /// </summary>
        /// <param name="type"><inheritdoc cref="Type" path="/node()"/></param>
        /// <param name="values"><inheritdoc cref="Values" path="/node()"/></param>
        public EnumTypeMetadata(Type type, List<IEnumValueMetadata> values)
        {
            Type = type;
            Values = values;
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumTypeMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public EnumTypeMetadata(IEnumTypeMetadata other)
        {
            Type = other.Type;
            Values = other.Values.Select(value => new EnumValueMetadata(value)).Cast<IEnumValueMetadata>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as EnumTypeMetadata;
            return other != null
                   && Type.Equals(other.Type)

                   // Since Enum.GetValues and Enum.GetName returns the
                   // items sorted by the binary values, the order does not equals to
                   // the order of declaration in the cs file.
                   && Values.SequenceEquivalence(other.Values);
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
