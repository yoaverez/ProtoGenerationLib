using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Replacers.Abstracts;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Replacer for array types.
    /// </summary>
    public class ArrayTypeReplacer : ITypeReplacer
    {
        /// <summary>
        /// A provider for new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="ArrayTypeReplacer"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public ArrayTypeReplacer(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public bool CanReplaceType(Type type)
        {
            return type.IsArray;
        }

        /// <inheritdoc/>
        public Type ReplaceType(Type type, IProtoGeneratorConfiguration generationOptions)
        {
            if (!CanReplaceType(type))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not an array and can not be replaced by the {nameof(ArrayTypeReplacer)}.");

            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(type);

            Type newType;
            if (type.IsSingleDimensionalArray())
            {
                var props = new List<(Type, string)> { (type, "items") };
                newType = TypeCreator.CreateDataType(newTypeName, props);
            }

            // Multi dimensional array or a jagged array.
            else
            {
                var elementType = type.GetArrayElementType();
                newType = TypeCreator.CreateArrayType(elementType, newTypeName);
            }

            return newType;
        }
    }
}
