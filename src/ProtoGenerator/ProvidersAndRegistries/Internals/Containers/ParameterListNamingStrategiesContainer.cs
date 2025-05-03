using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerator.Strategies.Abstracts;
using System.Collections.Generic;
using static ProtoGenerator.ProvidersAndRegistries.Internals.Containers.ContainersUtils;

namespace ProtoGenerator.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for parameter list naming strategies.
    /// </summary>
    public class ParameterListNamingStrategiesContainer : IParameterListNamingStrategiesRegistry, IParameterListNamingStrategiesProvider
    {
        /// <summary>
        /// A mapping between strategy name to its parameter list naming strategy.
        /// </summary>
        private Dictionary<string, IParameterListNamingStrategy> parameterListNamingStrategies;

        /// <summary>
        /// Create new instance of the <see cref="ParameterListNamingStrategiesContainer"/> class.
        /// </summary>
        public ParameterListNamingStrategiesContainer()
        {
            parameterListNamingStrategies = new Dictionary<string, IParameterListNamingStrategy>();
        }

        #region IParameterListNamingStrategiesProvider Implementation

        /// <inheritdoc/>
        public IParameterListNamingStrategy GetParameterListNamingStrategy(string strategyName)
        {
            return GetStrategy(parameterListNamingStrategies, strategyName);
        }

        #endregion IParameterListNamingStrategiesProvider Implementation

        #region IParameterListNamingStrategiesRegistry Implementation

        /// <inheritdoc/>
        public void RegisterParameterListNamingStrategy(string strategyName, IParameterListNamingStrategy strategy)
        {
            RegisterStrategy(parameterListNamingStrategies, strategyName, strategy);
        }

        #endregion IParameterListNamingStrategiesRegistry Implementation
    }
}
