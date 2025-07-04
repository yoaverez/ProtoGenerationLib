using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Abstracts
{
    /// <summary>
    /// Abstract class for wrapper element type extractors.
    /// </summary>
    internal abstract class BaseWrapperElementTypeExtractor : IWrapperElementTypeExtractor
    {
        /// <inheritdoc/>
        public IEnumerable<Type> ExtractUsedTypes(Type type)
        {
            if (CanHandle(type))
            {
                return BaseExtractUsedTypes(type);
            }
            else
            {
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by the {GetType().Name} extractor.", nameof(type));
            }
        }

        /// <inheritdoc/>
        public abstract bool CanHandle(Type type);

        /// <inheritdoc cref="IWrapperElementTypeExtractor.ExtractUsedTypes(Type)" path="/summary"/>
        /// <inheritdoc cref="IWrapperElementTypeExtractor.ExtractUsedTypes(Type)" path="/param"/>
        /// <remarks>
        /// Implementers can assume that this method is called only if the
        /// given <paramref name="type"/> can be handled by this extractor.
        /// </remarks>
        protected abstract IEnumerable<Type> BaseExtractUsedTypes(Type type);
    }
}
