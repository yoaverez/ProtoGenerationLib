using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Creator of default types extractors.
    /// </summary>
    internal static class DefaultTypesExtractorsCreator
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
        /// Create the default types extractors for the extraction of used types
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
                new DefaultDataTypesExtractor(componentsProvider, FieldsTypesExtractor.Instance),
            };
        }

        /// <summary>
        /// Create the default types extractors for the extraction of elements
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
                // Note that the order of the extractors is important.
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
