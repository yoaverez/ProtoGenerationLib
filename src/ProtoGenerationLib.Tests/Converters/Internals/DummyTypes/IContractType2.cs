using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Converters.Internals.DummyTypes
{
    [ProtoService]
    internal interface IContractType2
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public void Method1();

        [ProtoRpc(ProtoRpcType.ClientStreaming)]
        public void Method2(int a);

        [ProtoRpc(ProtoRpcType.BidirectionalStreaming)]
        public double Method3(int a, bool b);
    }
}
