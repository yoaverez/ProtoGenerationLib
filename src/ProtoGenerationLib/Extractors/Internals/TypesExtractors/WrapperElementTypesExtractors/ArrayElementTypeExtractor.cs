using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors
{
    /// <summary>
    /// Extractor for extracting the element type of an array.
    /// e.g. given the type that represent int[,] the extractor will return the type
    /// of int and the type of int[,].
    /// </summary>
    internal class ArrayElementTypeExtractor : BaseWrapperElementTypeExtractor
    {
        /// <inheritdoc/>
        public override bool CanHandle(Type type)
        {
            return type.IsArray;
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type)
        {
            var elementType = type.GetArrayElementType();
            var types = new HashSet<Type> { elementType };

            if (!type.IsSingleDimensionalArray())
            {
                // The given type is a multi dimensional array or jagged array
                // So we also need to create a special proto array type.
                types.Add(type);
            }
            return types;
        }
    }
}
