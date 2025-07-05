using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.DummyTypes
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

        [ProtoRpc(ProtoRpcType.ClientStreaming)]
        public int Method3()
        {
            return 0;
        }

        public int Method4(Type type)
        {
            return 0;
        }
    }
}
