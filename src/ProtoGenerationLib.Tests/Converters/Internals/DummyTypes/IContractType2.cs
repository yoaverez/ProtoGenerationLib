using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Converters.Internals.DummyTypes
{
    internal interface IContractType2
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void Method1();

        [ProtoRpc(ProtoRpcType.ClientStreaming)]
        void Method2(int a);

        [ProtoRpc(ProtoRpcType.BidirectionalStreaming)]
        double Method3(int a, bool b);

        double Method4(int a, bool b);

        double Method5(int a, bool b);
    }
}
