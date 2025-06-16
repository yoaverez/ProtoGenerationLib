using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerationLib.Strategies.Abstracts;
using System.Collections.Generic;
using static ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers.ContainersUtils;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for proto naming strategies.
    /// </summary>
    public class ProtoNamingStrategiesContainer : IProtoNamingStrategiesRegistry, IProtoNamingStrategiesProvider
    {
        /// <summary>
        /// A mapping between strategy name to its type naming strategy.
        /// </summary>
        private Dictionary<string, ITypeNamingStrategy> typeNamingStrategies;

        /// <summary>
        /// A mapping between strategy name to its package naming strategy.
        /// </summary>
        private Dictionary<string, IPackageNamingStrategy> packageNamingStrategies;

        /// <summary>
        /// A mapping between strategy name to its file naming strategy.
        /// </summary>
        private Dictionary<string, IFileNamingStrategy> fileNamingStrategies;

        /// <summary>
        /// Create new instance of the <see cref="ProtoNamingStrategiesContainer"/> class.
        /// </summary>
        public ProtoNamingStrategiesContainer()
        {
            typeNamingStrategies = new Dictionary<string, ITypeNamingStrategy>();
            packageNamingStrategies = new Dictionary<string, IPackageNamingStrategy>();
            fileNamingStrategies = new Dictionary<string, IFileNamingStrategy>();
        }

        #region IProtoNamingStrategiesProvider Implementation

        /// <inheritdoc/>
        public IFileNamingStrategy GetFileNamingStrategy(string strategyName)
        {
            return GetStrategy(fileNamingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IPackageNamingStrategy GetPackageNamingStrategy(string strategyName)
        {
            return GetStrategy(packageNamingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public ITypeNamingStrategy GetTypeNamingStrategy(string strategyName)
        {
            return GetStrategy(typeNamingStrategies, strategyName);
        }

        #endregion IProtoNamingStrategiesProvider Implementation

        #region IProtoNamingStrategiesRegistry Implementation

        /// <inheritdoc/>
        public void RegisterFileNamingStrategy(string strategyName, IFileNamingStrategy strategy)
        {
            RegisterStrategy(fileNamingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterPackageNamingStrategy(string strategyName, IPackageNamingStrategy strategy)
        {
            RegisterStrategy(packageNamingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterTypeNamingStrategy(string strategyName, ITypeNamingStrategy strategy)
        {
            RegisterStrategy(typeNamingStrategies, strategyName, strategy);
        }

        #endregion IProtoNamingStrategiesRegistry Implementation
    }
}
