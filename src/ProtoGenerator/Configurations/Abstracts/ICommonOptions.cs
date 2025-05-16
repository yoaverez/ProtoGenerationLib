namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// Options that holds all common options between extraction components
    /// and conversion components.
    /// </summary>
    public interface ICommonOptions
    {
        /// <inheritdoc cref="IAnalysisOptions"/>
        IAnalysisOptions AnalysisOptions { get; }

        /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
        INewTypeNamingStrategiesOptions NewTypeNamingStrategiesOptions { get; }
    }
}
