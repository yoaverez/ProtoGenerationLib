using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="IProtoGeneratorConfiguration"/>
    public class ProtoGeneratorConfiguration : IProtoGeneratorConfiguration
    {
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
        public ProtoStylingConventionsStrategiesOptions ProtoStylingConventionsStrategiesOptions { get; set; }
        IProtoStylingConventionsStrategiesOptions IProtoGeneratorConfiguration.ProtoStylingConventionsStrategiesOptions => ProtoStylingConventionsStrategiesOptions;

        /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
        public ProtoNamingStrategiesOptions ProtoNamingStrategiesOptions { get; set; }
        IProtoNamingStrategiesOptions IProtoGeneratorConfiguration.ProtoNamingStrategiesOptions => ProtoNamingStrategiesOptions;

        /// <inheritdoc cref="INumberingStrategiesOptions"/>
        public NumberingStrategiesOptions NumberingStrategiesOptions { get; set; }
        INumberingStrategiesOptions IProtoGeneratorConfiguration.NumberingStrategiesOptions => NumberingStrategiesOptions;

        /// <inheritdoc cref="IAnalysisOptions"/>
        public AnalysisOptions AnalysisOptions { get; set; }
        IAnalysisOptions IProtoGeneratorConfiguration.AnalysisOptions => AnalysisOptions;

        /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
        public NewTypeNamingStrategiesOptions NewTypeNamingStrategiesOptions { get; set; }
        INewTypeNamingStrategiesOptions IProtoGeneratorConfiguration.NewTypeNamingStrategiesOptions => NewTypeNamingStrategiesOptions;
    }
}
