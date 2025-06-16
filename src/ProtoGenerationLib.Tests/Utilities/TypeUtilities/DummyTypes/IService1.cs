using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Utilities.TypeUtilities.DummyTypes
{
    internal interface IService1
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        void IService1Method1();
    }
}
