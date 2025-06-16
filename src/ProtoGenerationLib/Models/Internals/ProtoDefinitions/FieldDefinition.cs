using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;

namespace ProtoGenerationLib.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IFieldDefinition"/>
    public class FieldDefinition : IFieldDefinition
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string Type { get; set; }

        /// <inheritdoc/>
        public uint Number { get; set; }

        /// <inheritdoc/>
        public FieldRule Rule { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="FieldDefinition"/> class.
        /// </summary>
        public FieldDefinition()
        {
            Rule = FieldRule.None;
        }

        /// <summary>
        /// Create new instance of the <see cref="FieldDefinition"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name" path="/node()"/></param>
        /// <param name="type"><inheritdoc cref="Type" path="/node()"/></param>
        /// <param name="number"><inheritdoc cref="Number" path="/node()"/></param>
        /// <param name="rule"><inheritdoc cref="Rule" path="/node()"/></param>
        public FieldDefinition(string name, string type, uint number, FieldRule rule = FieldRule.None)
        {
            Name = name;
            Type = type;
            Number = number;
            Rule = rule;
        }

        /// <summary>
        /// Create new instance of the <see cref="FieldDefinition"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public FieldDefinition(IFieldDefinition other)
        {
            Name = other.Name;
            Type = other.Type;
            Number = other.Number;
            Rule = other.Rule;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as FieldDefinition;
            return other != null
                   && Name.Equals(other.Name)
                   && Type.Equals(other.Type)
                   && Number.Equals(other.Number)
                   && Rule.Equals(other.Rule);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Name,
                    Type,
                    Number,
                    Rule).GetHashCode();
        }

        #endregion Object Overrides
    }
}
