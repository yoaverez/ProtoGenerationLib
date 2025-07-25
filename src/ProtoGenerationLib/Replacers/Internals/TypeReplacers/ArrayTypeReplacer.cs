﻿using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;

namespace ProtoGenerationLib.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Replacer for array types.
    /// </summary>
    internal class ArrayTypeReplacer : ITypeReplacer
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
        public Type ReplaceType(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!CanReplaceType(type))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not an array and can not be replaced by the {nameof(ArrayTypeReplacer)}.");

            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.NewTypeNamingStrategy);

            var arrayElementType = type.GetArrayElementType();
            var newType = TypeCreator.CreateProtoArrayType(type, newTypeNamingStrategy.GetNewTypeName, arrayElementType.Namespace);

            return newType;
        }
    }
}
