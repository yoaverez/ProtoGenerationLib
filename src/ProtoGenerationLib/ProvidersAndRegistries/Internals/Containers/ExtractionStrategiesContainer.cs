using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerationLib.Strategies.Abstracts;
using System.Collections.Generic;
using static ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers.ContainersUtils;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// A container for extraction strategies.
    /// </summary>
    internal class ExtractionStrategiesContainer : IExtractionStrategiesRegistry, IExtractionStrategiesProvider
    {
        /// <summary>
        /// A mapping between strategy name to its fields and properties extraction strategy.
        /// </summary>
        private Dictionary<string, IFieldsAndPropertiesExtractionStrategy> fieldsAndPropertiesStrategies;

        /// <summary>
        /// A mapping between strategy name to its documentation extraction strategy.
        /// </summary>
        private Dictionary<string, IDocumentationExtractionStrategy> documentationExtractionStrategies;

        /// <summary>
        /// A mapping between strategy name to its method signature extraction strategy.
        /// </summary>
        private Dictionary<string, IMethodSignatureExtractionStrategy> methodSignatureExtractionStrategies;

        /// <summary>
        /// Create new instance of the <see cref="ExtractionStrategiesContainer"/> class.
        /// </summary>
        public ExtractionStrategiesContainer()
        {
            fieldsAndPropertiesStrategies = new Dictionary<string, IFieldsAndPropertiesExtractionStrategy>();
            documentationExtractionStrategies = new Dictionary<string, IDocumentationExtractionStrategy>();
            methodSignatureExtractionStrategies = new Dictionary<string, IMethodSignatureExtractionStrategy>();
        }

        #region IExtractionStrategiesProvider Implementation

        /// <inheritdoc/>
        public IFieldsAndPropertiesExtractionStrategy GetFieldsAndPropertiesExtractionStrategy(string strategyName)
        {
            return GetStrategy(fieldsAndPropertiesStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IDocumentationExtractionStrategy GetDocumentationExtractionStrategy(string strategyName)
        {
            return GetStrategy(documentationExtractionStrategies, strategyName);
        }

        /// <inheritdoc/>
        public IMethodSignatureExtractionStrategy GetMethodSignatureExtractionStrategy(string strategyName)
        {
            return GetStrategy(methodSignatureExtractionStrategies, strategyName);
        }

        #endregion IExtractionStrategiesProvider Implementation

        #region IExtractionStrategiesRegistry Implementation

        /// <inheritdoc/>
        public void RegisterFieldsAndPropertiesExtractionStrategy(string strategyName, IFieldsAndPropertiesExtractionStrategy strategy)
        {
            RegisterStrategy(fieldsAndPropertiesStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterDocumentationExtractionStrategy(string strategyName, IDocumentationExtractionStrategy strategy)
        {
            RegisterStrategy(documentationExtractionStrategies, strategyName, strategy);
        }

        /// <inheritdoc/>
        public void RegisterMethodSignatureExtractionStrategy(string strategyName, IMethodSignatureExtractionStrategy strategy)
        {
            RegisterStrategy(methodSignatureExtractionStrategies, strategyName, strategy);
        }

        #endregion IExtractionStrategiesRegistry Implementation
    }
}
