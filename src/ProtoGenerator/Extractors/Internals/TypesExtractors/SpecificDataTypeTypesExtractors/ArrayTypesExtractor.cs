using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors
{
    /// <summary>
    /// Extractor of used types for array types.
    /// </summary>
    public class ArrayTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// A provider for new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="ArrayTypesExtractor"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public ArrayTypesExtractor(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type)
        {
            return type.IsArray;
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(typeExtractionOptions.NewTypeNamingStrategy);
            var newTypeName = newTypeNamingStrategy.GetNewTypeName(type);

            var elementType = type.GetArrayElementType();
            Type newType;
            if (type.IsSingleDimensionalArray())
            {
                var props = new List<(Type, string)> { (type, "items") };
                newType = TypeCreator.CreateDataType(newTypeName, props);
            }

            // Multi dimensional array or a jagged array.
            else
            {
                newType = TypeCreator.CreateArrayType(elementType, newTypeName);
            }

            return new Type[] { newType, elementType };
        }
    }
}
