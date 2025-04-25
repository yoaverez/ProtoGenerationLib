using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="IConversionOptions"/>
    public class ConversionOptions : IConversionOptions
    {
        /// <inheritdoc cref="Internals.ProtoStylingConventionsStrategiesOptions"/>
        public ProtoStylingConventionsStrategiesOptions ProtoStylingConventionsStrategiesOptions { get; set; }
        IProtoStylingConventionsStrategiesOptions IConversionOptions.ProtoStylingConventionsStrategiesOptions => ProtoStylingConventionsStrategiesOptions;

        /// <inheritdoc cref="Internals.ProtoNamingStrategiesOptions"/>
        public ProtoNamingStrategiesOptions ProtoNamingStrategiesOptions { get; set; }
        IProtoNamingStrategiesOptions IConversionOptions.ProtoNamingStrategiesOptions => ProtoNamingStrategiesOptions;

        /// <inheritdoc cref="Internals.NumberingStrategiesOptions"/>
        public NumberingStrategiesOptions NumberingStrategiesOptions { get; set; }
        INumberingStrategiesOptions IConversionOptions.NumberingStrategiesOptions => NumberingStrategiesOptions;

        /// <inheritdoc cref="Internals.AnalysisOptions"/>
        public AnalysisOptions AnalysisOptions { get; set; }
        IAnalysisOptions IConversionOptions.AnalysisOptions => AnalysisOptions;
    }
}
