using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Types extractor for enum types.
    /// </summary>
    public class EnumTypesExtractor : BaseTypesExtractor
    {
        /// <inheritdoc/>
        public override bool CanHandle(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            return type.IsEnum;
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            // Enum does not contains any types.
            return new HashSet<Type>();
        }
    }
}
