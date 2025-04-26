using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerator.Strategies.Abstracts;
using System.Collections.Generic;
using static ProtoGenerator.ProvidersAndRegistries.Internals.Containers.ContainersUtils;

namespace ProtoGenerator.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// A container for extraction strategies.
    /// </summary>
    public class ExtractionStrategiesContainer : IExtractionStrategiesRegistry, IExtractionStrategiesProvider
    {
        /// <summary>
        /// A mapping between strategy name to its fields and properties extraction strategy.
        /// </summary>
        private Dictionary<string, IFieldsAndPropertiesExtractionStrategy> fieldsAndPropertiesStrategies;

        /// <summary>
        /// Create new instance of the <see cref="ExtractionStrategiesContainer"/> class.
        /// </summary>
        public ExtractionStrategiesContainer()
        {
            fieldsAndPropertiesStrategies = new Dictionary<string, IFieldsAndPropertiesExtractionStrategy>();
        }

        #region IExtractionStrategiesProvider Implementation

        /// <inheritdoc/>
        public IFieldsAndPropertiesExtractionStrategy GetFieldsAndPropertiesExtractionStrategy(string strategyName)
        {
            return GetStrategy(fieldsAndPropertiesStrategies, strategyName);
        }

        #endregion IExtractionStrategiesProvider Implementation

        #region IExtractionStrategiesRegistry Implementation

        /// <inheritdoc/>
        public void RegisterFieldsAndPropertiesExtractionStrategy(string strategyName, IFieldsAndPropertiesExtractionStrategy strategy)
        {
            RegisterStrategy(fieldsAndPropertiesStrategies, strategyName, strategy);
        }

        #endregion IExtractionStrategiesRegistry Implementation
    }
}
