namespace ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations
{
    /// <summary>
    /// Represents a single enum value meta data.
    /// </summary>
    public interface IEnumValueMetadata : IDocumentable
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
