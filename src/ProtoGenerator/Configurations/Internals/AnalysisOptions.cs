using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="IAnalysisOptions"/>
    public class AnalysisOptions : IAnalysisOptions
    {
        /// <inheritdoc/>
        public bool IncludeFields { get; set; }

        /// <inheritdoc/>
        public bool IncludePrivates { get; set; }

        /// <inheritdoc/>
        public bool IncludeStatics { get; set; }

        /// <inheritdoc/>
        public string FieldsAndPropertiesExtractionStrategy { get; set; }
    }
}
