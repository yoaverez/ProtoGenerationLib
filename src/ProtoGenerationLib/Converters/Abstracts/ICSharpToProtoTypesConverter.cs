using System.Collections.Generic;
using System;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;

namespace ProtoGenerationLib.Converters.Abstracts
{
    /// <summary>
    /// Converter between csharp types to proto types.
    /// </summary>
    internal interface ICSharpToProtoTypesConverter
    {
        /// <summary>
        /// Convert the given csharp <paramref name="types"/> to proto files definitions.
        /// </summary>
        /// <param name="types">The csharp types to convert.</param>
        /// <param name="protoTypesMetadatas">The proto metadata of all the types.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>A mapping between the proto file relative path to its file definition.</returns>
        /// <exception cref="Exception">
        /// Thrown when there are at least two types in the same proto file
        /// which have different packages.
        /// </exception>
        IDictionary<string, IProtoDefinition> Convert(IEnumerable<Type> types,
                                                      IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                      IProtoGenerationOptions generationOptions);
    }
}
