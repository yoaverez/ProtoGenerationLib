namespace ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class TypeContainsFieldThatContainsOtherFieldType
    {
        public class NestedClass1
        {
            public NestedClass2 NestedClass2 { get; set; }
        }

        public class NestedClass2
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
        }

        public NestedClass1 Prop1 { get; set; }

        public NestedClass2 Prop2 { get; set; }
    }
}
