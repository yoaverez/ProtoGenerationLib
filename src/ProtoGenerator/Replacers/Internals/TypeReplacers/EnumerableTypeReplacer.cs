using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.TypeUtilities;
using System.Collections.Generic;
using System;
using ProtoGenerator.Replacers.Abstracts;

namespace ProtoGenerator.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Replacer for enumerable types.
    /// </summary>
    public class EnumerableTypeReplacer : ITypeReplacer
    {
        /// <summary>
        /// A provider for new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="EnumerableTypeReplacer"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public EnumerableTypeReplacer(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public bool CanReplaceType(Type type)
        {
            return type.IsEnumerableType();
        }

        /// <inheritdoc/>
        public Type ReplaceType(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            if (!CanReplaceType(type))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not an enumerable and can not be replaced by the {nameof(EnumerableTypeReplacer)}.");

            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(typeExtractionOptions.NewTypeNamingStrategiesOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(type);

            var props = new List<(Type, string)> { (type, "items") };
            var newType = TypeCreator.CreateDataType(newTypeName, props);
            return newType;
        }
    }
}
