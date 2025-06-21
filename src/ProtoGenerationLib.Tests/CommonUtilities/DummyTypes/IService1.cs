using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.CommonUtilities.DummyTypes
{
    internal interface IService1
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void IService1Method1();

        void IService1Method2();
    }
}
