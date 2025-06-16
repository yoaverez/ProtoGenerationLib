using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerationLib.Strategies.Abstracts;
using System.Collections.Generic;
using static ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers.ContainersUtils;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for parameter list naming strategies.
    /// </summary>
    public class NewTypeNamingStrategiesContainer : INewTypeNamingStrategiesRegistry, INewTypeNamingStrategiesProvider
    {
        /// <summary>
        /// A mapping between strategy name to its parameter list naming strategy.
        /// </summary>
        private Dictionary<string, IParameterListNamingStrategy> parameterListNamingStrategies;

        /// <summary>
        /// A mapping between strategy name to its new type naming strategy.
        /// </summary>
        private Dictionary<string, INewTypeNamingStrategy> newTypeNamingStrategies;

        /// <summary>
        /// Create new instance of the <see cref="NewTypeNamingStrategiesContainer"/> class.
        /// </summary>
        public NewTypeNamingStrategiesContainer()
        {
            parameterListNamingStrategies = new Dictionary<string, IParameterListNamingStrategy>();
            newTypeNamingStrategies = new Dictionary<string, INewTypeNamingStrategy>();
        }

        #region INewTypeNamingStrategiesProvider Implementation

        /// <inheritdoc/>
        public IParameterListNamingStrategy GetParameterListNamingStrategy(string strategyName)
        {
            return GetStrategy(parameterListNamingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public INewTypeNamingStrategy GetNewTypeNamingStrategy(string strategyName)
        {
            return GetStrategy(newTypeNamingStrategies, strategyName);
        }

        #endregion INewTypeNamingStrategiesProvider Implementation

        #region INewTypeNamingStrategiesRegistry Implementation

        /// <inheritdoc/>
        public void RegisterParameterListNamingStrategy(string strategyName, IParameterListNamingStrategy strategy)
        {
            RegisterStrategy(parameterListNamingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterNewTypeNamingStrategy(string strategyName, INewTypeNamingStrategy strategy)
        {
            RegisterStrategy(newTypeNamingStrategies, strategyName, strategy);
        }

        #endregion INewTypeNamingStrategiesRegistry Implementation
    }
}
