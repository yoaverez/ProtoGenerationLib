using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;

namespace ProtoGenerationLib.Tests.Discovery.Internals.DummyTypes
{
    internal class DummyIProtoTypeBaseMetadata : IProtoTypeBaseMetadata
    {
        public string? Name { get; set; }

        public string? Package { get; set; }

        public string? FilePath { get; set; }

        public bool ShouldCreateProtoType { get; set; }
    }
}
