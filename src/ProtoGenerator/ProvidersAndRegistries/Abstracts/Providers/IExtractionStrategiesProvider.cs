using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider of extraction strategies.
    /// </summary>
    public interface IExtractionStrategiesProvider
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
    }
}
