using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;

namespace ProtoGenerator.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IEnumValueMetadata"/>
    public class EnumValueMetadata : IEnumValueMetadata
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public int Value { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="EnumValueMetadata"/> class.
        /// </summary>
        public EnumValueMetadata()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumValueMetadata"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name" path="/node()"/></param>
        /// <param name="value"><inheritdoc cref="Value" path="/node()"/></param>
        public EnumValueMetadata(string name, int value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumValueMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public EnumValueMetadata(IEnumValueMetadata other)
        {
            Name = other.Name;
            Value = other.Value;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as EnumValueMetadata;
            return other != null
                   && Name.Equals(other.Name)
                   && Value.Equals(other.Value);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Name,
                    Value).GetHashCode();
        }

        #endregion Object Overrides
    }
}
