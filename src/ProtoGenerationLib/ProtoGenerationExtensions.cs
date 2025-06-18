using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Serialization;
using System.Collections.Generic;
using System.IO;

namespace ProtoGenerationLib
{
    /// <summary>
    /// Extension methods to use on the <see cref="ProtoGenerator"/> methods results.
    /// </summary>
    public static class ProtoGenerationExtensions
    {
        /// <summary>
        /// Write the <paramref name="protoDefinitions"/> to strings.
        /// </summary>
        /// <param name="protoDefinitions">A mapping between proto definition relative file path to the proto definitions to write.</param>
        /// <param name="pathFromProtoRoot">
        /// <inheritdoc cref="ProtoDefinitionToStringWriter.WriteToString(IProtoDefinition, string, ISerializationOptions)" path="/param[@name = 'pathFromProtoRoot']"/><br/>
        /// Default to <see cref="string.Empty"/> meaning that the proto files are generated inside
        /// the proto root directory.
        /// </param>
        /// <param name="serializationOptions">The serialization options. Default to null converted to <see cref="SerializationOptions.Default"/>.</param>
        /// <returns>
        /// A mapping between the proto files relative paths to their proto definition
        /// string representations.
        /// </returns>
        /// <inheritdoc cref="ProtoDefinitionToStringWriter.WriteToString(IProtoDefinition, string, ISerializationOptions)" path="/remarks"/>
        public static IDictionary<string, string> WriteToStrings(this IDictionary<string, IProtoDefinition> protoDefinitions, string pathFromProtoRoot = "", ISerializationOptions? serializationOptions = null)
        {
            serializationOptions ??= SerializationOptions.Default;
            var protoDefinitionAsStrings = new Dictionary<string, string>();
            foreach (var kvp in protoDefinitions)
            {
                var relativeFilePath = kvp.Key;
                var protoDefinition = kvp.Value;
                protoDefinitionAsStrings.Add(relativeFilePath, ProtoDefinitionToStringWriter.WriteToString(protoDefinition, pathFromProtoRoot, serializationOptions));
            }

            return protoDefinitionAsStrings;
        }

        /// <summary>
        /// Write the <paramref name="protoDefinitions"/> to files.
        /// </summary>
        /// <param name="protoDefinitions">A mapping between proto definition relative file path to the proto definitions to write.</param>
        /// <param name="protoRootPath">
        /// The default of the proto root is the c# project
        /// directory in which the proto is compiled).
        /// <see href="https://chromium.googlesource.com/external/github.com/grpc/grpc/+/HEAD/src/csharp/BUILD-INTEGRATION.md">
        /// For more information see ProtoRoot in Protocol Buffers/gRPC Codegen Integration Into .NET Build
        /// </see>
        /// </param>
        /// <param name="pathFromProtoRoot">
        /// The path from the given <paramref name="protoRootPath"/> to the directory in which
        /// you want to start writing the proto files to.
        /// Default to <see cref="string.Empty"/> meaning that the proto files are generated inside
        /// the <paramref name="protoRootPath"/> directory.
        /// </param>
        /// <param name="serializationOptions">The serialization options. Default to null converted to <see cref="SerializationOptions.Default"/>.</param>
        /// <inheritdoc cref="ProtoDefinitionToStringWriter.WriteToString(IProtoDefinition, string, ISerializationOptions)" path="/remarks"/>
        public static void WriteToFiles(this IDictionary<string, IProtoDefinition> protoDefinitions, string protoRootPath, string pathFromProtoRoot = "", ISerializationOptions? serializationOptions = null)
        {
            serializationOptions ??= SerializationOptions.Default;
            foreach (var kvp in protoDefinitions)
            {
                var relativeFilePath = kvp.Key;
                var protoDefinition = kvp.Value;

                var path = Path.Combine(protoRootPath, pathFromProtoRoot, relativeFilePath);

                // Create the path of directories if needed.
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                var protoDefinitionAsString = ProtoDefinitionToStringWriter.WriteToString(protoDefinition, pathFromProtoRoot, serializationOptions);
                File.WriteAllText(path, protoDefinitionAsString);
            }
        }
    }
}
