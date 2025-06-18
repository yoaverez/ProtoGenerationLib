using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Serialization;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using ProtoGenerationLib.Constants;

namespace ProtoGenerationLib.Tests.Serialization
{
    [TestClass]
    public class ProtoDefinitionToStringWriterTests
    {
        private static SerializationOptions serializationOptions;

        private ProtoDefinition protoDefinition;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            serializationOptions = new SerializationOptions(indentSize: 4);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            protoDefinition = new ProtoDefinition
            {
                Syntax = "SyntaxDummy",
                Package = "a.b.c",
                Imports = new HashSet<string> { "", "import1", "aImport" },
                Services = new List<IServiceDefinition>
                {
                    new ServiceDefinition
                    {
                        Name = "DService1",
                        Package = "z",
                        Imports = new HashSet<string> { "dummyImport" },
                        RpcMethods = new List<IRpcDefinition>
                        {
                            new RpcDefinition("DRpc", "ResponseType1", "RequestType1", ProtoRpcType.Unary),
                            new RpcDefinition("ARpc", "ResponseType2", "RequestType2", ProtoRpcType.ClientStreaming),
                            new RpcDefinition("BRpc", "ResponseType3", "RequestType3", ProtoRpcType.ServerStreaming),
                            new RpcDefinition("CRpc", "ResponseType4", "RequestType4", ProtoRpcType.BidirectionalStreaming),
                        }
                    },
                    new ServiceDefinition
                    {
                        Name = "BService1",
                        Package = "z",
                        Imports = new HashSet<string> { "dummyImport" },
                        RpcMethods = new List<IRpcDefinition>
                        {
                            new RpcDefinition("Rpc", "ResponseType", "RequestType", ProtoRpcType.Unary),
                        }
                    },
                },
                Messages = new List<IMessageDefinition>
                {
                    new MessageDefinition
                    {
                        Name = "DMessage",
                        Package = "z",
                        Imports = new HashSet<string> {"dummyImport2"},
                        Fields = new List<IFieldDefinition>
                        {
                            new FieldDefinition("DField", "DType", 4, FieldRule.None),
                            new FieldDefinition("CField", "CType", 2, FieldRule.Repeated),
                            new FieldDefinition("BField", "BType", 3, FieldRule.Optional),
                        },
                        NestedMessages = new List<IMessageDefinition>
                        {
                            new MessageDefinition
                            {
                                Name = "DInnerMessage",
                                Package = "z",
                                Imports = new HashSet<string> {"dummyImport2"},
                                Fields = new List<IFieldDefinition>
                                {
                                    new FieldDefinition("DInnerField", "DType", 4, FieldRule.None),
                                    new FieldDefinition("CInnerField", "CType", 2, FieldRule.Repeated),
                                    new FieldDefinition("BInnerField", "BType", 3, FieldRule.Optional),
                                },
                                NestedMessages = new List<IMessageDefinition>
                                {
                                    new MessageDefinition
                                    {
                                        Name = "DInnerInnerMessage",
                                        Package = "z",
                                        Imports = new HashSet<string> {"dummyImport2"},
                                        Fields = new List<IFieldDefinition>
                                        {
                                            new FieldDefinition("DInnerInnerField", "DType", 4, FieldRule.None),
                                            new FieldDefinition("CInnerInnerField", "CType", 2, FieldRule.Repeated),
                                            new FieldDefinition("BInnerInnerField", "BType", 3, FieldRule.Optional),
                                        },
                                    }
                                },
                                NestedEnums = new List<IEnumDefinition>
                                {
                                    new EnumDefinition
                                    {
                                        Name = "DInnerInnerEnum",
                                        Package = "z",
                                        Imports = new HashSet<string> { "dummyEnumImport" },
                                        Values = new List<IEnumValueDefinition>
                                        {
                                            new EnumValueDefinition("aInnerInner", -1),
                                            new EnumValueDefinition("bInnerInner", 1),
                                            new EnumValueDefinition("cInnerInner", 0),
                                        }
                                    }
                                }
                            }
                        },
                        NestedEnums = new List<IEnumDefinition>
                        {
                            new EnumDefinition
                            {
                                Name = "DInnerEnum",
                                Package = "z",
                                Imports = new HashSet<string> { "dummyEnumImport" },
                                Values = new List<IEnumValueDefinition>
                                {
                                    new EnumValueDefinition("aInner", 1),
                                    new EnumValueDefinition("bInner", -1),
                                    new EnumValueDefinition("cInner", 0),
                                }
                            }
                        }
                    },
                    new MessageDefinition
                    {
                        Name = "AMessage",
                        Package = "z",
                        Fields = new List<IFieldDefinition>
                        {
                            new FieldDefinition("DField2", "DType", 4, FieldRule.None),
                            new FieldDefinition("CField2", "CType", 2, FieldRule.Repeated),
                            new FieldDefinition("BField2", "BType", 3, FieldRule.Optional),
                        },
                    },
                },
                Enums = new List<IEnumDefinition>
                {
                    new EnumDefinition
                            {
                                Name = "DEnum",
                                Package = "z",
                                Imports = new HashSet<string> { "dummyEnumImport" },
                                Values = new List<IEnumValueDefinition>
                                {
                                    new EnumValueDefinition("a1", 1),
                                    new EnumValueDefinition("b1", -500),
                                    new EnumValueDefinition("c1", 0),
                                }
                            },
                    new EnumDefinition
                            {
                                Name = "BEnum",
                                Package = "z",
                                Imports = new HashSet<string> { "dummyEnumImport" },
                                Values = new List<IEnumValueDefinition>
                                {
                                    new EnumValueDefinition("a2", -101),
                                    new EnumValueDefinition("b2", 0),
                                    new EnumValueDefinition("c2", 400),
                                }
                            },
                },
            };
        }

