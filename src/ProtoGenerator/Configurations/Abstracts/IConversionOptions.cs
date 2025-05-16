namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// Options for conversion operations.
    /// </summary>
    public interface IConversionOptions : ICommonOptions
    {
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
        IProtoStylingConventionsStrategiesOptions ProtoStylingConventionsStrategiesOptions { get; }

        /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
        IProtoNamingStrategiesOptions ProtoNamingStrategiesOptions { get; }

        /// <inheritdoc cref="INumberingStrategiesOptions"/>
        INumberingStrategiesOptions NumberingStrategiesOptions { get; }
    }
}
