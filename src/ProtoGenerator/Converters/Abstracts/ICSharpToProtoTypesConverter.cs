using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using System.Collections.Generic;
using System;

namespace ProtoGenerator.Converters.Abstracts
{
    /// <summary>
    /// Converter between csharp types to proto types.
    /// </summary>
    public interface ICSharpToProtoTypesConverter
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
