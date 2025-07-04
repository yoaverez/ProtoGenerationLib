using ProtoGenerationLib.Converters.Internals.CSharpToIntermediate;
using ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition;
using System;
using System.Collections.Generic;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Converters.Abstracts;

namespace ProtoGenerationLib.Converters.Internals.CSharpToProtoDefinition
{
    /// <summary>
    /// Converter from csharp contract type to its proto service representation.
    /// </summary>
    internal class ContractTypeToServiceConverter : ICSharpToProtoTypeConverter<IServiceDefinition>
    {
        /// <summary>
        /// A converter from csharp contract type to its intermediate representation.
        /// </summary>
        private ICSharpToIntermediateConverter<IContractTypeMetadata> csharpToIntermediateConverter;

        /// <summary>
        /// A converter from intermediate contract type to its proto definition.
        /// </summary>
        private IIntermediateToProtoDefinitionConverter<IContractTypeMetadata, IServiceDefinition> intermediateToProtoConverter;

        /// <summary>
        /// Create new instance of the <see cref="ContractTypeToServiceConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider">A provider of all the proto generator customizations.</param>
        /// <param name="csharpToIntermediateConverter"><inheritdoc cref="csharpToIntermediateConverter" path="/node()"/></param>
        /// <param name="intermediateToProtoConverter"><inheritdoc cref="intermediateToProtoConverter" path="/node()"/></param>
        public ContractTypeToServiceConverter(IProvider componentsProvider,
                                              ICSharpToIntermediateConverter<IContractTypeMetadata>? csharpToIntermediateConverter = null,
                                              IIntermediateToProtoDefinitionConverter<IContractTypeMetadata, IServiceDefinition>? intermediateToProtoConverter = null)
        {
            this.csharpToIntermediateConverter = csharpToIntermediateConverter ?? new CSharpContractTypeToContractTypeMetadataConverter(componentsProvider);
            this.intermediateToProtoConverter = intermediateToProtoConverter ?? new ContractMetadataToServiceDefinitionConverter(componentsProvider);
        }

        /// <inheritdoc/>
        public IServiceDefinition ConvertTypeToProtoDefinition(Type type,
                                                               IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                               IProtoGenerationOptions generationOptions)
        {
            var intermediateRepresentation = csharpToIntermediateConverter.ConvertTypeToIntermediateRepresentation(type, generationOptions);
            var protoDefinition = intermediateToProtoConverter.ConvertIntermediateRepresentationToProtoDefinition(intermediateRepresentation,
                                                                                                                  protoTypesMetadatas,
                                                                                                                  generationOptions);
            return protoDefinition;
        }
    }
}
