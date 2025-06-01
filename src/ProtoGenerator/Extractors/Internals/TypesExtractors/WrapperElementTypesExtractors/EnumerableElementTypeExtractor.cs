using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors
{
    /// <summary>
    /// Extractor for extracting the element type from enumerable types.
    /// </summary>
    public class EnumerableElementTypeExtractor : BaseTypesExtractor
    {
        /// <inheritdoc/>
        public override bool CanHandle(Type type, IProtoGeneratorConfiguration generationOptions)
        {
            return type.IsEnumerableType();
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, IProtoGeneratorConfiguration generationOptions)
        {
            type.TryGetElementOfEnumerableType(out var elementType);
            return new Type[] { elementType };
        }
    }
}
