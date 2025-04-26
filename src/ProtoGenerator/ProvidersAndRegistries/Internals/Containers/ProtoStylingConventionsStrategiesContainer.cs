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
        /// A mapping between strategy name to its message styling strategy.
        /// </summary>
        private Dictionary<string, IProtoStylingStrategy> messageStylingStrategies;

        /// <summary>
        /// A mapping between strategy name to its enum styling strategy.
        /// </summary>
        private Dictionary<string, IProtoStylingStrategy> enumStylingStrategies;

        /// <summary>
        /// A mapping between strategy name to its enum value styling strategy.
        /// </summary>
        private Dictionary<string, IProtoStylingStrategy> enumValueStylingStrategies;

        /// <summary>
        /// A mapping between strategy name to its service styling strategy.
        /// </summary>
        private Dictionary<string, IProtoStylingStrategy> serviceStylingStrategies;

        /// <summary>
        /// A mapping between strategy name to its field styling strategy.
        /// </summary>
        private Dictionary<string, IProtoStylingStrategy> fieldStylingStrategies;

        /// <summary>
        /// A mapping between strategy name to its package styling strategy.
        /// </summary>
        private Dictionary<string, IProtoStylingStrategy> packageStylingStrategies;

        /// <summary>
        /// Create new instance of the <see cref="ProtoStylingConventionsStrategiesContainer"/> class.
        /// </summary>
        public ProtoStylingConventionsStrategiesContainer()
        {
            messageStylingStrategies = new Dictionary<string, IProtoStylingStrategy>();
            enumStylingStrategies = new Dictionary<string, IProtoStylingStrategy>();
            enumValueStylingStrategies = new Dictionary<string, IProtoStylingStrategy>();
            serviceStylingStrategies = new Dictionary<string, IProtoStylingStrategy>();
            fieldStylingStrategies = new Dictionary<string, IProtoStylingStrategy>();
            packageStylingStrategies = new Dictionary<string, IProtoStylingStrategy>();
        }

        #region IProtoStylingConventionsStrategiesProvider Implementation

        /// <inheritdoc/>
        public IProtoStylingStrategy GetEnumStylingStrategy(string strategyName)
        {
            return GetStrategy(enumStylingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IProtoStylingStrategy GetEnumValueStylingStrategy(string strategyName)
        {
            return GetStrategy(enumStylingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IProtoStylingStrategy GetFieldStylingStrategy(string strategyName)
        {
            return GetStrategy(enumStylingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IProtoStylingStrategy GetMessageStylingStrategy(string strategyName)
        {
            return GetStrategy(enumStylingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IProtoStylingStrategy GetPackageStylingStrategy(string strategyName)
        {
            return GetStrategy(enumStylingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IProtoStylingStrategy GetServiceStylingStrategy(string strategyName)
        {
            return GetStrategy(enumStylingStrategies, strategyName);
        }

        #endregion IProtoStylingConventionsStrategiesProvider Implementation

        #region IProtoStylingConventionsStrategiesRegistry Implementation

        /// <inheritdoc/>
        public void RegisterEnumStylingStrategy(string strategyName, IProtoStylingStrategy strategy)
        {
            RegisterStrategy(enumStylingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterEnumValueStylingStrategy(string strategyName, IProtoStylingStrategy strategy)
        {
            RegisterStrategy(enumValueStylingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterFieldStylingStrategy(string strategyName, IProtoStylingStrategy strategy)
        {
            RegisterStrategy(fieldStylingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterMessageStylingStrategy(string strategyName, IProtoStylingStrategy strategy)
        {
            RegisterStrategy(messageStylingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterPackageStylingStrategy(string strategyName, IProtoStylingStrategy strategy)
        {
            RegisterStrategy(packageStylingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterServiceStylingStrategy(string strategyName, IProtoStylingStrategy strategy)
        {
            RegisterStrategy(serviceStylingStrategies, strategyName, strategy);
        }

        #endregion IProtoStylingConventionsStrategiesRegistry Implementation
    }
}
