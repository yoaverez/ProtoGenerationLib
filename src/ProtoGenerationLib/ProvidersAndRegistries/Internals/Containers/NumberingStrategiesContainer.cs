using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerationLib.Strategies.Abstracts;
using System.Collections.Generic;
using static ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers.ContainersUtils;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for numbering strategies.
    /// </summary>
    public class NumberingStrategiesContainer : INumberingStrategiesRegistry, INumberingStrategiesProvider
    {
        /// <summary>
        /// A mapping between strategy name to its field numbering strategy.
        /// </summary>
        private Dictionary<string, IFieldNumberingStrategy> fieldNumberingStrategies;

        /// <summary>
        /// A mapping between strategy name to its enum value numbering strategy.
        /// </summary>
        private Dictionary<string, IEnumValueNumberingStrategy> enumValueNumberingStrategies;

        /// <summary>
        /// Create new instance of the <see cref="NumberingStrategiesContainer"/> class.
        /// </summary>
        public NumberingStrategiesContainer()
        {
            fieldNumberingStrategies = new Dictionary<string, IFieldNumberingStrategy>();
            enumValueNumberingStrategies = new Dictionary<string, IEnumValueNumberingStrategy>();
        }

        #region INumberingStrategiesProvider Implementation

        /// <inheritdoc/>
        public IEnumValueNumberingStrategy GetEnumValueNumberingStrategy(string strategyName)
        {
            return GetStrategy(enumValueNumberingStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IFieldNumberingStrategy GetFieldNumberingStrategy(string strategyName)
        {
            return GetStrategy(fieldNumberingStrategies, strategyName);
        }

        #endregion INumberingStrategiesProvider Implementation

        #region INumberingStrategiesRegistry Implementation

        /// <inheritdoc/>
        public void RegisterEnumValueNumberingStrategy(string strategyName, IEnumValueNumberingStrategy strategy)
        {
            RegisterStrategy(enumValueNumberingStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterFieldNumberingStrategy(string strategyName, IFieldNumberingStrategy strategy)
        {
            RegisterStrategy(fieldNumberingStrategies, strategyName, strategy);
        }

        #endregion INumberingStrategiesRegistry Implementation
    }
}
