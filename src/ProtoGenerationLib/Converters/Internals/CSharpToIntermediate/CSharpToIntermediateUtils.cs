using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Strategies.Abstracts;
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

        /// <summary>
        /// Try getting the documentation of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose documentation is requested.</param>
        /// <param name="documentationProvider">A provider for user defined documentation.</param>
        /// <param name="documentationExtractionStrategy">An extractor for csharp entities documentation.</param>
        /// <param name="documentation">The documentation if found.</param>
        /// <returns>
        /// <see langword="true"/> if the documentation of the given <paramref name="type"/>
        /// was found otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryGetTypeDocumentation(Type type, IDocumentationProvider documentationProvider , IDocumentationExtractionStrategy documentationExtractionStrategy, out string documentation)
        {
            if(!documentationProvider.TryGetTypeDocumentation(type, out documentation))
            {
                if(!documentationExtractionStrategy.TryGetTypeDocumentation(type, out documentation))
                {
                    documentation = string.Empty;
                    return false;
                }
            }

            return true;
        }
    }
}
