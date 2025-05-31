using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider of styling conventions strategies.
    /// </summary>
    public interface IProtoStylingConventionsStrategiesProvider
    {
        /// <summary>
        /// Get the requested styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IProtoStylingStrategy GetProtoStylingStrategy(string strategyName);

        /// <summary>
        /// Get the requested package styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IPackageStylingStrategy GetPackageStylingStrategy(string strategyName);
    }
}
