namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// Options for the csharp types analysis.
    /// </summary>
    public interface IAnalysisOptions
    {
        /// <summary>
        /// Whether or not to include csharp fields in the proto message fields.
        /// </summary>
        /// <remarks>Properties will always be included in the proto message fields.</remarks>
        bool IncludeFields { get; }

        /// <summary>
        /// Whether or not to include private properties (and private fields
        /// if <see cref="IncludeFields"/> is <see langword="true"/>).
        /// </summary>
        bool IncludePrivates { get; }

        /// <summary>
        /// Whether or not to include static properties (and static fields
        /// if <see cref="IncludeFields"/> is <see langword="true"/>).
        /// </summary>
        bool IncludeStatics { get; }

        /// <summary>
        /// The name of the strategy for extracting fields from csharp types.
        /// </summary>
        string FieldsAndPropertiesExtractionStrategy { get; }
    }
}
