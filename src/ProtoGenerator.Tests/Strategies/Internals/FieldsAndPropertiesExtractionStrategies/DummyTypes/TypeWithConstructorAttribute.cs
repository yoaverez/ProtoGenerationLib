using ProtoGenerator.Attributes;

namespace ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class TypeWithConstructorAttribute : ClassWithThings2
    {
        public int C {  get; set; }

        [ProtoMessageConstructor]
        public TypeWithConstructorAttribute(int a, bool b)
        {
        }
    }
}
