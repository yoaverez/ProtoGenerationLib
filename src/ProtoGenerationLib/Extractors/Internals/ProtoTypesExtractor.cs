using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Constants;
using ProtoGenerationLib.Customizations;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Replacers.Internals;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Internals
{
    /// <summary>
    /// Extract the types needed for the completeness of the proto types generation.
    /// </summary>
    internal class ProtoTypesExtractor : IProtoTypesExtractor
    {
        /// <summary>
        /// The default types extractors to run after running the custom ones.
        /// </summary>
        private IEnumerable<ITypesExtractor> defaultTypesExtractors;

        /// <summary>
        /// The type replacers for replacing types that can be regular proto
        /// messages such as enumerables.
        /// </summary>
        private IEnumerable<ITypeReplacer> typeReplacers;

        /// <summary>
        /// A set of types that are proto well known types or primitives.
        /// </summary>
        private ISet<Type> wellKnownTypes;

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypesExtractor"/> class.
        /// </summary>
        /// <param name="componentsProvider">A provider of the proto generator customizations.</param>
        /// <param name="defaultTypesExtractors"><inheritdoc cref="defaultTypesExtractors" path="/node()"/></param>
        /// <param name="typeReplacers"><inheritdoc cref="typeReplacers" path="/node()"/></param>
        /// <param name="wellKnownTypes"><inheritdoc cref="wellKnownTypes" path="/node()"/></param>
        public ProtoTypesExtractor(IProvider componentsProvider,
                                   IEnumerable<ITypesExtractor>? defaultTypesExtractors = null,
                                   IEnumerable<ITypeReplacer>? typeReplacers = null,
                                   ISet<Type>? wellKnownTypes = null)
        {
            this.defaultTypesExtractors = defaultTypesExtractors ?? DefaultTypesExtractorsCreator.CreateStructuralTypesExtractors(componentsProvider);
            this.typeReplacers = typeReplacers ?? DefaultTypeReplacersCreator.CreateDefaultTypeReplacers(componentsProvider);
            this.wellKnownTypes = wellKnownTypes ?? WellKnownTypesConstants.WellKnownTypes.Keys.ToHashSet();
        }

        /// <inheritdoc/>
        public IEnumerable<Type> ExtractProtoTypes(IEnumerable<Type> types, IProtoGenerationOptions generationOptions, out IReadOnlyDictionary<Type, Type> originTypeToNewTypeMapping)
        {
            var alreadyCheckedTypes = new HashSet<Type>();
            var usedTypes = new HashSet<Type>();
            var originTypeToNewType = new Dictionary<Type, Type>();
            foreach (var type in types)
            {
                usedTypes.AddRange(ExtractProtoTypes(type, generationOptions, alreadyCheckedTypes, out var originTypeToNewTypeMappingResult));
                originTypeToNewType.AddRange(originTypeToNewTypeMappingResult);
            }

            originTypeToNewTypeMapping = originTypeToNewType;
            return usedTypes;
        }

        /// <inheritdoc/>
        public IEnumerable<Type> ExtractProtoTypes(Type type, IProtoGenerationOptions generationOptions, out IReadOnlyDictionary<Type, Type> originTypeToNewTypeMapping)
        {
            return ExtractProtoTypes(type, generationOptions, new HashSet<Type>(), out originTypeToNewTypeMapping);
        }

        /// <inheritdoc cref="IProtoTypesExtractor.ExtractProtoTypes(Type, IProtoGenerationOptions, out IReadOnlyDictionary{Type, Type})"/>
        private IEnumerable<Type> ExtractProtoTypes(Type type,
                                                    IProtoGenerationOptions generationOptions,
                                                    HashSet<Type> alreadyCheckedTypes,
                                                    out IReadOnlyDictionary<Type, Type> originTypeToNewTypeMapping)
        {
            var originTypeToNewTypeMappingDictionary = new Dictionary<Type, Type>();
            var protoTypes = ExtractProtoTypes(type, generationOptions, typeReplacers, alreadyCheckedTypes, ref originTypeToNewTypeMappingDictionary);

            originTypeToNewTypeMapping = originTypeToNewTypeMappingDictionary;
            return protoTypes;
        }

        /// <param name="alreadyCheckedTypes">Types that this method was called on (To prevent endless recursion).</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="type"/> can not be handled
        /// by any of the extractors both predefined and custom ones.
        /// </exception>
        /// <inheritdoc cref="ExtractProtoTypes(Type, IProtoGenerationOptions, out IReadOnlyDictionary{Type, Type})"/>
        private IEnumerable<Type> ExtractProtoTypes(Type type,
                                                    IProtoGenerationOptions generationOptions,
                                                    IEnumerable<ITypeReplacer> typeReplacers,
                                                    HashSet<Type> alreadyCheckedTypes,
                                                    ref Dictionary<Type, Type> originTypeToNewTypeMapping)
        {
            // There is no reason to create a new proto type from a known types.
            if (wellKnownTypes.Contains(type))
            {
                // There is no need to check well known types.
                alreadyCheckedTypes.Add(type);
                return new HashSet<Type> { type };
            }

            foreach (var typeReplacer in typeReplacers)
            {
                // Check if the type can be replaced,
                // if so then replace it.
                if (typeReplacer.CanReplaceType(type))
                {
                    var newType = typeReplacer.ReplaceType(type, generationOptions);
                    originTypeToNewTypeMapping[type] = newType;
                    type = newType;
                    break;
                }
            }

            var types = new HashSet<Type> { type };
            IEnumerable<Type> usedTypes;

            var customTypesExtractors = generationOptions.GetCustomTypesExtractors();
            if (!TryExtractUsedTypes(type, customTypesExtractors, out usedTypes))
            {
                // Try using predefined extractors.
                if(!TryExtractUsedTypes(type, defaultTypesExtractors, generationOptions, out usedTypes))
                {
                    throw new ArgumentException($"There is no types extractor that could extract types from the given {type.FullName}", nameof(type));
                }
            }

            alreadyCheckedTypes.Add(type);
            foreach (var usedType in usedTypes)
            {
                if (!alreadyCheckedTypes.Contains(usedType))
                {
                    alreadyCheckedTypes.Add(usedType);
                    types.AddRange(ExtractProtoTypes(usedType, generationOptions, typeReplacers, alreadyCheckedTypes, ref originTypeToNewTypeMapping));
                }
            }
            return types;
        }

        /// <summary>
        /// Try extracting the given <paramref name="type"/> used types
        /// using the given <paramref name="customTypesExtractors"/>.
        /// </summary>
        /// <param name="type">The types whose used types are requested.</param>
        /// <param name="customTypesExtractors">The custom types extractors to try.</param>
        /// <param name="usedTypes">
        /// The used types that the given <paramref name="type"/> is using if any
        /// of the given <paramref name="customTypesExtractors"/> can handle
        /// the given <paramref name="type"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if any of the given <paramref name="customTypesExtractors"/> can
        /// extract the used type of the given <paramref name="type"/> otherwise <see langword="false"/>.
        /// </returns>
        private bool TryExtractUsedTypes(Type type, IEnumerable<ICustomTypesExtractor> customTypesExtractors, out IEnumerable<Type> usedTypes)
        {
            foreach (var customTypeExtractor in customTypesExtractors)
            {
                if (customTypeExtractor.CanHandle(type))
                {
                    usedTypes = customTypeExtractor.ExtractUsedTypes(type);
                    return true;
                }
            }

            usedTypes = new List<Type>();
            return false;
        }

        /// <summary>
        /// Try extracting the given <paramref name="type"/> used types
        /// using the given <paramref name="typesExtractors"/>.
        /// </summary>
        /// <param name="type">The types whose used types are requested.</param>
        /// <param name="typesExtractors">The predefined types extractors to try.</param>
        /// <param name="generationOptions">The generation options.</param>
        /// <param name="usedTypes">
        /// The used types that the given <paramref name="type"/> is using if any
        /// of the given <paramref name="typesExtractors"/> can handle
        /// the given <paramref name="type"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if any of the given <paramref name="typesExtractors"/> can
        /// extract the used type of the given <paramref name="type"/> otherwise <see langword="false"/>.
        /// </returns>
        private bool TryExtractUsedTypes(Type type, IEnumerable<ITypesExtractor> typesExtractors, IProtoGenerationOptions generationOptions, out IEnumerable<Type> usedTypes)
        {
            foreach (var typeExtractor in typesExtractors)
            {
                if (typeExtractor.CanHandle(type, generationOptions))
                {
                    usedTypes = typeExtractor.ExtractUsedTypes(type, generationOptions);
                    return true;
                }
            }

            usedTypes = new List<Type>();
            return false;
        }
    }
}
