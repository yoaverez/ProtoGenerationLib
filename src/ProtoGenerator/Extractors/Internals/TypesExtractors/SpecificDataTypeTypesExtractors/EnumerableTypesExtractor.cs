using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.TypeUtilities;
using System.Collections.Generic;
using System;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors
{
    /// <summary>
    /// Extractor of used types for enumerable types.
    /// </summary>
    public class EnumerableTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// A provider for new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="EnumerableTypesExtractor"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public EnumerableTypesExtractor(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            return type.IsEnumerableType();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(typeExtractionOptions.NewTypeNamingStrategiesOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(type);

            type.TryGetElementOfEnumerableType(out var elementType);
            var props = new List<(Type, string)> { (type, "items") };
            var newType = TypeCreator.CreateDataType(newTypeName, props);
            return new Type[] { newType, elementType };
        }
    }
}
