using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
    public class NewTypeNamingStrategiesOptions : INewTypeNamingStrategiesOptions
    {
        /// <inheritdoc/>
        public string ParameterListNamingStrategy {  get; set; }

        /// <inheritdoc/>
        public string NewTypeNamingStrategy { get; set; }
    }
}
