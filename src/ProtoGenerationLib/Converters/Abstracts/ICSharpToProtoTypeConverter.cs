using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Converters.Abstracts
{
    /// <summary>
    /// Converter between csharp type to the type's proto representation.
    /// </summary>
    /// <typeparam name="TProtoDefinition">The type of the proto representation object.</typeparam>
    internal interface ICSharpToProtoTypeConverter<TProtoDefinition>
    {
        /// <summary>
        /// Convert the given <paramref name="type"/> to it's proto definition.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <param name="protoTypesMetadatas">The proto metadata of all the types.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>The proto definition that represents the given <paramref name="type"/>.</returns>
        TProtoDefinition ConvertTypeToProtoDefinition(Type type,
                                                      IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                      IProtoGenerationOptions generationOptions);
    }
}
