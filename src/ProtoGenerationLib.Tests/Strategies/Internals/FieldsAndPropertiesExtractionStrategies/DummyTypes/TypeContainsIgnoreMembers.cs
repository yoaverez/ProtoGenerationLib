﻿using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.DummyTypes
{
    internal class TypeContainsIgnoreMembers
    {
        [ProtoIgnore]
        public int A { get; set; }

        [ProtoIgnore]
        private int b;

        [ProtoIgnore]
        private static int c;

        private bool d;
    }
}
