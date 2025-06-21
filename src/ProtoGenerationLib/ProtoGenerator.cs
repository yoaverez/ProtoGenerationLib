using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Converters.Internals;
using ProtoGenerationLib.Discovery.Abstracts;
using ProtoGenerationLib.Discovery.Internals;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Extractors.Internals;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerationLib.ProvidersAndRegistries.Internals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib
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
        /// Create new instance of the <see cref="ProtoGenerator"/> class.
        /// </summary>
        /// <param name="providerAndRegister">A provider and register of all the proto generator customizations.</param>
        /// <param name="protoTypesExtractor"><inheritdoc cref="protoTypesExtractor" path="/node()"/></param>
        /// <param name="protoTypeMetadataDiscoverer"><inheritdoc cref="protoTypeMetadataDiscoverer" path="/node()"/></param>
        /// <param name="csharpToProtoTypesConverter"><inheritdoc cref="csharpToProtoTypesConverter" path="/node()"/></param>
        public ProtoGenerator(IProviderAndRegister? providerAndRegister = null,
                              IProtoTypesExtractor? protoTypesExtractor = null,
                              IProtoTypeMetadataDiscoverer? protoTypeMetadataDiscoverer = null,
                              ICSharpToProtoTypesConverter? csharpToProtoTypesConverter = null)
        {
            providerAndRegister = providerAndRegister ?? DefaultServicesContainer.CreateDefaultServicesContainer();
            Registry = providerAndRegister;
            this.protoTypesExtractor = protoTypesExtractor ?? new ProtoTypesExtractor(providerAndRegister);
            this.protoTypeMetadataDiscoverer = protoTypeMetadataDiscoverer ?? new ProtoTypeMetadataDiscoverer(providerAndRegister);
            this.csharpToProtoTypesConverter = csharpToProtoTypesConverter ?? new CSharpToProtoConverter(providerAndRegister);
        }

        /// <summary>
        /// Generate <see cref="IProtoDefinition"/> for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to generate the protos from.</param>
        /// <param name="generationOptions">The generation options. Default to null will be converted to <see cref="ProtoGenerationOptions.Default"/>.</param>
        /// <returns>A mapping between file relative path to it proto file definition.</returns>
        public IDictionary<string, IProtoDefinition> GenerateProtos(Type type, IProtoGenerationOptions? generationOptions = null)
        {
            return GenerateProtos(new Type[] { type }, generationOptions ?? ProtoGenerationOptions.Default);
        }

        /// <summary>
        /// Generate <see cref="IProtoDefinition"/> for the given <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The types to generate the protos from.</param>
        /// <param name="generationOptions">The generation options. Default to null will be converted to <see cref="ProtoGenerationOptions.Default"/>.</param>
        /// <returns>A mapping between file relative path to it proto file definition.</returns>
        public IDictionary<string, IProtoDefinition> GenerateProtos(IEnumerable<Type> types, IProtoGenerationOptions? generationOptions = null)
        {
            generationOptions = generationOptions ?? ProtoGenerationOptions.Default;

            // Extract all the used c# types that are needed for the
            // proto generation.
            var usedTypes = protoTypesExtractor.ExtractProtoTypes(types, generationOptions, out var originTypeToNewTypeMapping);

            // Discover all the above used types proto metadata.
            var typesToMetadatas = protoTypeMetadataDiscoverer.DiscoverProtosMetadata(usedTypes, generationOptions);

            // Add the meta data for c# types that were replaced
            // with a new type in order to become proto type.
            AddOriginTypesMetadata(typesToMetadatas, originTypeToNewTypeMapping);

            // Take only the types whose proto type should
            // be generated.
            var neededProtoTypes = usedTypes.Where(type => typesToMetadatas[type].ShouldCreateProtoType).ToArray();

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
