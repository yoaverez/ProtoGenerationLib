using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Converters.Abstracts
{
    /// <summary>
    /// Converter between intermediate type to the type's proto representation.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate representation object.</typeparam>
    /// <typeparam name="TProtoDefinition">The type of the proto representation object.</typeparam>
    public interface IIntermediateToProtoDefinitionConverter<TIntermediate, TProtoDefinition>
    {
        /// <summary>
        /// Convert the given <paramref name="intermediateType"/> to it's proto definition.
        /// </summary>
        /// <param name="intermediateType">The intermediate type to convert.</param>
        /// <param name="protoTypesMetadatas">The proto metadata of all the types.</param>
        /// <param name="conversionOptions">The options for the conversion.</param>
        /// <returns>The proto definition that represents the given <paramref name="intermediateType"/>.</returns>
        TProtoDefinition ConvertIntermediateRepresentationToProtoDefinition(TIntermediate intermediateType,
                                                                            IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                                            IConversionOptions conversionOptions);
    }
}
