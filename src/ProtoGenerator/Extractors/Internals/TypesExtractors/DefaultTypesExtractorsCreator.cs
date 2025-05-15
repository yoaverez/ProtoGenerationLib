using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Extractors.Internals.TypesExtractors.SpecificDataTypeTypesExtractors;
using ProtoGenerator.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using System.Collections.Generic;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Creator of default types extractors.
    /// </summary>
    public static class DefaultTypesExtractorsCreator
    {
        /// <summary>
        /// Create the default types extractors to for the extraction of
        /// different structures such as data-type, enum and contract-type.
        /// </summary>
        /// <param name="componentsProvider">The components provider.</param>
        /// <returns>
        /// The default types extractors to for the extraction of
        /// different structures such as data-type, enum and contract-type.
        /// </returns>
        public static IEnumerable<ITypesExtractor> CreateStructuralTypesExtractors(IProvider componentsProvider)
        {
            return new ITypesExtractor[]
            {
                new EnumTypesExtractor(),
                new ContractTypesExtractor(componentsProvider),
                new DataTypesExtractor(componentsProvider),
            };
        }

        /// <summary>
        /// Create the default types extractors to for the extraction of used types
        /// in a data-type type.
        /// </summary>
        /// <param name="componentsProvider">The components provider.</param>
        /// <returns>
        /// The default types extractors to for the extraction of used types
        /// in a data-type type.
        /// </returns>
        public static IEnumerable<ITypesExtractor> CreateDefaultDataTypeTypesExtractors(IProvider componentsProvider)
        {
            return new ITypesExtractor[]
            {
                // Not that the order of the extractors is important.
                // Specific type extractors should be located before
                // less specific type extractors.
                // e.g Dictionary extractor should be located before Enumerable extractor
                // since dictionary is also enumerable.
                new NullableTypesExtractor(componentsProvider),
                new TupleTypesExtractor(componentsProvider),
                new ArrayTypesExtractor(componentsProvider),
                new DictionaryTypesExtractor(componentsProvider),
                new EnumerableTypesExtractor(componentsProvider),
                new DefaultDataTypesExtractor(componentsProvider, CreateDefaultWrapperElementTypesExtractors()),
            };
        }

        /// <summary>
        /// Create the default types extractors to for the extraction of elements
        /// types of a wrapper type.
        /// </summary>
        /// <returns>
        /// The default types extractors to for the extraction of elements
        /// types of a wrapper type.
        /// </returns>
        public static IEnumerable<ITypesExtractor> CreateDefaultWrapperElementTypesExtractors()
        {
            return new ITypesExtractor[]
            {
                // Not that the order of the extractors is important.
                // Specific type extractors should be located before
                // less specific type extractors.
                // e.g Dictionary extractor should be located before Enumerable extractor
                // since dictionary is also enumerable.
                new NullableElementTypeExtractor(),
                new ArrayElementTypeExtractor(),
                new DictionaryElementTypesExtractor(),
                new EnumerableElementTypeExtractor(),
            };
        }
    }
}
