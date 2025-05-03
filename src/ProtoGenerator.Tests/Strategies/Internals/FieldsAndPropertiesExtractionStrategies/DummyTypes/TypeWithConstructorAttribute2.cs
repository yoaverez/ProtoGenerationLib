using ProtoGenerator.Attributes;

namespace ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class TypeWithConstructorAttribute2 : ClassWithThings2
    {
        public int C { get; set; }

        [ProtoMessageConstructor]
        private TypeWithConstructorAttribute2(int a, bool b)
        {
        }
    }
}
