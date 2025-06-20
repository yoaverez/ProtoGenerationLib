using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using System;
using System.Collections.Generic;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;

namespace ProtoGenerationLib.Constants
{
    /// <summary>
    /// Contains all the well known types metadatas.
    /// </summary>
    public static class WellKnownTypesConstants
    {
        /// <summary>
        /// A mapping between a csharp type to its primitive of well known type metadata.
        /// </summary>
        public static IReadOnlyDictionary<Type, IProtoTypeMetadata> WellKnownTypes { get; }

        /// <summary>
        /// A mapping between a csharp type primitive type to its protobuf wrapper type metadata.
        /// </summary>
        /// <remarks>
        /// This exists because protobuf primitive types are not considered messages and
        /// therefore can not be passed as rpc parameter or return value.
        /// </remarks>
        public static IReadOnlyDictionary<Type, IProtoTypeMetadata> PrimitiveTypesWrappers { get; }

        /// <summary>
        /// A prefix that should be added to external protos file path in their proto metadata.
        /// </summary>
        /// <remarks>
        /// External protos can be google well known types or any proto that is not primitive or
        /// shouldn't be generated using this library like protos from another project.
        /// </remarks>
        public static string EXTERNAL_FILE_PATH_PREFIX = "~ExternalFilePathPrefix~";

        /// <summary>
        /// Constant holding the directory in which
        /// all the google well known types exists.
        /// </summary>
        private const string GOOGLE_PROTOBUF_DIR = "google/protobuf";

        /// <summary>
        /// Constant holding the package in which
        /// all the google well known types are declared.
        /// </summary>
        private const string GOOGLE_PROTOBUF_PACKAGE = "google.protobuf";

        /// <summary>
        /// Constant holding the proto file extension.
        /// </summary>
        private const string FILE_EXTENSION = "proto";

        /// <summary>
        /// Initialize the static members of the <see cref="WellKnownTypesConstants"/> class.
        /// </summary>
        static WellKnownTypesConstants()
        {
            WellKnownTypes = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(bool)] = CreatePrimitiveTypeMetadata("bool"),
                [typeof(byte)] = CreatePrimitiveTypeMetadata("uint32"),
                [typeof(sbyte)] = CreatePrimitiveTypeMetadata("int32"),
                [typeof(short)] = CreatePrimitiveTypeMetadata("int32"),
                [typeof(ushort)] = CreatePrimitiveTypeMetadata("uint32"),
                [typeof(int)] = CreatePrimitiveTypeMetadata("int32"),
                [typeof(uint)] = CreatePrimitiveTypeMetadata("uint32"),
                [typeof(long)] = CreatePrimitiveTypeMetadata("int64"),
                [typeof(ulong)] = CreatePrimitiveTypeMetadata("uint64"),
                [typeof(float)] = CreatePrimitiveTypeMetadata("float"),
                [typeof(double)] = CreatePrimitiveTypeMetadata("double"),
                [typeof(byte[])] = CreatePrimitiveTypeMetadata("bytes"),
                [typeof(char)] = CreatePrimitiveTypeMetadata("uint32"),
                [typeof(string)] = CreatePrimitiveTypeMetadata("string"),
                // Proto3 doesn't have a decimal type.
                [typeof(decimal)] = CreatePrimitiveTypeMetadata("string"),

                [typeof(void)] = CreateProtobufWellKnownTypeMetadata("Empty", "empty"),
                [typeof(object)] = CreateProtobufWellKnownTypeMetadata("Any", "any"),
                [typeof(DateTime)] = CreateProtobufWellKnownTypeMetadata("Timestamp", "timestamp"),
                [typeof(DateTimeOffset)] = CreateProtobufWellKnownTypeMetadata("Timestamp", "timestamp"),
                [typeof(TimeSpan)] = CreateProtobufWellKnownTypeMetadata("Duration", "duration"),
                [typeof(Guid)] = CreatePrimitiveTypeMetadata("string"),
            };

            PrimitiveTypesWrappers = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(bool)] = CreateProtobufWellKnownTypeMetadata("BoolValue", "wrappers"),
                [typeof(byte)] = CreateProtobufWellKnownTypeMetadata("UInt32Value", "wrappers"),
                [typeof(sbyte)] = CreateProtobufWellKnownTypeMetadata("Int32Value", "wrappers"),
                [typeof(short)] = CreateProtobufWellKnownTypeMetadata("Int32Value", "wrappers"),
                [typeof(ushort)] = CreateProtobufWellKnownTypeMetadata("UInt32Value", "wrappers"),
                [typeof(int)] = CreateProtobufWellKnownTypeMetadata("Int32Value", "wrappers"),
                [typeof(uint)] = CreateProtobufWellKnownTypeMetadata("UInt32Value", "wrappers"),
                [typeof(long)] = CreateProtobufWellKnownTypeMetadata("Int64Value", "wrappers"),
                [typeof(ulong)] = CreateProtobufWellKnownTypeMetadata("UInt64Value", "wrappers"),
                [typeof(float)] = CreateProtobufWellKnownTypeMetadata("FloatValue", "wrappers"),
                [typeof(double)] = CreateProtobufWellKnownTypeMetadata("DoubleValue", "wrappers"),
                [typeof(byte[])] = CreateProtobufWellKnownTypeMetadata("BytesValue", "wrappers"),
                [typeof(char)] = CreateProtobufWellKnownTypeMetadata("UInt32Value", "wrappers"),
                [typeof(string)] = CreateProtobufWellKnownTypeMetadata("StringValue", "wrappers"),
                // Proto3 doesn't have a decimal type.
                [typeof(decimal)] = CreateProtobufWellKnownTypeMetadata("StringValue", "wrappers"),
            };
        }

        /// <summary>
        /// Create metadata for protobuf primitive types.
        /// </summary>
        /// <param name="primitiveProtoName">The name of the protobuf primitive type.</param>
        /// <returns>The metadata of the primitive type.</returns>
        private static IProtoTypeMetadata CreatePrimitiveTypeMetadata(string primitiveProtoName)
        {
            return new ProtoTypeMetadata(primitiveProtoName, string.Empty, primitiveProtoName, string.Empty);
        }

        /// <summary>
        /// Create metadata for protobuf well known types.
        /// </summary>
        /// <param name="typeName">The name of the well known protobuf type.</param>
        /// <param name="fileName">The name of the file in which the well known protobuf type is declared.</param>
        /// <returns>The metadata of the well known type.</returns>
        private static IProtoTypeMetadata CreateProtobufWellKnownTypeMetadata(string typeName, string fileName)
        {
            return new ProtoTypeMetadata(typeName, GOOGLE_PROTOBUF_PACKAGE, $"{GOOGLE_PROTOBUF_PACKAGE}.{typeName}", $"{EXTERNAL_FILE_PATH_PREFIX}{GOOGLE_PROTOBUF_DIR}/{fileName}.{FILE_EXTENSION}");
        }
    }
}
