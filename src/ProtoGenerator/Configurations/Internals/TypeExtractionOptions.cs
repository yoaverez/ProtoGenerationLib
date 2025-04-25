using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="ITypeExtractionOptions"/>
    public class TypeExtractionOptions : ITypeExtractionOptions
    {
        /// <inheritdoc cref="Internals.AnalysisOptions"/>
        public AnalysisOptions AnalysisOptions { get; set; }
        IAnalysisOptions ITypeExtractionOptions.AnalysisOptions => AnalysisOptions;
    }
}
