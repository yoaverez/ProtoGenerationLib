using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for numbering strategies.
    /// </summary>
    public interface INumberingStrategiesRegistry
    {
        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the field numbering strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        void RegisterFieldNumberingStrategy(string strategyName, IFieldNumberingStrategy strategy);

        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the enum value numbering strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        void RegisterEnumValueNumberingStrategy(string strategyName, IEnumValueNumberingStrategy strategy);
    }
}
