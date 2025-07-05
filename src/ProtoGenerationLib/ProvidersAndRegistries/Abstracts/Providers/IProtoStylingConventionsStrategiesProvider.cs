using ProtoGenerationLib.Strategies.Abstracts;
using System;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider of styling conventions strategies.
    /// </summary>
    internal interface IProtoStylingConventionsStrategiesProvider
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

        /// <summary>
        /// Get the requested file path styling strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IFilePathStylingStrategy GetFilePathStylingStrategy(string strategyName);
    }
}
