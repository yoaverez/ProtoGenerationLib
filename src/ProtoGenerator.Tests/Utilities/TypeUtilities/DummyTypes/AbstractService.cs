using ProtoGenerator.Attributes;

namespace ProtoGenerator.Tests.Utilities.TypeUtilities.DummyTypes
{
    internal abstract class AbstractService
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public abstract void Method1();

        [ProtoRpc(ProtoRpcType.Unary)]
        public virtual void Method2() { }
    }
}
