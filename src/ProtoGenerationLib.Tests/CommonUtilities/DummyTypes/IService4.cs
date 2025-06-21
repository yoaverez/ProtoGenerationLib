using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.CommonUtilities.DummyTypes
{
    internal interface IService4 : IService3
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void IService4Method1();
    }
}
