using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.TypeUtilities;
using System.Collections.Generic;
using System;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors
{
    /// <summary>
    /// Extractor of used types for nullable types.
    /// </summary>
    public class NullableTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// A provider for new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="NullableTypesExtractor"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public NullableTypesExtractor(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type)
        {
            return type.IsNullable();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(typeExtractionOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(type);

            type.TryGetElementOfNullableType(out var elementType);
            var props = new List<(Type, string)> { (type, "value") };
            var newType = TypeCreator.CreateDataType(newTypeName, props);
            return new Type[] { newType, elementType };
        }
    }
}
