using ProtoGenerationLib.Configurations.Abstracts;

namespace ProtoGenerationLib.Configurations.Internals
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

        /// <inheritdoc/>
        public string FilePathStylingStrategy { get; set; }

        /// <summary>
        /// Create new instance of the <see cref="ProtoStylingConventionsStrategiesOptions"/> class.
        /// </summary>
        public ProtoStylingConventionsStrategiesOptions()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoStylingConventionsStrategiesOptions"/> class.
        /// </summary>
        /// <param name="messageStylingStrategy"><inheritdoc cref="MessageStylingStrategy" path="/node()"/></param>
        /// <param name="enumStylingStrategy"><inheritdoc cref="EnumStylingStrategy" path="/node()"/></param>
        /// <param name="enumValueStylingStrategy"><inheritdoc cref="EnumValueStylingStrategy" path="/node()"/></param>
        /// <param name="serviceStylingStrategy"><inheritdoc cref="ServiceStylingStrategy" path="/node()"/></param>
        /// <param name="fieldStylingStrategy"><inheritdoc cref="FieldStylingStrategy" path="/node()"/></param>
        /// <param name="packageStylingStrategy"><inheritdoc cref="PackageStylingStrategy" path="/node()"/></param>
        /// <param name="rpcStylingStrategy"><inheritdoc cref="RpcStylingStrategy" path="/node()"/></param>
        /// <param name="filePathStylingStrategy"><inheritdoc cref="FilePathStylingStrategy" path="/node()"/></param>
        public ProtoStylingConventionsStrategiesOptions(string messageStylingStrategy,
                                                        string enumStylingStrategy,
                                                        string enumValueStylingStrategy,
                                                        string serviceStylingStrategy,
                                                        string fieldStylingStrategy,
                                                        string packageStylingStrategy,
                                                        string rpcStylingStrategy,
                                                        string filePathStylingStrategy)
        {
            MessageStylingStrategy = messageStylingStrategy;
            EnumStylingStrategy = enumStylingStrategy;
            EnumValueStylingStrategy = enumValueStylingStrategy;
            ServiceStylingStrategy = serviceStylingStrategy;
            FieldStylingStrategy = fieldStylingStrategy;
            PackageStylingStrategy = packageStylingStrategy;
            RpcStylingStrategy = rpcStylingStrategy;
            FilePathStylingStrategy = filePathStylingStrategy;
        }
    }
}
