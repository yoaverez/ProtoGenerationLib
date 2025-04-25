using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
    public class ProtoNamingStrategiesOptions : IProtoNamingStrategiesOptions
    {
        /// <inheritdoc/>
        public string TypeNamingStrategy { get; set; }

        /// <inheritdoc/>
        public string PackageNamingStrategy { get; set; }

        /// <inheritdoc/>
        public string FileNamingStrategy { get; set; }
    }
}
