namespace ProtoGenerationLib.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents a value in a Protocol Buffer enum, equivalent to a C# enum value.
    /// </summary>
    public interface IEnumValueDefinition : IDocumentable
    {
        /// <summary>
        /// The name of the enum value.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The numeric value of the enum value.
        /// </summary>
        int Value { get; }
    }
}
