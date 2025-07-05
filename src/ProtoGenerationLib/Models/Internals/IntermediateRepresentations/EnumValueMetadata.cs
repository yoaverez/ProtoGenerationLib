using ProtoGenerationLib.Models.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;

namespace ProtoGenerationLib.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IEnumValueMetadata"/>
    public class EnumValueMetadata : DocumentableObject, IEnumValueMetadata
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

        /// <inheritdoc cref="EnumValueMetadata(string, int)"/>
        /// <inheritdoc cref="DocumentableObject(string)" path="/param"/>
        public EnumValueMetadata(string name, int value, string documentation) : base(documentation)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumValueMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public EnumValueMetadata(IEnumValueMetadata other) : base(other)
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
                   && base.Equals(other)
                   && Name.Equals(other.Name)
                   && Value.Equals(other.Value);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (base.GetHashCode(),
                    Name,
                    Value).GetHashCode();
        }

        #endregion Object Overrides
    }
}
