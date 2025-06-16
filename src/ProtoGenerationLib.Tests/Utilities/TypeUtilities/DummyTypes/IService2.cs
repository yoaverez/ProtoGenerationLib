using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Utilities.TypeUtilities.DummyTypes
{
    internal interface IService2
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void IService2Method1();

        void IService3Method1();
    }
}
