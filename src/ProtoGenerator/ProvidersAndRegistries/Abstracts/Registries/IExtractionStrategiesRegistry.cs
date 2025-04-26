using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for extraction strategies.
    /// </summary>
    public interface IExtractionStrategiesRegistry
    {
        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the fields and properties extractions strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        void RegisterFieldsAndPropertiesExtractionStrategy(string strategyName, IFieldsAndPropertiesExtractionStrategy strategy);
    }
}
