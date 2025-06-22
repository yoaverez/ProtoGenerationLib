using ProtoGenerationLib.Strategies.Abstracts;
using System;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider for new type naming strategies.
    /// </summary>
    internal interface INewTypeNamingStrategiesProvider
    {
        /// <summary>
        /// Get the parameter list naming strategy with the given <paramref name="strategyName"/>.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IParameterListNamingStrategy GetParameterListNamingStrategy(string strategyName);

        /// <summary>
        /// Get the new type naming strategy with the given <paramref name="strategyName"/>.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        INewTypeNamingStrategy GetNewTypeNamingStrategy(string strategyName);
    }
}
