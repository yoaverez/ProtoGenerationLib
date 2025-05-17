using ProtoGenerator.Attributes;

namespace ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    public class TypeHasAttributedMembers2 : TypeHasAttributedMembers1
    {
        [OptionalDataMember]
        public int Prop2 { get; set; }

        [OptionalDataMember]
        public int field3;
    }
}
