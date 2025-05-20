using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.TypeUtilities;
using System.Collections.Generic;
using System;
using ProtoGenerator.Replacers.Abstracts;

namespace ProtoGenerator.Replacers.Internals.TypeReplacers
{
    /// <summary>
    /// Replacer for nullable types.
    /// </summary>
    public class NullableTypeReplacer : ITypeReplacer
    {
        /// <summary>
        /// A provider for new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="NullableTypeReplacer"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public NullableTypeReplacer(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public bool CanReplaceType(Type type)
        {
            return type.IsNullable();
        }

        /// <inheritdoc/>
        public Type ReplaceType(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            if (!CanReplaceType(type))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not nullable type and can not be replaced by the {nameof(NullableTypeReplacer)}.");

            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(typeExtractionOptions.NewTypeNamingStrategiesOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(type);

            var props = new List<(Type, string)> { (type, "value") };
            var newType = TypeCreator.CreateDataType(newTypeName, props);
            return newType;
        }
    }
}
