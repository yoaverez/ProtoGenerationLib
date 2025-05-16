using ProtoGenerator.Configurations.Abstracts;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Extractors.Abstracts
{
    /// <summary>
    /// Abstract class for types extractors.
    /// </summary>
    public abstract class BaseTypesExtractor : ITypesExtractor
    {
        /// <inheritdoc/>
        public IEnumerable<Type> ExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            if (CanHandle(type, typeExtractionOptions))
            {
                return BaseExtractUsedTypes(type, typeExtractionOptions);
            }
            else
            {
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by the {GetType().Name} extractor.", nameof(type));
            }
        }

        /// <inheritdoc/>
        public abstract bool CanHandle(Type type, ITypeExtractionOptions typeExtractionOptions);

        /// <inheritdoc cref="ITypesExtractor.ExtractUsedTypes(Type, ITypeExtractionOptions)" path="/summary"/>
        /// <inheritdoc cref="ITypesExtractor.ExtractUsedTypes(Type, ITypeExtractionOptions)" path="/param"/>
        /// <remarks>
        /// Implementers can assume that this method is called only if the
        /// given <paramref name="type"/> can be handled by this extractor.
        /// </remarks>
        protected abstract IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions);
    }
}
