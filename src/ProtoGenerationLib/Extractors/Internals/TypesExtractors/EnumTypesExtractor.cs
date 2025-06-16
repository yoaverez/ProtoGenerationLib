using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Types extractor for enum types.
    /// </summary>
    public class EnumTypesExtractor : BaseTypesExtractor
    {
        /// <inheritdoc/>
        public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
        {
            return type.IsEnum;
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            // Enum does not contains any types.
            return new HashSet<Type>();
        }
    }
}
