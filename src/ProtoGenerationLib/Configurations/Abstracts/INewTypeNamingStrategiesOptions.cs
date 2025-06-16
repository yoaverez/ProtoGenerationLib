namespace ProtoGenerationLib.Configurations.Abstracts
{
    /// <summary>
    /// Options for new type naming strategies.
    /// </summary>
    public interface INewTypeNamingStrategiesOptions
    {
        /// <summary>
        /// The name of the strategy that names a new type that
        /// represents a method parameter list.
        /// </summary>
        string ParameterListNamingStrategy { get; }

        /// <summary>
        /// The name of the strategy that names a new type that
        /// is needed in order to generate the protos.
        /// </summary>
        string NewTypeNamingStrategy { get; }
    }
}
