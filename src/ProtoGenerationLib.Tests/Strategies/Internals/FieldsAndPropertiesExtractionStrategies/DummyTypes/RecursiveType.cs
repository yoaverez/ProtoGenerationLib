namespace ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class RecursiveType
    {
        public int Value { get; set; }

        public RecursiveType Next { get; set; }
    }
}
