using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for new type naming strategies.
    /// </summary>
    public interface INewTypeNamingStrategiesRegistry
    {
        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the parameter list naming strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        void RegisterParameterListNamingStrategy(string strategyName, IParameterListNamingStrategy strategy);

        /// <summary>
        /// Register the given <paramref name="strategy"/> with it's
        /// associated <paramref name="strategyName"/>
        /// to the new type naming strategies.
        /// </summary>
        /// <param name="strategyName">The name to associate with the <paramref name="strategy"/>.</param>
        /// <param name="strategy">The strategy to register.</param>
        void RegisterNewTypeNamingStrategy(string strategyName, INewTypeNamingStrategy strategy);
    }
}
