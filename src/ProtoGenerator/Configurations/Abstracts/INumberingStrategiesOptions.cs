namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// Options for numbering strategies.
    /// </summary>
    public interface INumberingStrategiesOptions
    {
        /// <summary>
        /// The name of the strategy that responsible for matching a proto field
        /// number to a csharp field.
        /// </summary>
        string FieldNumberingStrategy { get; }

        /// <summary>
        /// The name of the strategy that responsible for matching a proto enum
        /// value number to a csharp enum value.
        /// </summary>
        string EnumValueNumberingStrategy { get; }
    }
}
