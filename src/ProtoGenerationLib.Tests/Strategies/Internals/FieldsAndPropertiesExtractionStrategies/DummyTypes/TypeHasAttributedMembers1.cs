using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class TypeHasAttributedMembers1
    {
        [OptionalDataMember]
        public int Prop1 { get; set; }

        [OptionalDataMember]
        public int field1;

        public int field2;
    }
}
