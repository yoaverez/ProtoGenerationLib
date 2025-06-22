using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Replacer for tuple types.
    /// </summary>
    internal class TupleTypeReplacer : ITypeReplacer
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

            // Since tuple can contain multiple different types,
            // In order to not have a recursive imports in the protos,
            // the new tuple type will be assigned a unique namespace.
            var nameSpace = $"{TypeCreator.DEFAULT_NAMESPACE_NAME}.{newTypeName}Lib";
            var newType = TypeCreator.CreateDataType(newTypeName, props, nameSpace: nameSpace);
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
