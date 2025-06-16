using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class TypeHasAttributedMembers2 : TypeHasAttributedMembers1
    {
        [OptionalDataMember]
        public int Prop2 { get; set; }

        [OptionalDataMember]
        public int field3;
    }
}
