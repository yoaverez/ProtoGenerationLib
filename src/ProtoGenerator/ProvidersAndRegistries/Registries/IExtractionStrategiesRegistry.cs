using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.ProvidersAndRegistries.Registries
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
        void RegisterFieldsAndPropertiesExtractionStrategy(string strategyName, IFieldsAndPropertiesExtractionStrategy strategy);
    }
}
