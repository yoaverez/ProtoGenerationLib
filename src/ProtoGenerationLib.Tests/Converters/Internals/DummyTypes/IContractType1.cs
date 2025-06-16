using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Converters.Internals.DummyTypes
{
    [ProtoService]
    internal interface IContractType1
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public void Method1(int a);

        [ProtoRpc(ProtoRpcType.BidirectionalStreaming)]
        public double Method2(int a, bool b);

        public double Method3(int a);
    }
}
