using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Utilities.TypeUtilities.DummyTypes
{
    internal interface IService4 : IService3
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void IService4Method1();
    }
}
