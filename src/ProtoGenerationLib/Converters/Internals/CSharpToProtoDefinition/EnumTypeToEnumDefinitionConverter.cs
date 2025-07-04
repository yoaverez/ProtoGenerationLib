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
    /// Converter from csharp enum type to its proto message representation.
    /// </summary>
    internal class EnumTypeToEnumDefinitionConverter : ICSharpToProtoTypeConverter<IEnumDefinition>
    {
        /// <summary>
        /// A converter from csharp enum type to its intermediate representation.
        /// </summary>
        private ICSharpToIntermediateConverter<IEnumTypeMetadata> csharpToIntermediateConverter;

        /// <summary>
        /// A converter from intermediate enum type to its proto definition.
        /// </summary>
        private IIntermediateToProtoDefinitionConverter<IEnumTypeMetadata, IEnumDefinition> intermediateToProtoConverter;

        /// <summary>
        /// Create new instance of the <see cref="EnumTypeToEnumDefinitionConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider">A provider of all the proto generator customizations.</param>
        /// <param name="csharpToIntermediateConverter"><inheritdoc cref="csharpToIntermediateConverter" path="/node()"/></param>
        /// <param name="intermediateToProtoConverter"><inheritdoc cref="intermediateToProtoConverter" path="/node()"/></param>
        public EnumTypeToEnumDefinitionConverter(IProvider componentsProvider,
                                                 ICSharpToIntermediateConverter<IEnumTypeMetadata>? csharpToIntermediateConverter = null,
                                                 IIntermediateToProtoDefinitionConverter<IEnumTypeMetadata, IEnumDefinition>? intermediateToProtoConverter = null)
        {
            this.csharpToIntermediateConverter = csharpToIntermediateConverter ?? new CSharpEnumTypeToEnumTypeMetadataConverter(componentsProvider);
            this.intermediateToProtoConverter = intermediateToProtoConverter ?? new EnumTypeMetadataToEnumDefinitionConverter(componentsProvider);
        }

        /// <inheritdoc/>
        public IEnumDefinition ConvertTypeToProtoDefinition(Type type,
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
