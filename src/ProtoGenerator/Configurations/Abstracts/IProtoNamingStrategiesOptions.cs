namespace ProtoGenerator.Configurations.Abstracts
{
    /// <summary>
    /// Options for naming strategies.
    /// </summary>
    public interface IProtoNamingStrategiesOptions
    {
        /// <summary>
        /// The name of the strategy that responsible for converting a type
        /// to its proto type name.
        /// </summary>
        string TypeNamingStrategy { get; }

        /// <summary>
        /// The name of the strategy that responsible for converting a type
        /// to its proto package name.
        /// </summary>
        string PackageNamingStrategy { get; }

        /// <summary>
        /// The name of the strategy that responsible for converting a type
        /// to its proto file name.
        /// </summary>
        string FileNamingStrategy { get; }
    }
}
