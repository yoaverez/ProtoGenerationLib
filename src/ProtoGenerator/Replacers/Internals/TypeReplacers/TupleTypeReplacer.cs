using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Replacers.Abstracts;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Replacer for tuple types.
    /// </summary>
    public class TupleTypeReplacer : ITypeReplacer
    {
        /// <summary>
        /// A provider for new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// The base name of each item in a tuple.
        /// </summary>
        private const string BASE_ITEM_NAME = "item";

        /// <summary>
        /// Create new instance of the <see cref="TupleTypeReplacer"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public TupleTypeReplacer(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public bool CanReplaceType(Type type)
        {
            return type.IsValueTuple() || type.IsTuple();
        }

        /// <inheritdoc/>
        public Type ReplaceType(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!CanReplaceType(type))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not a tuple and can not be replaced by the {nameof(TupleTypeReplacer)}.");

            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(type);

            var props = GetItems(type);
            var newType = TypeCreator.CreateDataType(newTypeName, props);
            return newType;
        }

        /// <summary>
        /// Get all the items (types and names) of the given tuple <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the tuple.</param>
        /// <returns>
        /// All the items (types and names) of the given tuple <paramref name="type"/>.
        /// </returns>
        private IEnumerable<(Type Type, string Name)> GetItems(Type type)
        {
            var items = new List<(Type, string)>();
            var itemsTypes = type.GetGenericArguments();
            for (int i = 1; i <= itemsTypes.Length; i++)
            {
                items.Add((itemsTypes[i - 1], $"{BASE_ITEM_NAME}{i}"));
            }
            return items;
        }
    }
}
