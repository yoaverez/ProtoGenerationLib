using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Converters.Internals.DummyTypes
{
    [ProtoService]
    internal interface IContractType4
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void Method1(int a);
    }
}
