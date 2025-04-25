namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// Options for csharp types extraction.
    /// </summary>
    public interface ITypeExtractionOptions
    {
        /// <inheritdoc cref="IAnalysisOptions"/>
        IAnalysisOptions AnalysisOptions { get; }
    }
}
