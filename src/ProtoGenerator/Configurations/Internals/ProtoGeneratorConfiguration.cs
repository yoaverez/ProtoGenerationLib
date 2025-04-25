using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="IProtoGeneratorConfiguration"/>
    public class ProtoGeneratorConfiguration : IProtoGeneratorConfiguration
    {
        /// <inheritdoc cref="Internals.ConversionOptions"/>
        public ConversionOptions ConversionOptions { get; set; }
        IConversionOptions IProtoGeneratorConfiguration.ConversionOptions => ConversionOptions;

        /// <inheritdoc cref="Internals.TypeExtractionOptions"/>
        public TypeExtractionOptions TypeExtractionOptions { get; set; }
        ITypeExtractionOptions IProtoGeneratorConfiguration.TypeExtractionOptions => TypeExtractionOptions;
    }
}
