using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
    public class ProtoStylingConventionsStrategiesOptions : IProtoStylingConventionsStrategiesOptions
    {
        /// <inheritdoc/>
        public string MessageStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string EnumStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string EnumValueStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string ServiceStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string FieldStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string PackageStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string RpcStylingStrategy { get; set; }
    }
}
