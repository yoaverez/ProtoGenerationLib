using ProtoGenerationLib.Replacers.Internals.TypeReplacers;
using System.Collections.Generic;
using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;

namespace ProtoGenerationLib.Replacers.Internals
{
    /// <summary>
    /// Creator for the default type replacers in the proto generation process.
    /// </summary>
    internal static class DefaultTypeReplacersCreator
    {
        /// <summary>
        /// Create the default type replacers for the replacement of
        /// types that can not be protos.
        /// </summary>
        /// <param name="componentsProvider">The components provider.</param>
        /// <returns>
        /// The default type replacers for the replacement of
        /// types that can not be protos.
        /// </returns>
        public static IEnumerable<ITypeReplacer> CreateDefaultTypeReplacers(IProvider componentsProvider)
        {
            return new ITypeReplacer[]
            {
                // Note that the order of the extractors is important.
                // Specific type extractors should be located before
                // less specific type extractors.
                // e.g Dictionary extractor should be located before Enumerable extractor
                // since dictionary is also enumerable.
                new NullableTypeReplacer(componentsProvider),
                new TupleTypeReplacer(componentsProvider),
                new ArrayTypeReplacer(componentsProvider),
                new DictionaryTypeReplacer(componentsProvider),
                new EnumerableTypeReplacer(componentsProvider),
            };
        }
    }
}
