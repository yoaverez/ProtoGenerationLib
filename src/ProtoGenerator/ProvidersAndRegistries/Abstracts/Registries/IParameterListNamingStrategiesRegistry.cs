using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for parameter list naming strategies.
    /// </summary>
    public interface IParameterListNamingStrategiesRegistry
    {
        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the field numbering strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        void RegisterParameterListNamingStrategy(string strategyName, IParameterListNamingStrategy strategy);
    }
}
