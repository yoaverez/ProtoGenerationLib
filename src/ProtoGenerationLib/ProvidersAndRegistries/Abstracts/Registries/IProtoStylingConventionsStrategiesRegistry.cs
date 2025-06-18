using ProtoGenerationLib.Strategies.Abstracts;
using System;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for proto styling conventions strategies.
    /// </summary>
    public interface IProtoStylingConventionsStrategiesRegistry
    {
        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the message styling strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        void RegisterProtoStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the package styling strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        void RegisterPackageStylingStrategy(string strategyName, IPackageStylingStrategy strategy);

        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the package styling strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        void RegisterFilePathStylingStrategy(string strategyName, IFilePathStylingStrategy strategy);
    }
}
