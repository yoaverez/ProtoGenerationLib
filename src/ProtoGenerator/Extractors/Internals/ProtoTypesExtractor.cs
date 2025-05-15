using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Constants;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Extractors.Internals.TypesExtractors;
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
        /// A mapping between csharp well known types to proto well known types.
        /// </summary>
        private IDictionary<Type, string> wellKnownTypes;

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypesExtractor"/> class.
        /// </summary>
        /// <param name="componentsProvider">A provider of the proto generator customizations.</param>
        /// <param name="defaultTypesExtractors"><inheritdoc cref="defaultTypesExtractors" path="/node()"/></param>
        /// <param name="wellKnownTypes"><inheritdoc cref="wellKnownTypes" path="/node()"/></param>
        public ProtoTypesExtractor(IProvider componentsProvider,
                                   IEnumerable<ITypesExtractor>? defaultTypesExtractors = null,
                                   IDictionary<Type, string>? wellKnownTypes = null)
        {
            this.customConvertersProvider = componentsProvider;
            this.defaultTypesExtractors = defaultTypesExtractors ?? DefaultTypesExtractorsCreator.CreateStructuralTypesExtractors(componentsProvider);
            this.wellKnownTypes = wellKnownTypes ?? WellKnownTypesConstants.WellKnownTypes;
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
            // There is no reason to create a new proto type from a known types.
            if (wellKnownTypes.ContainsKey(type))
            {
                return new HashSet<Type>();
            }

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
