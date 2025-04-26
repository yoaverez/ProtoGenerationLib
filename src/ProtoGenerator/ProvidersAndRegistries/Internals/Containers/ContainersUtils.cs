using System;
using System.Collections.Generic;

namespace ProtoGenerator.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Utilities for containers classes.
    /// </summary>
    internal static class ContainersUtils
    {
        /// <summary>
        /// Get the strategy with the given <paramref name="strategyName"/>
        /// from the given <paramref name="strategies"/>.
        /// </summary>
        /// <typeparam name="TStrategy">The type of the strategy.</typeparam>
        /// <param name="strategies">The strategies to search in.</param>
        /// <param name="strategyName">The name of the wanted strategy.</param>
        /// <returns>
        /// The strategy with the given <paramref name="strategyName"/>
        /// from the given <paramref name="strategies"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if a strategy with the given <paramref name="strategyName"/>
        /// is not found.
        /// </exception>
        public static TStrategy GetStrategy<TStrategy>(IDictionary<string, TStrategy> strategies, string strategyName)
        {
            if (strategies.TryGetValue(strategyName, out var strategy))
            {
                return strategy;
            }
            else
            {
                throw new ArgumentException($"The given strategy: {strategyName} is not recognized.", nameof(strategyName));
            }
        }

        /// <summary>
        /// Register the given <paramref name="strategy"/> with the given
        /// <paramref name="strategyName"/> to the <paramref name="strategies"/>.
        /// </summary>
        /// <typeparam name="TStrategy">The type of the strategy.</typeparam>
        /// <param name="strategies">The current collection of strategies.</param>
        /// <param name="strategyName">The name of the new strategy.</param>
        /// <param name="strategy">The strategy to register.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already strategy with the given <paramref name="strategyName"/>.
        /// </exception>
        public static void RegisterStrategy<TStrategy>(IDictionary<string, TStrategy> strategies, string strategyName, TStrategy strategy)
        {
            if (strategies.ContainsKey(strategyName))
            {
                throw new ArgumentException($"There is already a strategy with the name {strategyName}.", nameof(strategyName));
            }
            else
            {
                strategies.Add(strategyName, strategy);
            }
        }
    }
}
