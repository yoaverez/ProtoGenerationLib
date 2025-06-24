using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors
{
    /// <summary>
    /// Extractor for extracting key and value types from key value mapping types
    /// like <see cref="Dictionary{TKey, TValue}"/>s
    /// and <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/>.
    /// </summary>
    internal class DictionaryElementTypesExtractor : BaseWrapperElementTypeExtractor
    {
        /// <inheritdoc/>
        public override bool CanHandle(Type type)
        {
            return type.IsKeyValuePairEnumerableType();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type)
        {
            type.TryGetElementsOfKeyValuePairEnumerableType(out var keyType, out var valueType);
            return new Type[] { keyType, valueType };
        }
    }
}
