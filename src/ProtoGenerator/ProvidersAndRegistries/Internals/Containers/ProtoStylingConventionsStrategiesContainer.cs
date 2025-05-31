using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerator.Strategies.Abstracts;
using System.Collections.Generic;
using static ProtoGenerator.ProvidersAndRegistries.Internals.Containers.ContainersUtils;

namespace ProtoGenerator.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// A container for proto styling conventions strategies.
    /// </summary>
    public class ProtoStylingConventionsStrategiesContainer : IProtoStylingConventionsStrategiesRegistry, IProtoStylingConventionsStrategiesProvider
    {
        /// <summary>
        /// A mapping between strategy name to its styling strategy.
        /// </summary>
        private Dictionary<string, IProtoStylingStrategy> stylingStrategies;

        /// <summary>
        /// A mapping between strategy name to its package styling strategy.
        /// </summary>
        private Dictionary<string, IPackageStylingStrategy> packageStylingStrategies;

        /// <summary>
        /// Create new instance of the <see cref="ProtoStylingConventionsStrategiesContainer"/> class.
        /// </summary>
        public ProtoStylingConventionsStrategiesContainer()
        {
            stylingStrategies = new Dictionary<string, IProtoStylingStrategy>();
            packageStylingStrategies = new Dictionary<string, IPackageStylingStrategy>();
        }

        #region IProtoStylingConventionsStrategiesProvider Implementation

        /// <inheritdoc/>
        public IProtoStylingStrategy GetProtoStylingStrategy(string strategyName)
        {
            return GetStrategy(stylingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IPackageStylingStrategy GetPackageStylingStrategy(string strategyName)
        {
            return GetStrategy(packageStylingStrategies, strategyName);
        }

        #endregion IProtoStylingConventionsStrategiesProvider Implementation

        #region IProtoStylingConventionsStrategiesRegistry Implementation

        /// <inheritdoc/>
        public void RegisterProtoStylingStrategy(string strategyName, IProtoStylingStrategy strategy)
        {
            RegisterStrategy(stylingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterPackageStylingStrategy(string strategyName, IPackageStylingStrategy strategy)
        {
            // Package strategy is also a regular styling strategy.
            RegisterStrategy(stylingStrategies, strategyName, strategy);
            RegisterStrategy(packageStylingStrategies, strategyName, strategy);
        }

        #endregion IProtoStylingConventionsStrategiesRegistry Implementation
    }
}
