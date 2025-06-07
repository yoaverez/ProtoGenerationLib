using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Converters.Internals.CSharpToIntermediate;
using ProtoGenerator.Converters.Internals.IntermediateToProtoDefinition;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Converters.Internals.CSharpToProtoDefinition
{
    /// <summary>
    /// Converter from csharp contract type to its proto service representation.
    /// </summary>
    public class ContractTypeToServiceConverter : ICSharpToProtoTypeConverter<IServiceDefinition>
    {
        /// <summary>
        /// A converter from csharp contract type to its intermediate representation.
        /// </summary>
        ICSharpToIntermediateConverter<IContractTypeMetadata> csharpToIntermediateConverter;

        /// <summary>
        /// A converter from intermediate contract type to its proto definition.
        /// </summary>
        IIntermediateToProtoDefinitionConverter<IContractTypeMetadata, IServiceDefinition> intermediateToProtoConverter;

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
