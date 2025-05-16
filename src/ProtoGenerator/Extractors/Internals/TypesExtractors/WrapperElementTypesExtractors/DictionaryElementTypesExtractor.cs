using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors
{
    /// <summary>
    /// Extractor for extracting key and value types from key value mapping types
    /// like <see cref="Dictionary{TKey, TValue}"/>s
    /// and <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    public class DictionaryElementTypesExtractor : BaseTypesExtractor
    {
        /// <inheritdoc/>
        public override bool CanHandle(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            return type.IsKeyValuePairEnumerableType();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            type.TryGetElementsOfKeyValuePairEnumerableType(out var keyType, out var valueType);
            return new Type[] { keyType, valueType };
        }
    }
}
