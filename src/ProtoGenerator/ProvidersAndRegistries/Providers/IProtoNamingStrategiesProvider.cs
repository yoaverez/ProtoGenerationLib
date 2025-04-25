using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.ProvidersAndRegistries.Providers
{
    /// <summary>
    /// Provider of proto naming strategies.
    /// </summary>
    public interface IProtoNamingStrategiesProvider
    {
        /// <summary>
        /// Get the requested type naming strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        ITypeNamingStrategy GetTypeNamingStrategy(string strategyName);

        /// <summary>
        /// Get the requested package naming strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IPackageNamingStrategy GetPackageNamingStrategy(string strategyName);

        /// <summary>
        /// Get the requested file naming strategy.
        /// </summary>
        /// <param name="strategyName">The name of the strategy.</param>
        /// <returns>The requested strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        IFileNamingStrategy GetFileNamingStrategy(string strategyName);
    }
}
