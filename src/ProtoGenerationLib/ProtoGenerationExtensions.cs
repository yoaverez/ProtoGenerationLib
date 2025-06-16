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
        /// <param name="serializationOptions">The serialization options. Default to null converted to <see cref="SerializationOptions.Default"/>.</param>
        /// <returns>
        /// A mapping between the proto files relative paths to their proto definition
        /// string representations.
        /// </returns>
        public static IDictionary<string, string> WriteToStrings(this IDictionary<string, IProtoDefinition> protoDefinitions, ISerializationOptions? serializationOptions = null)
        {
            serializationOptions ??= SerializationOptions.Default;
            var protoDefinitionAsStrings = new Dictionary<string, string>();
            foreach (var kvp in protoDefinitions)
            {
                var relativeFilePath = kvp.Key;
                var protoDefinition = kvp.Value;
                protoDefinitionAsStrings.Add(relativeFilePath, ProtoDefinitionToStringWriter.WriteToString(protoDefinition, serializationOptions));
            }

            return protoDefinitionAsStrings;
        }

        /// <summary>
        /// Write the <paramref name="protoDefinitions"/> to files.
        /// </summary>
        /// <param name="protoDefinitions">A mapping between proto definition relative file path to the proto definitions to write.</param>
        /// <param name="baseDirectory">The base directory path in which to write the proto definitions.</param>
        /// <param name="serializationOptions">The serialization options. Default to null converted to <see cref="SerializationOptions.Default"/>.</param>
        public static void WriteToFiles(this IDictionary<string, IProtoDefinition> protoDefinitions, string baseDirectory, ISerializationOptions? serializationOptions = null)
        {
            serializationOptions ??= SerializationOptions.Default;
            foreach (var kvp in protoDefinitions)
            {
                var relativeFilePath = kvp.Key;
                var protoDefinition = kvp.Value;

                var path = Path.Combine(baseDirectory, relativeFilePath);

                // Create the path of directories if needed.
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                var protoDefinitionAsString = ProtoDefinitionToStringWriter.WriteToString(protoDefinition, serializationOptions);
                File.WriteAllText(path, protoDefinitionAsString);
            }
        }
    }
}
