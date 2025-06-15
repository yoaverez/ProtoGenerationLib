using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Constants;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Converters.Internals;
using ProtoGenerator.Discovery.Abstracts;
using ProtoGenerator.Discovery.Internals;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Extractors.Internals;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerator.ProvidersAndRegistries.Internals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator
{
    /// <summary>
    /// Generator of <see cref="IProtoDefinition"/> from c# types.
    /// </summary>
    public class ProtoGenerator
    {
        /// <summary>
        /// Registry for the registry of user customizations.
        /// </summary>
        public IRegistry Registry { get; }

        /// <inheritdoc cref="IProtoTypesExtractor"/>
        private IProtoTypesExtractor protoTypesExtractor;

        /// <inheritdoc cref="IProtoTypeMetadataDiscoverer"/>
        private IProtoTypeMetadataDiscoverer protoTypeMetadataDiscoverer;

        /// <inheritdoc cref="ICSharpToProtoTypesConverter"/>
        private ICSharpToProtoTypesConverter csharpToProtoTypesConverter;

        /// <summary>
        /// A set of all the well known types in protobuf language.
        /// </summary>
        private ISet<Type> wellKnownTypes;

        /// <summary>
        /// Create new instance of the <see cref="ProtoGenerator"/> class.
        /// </summary>
        /// <param name="componentsProvider">A provider of all the proto generator customizations.</param>
        /// <param name="protoTypesExtractor"><inheritdoc cref="protoTypesExtractor" path="/node()"/></param>
        /// <param name="protoTypeMetadataDiscoverer"><inheritdoc cref="protoTypeMetadataDiscoverer" path="/node()"/></param>
        /// <param name="csharpToProtoTypesConverter"><inheritdoc cref="csharpToProtoTypesConverter" path="/node()"/></param>
        /// <param name="wellKnownTypes"><inheritdoc cref="wellKnownTypes" path="/node()"/></param>
        public ProtoGenerator(IProvider? componentsProvider = null,
                              IProtoTypesExtractor? protoTypesExtractor = null,
                              IProtoTypeMetadataDiscoverer? protoTypeMetadataDiscoverer = null,
                              ICSharpToProtoTypesConverter? csharpToProtoTypesConverter = null,
                              ISet<Type>? wellKnownTypes = null)
        {
            IProvider provider = componentsProvider ?? DefaultServicesContainer.Instance;
            Registry = DefaultServicesContainer.Instance;
            this.protoTypesExtractor = protoTypesExtractor ?? new ProtoTypesExtractor(provider);
            this.protoTypeMetadataDiscoverer = protoTypeMetadataDiscoverer ?? new ProtoTypeMetadataDiscoverer(provider);
            this.csharpToProtoTypesConverter = csharpToProtoTypesConverter ?? new CSharpToProtoConverter(provider);
            this.wellKnownTypes = wellKnownTypes ?? new HashSet<Type>(WellKnownTypesConstants.WellKnownTypes.Keys);
        }

        /// <summary>
        /// Generate <see cref="IProtoDefinition"/> for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to generate the protos from.</param>
        /// <param name="generationOptions">The generation options.</param>
        /// <returns>A mapping between file relative path to it proto file definition.</returns>
        public IDictionary<string, IProtoDefinition> GenerateProtos(Type type, IProtoGenerationOptions generationOptions)
        {
            return GenerateProtos(new Type[] { type }, generationOptions);
        }

        /// <summary>
        /// Generate <see cref="IProtoDefinition"/> for the given <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The types to generate the protos from.</param>
        /// <param name="generationOptions">The generation options.</param>
        /// <returns>A mapping between file relative path to it proto file definition.</returns>
        public IDictionary<string, IProtoDefinition> GenerateProtos(IEnumerable<Type> types, IProtoGenerationOptions generationOptions)
        {
            // Extract all the used c# types that are needed for the
            // proto generation.
            var usedTypes = protoTypesExtractor.ExtractProtoTypes(types, generationOptions, out var originTypeToNewTypeMapping);

            // Discover all the above used types proto metadata.
            var typesToMetadatas = protoTypeMetadataDiscoverer.DiscoverProtosMetadata(usedTypes, generationOptions);

            // Add the meta data for c# types that were replaced
            // with a new type in order to become proto type.
            AddOriginTypesMetadata(typesToMetadatas, originTypeToNewTypeMapping);

            // Remove the well known types from the used types
            // in order to get all the types that needs to be
            // created in protobuf.
            var neededProtoTypes = usedTypes.Where(type => !wellKnownTypes.Contains(type)).ToArray();

            // Convert all the needed proto types to proto file definitions.
            var fileRelativePathToProtoDefinitions = csharpToProtoTypesConverter.Convert(neededProtoTypes,
                                                                                         typesToMetadatas,
                                                                                         generationOptions);

            return fileRelativePathToProtoDefinitions;
        }

        /// <summary>
        /// Add the metadata of types that were replaced
        /// to the given <paramref name="protoTypesMetadatas"/>.
        /// </summary>
        /// <param name="protoTypesMetadatas">The mapping from type to its proto metadata.</param>
        /// <param name="originTypeToNewTypeMapping">The mapping from replaced types to their replacers.</param>
        /// <remarks>
        /// The metadata of types that were replaced is the
        /// metadata of the types that replace them.
        /// </remarks>
        private static void AddOriginTypesMetadata(IDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                   IReadOnlyDictionary<Type, Type> originTypeToNewTypeMapping)
        {
            foreach (var item in originTypeToNewTypeMapping)
            {
                var originType = item.Key;
                var newType = item.Value;
                protoTypesMetadatas[originType] = protoTypesMetadatas[newType];
            }
        }
    }
}
