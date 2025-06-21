using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.DummyTypes
{
    [Obsolete]
    internal class DummyService2
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public int Method1(bool b) {  return 1; }

        public int Method2(string s) { return 1; }

        public void Method3(double d) { }
    }
}
