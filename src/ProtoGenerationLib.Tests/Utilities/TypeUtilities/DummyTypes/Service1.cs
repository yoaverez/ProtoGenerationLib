using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Utilities.TypeUtilities.DummyTypes
{
    internal class Service1 : AbstractService
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public void Foo() { }

        public override void Method1()
        {
            throw new NotImplementedException();
        }
    }
}
