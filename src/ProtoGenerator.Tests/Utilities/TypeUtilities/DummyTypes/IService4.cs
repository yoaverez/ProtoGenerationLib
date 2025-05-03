using ProtoGenerator.Attributes;

namespace ProtoGenerator.Tests.Utilities.TypeUtilities.DummyTypes
{
    internal interface IService4 : IService3
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void IService4Method1();
    }
}
