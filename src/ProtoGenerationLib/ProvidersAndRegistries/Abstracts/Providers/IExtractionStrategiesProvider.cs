using ProtoGenerationLib.Strategies.Abstracts;
using System;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider of extraction strategies.
    /// </summary>
    internal interface IExtractionStrategiesProvider
    {
        /// <summary>
        /// Get the requested fields and properties extraction strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IFieldsAndPropertiesExtractionStrategy GetFieldsAndPropertiesExtractionStrategy(string strategyName);

        /// <summary>
        /// Get the requested documentation extraction strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IDocumentationExtractionStrategy GetDocumentationExtractionStrategy(string strategyName);

        /// <summary>
        /// Get the requested method signature extraction strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IMethodSignatureExtractionStrategy GetMethodSignatureExtractionStrategy(string strategyName);
    }
}
