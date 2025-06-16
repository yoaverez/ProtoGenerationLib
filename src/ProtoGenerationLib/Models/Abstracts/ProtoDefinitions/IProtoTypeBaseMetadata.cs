namespace ProtoGenerationLib.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// The base meta data of a proto type.
    /// </summary>
    public interface IProtoTypeBaseMetadata
    {
        /// <summary>
        /// The name of the proto type.
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// The package that the proto type is define within.
        /// </summary>
        /// <remarks>The package components should be separated by '.'.</remarks>
        string? Package { get; }

        /// <summary>
        /// The file relative path (relative to the project you ran this)
        /// in which this proto type is defined.
        /// </summary>
        string? FilePath { get; }
    }
}
