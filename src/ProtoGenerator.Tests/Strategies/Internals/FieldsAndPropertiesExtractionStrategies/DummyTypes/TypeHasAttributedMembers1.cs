using ProtoGenerator.Attributes;

namespace ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    public class TypeHasAttributedMembers1
    {
        [OptionalDataMember]
        public int Prop1 { get; set; }

        [OptionalDataMember]
        public int field1;

        public int field2;
    }
}
