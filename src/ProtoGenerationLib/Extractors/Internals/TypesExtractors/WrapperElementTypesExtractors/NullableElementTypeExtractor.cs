using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors
{
    /// <summary>
    /// Extractor for extracting the element type from a <see cref="Nullable{T}"/> type.
    /// </summary>
    internal class NullableElementTypeExtractor : BaseWrapperElementTypeExtractor
    {
        /// <inheritdoc/>
        public override bool CanHandle(Type type)
        {
            return type.IsNullable();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type)
        {
            type.TryGetElementOfNullableType(out var elementType);
            return new Type[] { elementType };
        }
    }
}
