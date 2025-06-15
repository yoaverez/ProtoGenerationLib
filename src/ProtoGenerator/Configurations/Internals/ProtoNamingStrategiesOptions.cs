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

        /// <summary>
        /// Create new instance of the <see cref="ProtoNamingStrategiesOptions"/> class.
        /// </summary>
        public ProtoNamingStrategiesOptions()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoNamingStrategiesOptions"/> class.
        /// </summary>
        /// <param name="typeNamingStrategy"><inheritdoc cref="TypeNamingStrategy" path="/node()"/></param>
        /// <param name="packageNamingStrategy"><inheritdoc cref="PackageNamingStrategy" path="/node()"/></param>
        /// <param name="fileNamingStrategy"><inheritdoc cref="FileNamingStrategy" path="/node()"/></param>
        public ProtoNamingStrategiesOptions(string typeNamingStrategy, string packageNamingStrategy, string fileNamingStrategy)
        {
            TypeNamingStrategy = typeNamingStrategy;
            PackageNamingStrategy = packageNamingStrategy;
            FileNamingStrategy = fileNamingStrategy;
        }
    }
}
