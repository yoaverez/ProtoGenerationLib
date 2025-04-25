using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.ProvidersAndRegistries.Providers
{
    /// <summary>
    /// Provider of styling conventions strategies.
    /// </summary>
    public interface IProtoStylingConventionsStrategiesProvider
    {
        /// <summary>
        /// Get the requested message styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IProtoStylingStrategy GetMessageStylingStrategy(string strategyName);

        /// <summary>
        /// Get the requested enum styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IProtoStylingStrategy GetEnumStylingStrategy(string strategyName);

        /// <summary>
        /// Get the requested enum value styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IProtoStylingStrategy GetEnumValueStylingStrategy(string strategyName);

        /// <summary>
        /// Get the requested service styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IProtoStylingStrategy GetServiceStylingStrategy(string strategyName);

        /// <summary>
        /// Get the requested field styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IProtoStylingStrategy GetFieldStylingStrategy(string strategyName);

        /// <summary>
        /// Get the requested package styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IProtoStylingStrategy GetPackageStylingStrategy(string strategyName);
    }
}
