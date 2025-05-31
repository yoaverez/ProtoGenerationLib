using ProtoGenerator.Models.Abstracts.ProtoDefinitions;

namespace ProtoGenerator.Tests.Discovery.Internals.DummyTypes
{
    internal class DummyIProtoTypeBaseMetadata : IProtoTypeBaseMetadata
    {
        public string? Name { get; set; }

        public string? Package { get; set; }

        public string? FilePath { get; set; }
    }
}
