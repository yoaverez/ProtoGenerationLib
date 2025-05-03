namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// Options for csharp types extraction.
    /// </summary>
    public interface ITypeExtractionOptions
    {
        /// <inheritdoc cref="IAnalysisOptions"/>
        IAnalysisOptions AnalysisOptions { get; }

        /// <summary>
        /// The name of the parameter list naming strategy.
        /// </summary>
        string ParameterListNamingStrategy { get; }
    }
}
