using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;

namespace ProtoGenerationLib.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IEnumValueDefinition"/>
    public class EnumValueDefinition : IEnumValueDefinition
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public int Value { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="EnumValueDefinition"/> class.
        /// </summary>
        public EnumValueDefinition()
        {
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumValueDefinition"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name" path="/node()"/></param>
        /// <param name="value"><inheritdoc cref="Name" path="/node()"/></param>
        public EnumValueDefinition(string name, int value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumValueDefinition"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public EnumValueDefinition(IEnumValueDefinition other)
        {
            Name = other.Name;
            Value = other.Value;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as EnumValueDefinition;
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
