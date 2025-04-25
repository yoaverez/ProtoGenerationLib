using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider for numbering strategies.
    /// </summary>
    public interface INumberingStrategiesProvider
    {
        /// <summary>
        /// Get the requested field numbering strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IFieldNumberingStrategy GetFieldNumberingStrategy(string strategyName);

        /// <summary>
        /// Get the requested enum value numbering strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IEnumValueNumberingStrategy GetEnumValueNumberingStrategy(string strategyName);
    }
}
