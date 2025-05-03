namespace ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class TypeContainsOnlyEmptyMembers
    {
        public interface I1
        {
            void Foo1();
        }

        public interface I2
        {
            I1 I1Prop { get; }
            void Foo2();
        }

        public class C1
        {
            public I2 I2Prop { get; set; }
        }

        public I1 I1Prop { get; }

        public C1 C1Prop { get; }
    }
}
