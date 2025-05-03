namespace ProtoGenerator.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class ClassWithThings1 : I3
    {
        public static int PublicStaticProp {  get; set; }
        protected int _publicStaticProp;
        private int publicStaticProp;
        private int _PublicStaticProp;

        private static int PrivateStaticProp { get; set; }
        private int PrivateProp {  get; set; }

        public int publicField;
        protected int protectedField;
        private int privateField;

        #region I3 Implementation

        public string Str => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        #endregion I3 Implementation
    }
}
