namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// The proto generator configurations.
    /// </summary>
    public interface IProtoGeneratorConfiguration
    {
        /// <inheritdoc cref="IConversionOptions"/>
        IConversionOptions ConversionOptions { get; }

        /// <inheritdoc cref="ITypeExtractionOptions"/>
        ITypeExtractionOptions TypeExtractionOptions { get; }
    }
}
