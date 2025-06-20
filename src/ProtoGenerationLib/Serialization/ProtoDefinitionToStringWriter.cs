using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Constants;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoGenerationLib.Serialization
{
    /// <summary>
    /// String writer for <see cref="IProtoDefinition"/>s.
    /// </summary>
    public static class ProtoDefinitionToStringWriter
    {
        /// <summary>
        /// A constant representing the name of protobuf stream.
        /// </summary>
        private const string STREAM = "stream";

        /// <summary>
        /// Write the given <paramref name="protoDefinition"/> to string.
        /// </summary>
        /// <param name="protoDefinition">The <see cref="IProtoDefinition"/> to write to string.</param>
        /// <param name="pathFromProtoRoot">
        /// The path from the proto root (The default of the proto root is the c# project
        /// directory in which the proto is compiled).
        /// <see href="https://chromium.googlesource.com/external/github.com/grpc/grpc/+/HEAD/src/csharp/BUILD-INTEGRATION.md">
        /// For more information see ProtoRoot in Protocol Buffers/gRPC Codegen Integration Into .NET Build.
        /// </see>
        /// </param>
        /// <param name="serializationOptions">The serialization options.</param>
        /// <returns>The string that represents the given <paramref name="protoDefinition"/>.</returns>
        /// <remarks>
        /// The <paramref name="pathFromProtoRoot"/> is very important because it effects
        /// all the imports in the protos.
        /// <b>Note that the directories in the <paramref name="pathFromProtoRoot"/> should be separated
        /// by a forward slash (/).</b>
        /// </remarks>
        public static string WriteToString(IProtoDefinition protoDefinition, string pathFromProtoRoot, ISerializationOptions serializationOptions)
        {
            var writer = new StringBuilder();
            uint indentLevel = 0;

            // Write syntax.
            writer.AppendLine($"syntax = \"{protoDefinition.Syntax}\";");

            // Write package.
            if (!string.IsNullOrEmpty(protoDefinition.Package))
            {
                writer.AppendLine();
                writer.AppendLine($"package {protoDefinition.Package};");
            }

            WriteImports(writer, protoDefinition.Imports, pathFromProtoRoot);

            if (protoDefinition.Services.Any())
                writer.AppendLine();

            WriteServices(writer, protoDefinition.Services, indentLevel, serializationOptions);

            if (protoDefinition.Messages.Any())
                writer.AppendLine();

            WriteMessages(writer, protoDefinition.Messages, indentLevel, serializationOptions);

            if (protoDefinition.Enums.Any())
                writer.AppendLine();

            WriteEnums(writer, protoDefinition.Enums, indentLevel, serializationOptions);

            return writer.ToString();
        }

        /// <summary>
        /// Write the given <paramref name="protoImports"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="protoImports"/> to.</param>
        /// <param name="protoImports">The imports to write.</param>
        /// <param name="pathFromProtoRoot">The path of the file from the proto root to the relative file location.</param>
        private static void WriteImports(StringBuilder writer, ISet<string> protoImports, string pathFromProtoRoot)
        {
            var imports = protoImports.ToList();
            imports.Remove(string.Empty);

            if (imports.Any())
                writer.AppendLine();

            var protoPathPrefixAddition = string.IsNullOrEmpty(pathFromProtoRoot) ? "" : $"{pathFromProtoRoot}/";
            var prefixedImports = new List<string>();
            foreach (var import in imports)
            {
                // Add the prefix to only generated types.
                if (!import.StartsWith(WellKnownTypesConstants.EXTERNAL_FILE_PATH_PREFIX))
                    prefixedImports.Add($"{protoPathPrefixAddition}{import}");
                else
                    // Write the import as is without the external file path prefix.
                    prefixedImports.Add($"{import.Substring(WellKnownTypesConstants.EXTERNAL_FILE_PATH_PREFIX.Length)}");
            }

            // We sort the imports in order to get the same result for the same imports
            // regardless of their order.
            prefixedImports.Sort();
            foreach (var import in prefixedImports)
            {
                writer.AppendLine($"import \"{import}\";");
            }
        }

        /// <summary>
        /// Write the given <paramref name="serviceDefinitions"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="serviceDefinitions"/> to.</param>
        /// <param name="serviceDefinitions">The service definitions to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="serviceDefinitions"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteServices(StringBuilder writer, IEnumerable<IServiceDefinition> serviceDefinitions, uint indentLevel, ISerializationOptions serializationOptions)
        {
            // We sort the services by name in order to get the same result for the same services
            // regardless of their order.
            var services = serviceDefinitions.ToList();
            services.Sort(ProtoObjectComparer);
            for (int i = 0; i < services.Count; i++)
            {
                var serviceDefinition = services[i];
                WriteServiceDefinition(writer, serviceDefinition, indentLevel, serializationOptions);

                // Add empty line between the services.
                if (i < services.Count - 1)
                    writer.AppendLine();
            }
        }

        /// <summary>
        /// Write the given <paramref name="serviceDefinition"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="serviceDefinition"/> to.</param>
        /// <param name="serviceDefinition">The service definition to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="serviceDefinition"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteServiceDefinition(StringBuilder writer, IServiceDefinition serviceDefinition, uint indentLevel, ISerializationOptions serializationOptions)
        {
            var indent = GetIndent(indentLevel, serializationOptions.IndentSize);

            // Write service declaration.
            writer.AppendLine($"{indent}service {serviceDefinition.Name} {{");

            // Write rpcs.
            // We sort the rpcs by name in order to get the same result for the same rpcs
            // regardless of their order.
            var rpcs = serviceDefinition.RpcMethods.ToList();
            rpcs.Sort((rpc1, rpc2) => rpc1.Name.CompareTo(rpc2.Name));
            for (int i = 0; i < rpcs.Count; i++)
            {
                var rpcDefinition = rpcs[i];
                WriteRpcDefinition(writer, rpcDefinition, indentLevel + 1, serializationOptions);

                // Add empty line between the rpcs.
                if (i < rpcs.Count - 1)
                    writer.AppendLine();
            }

            // Close service declaration.
            writer.AppendLine($"{indent}}}");
        }

        /// <summary>
        /// Write the given <paramref name="rpcDefinition"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="rpcDefinition"/> to.</param>
        /// <param name="rpcDefinition">The rpc definition to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="rpcDefinition"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteRpcDefinition(StringBuilder writer, IRpcDefinition rpcDefinition, uint indentLevel, ISerializationOptions serializationOptions)
        {
            var indent = GetIndent(indentLevel, serializationOptions.IndentSize);

            var rpcRequestType = ShouldRequestBeAStream(rpcDefinition.RpcType) ? $"{STREAM} {rpcDefinition.RequestType}" : $"{rpcDefinition.RequestType}";
            var rpcResponseType = ShouldResponseBeAStream(rpcDefinition.RpcType) ? $"{STREAM} {rpcDefinition.ResponseType}" : $"{rpcDefinition.ResponseType}";
            writer.AppendLine($"{indent}rpc {rpcDefinition.Name}({rpcRequestType}) returns ({rpcResponseType});");
        }

        /// <summary>
        /// Write the given <paramref name="messageDefinitions"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="messageDefinitions"/> to.</param>
        /// <param name="messageDefinitions">The message definitions to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="messageDefinitions"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteMessages(StringBuilder writer, IEnumerable<IMessageDefinition> messageDefinitions, uint indentLevel, ISerializationOptions serializationOptions)
        {
            // We sort the messages by name in order to get the same result for the same messages
            // regardless of their order.
            var messages = messageDefinitions.ToList();
            messages.Sort(ProtoObjectComparer);
            for (int i = 0; i < messages.Count; i++)
            {
                var messageDefinition = messages[i];
                WriteMessageDefinition(writer, messageDefinition, indentLevel, serializationOptions);

                // Add empty line between the messages.
                if (i < messages.Count - 1)
                    writer.AppendLine();
            }
        }

        /// <summary>
        /// Write the given <paramref name="messageDefinition"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="messageDefinition"/> to.</param>
        /// <param name="messageDefinition">The message definition to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="messageDefinition"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteMessageDefinition(StringBuilder writer, IMessageDefinition messageDefinition, uint indentLevel, ISerializationOptions serializationOptions)
        {
            var indent = GetIndent(indentLevel, serializationOptions.IndentSize);

            // Write message declaration.
            writer.AppendLine($"{indent}message {messageDefinition.Name} {{");

            // Write inner messages.
            WriteMessages(writer, messageDefinition.NestedMessages, indentLevel + 1, serializationOptions);

            if (messageDefinition.NestedMessages.Any() && messageDefinition.NestedEnums.Any())
                writer.AppendLine();

            // Write inner enums.
            WriteEnums(writer, messageDefinition.NestedEnums, indentLevel + 1, serializationOptions);

            if (messageDefinition.Fields.Any() && (messageDefinition.NestedMessages.Any() || messageDefinition.NestedEnums.Any()))
                writer.AppendLine();

            // Write fields.
            WriteFields(writer, messageDefinition.Fields, indentLevel + 1, serializationOptions);

            // Close message declaration.
            writer.AppendLine($"{indent}}}");
        }

        /// <summary>
        /// Write the given <paramref name="fieldDefinitions"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="fieldDefinitions"/> to.</param>
        /// <param name="fieldDefinitions">The field definitions to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="fieldDefinitions"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteFields(StringBuilder writer, IEnumerable<IFieldDefinition> fieldDefinitions, uint indentLevel, ISerializationOptions serializationOptions)
        {
            var indent = GetIndent(indentLevel, serializationOptions.IndentSize);

            var fields = fieldDefinitions.ToList();
            fields.Sort((field1, field2) => field1.Number.CompareTo(field2.Number));

            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                var rule = GetFieldRule(field.Rule);
                if (string.IsNullOrEmpty(rule))
                    writer.AppendLine($"{indent}{field.Type} {field.Name} = {field.Number};");
                else
                    writer.AppendLine($"{indent}{rule} {field.Type} {field.Name} = {field.Number};");

                if (i < fields.Count - 1)
                    writer.AppendLine();
            }
        }

        /// <summary>
        /// Write the given <paramref name="enumDefinitions"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="enumDefinitions"/> to.</param>
        /// <param name="enumDefinitions">The enum definitions to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="enumDefinitions"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteEnums(StringBuilder writer, IEnumerable<IEnumDefinition> enumDefinitions, uint indentLevel, ISerializationOptions serializationOptions)
        {
            // We sort the enums by name in order to get the same result for the same enums
            // regardless of their order.
            var enums = enumDefinitions.ToList();
            enums.Sort(ProtoObjectComparer);
            for (int i = 0; i < enums.Count; i++)
            {
                var enumDefinition = enums[i];
                WriteEnumDefinition(writer, enumDefinition, indentLevel, serializationOptions);

                // Add empty line between the enums.
                if (i < enums.Count - 1)
                    writer.AppendLine();
            }
        }

        /// <summary>
        /// Write the given <paramref name="enumDefinition"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="enumDefinition"/> to.</param>
        /// <param name="enumDefinition">The enum definition to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="enumDefinition"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteEnumDefinition(StringBuilder writer, IEnumDefinition enumDefinition, uint indentLevel, ISerializationOptions serializationOptions)
        {
            var indent = GetIndent(indentLevel, serializationOptions.IndentSize);

            // Write enum declaration.
            writer.AppendLine($"{indent}enum {enumDefinition.Name} {{");

            // Write enum values.
            // We sort the enum values by value in order to get the same result for the same enums
            // regardless of their order.
            var enumValues = enumDefinition.Values.ToList();
            enumValues.Sort((enumValue1, enumValue2) =>
            {
                var compareResult = enumValue1.Value.CompareTo(enumValue2.Value);
                if (compareResult == 0)
                    return 0;

                // Zero value must be the first value.
                if (enumValue1.Value == 0)
                    return int.MinValue;

                // Zero value must be the first value.
                if (enumValue2.Value == 0)
                    return int.MaxValue;

                return compareResult;
            });
            for (int i = 0; i < enumValues.Count; i++)
            {
                var enumValue = enumValues[i];
                WriteEnumValueDefinition(writer, enumValue, indentLevel + 1, serializationOptions);

                // Add empty line between the enum values.
                if (i < enumValues.Count - 1)
                    writer.AppendLine();
            }

            // Close enum declaration.
            writer.AppendLine($"{indent}}}");
        }

        /// <summary>
        /// Write the given <paramref name="enumValue"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write the <paramref name="enumValue"/> to.</param>
        /// <param name="enumValue">The enum value definition to write.</param>
        /// <param name="indentLevel">The indentation level at which to write the <paramref name="enumValue"/>.</param>
        /// <param name="serializationOptions">The serialization options.</param>
        private static void WriteEnumValueDefinition(StringBuilder writer, IEnumValueDefinition enumValue, uint indentLevel, ISerializationOptions serializationOptions)
        {
            var indent = GetIndent(indentLevel, serializationOptions.IndentSize);

            writer.AppendLine($"{indent}{enumValue.Name} = {enumValue.Value};");
        }

        /// <summary>
        /// Get the indentation of the given <paramref name="indentLevel"/>
        /// when using the given <paramref name="indentSize"/>.
        /// </summary>
        /// <param name="indentLevel">The level of indentation e.g. number of indentations (tabs).</param>
        /// <param name="indentSize">The size of each indentation in spaces.</param>
        /// <returns>
        /// The indentation of the given <paramref name="indentLevel"/>
        /// when using the given <paramref name="indentSize"/>.
        /// </returns>
        private static string GetIndent(uint indentLevel, uint indentSize)
        {
            return new string(' ', Convert.ToInt32(indentLevel * indentSize));
        }

        /// <summary>
        /// Comparer for <see cref="IProtoObject"/> that
        /// compare <see cref="IProtoObject"/> by their <see cref="IProtoObject.Name"/>.
        /// </summary>
        /// <param name="protoObject1">The first object in the comparison.</param>
        /// <param name="protoObject2">The second object in the comparison.</param>
        /// <returns>
        /// <b>0</b> if the given <see cref="IProtoObject"/>s have the same <see cref="IProtoObject.Name"/>.<br/>
        /// <b>Positive number</b> when the <paramref name="protoObject1"/> name should become after the <paramref name="protoObject2"/> name
        /// in alphabetic order.<br/>
        /// <b>Negative number</b> when the <paramref name="protoObject1"/> name should become before the <paramref name="protoObject2"/> name
        /// in alphabetic order.<br/>
        /// </returns>
        private static int ProtoObjectComparer(IProtoObject protoObject1, IProtoObject protoObject2)
        {
            return protoObject1.Name.CompareTo(protoObject2.Name);
        }

        /// <summary>
        /// Checks whether or not the given <paramref name="rpcType"/> means
        /// that the rpc request should be a stream.
        /// </summary>
        /// <param name="rpcType">The rpc type.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="rpcType"/> means that the
        /// rpc request should be a stream otherwise <see langword="false"/>.
        /// </returns>
        private static bool ShouldRequestBeAStream(ProtoRpcType rpcType)
        {
            return rpcType == ProtoRpcType.ClientStreaming || rpcType == ProtoRpcType.BidirectionalStreaming;
        }

        /// <summary>
        /// Checks whether or not the given <paramref name="rpcType"/> means
        /// that the rpc response should be a stream.
        /// </summary>
        /// <param name="rpcType">The rpc type.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="rpcType"/> means that the
        /// rpc response should be a stream otherwise <see langword="false"/>.
        /// </returns>
        private static bool ShouldResponseBeAStream(ProtoRpcType rpcType)
        {
            return rpcType == ProtoRpcType.ServerStreaming || rpcType == ProtoRpcType.BidirectionalStreaming;
        }

        /// <summary>
        /// Get the rule protobuf name.
        /// </summary>
        /// <param name="rule">The rule whose protobuf name is requested.</param>
        /// <returns>The protobuf name of the given <paramref name="rule"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="rule"/> is not recognized and not yet supported.
        /// </exception>
        private static string GetFieldRule(FieldRule rule)
        {
            return rule switch
            {
                FieldRule.None => "",
                FieldRule.Repeated => "repeated",
                FieldRule.Optional => "optional",
                _ => throw new ArgumentException($"The given {nameof(rule)}: {rule} is not supported.", nameof(rule)),
            };
        }
    }
}
