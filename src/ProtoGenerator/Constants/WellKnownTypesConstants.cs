using System;
using System.Collections.Generic;

namespace ProtoGenerator.Constants
{
    public static class WellKnownTypesConstants
    {
        public static Dictionary<Type, string> WellKnownTypes;

        static WellKnownTypesConstants()
        {
            WellKnownTypes = new Dictionary<Type, string>
            {
                [typeof(void)] = "google.protobuf.Empty",
                [typeof(bool)] = "bool",
                [typeof(byte)] = "uint32",
                [typeof(sbyte)] = "int32",
                [typeof(short)] = "int32",
                [typeof(ushort)] = "uint32",
                [typeof(int)] = "int32",
                [typeof(uint)] = "uint32",
                [typeof(long)] = "int64",
                [typeof(ulong)] = "uint64",
                [typeof(float)] = "float",
                [typeof(double)] = "double",

                // Proto3 doesn't have a decimal type.
                [typeof(decimal)] = "double",
                [typeof(char)] = "uint32",
                [typeof(string)] = "string",
                [typeof(DateTime)] = "google.protobuf.Timestamp",
                [typeof(DateTimeOffset)] = "google.protobuf.Timestamp",
                [typeof(TimeSpan)] = "google.protobuf.Duration",
                [typeof(Guid)] = "string",
                [typeof(byte[])] = "bytes",
            };
        }
    }
}
