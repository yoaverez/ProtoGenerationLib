using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerationLib.Strategies.Abstracts;
using System.Collections.Generic;
using static ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers.ContainersUtils;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
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
        /// A mapping between strategy name to its file path styling strategy.
        /// </summary>
        private Dictionary<string, IFilePathStylingStrategy> filePathStylingStrategies;

        /// <summary>
        /// Create new instance of the <see cref="ProtoStylingConventionsStrategiesContainer"/> class.
        /// </summary>
        public ProtoStylingConventionsStrategiesContainer()
        {
            stylingStrategies = new Dictionary<string, IProtoStylingStrategy>();
            packageStylingStrategies = new Dictionary<string, IPackageStylingStrategy>();
            filePathStylingStrategies = new Dictionary<string, IFilePathStylingStrategy>();
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

        /// <inheritdoc/>
        public IFilePathStylingStrategy GetFilePathStylingStrategy(string strategyName)
        {
            return GetStrategy(filePathStylingStrategies, strategyName);
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

        /// <inheritdoc/>
        public void RegisterFilePathStylingStrategy(string strategyName, IFilePathStylingStrategy strategy)
        {
            RegisterStrategy(filePathStylingStrategies, strategyName, strategy);
        }

        #endregion IProtoStylingConventionsStrategiesRegistry Implementation
    }
}
