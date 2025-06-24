using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors
{
    /// <summary>
    /// Extractor for extracting the element type from enumerable types.
    /// </summary>
    internal class EnumerableElementTypeExtractor : BaseWrapperElementTypeExtractor
    {
        /// <inheritdoc/>
        public override bool CanHandle(Type type)
        {
            return type.IsEnumerableType();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type)
        {
            type.TryGetElementOfEnumerableType(out var elementType);
            return new Type[] { elementType };
        }
    }
}
