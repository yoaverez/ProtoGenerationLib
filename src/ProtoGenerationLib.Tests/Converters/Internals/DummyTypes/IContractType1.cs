using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Converters.Internals.DummyTypes
{
    [ProtoService]
    internal interface IContractType1
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void Method1(int a);

        [ProtoRpc(ProtoRpcType.BidirectionalStreaming)]
        double Method2(int a, bool b);

        double Method3(int a);

        double Method4(int a);
    }
}
