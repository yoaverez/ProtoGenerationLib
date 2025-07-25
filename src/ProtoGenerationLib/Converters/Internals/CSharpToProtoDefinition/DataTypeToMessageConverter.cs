﻿using ProtoGenerationLib.Converters.Internals.CSharpToIntermediate;
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
    /// Converter from csharp data type to its proto message representation.
    /// </summary>
    internal class DataTypeToMessageConverter : ICSharpToProtoTypeConverter<IMessageDefinition>
    {
        /// <summary>
        /// A converter from csharp data type to its intermediate representation.
        /// </summary>
        private ICSharpToIntermediateConverter<IDataTypeMetadata> csharpToIntermediateConverter;

        /// <summary>
        /// A converter from intermediate data type to its proto definition.
        /// </summary>
        private IIntermediateToProtoDefinitionConverter<IDataTypeMetadata, IMessageDefinition> intermediateToProtoConverter;

        /// <summary>
        /// Create new instance of the <see cref="DataTypeToMessageConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider">A provider of all the proto generator customizations.</param>
        /// <param name="csharpToIntermediateConverter"><inheritdoc cref="csharpToIntermediateConverter" path="/node()"/></param>
        /// <param name="intermediateToProtoConverter"><inheritdoc cref="intermediateToProtoConverter" path="/node()"/></param>
        public DataTypeToMessageConverter(IProvider componentsProvider,
                                          ICSharpToIntermediateConverter<IDataTypeMetadata>? csharpToIntermediateConverter = null,
                                          IIntermediateToProtoDefinitionConverter<IDataTypeMetadata, IMessageDefinition>? intermediateToProtoConverter = null)
        {
            this.csharpToIntermediateConverter = csharpToIntermediateConverter ?? new CSharpDataTypeToDataTypeMetadataConverter(componentsProvider);
            this.intermediateToProtoConverter = intermediateToProtoConverter ?? new DataTypeMetadataToMessageDefinitionConverter(componentsProvider);
        }

        /// <inheritdoc/>
        public IMessageDefinition ConvertTypeToProtoDefinition(Type type,
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
