using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Replacers.Abstracts;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Replacer for dictionary types.
    /// </summary>
    public class DictionaryTypeReplacer : ITypeReplacer
    {
        /// <summary>
        /// A provider for new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="DictionaryTypeReplacer"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public DictionaryTypeReplacer(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public bool CanReplaceType(Type type)
        {
            return type.IsKeyValuePairEnumerableType();
        }

        /// <inheritdoc/>
        public Type ReplaceType(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!CanReplaceType(type))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not a dictionary type and can not be replaced by the {nameof(DictionaryTypeReplacer)}.");

            type.TryGetElementsOfKeyValuePairEnumerableType(out var keyType, out var valueType);
            var unifiedDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(unifiedDictionaryType);

            var props = new List<(Type, string)> { (unifiedDictionaryType, "mapping") };
            var newType = TypeCreator.CreateDataType(newTypeName, props);
            return newType;
        }
    }
}
