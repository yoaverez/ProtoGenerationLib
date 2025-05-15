using ProtoGenerator.Attributes;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.DummyTypes
{
    [ProtoService]
    internal class DummyService1
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public void Method1(int prop1, (object, bool, string) prop2, IEnumerable<bool> prop3)
        {

        }

        [ProtoRpc(ProtoRpcType.ClientStreaming)]
        public int Method2(Type type)
        {
            return 0;
        }

        public int Method3(Type type)
        {
            return 0;
        }
    }
}
