using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.DummyTypes
{
    [ProtoService]
    internal interface IDummyService3
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        DummyEnum1 Method1(DummyEnum1 b);
    }
}
