using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Customizations;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// Common utility method for the CSharpToIntermediate converters.
    /// </summary>
    internal static class CSharpToIntermediateUtils
    {
        /// <summary>
        /// Try converting the given type to its intermediate representation
        /// using the given <paramref name="customConverters"/>.
        /// </summary>
        /// <typeparam name="T">The type of the intermediate representation.</typeparam>
        /// <param name="type">The type to try to convert.</param>
        /// <param name="customConverters">The custom converters which may be able to convert the given <paramref name="type"/>.</param>
        /// <param name="intermediateRepresentation">The intermediate representation if could convert, otherwise the <see langword="default"/> of <typeparamref name="T"/>.</param>
        /// <returns>
        /// <see langword="true"/> if any of the given <paramref name="customConverters"/> could convert
        /// the given <paramref name="type"/> otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryConvertWithCustomConverters<T>(Type type,
                                                             IEnumerable<ICSharpToIntermediateCustomConverter<T>> customConverters,
                                                             out T intermediateRepresentation)
        {
            intermediateRepresentation = default;
            foreach (var customConverter in customConverters)
            {
                if (customConverter.CanHandle(type))
                {
                    intermediateRepresentation = customConverter.ConvertTypeToIntermediateRepresentation(type);
                    return true;
                }
            }
            return false;
        }
    }
}
