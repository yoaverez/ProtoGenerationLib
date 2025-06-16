using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Utilities.TypeUtilities.DummyTypes
{
    internal interface IService3 : IService2, IService1
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void IService3Method1();

        void IService3Method2();
    }
}
