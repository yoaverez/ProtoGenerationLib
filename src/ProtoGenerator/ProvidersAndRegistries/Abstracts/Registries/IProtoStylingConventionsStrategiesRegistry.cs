using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries
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
        void RegisterMessageStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the enum styling strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        void RegisterEnumStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the enum value styling strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        void RegisterEnumValueStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the service styling strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        void RegisterServiceStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the field styling strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        void RegisterFieldStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

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
        void RegisterPackageStylingStrategy(string strategyName, IProtoStylingStrategy strategy);
    }
}
