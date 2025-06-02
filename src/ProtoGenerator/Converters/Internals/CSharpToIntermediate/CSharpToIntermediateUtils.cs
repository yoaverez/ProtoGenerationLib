using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// Common utility method for the CSharpToIntermediate converters.
    /// </summary>
    public static class CSharpToIntermediateUtils
    {
        /// <summary>
        /// Try converting the given type to its intermediate representation
        /// using the given <paramref name="customConverters"/>.
        /// </summary>
        /// <typeparam name="T">The type of the intermediate representation.</typeparam>
        /// <param name="type">The type to try to convert.</param>
        /// <param name="customConverters">The custom converters which may be able to convert the given <paramref name="type"/>.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <param name="intermediateRepresentation">The intermediate representation if could convert, otherwise the <see langword="default"/> of <typeparamref name="T"/>.</param>
        /// <returns>
        /// <see langword="true"/> if any of the given <paramref name="customConverters"/> could convert
        /// the given <paramref name="type"/> otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryConvertWithCustomConverters<T>(Type type,
                                                             IEnumerable<ICSharpToIntermediateCustomConverter<T>> customConverters,
                                                             IProtoGenerationOptions generationOptions,
                                                             out T intermediateRepresentation)
        {
            intermediateRepresentation = default;
            foreach (var customConverter in customConverters)
            {
                if (customConverter.CanHandle(type, generationOptions))
                {
                    intermediateRepresentation = customConverter.ConvertTypeToIntermediateRepresentation(type, generationOptions);
                    return true;
                }
            }
            return false;
        }
    }
}
