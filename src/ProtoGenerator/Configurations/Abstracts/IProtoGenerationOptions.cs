namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// The proto generator configurations.
    /// </summary>
    public interface IProtoGenerationOptions
    {
        /// <inheritdoc cref="IAnalysisOptions"/>
        IAnalysisOptions AnalysisOptions { get; }

        /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
        INewTypeNamingStrategiesOptions NewTypeNamingStrategiesOptions { get; }

        /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
        IProtoStylingConventionsStrategiesOptions ProtoStylingConventionsStrategiesOptions { get; }

        /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
        IProtoNamingStrategiesOptions ProtoNamingStrategiesOptions { get; }

        /// <inheritdoc cref="INumberingStrategiesOptions"/>
        INumberingStrategiesOptions NumberingStrategiesOptions { get; }
    }
}
