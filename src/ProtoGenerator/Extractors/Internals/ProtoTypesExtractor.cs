using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Extractors.Internals
{
    /// <summary>
    /// Extract the types needed for the completeness of the proto types generation.
    /// </summary>
    public class ProtoTypesExtractor : IProtoTypesExtractor
    {
        /// <summary>
        /// A provider of custom converters.
        /// </summary>
        private ICustomConvertersProvider customConvertersProvider;

        /// <summary>
        /// The default types extractors to run after running the custom ones.
        /// </summary>
        private IEnumerable<ITypesExtractor> defaultTypesExtractors;

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypesExtractor"/> class.
        /// </summary>
        /// <param name="customConvertersProvider"><inheritdoc cref="customConvertersProvider" path="/node()"/></param>
        /// <param name="extractionStrategiesProvider">The provider of extraction strategies.</param>
        /// <param name="defaultTypesExtractors"><inheritdoc cref="defaultTypesExtractors" path="/node()"/></param>
        public ProtoTypesExtractor(ICustomConvertersProvider customConvertersProvider,
                                   IExtractionStrategiesProvider extractionStrategiesProvider,
                                   IEnumerable<ITypesExtractor> defaultTypesExtractors)
        {
            this.customConvertersProvider = customConvertersProvider;
            this.defaultTypesExtractors = defaultTypesExtractors;
        }

        /// <inheritdoc/>
        public IEnumerable<Type> ExtractProtoTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var customTypesExtractors = customConvertersProvider.GetCustomTypesExtractors();
            var typeExtractors = customTypesExtractors.Concat(defaultTypesExtractors).ToArray();

            return ExtractProtoTypes(type, typeExtractionOptions, typeExtractors, new HashSet<Type>());
        }

        /// <param name="typesExtractors">The types extractors.</param>
        /// <param name="alreadyCheckedTypes">Types that this method was called on (To prevent endless recursion).</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="type"/> can not be handled
        /// by any of the given <paramref name="typesExtractors"/>
        /// </exception>
        /// <inheritdoc cref="ExtractProtoTypes(Type, ITypeExtractionOptions)"/>
        private IEnumerable<Type> ExtractProtoTypes(Type type,
                                                    ITypeExtractionOptions typeExtractionOptions,
                                                    IEnumerable<ITypesExtractor> typesExtractors,
                                                    HashSet<Type> alreadyCheckedTypes)
        {
            var types = new HashSet<Type> { type };
            foreach (var typesExtractor in typesExtractors)
            {
                if (typesExtractor.CanHandle(type))
                {
                    var usedTypes = typesExtractor.ExtractUsedTypes(type, typeExtractionOptions);
                    alreadyCheckedTypes.Add(type);
                    foreach (var usedType in usedTypes)
                    {
                        if (!alreadyCheckedTypes.Contains(usedType))
                        {
                            alreadyCheckedTypes.Add(usedType);
                            types.AddRange(ExtractProtoTypes(usedType, typeExtractionOptions, typesExtractors, alreadyCheckedTypes));
                        }
                    }
                    return types;
                }
            }

            throw new ArgumentException($"There is no types extractor that could extract types from the given {type.FullName}", nameof(type));
        }
    }
}