        [TestMethod]
        public void WriteToString_MultipleComponents_StringIsCorrect()
        {
            // Arrange
            var expectedString =
@"syntax = ""SyntaxDummy"";

package a.b.c;

import ""aImport"";
import ""import1"";

service BService1 {
    rpc Rpc(RequestType) returns (ResponseType);
}

service DService1 {
    rpc ARpc(stream RequestType2) returns (ResponseType2);

    rpc BRpc(RequestType3) returns (stream ResponseType3);

    rpc CRpc(stream RequestType4) returns (stream ResponseType4);

    rpc DRpc(RequestType1) returns (ResponseType1);
}

message AMessage {
    repeated CType CField2 = 2;

    optional BType BField2 = 3;

    DType DField2 = 4;
}

message DMessage {
    message DInnerMessage {
        message DInnerInnerMessage {
            repeated CType CInnerInnerField = 2;

            optional BType BInnerInnerField = 3;

            DType DInnerInnerField = 4;
        }

        enum DInnerInnerEnum {
            cInnerInner = 0;

            aInnerInner = -1;

            bInnerInner = 1;
        }

        repeated CType CInnerField = 2;

        optional BType BInnerField = 3;

        DType DInnerField = 4;
    }

    enum DInnerEnum {
        cInner = 0;

        bInner = -1;

        aInner = 1;
    }

    repeated CType CField = 2;

    optional BType BField = 3;

    DType DField = 4;
}

enum BEnum {
    b2 = 0;

    a2 = -101;

    c2 = 400;
}

enum DEnum {
    c1 = 0;

    b1 = -500;

    a1 = 1;
}
";

            // Act
            var actualString = ProtoDefinitionToStringWriter.WriteToString(protoDefinition, "", serializationOptions);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void WriteToString_MultipleComponentsWithPathFromProtoRoot_StringIsCorrect()
        {
            // Arrange
            protoDefinition.Imports.Add($"{WellKnownTypesConstants.GOOGLE_PROTOBUF_DIR}/file");
            var pathFromProtoRoot = "path/from/proto_root";
            var expectedString =
@"syntax = ""SyntaxDummy"";

package a.b.c;

import ""google/protobuf/file"";
import ""path/from/proto_root/aImport"";
import ""path/from/proto_root/import1"";

service BService1 {
    rpc Rpc(RequestType) returns (ResponseType);
}

service DService1 {
    rpc ARpc(stream RequestType2) returns (ResponseType2);

    rpc BRpc(RequestType3) returns (stream ResponseType3);

    rpc CRpc(stream RequestType4) returns (stream ResponseType4);

    rpc DRpc(RequestType1) returns (ResponseType1);
}

message AMessage {
    repeated CType CField2 = 2;

    optional BType BField2 = 3;

    DType DField2 = 4;
}

message DMessage {
    message DInnerMessage {
        message DInnerInnerMessage {
            repeated CType CInnerInnerField = 2;

            optional BType BInnerInnerField = 3;

            DType DInnerInnerField = 4;
        }

        enum DInnerInnerEnum {
            cInnerInner = 0;

            aInnerInner = -1;

            bInnerInner = 1;
        }

        repeated CType CInnerField = 2;

        optional BType BInnerField = 3;

        DType DInnerField = 4;
    }

    enum DInnerEnum {
        cInner = 0;

        bInner = -1;

        aInner = 1;
    }

    repeated CType CField = 2;

    optional BType BField = 3;

    DType DField = 4;
}

enum BEnum {
    b2 = 0;

    a2 = -101;

    c2 = 400;
}

enum DEnum {
    c1 = 0;

    b1 = -500;

    a1 = 1;
}
";

            // Act
            var actualString = ProtoDefinitionToStringWriter.WriteToString(protoDefinition, pathFromProtoRoot, serializationOptions);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void WriteToString_MultipleComponentsWithNoImports_StringIsCorrect()
        {
            // Arrange
            protoDefinition.Imports.Clear();
            var pathFromProtoRoot = "";
            var expectedString =
@"syntax = ""SyntaxDummy"";

package a.b.c;

service BService1 {
    rpc Rpc(RequestType) returns (ResponseType);
}

service DService1 {
    rpc ARpc(stream RequestType2) returns (ResponseType2);

    rpc BRpc(RequestType3) returns (stream ResponseType3);

    rpc CRpc(stream RequestType4) returns (stream ResponseType4);

    rpc DRpc(RequestType1) returns (ResponseType1);
}

message AMessage {
    repeated CType CField2 = 2;

    optional BType BField2 = 3;

    DType DField2 = 4;
}

message DMessage {
    message DInnerMessage {
        message DInnerInnerMessage {
            repeated CType CInnerInnerField = 2;

            optional BType BInnerInnerField = 3;

            DType DInnerInnerField = 4;
        }

        enum DInnerInnerEnum {
            cInnerInner = 0;

            aInnerInner = -1;

            bInnerInner = 1;
        }

        repeated CType CInnerField = 2;

        optional BType BInnerField = 3;

        DType DInnerField = 4;
    }

    enum DInnerEnum {
        cInner = 0;

        bInner = -1;

        aInner = 1;
    }

    repeated CType CField = 2;

    optional BType BField = 3;

    DType DField = 4;
}

enum BEnum {
    b2 = 0;

    a2 = -101;

    c2 = 400;
}

enum DEnum {
    c1 = 0;

    b1 = -500;

    a1 = 1;
}
";

            // Act
            var actualString = ProtoDefinitionToStringWriter.WriteToString(protoDefinition, pathFromProtoRoot, serializationOptions);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
