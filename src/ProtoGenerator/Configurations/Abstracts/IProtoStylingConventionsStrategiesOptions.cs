namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// Options for proto styling strategies.
    /// </summary>
    public interface IProtoStylingConventionsStrategiesOptions
    {
        /// <summary>
        /// The name of the message name styling strategy.
        /// </summary>
        string MessageStylingStrategy { get; }

        /// <summary>
        /// The name of the enum name styling strategy.
        /// </summary>
        string EnumStylingStrategy { get; }

        /// <summary>
        /// The name of the enum value name styling strategy.
        /// </summary>
        string EnumValueStylingStrategy { get; }

        /// <summary>
        /// The name of the service name styling strategy.
        /// </summary>
        string ServiceStylingStrategy { get; }

        /// <summary>
        /// The name of the field name styling strategy.
        /// </summary>
        string FieldStylingStrategy { get; }

        /// <summary>
        /// The name of the package name styling strategy.
        /// </summary>
        string PackageStylingStrategy { get; }

        /// <summary>
        /// The name of the rpc name styling strategy.
        /// </summary>
        string RpcStylingStrategy { get; set; }
    }
}
