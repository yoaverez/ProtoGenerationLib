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

        /// <summary>
        /// The proto file syntax (The line in the head of a proto file).
        /// Should be either "proto2" or "proto3".
        /// </summary>
        string ProtoFileSyntax { get; }
    }
}
