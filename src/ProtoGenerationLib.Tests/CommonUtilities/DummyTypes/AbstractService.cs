using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.CommonUtilities.DummyTypes
{
    [ProtoService]
    internal abstract class AbstractService
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public abstract void Method1();

        [ProtoRpc(ProtoRpcType.Unary)]
        public virtual void Method2() { }
    }
}
