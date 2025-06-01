using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="IProtoGenerationOptions"/>
    public class ProtoGenerationOptions : IProtoGenerationOptions
    {
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
        public ProtoStylingConventionsStrategiesOptions ProtoStylingConventionsStrategiesOptions { get; set; }
        IProtoStylingConventionsStrategiesOptions IProtoGenerationOptions.ProtoStylingConventionsStrategiesOptions => ProtoStylingConventionsStrategiesOptions;

        /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
        public ProtoNamingStrategiesOptions ProtoNamingStrategiesOptions { get; set; }
        IProtoNamingStrategiesOptions IProtoGenerationOptions.ProtoNamingStrategiesOptions => ProtoNamingStrategiesOptions;

        /// <inheritdoc cref="INumberingStrategiesOptions"/>
        public NumberingStrategiesOptions NumberingStrategiesOptions { get; set; }
        INumberingStrategiesOptions IProtoGenerationOptions.NumberingStrategiesOptions => NumberingStrategiesOptions;

        /// <inheritdoc cref="IAnalysisOptions"/>
        public AnalysisOptions AnalysisOptions { get; set; }
        IAnalysisOptions IProtoGenerationOptions.AnalysisOptions => AnalysisOptions;

        /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
        public NewTypeNamingStrategiesOptions NewTypeNamingStrategiesOptions { get; set; }
        INewTypeNamingStrategiesOptions IProtoGenerationOptions.NewTypeNamingStrategiesOptions => NewTypeNamingStrategiesOptions;
    }
}
