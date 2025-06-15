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

        /// <summary>
        /// Create new instance of the <see cref="NewTypeNamingStrategiesOptions"/> class.
        /// </summary>
        public NewTypeNamingStrategiesOptions()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="NewTypeNamingStrategiesOptions"/> class.
        /// </summary>
        /// <param name="parameterListNamingStrategy"><inheritdoc cref="ParameterListNamingStrategy" path="/node()"/></param>
        /// <param name="newTypeNamingStrategy"><inheritdoc cref="NewTypeNamingStrategy" path="/node()"/></param>
        public NewTypeNamingStrategiesOptions(string parameterListNamingStrategy, string newTypeNamingStrategy)
        {
            ParameterListNamingStrategy = parameterListNamingStrategy;
            NewTypeNamingStrategy = newTypeNamingStrategy;
        }
    }
}
