using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider for parameter list naming strategies.
    /// </summary>
    public interface IParameterListNamingStrategiesProvider
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
    }
}
