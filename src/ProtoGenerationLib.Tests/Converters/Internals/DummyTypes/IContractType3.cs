using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Converters.Internals.DummyTypes
{
    [ProtoService]
    internal interface IContractType3
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        Enum1 Method1(Enum1 a);
    }
}
