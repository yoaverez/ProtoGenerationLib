namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents a field in a Protocol Buffer message, equivalent to a C# property.
    /// </summary>
    public interface IFieldDefinition
    {
        /// <summary>
        /// The name of the field.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The name of the type of the field in Protocol Buffer format.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The field number.
        /// </summary>
        uint Number { get; }

        /// <summary>
        /// The rule for the field (optional, repeated, etc.).
        /// </summary>
        FieldRule Rule { get; }
    }
}
