﻿using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;

namespace ProtoGenerationLib.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Replacer for enumerable types.
    /// </summary>
    internal class EnumerableTypeReplacer : ITypeReplacer
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
        public Type ReplaceType(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!CanReplaceType(type))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not an enumerable and can not be replaced by the {nameof(EnumerableTypeReplacer)}.");

            type.TryGetElementOfEnumerableType(out var elementType);

            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.NewTypeNamingStrategy);

            Type arrayType;
            if (elementType.IsArray)
            {
                // Create new type for the array type.
                var newArrayType = TypeCreator.CreateProtoArrayType(elementType, newTypeNamingStrategy.GetNewTypeName, elementType.GetArrayElementType().Namespace);
                arrayType = newArrayType.MakeArrayType();
            }
            else
            {
                // Make a single dimensional array
                // since the type is IEnumerable<NoneArrayType>.
                arrayType = elementType.MakeArrayType();
            }

            var newType = TypeCreator.CreateProtoArrayType(arrayType, newTypeNamingStrategy.GetNewTypeName, arrayType.GetArrayElementType().Namespace);
            return newType;
        }
    }
}
