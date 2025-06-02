using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.ProtoDefinitions;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Constants
{
    public static class WellKnownTypesConstants
    {
        public static IReadOnlyDictionary<Type, IProtoTypeMetadata> WellKnownTypes;

        private const string GOOGLE_PROTOBUF_DIR = "google/protobuf";

        private const string GOOGLE_PROTOBUF_PACKAGE = "google.protobuf";

        private const string FILE_EXTENSION = "proto";

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
                [typeof(decimal)] = CreatePrimitiveTypeMetadata("double"),

                [typeof(void)] = CreateProtobufWellKnownTypeMetadata("Empty", "empty"),
                [typeof(object)] = CreateProtobufWellKnownTypeMetadata("Any", "any"),
                [typeof(DateTime)] = CreateProtobufWellKnownTypeMetadata("Timestamp", "timestamp"),
                [typeof(DateTimeOffset)] = CreateProtobufWellKnownTypeMetadata("Timestamp", "timestamp"),
                [typeof(TimeSpan)] = CreateProtobufWellKnownTypeMetadata("Duration", "duration"),
                [typeof(Guid)] = CreatePrimitiveTypeMetadata("string"),
            };
        }

        private static IProtoTypeMetadata CreatePrimitiveTypeMetadata(string primitiveProtoName)
        {
            return new ProtoTypeMetadata(primitiveProtoName, string.Empty, primitiveProtoName, string.Empty);
        }

        private static IProtoTypeMetadata CreateProtobufWellKnownTypeMetadata(string typeName, string fileName)
        {
            return new ProtoTypeMetadata(typeName, GOOGLE_PROTOBUF_PACKAGE, $"{GOOGLE_PROTOBUF_PACKAGE}.{typeName}", $"{GOOGLE_PROTOBUF_DIR}/{fileName}.{FILE_EXTENSION}");
        }
    }
}
