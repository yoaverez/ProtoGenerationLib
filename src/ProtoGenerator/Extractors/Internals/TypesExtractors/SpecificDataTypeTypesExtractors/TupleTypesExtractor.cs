using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.TypeUtilities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors
{
    /// <summary>
    /// Extractor of used types for tuple types.
    /// </summary>
    public class TupleTypesExtractor : BaseTypesExtractor
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
        /// Create new instance of the <see cref="TupleTypesExtractor"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public TupleTypesExtractor(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            return type.IsValueTuple() || type.IsTuple();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(typeExtractionOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(type);

            var props = GetItems(type);
            var newType = TypeCreator.CreateDataType(newTypeName, props);
            return new Type[] { newType }.Concat(props.Select(pair => pair.Type)).ToArray();
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
