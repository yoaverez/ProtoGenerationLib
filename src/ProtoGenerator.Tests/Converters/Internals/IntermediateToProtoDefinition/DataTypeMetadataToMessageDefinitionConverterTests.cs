using Moq;
using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Converters.Internals.IntermediateToProtoDefinition;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Utilities.TypeUtilities;

namespace ProtoGenerator.Tests.Converters.Internals.IntermediateToProtoDefinition
{
    [TestClass]
    public class DataTypeMetadataToMessageDefinitionConverterTests
    {
        private Mock<IProvider> mockIProvider;

        private Mock<IPackageStylingStrategy> mockIPackageStylingStrategy;

        private Mock<IProtoStylingStrategy> mockIProtoStylingStrategy;

        private Mock<IFieldNumberingStrategy> mockIFieldNumberingStrategy;

        private DataTypeMetadataToMessageDefinitionConverter converter;

        private Mock<IIntermediateToProtoDefinitionConverter<IEnumTypeMetadata, IEnumDefinition>> mockEnumConverter;

        private static ProtoGenerationOptions generationOptions;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            generationOptions = new ProtoGenerationOptions
            {
                NumberingStrategiesOptions = new NumberingStrategiesOptions
                {
                    FieldNumberingStrategy = "1",
                },
                ProtoStylingConventionsStrategiesOptions = new ProtoStylingConventionsStrategiesOptions
                {
                    PackageStylingStrategy = "2",
                    FieldStylingStrategy = "3",
                },
                AnalysisOptions = new AnalysisOptions
                {
                    OptionalFieldAttribute = typeof(ProtoServiceAttribute),
                }
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            mockIPackageStylingStrategy = new Mock<IPackageStylingStrategy>();
            mockIPackageStylingStrategy.Setup(strategy => strategy.PackageComponentsSeparator)
                                       .Returns(".");

            mockIProtoStylingStrategy = new Mock<IProtoStylingStrategy>();
            mockIProtoStylingStrategy.Setup(strategy => strategy.ToProtoStyle(It.IsAny<string>()))
                                     .Returns<string>((name) => name.ToUpperInvariant());

            mockIFieldNumberingStrategy = new Mock<IFieldNumberingStrategy>();

            mockIProvider = new Mock<IProvider>();
            mockIProvider.Setup(provider => provider.GetFieldNumberingStrategy("1"))
                         .Returns(mockIFieldNumberingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetPackageStylingStrategy("2"))
                         .Returns(mockIPackageStylingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetProtoStylingStrategy("3"))
                         .Returns(mockIProtoStylingStrategy.Object);

            mockEnumConverter = new Mock<IIntermediateToProtoDefinitionConverter<IEnumTypeMetadata, IEnumDefinition>>();
            mockEnumConverter.Setup(converter => converter.ConvertIntermediateRepresentationToProtoDefinition(It.IsAny<IEnumTypeMetadata>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), generationOptions))
                             .Returns<IEnumTypeMetadata, IReadOnlyDictionary<Type, IProtoTypeMetadata>, IProtoGenerationOptions>((metadata, b, c) => new EnumDefinition(metadata.Type.Name, "", Array.Empty<IEnumValueDefinition>()));
            converter = new DataTypeMetadataToMessageDefinitionConverter(mockIProvider.Object, mockEnumConverter.Object);
        }

        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_TypeContainsDictionaryField_DictionaryFieldBecameAMap()
        {
            // Arrange
            mockIFieldNumberingStrategy.Setup(strategy => strategy.GetFieldNumber(It.IsAny<IFieldMetadata>(), It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns<IFieldMetadata, int, int>((a, idx, b) => Convert.ToUInt32(idx + 1));

            var type = typeof(object);
            var fields = new List<IFieldMetadata>
            {
                new FieldMetadata(typeof(IDictionary<string, bool>), "a", Array.Empty<Attribute>(), type),
                new FieldMetadata(typeof(int), "b", Array.Empty<Attribute>(), type),
                new FieldMetadata(typeof(IDictionary<string, bool>), "c", new Attribute[] { new ProtoServiceAttribute() }, type),
            };
            var dataTypeMetadata = new DataTypeMetadata(type, fields, Array.Empty<IDataTypeMetadata>(), Array.Empty<IEnumTypeMetadata>());

            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = new ProtoTypeMetadata(type.Name, "pac", $"pac.{type.Name}", "path1"),
                [typeof(string)] = new ProtoTypeMetadata("string", "pac.pac2", "pac.pac2.string", "path1"),
                [typeof(bool)] = new ProtoTypeMetadata("bool", "pac", "pac.bool", "path2"),
                [typeof(int)] = new ProtoTypeMetadata("int", "pac", "pac.int", "path2"),
            };

            var expectedImports = new HashSet<string>
            {
                "path1", "path2"
            };

            var expectedFields = new List<IFieldDefinition>
            {
                new FieldDefinition("a".ToUpperInvariant(), "Map<pac2.string, bool>", 1, FieldRule.None),
                new FieldDefinition("b".ToUpperInvariant(), "int", 2, FieldRule.None),
                new FieldDefinition("c".ToUpperInvariant(), "Map<pac2.string, bool>", 3, FieldRule.None),
            };
            var expectedDefinition = new MessageDefinition(type.Name, "pac", expectedImports, expectedFields, Array.Empty<IMessageDefinition>(), Array.Empty<IEnumDefinition>());

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(dataTypeMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedDefinition, actualDefinition);
        }

        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_TypeContainsMultiDimensionalArrayField_TheArrayFieldIsTheNewArrayType()
        {
            // Arrange
            mockIFieldNumberingStrategy.Setup(strategy => strategy.GetFieldNumber(It.IsAny<IFieldMetadata>(), It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns<IFieldMetadata, int, int>((a, idx, b) => Convert.ToUInt32(idx + 1));

            var type = typeof(object);
            var arrayType1 = typeof(string[][][]);
            var arrayType2 = typeof(bool[,,,]);

            var fields = new List<IFieldMetadata>
            {
                new FieldMetadata(arrayType1, "a", Array.Empty<Attribute>(), type),
                new FieldMetadata(typeof(int), "b", Array.Empty<Attribute>(), type),
                new FieldMetadata(arrayType2, "c", new Attribute[] { new ProtoServiceAttribute() }, type),
            };
            var dataTypeMetadata = new DataTypeMetadata(type, fields, Array.Empty<IDataTypeMetadata>(), Array.Empty<IEnumTypeMetadata>());

            var newArrayType1 = TypeCreator.CreateArrayType(typeof(string), "newStringArray");
            var newArrayType2 = TypeCreator.CreateArrayType(typeof(bool), "newBoolArray");
            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = new ProtoTypeMetadata(type.Name, "pac", $"pac.{type.Name}", "path1"),
                [typeof(int)] = new ProtoTypeMetadata("int", "pac", "pac.int", "path2"),

                [newArrayType1] = new ProtoTypeMetadata("newStringArray", "pac", "pac.newStringArray", "path3"),
                [arrayType1] = new ProtoTypeMetadata("newStringArray", "pac", "pac.newStringArray", "path3"),

                [newArrayType2] = new ProtoTypeMetadata("newBoolArray", "pac", "pac.newBoolArray", "path3"),
                [arrayType2] = new ProtoTypeMetadata("newBoolArray", "pac", "pac.newBoolArray", "path3"),
            };

            var expectedImports = new HashSet<string>
            {
                "path2", "path3",
            };

            var expectedFields = new List<IFieldDefinition>
            {
                new FieldDefinition("a".ToUpperInvariant(), "newStringArray", 1, FieldRule.None),
                new FieldDefinition("b".ToUpperInvariant(), "int", 2, FieldRule.None),
                new FieldDefinition("c".ToUpperInvariant(), "newBoolArray", 3, FieldRule.Optional),
            };
            var expectedDefinition = new MessageDefinition(type.Name, "pac", expectedImports, expectedFields, Array.Empty<IMessageDefinition>(), Array.Empty<IEnumDefinition>());

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(dataTypeMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedDefinition, actualDefinition);
        }

        [DataRow(typeof(IEnumerable<string>))]
        [DataRow(typeof(List<string>))]
        [DataRow(typeof(string[]))]
        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_TypeContainsNonMultidimensionalArrayEnumerable_EnumerableFieldBecameARepeatedField(Type enumerableFieldType)
        {
            // Arrange
            mockIFieldNumberingStrategy.Setup(strategy => strategy.GetFieldNumber(It.IsAny<IFieldMetadata>(), It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns<IFieldMetadata, int, int>((a, idx, b) => Convert.ToUInt32(idx + 1));

            var type = typeof(object);
            var fields = new List<IFieldMetadata>
            {
                new FieldMetadata(enumerableFieldType, "a", Array.Empty<Attribute>(), type),
                new FieldMetadata(typeof(int), "b", Array.Empty<Attribute>(), type),
                new FieldMetadata(enumerableFieldType, "c", new Attribute[] { new ProtoServiceAttribute() }, type),
            };
            var dataTypeMetadata = new DataTypeMetadata(type, fields, Array.Empty<IDataTypeMetadata>(), Array.Empty<IEnumTypeMetadata>());

            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = new ProtoTypeMetadata(type.Name, "pac", $"pac.{type.Name}", "path1"),
                [typeof(string)] = new ProtoTypeMetadata("string", "pac.pac2", "pac.pac2.string", "path1"),
                [typeof(int)] = new ProtoTypeMetadata("int", "pac", "pac.int", "path2"),
            };

            var expectedImports = new HashSet<string>
            {
                "path1", "path2"
            };

            var expectedFields = new List<IFieldDefinition>
            {
                new FieldDefinition("a".ToUpperInvariant(), "pac2.string", 1, FieldRule.Repeated),
                new FieldDefinition("b".ToUpperInvariant(), "int", 2, FieldRule.None),
                new FieldDefinition("c".ToUpperInvariant(), "pac2.string", 3, FieldRule.Repeated),
            };
            var expectedDefinition = new MessageDefinition(type.Name, "pac", expectedImports, expectedFields, Array.Empty<IMessageDefinition>(), Array.Empty<IEnumDefinition>());

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(dataTypeMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedDefinition, actualDefinition);
        }

        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_TypeContainsOptionalField_FieldIsOptional()
        {
            // Arrange
            mockIFieldNumberingStrategy.Setup(strategy => strategy.GetFieldNumber(It.IsAny<IFieldMetadata>(), It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns<IFieldMetadata, int, int>((a, idx, b) => Convert.ToUInt32(idx + 1));

            var type = typeof(object);
            var fields = new List<IFieldMetadata>
            {
                new FieldMetadata(typeof(bool?), "a", Array.Empty<Attribute>(), type),
                new FieldMetadata(typeof(int), "b", Array.Empty<Attribute>(), type),
                new FieldMetadata(typeof(string), "c", new Attribute[] { new ProtoServiceAttribute() }, type),
            };
            var dataTypeMetadata = new DataTypeMetadata(type, fields, Array.Empty<IDataTypeMetadata>(), Array.Empty<IEnumTypeMetadata>());

            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = new ProtoTypeMetadata(type.Name, "pac", $"pac.{type.Name}", "path1"),
                [typeof(string)] = new ProtoTypeMetadata("string", "pac.pac2", "pac.pac2.string", "path1"),
                [typeof(bool)] = new ProtoTypeMetadata("bool", "pac", "pac.bool", "path2"),
                [typeof(int)] = new ProtoTypeMetadata("int", "pac", "pac.int", "path2"),
            };

            var expectedImports = new HashSet<string>
            {
                "path1", "path2"
            };

            var expectedFields = new List<IFieldDefinition>
            {
                new FieldDefinition("a".ToUpperInvariant(), "bool", 1, FieldRule.Optional),
                new FieldDefinition("b".ToUpperInvariant(), "int", 2, FieldRule.None),
                new FieldDefinition("c".ToUpperInvariant(), "pac2.string", 3, FieldRule.Optional),
            };
            var expectedDefinition = new MessageDefinition(type.Name, "pac", expectedImports, expectedFields, Array.Empty<IMessageDefinition>(), Array.Empty<IEnumDefinition>());

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(dataTypeMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedDefinition, actualDefinition);
        }

        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_TypeContainsNestedTypes_MessageDefinitionContainsNestedTypes()
        {
            // Arrange
            mockIFieldNumberingStrategy.Setup(strategy => strategy.GetFieldNumber(It.IsAny<IFieldMetadata>(), It.IsAny<int>(), It.IsAny<int>()))
                                       .Returns<IFieldMetadata, int, int>((a, idx, b) => Convert.ToUInt32(idx + 1));

            var type = typeof(object);

            var nestedEnum1 = new EnumTypeMetadata(typeof(long), new List<IEnumValueMetadata>());
            var nestedEnum2 = new EnumTypeMetadata(typeof(char), new List<IEnumValueMetadata>());
            var nestedEnum3 = new EnumTypeMetadata(typeof(short), new List<IEnumValueMetadata>());

            // Those are used for the imports.
            var fieldMetadata1 = new FieldMetadata(typeof(byte), "a1", Array.Empty<Attribute>(), typeof(int));
            var fieldMetadata2 = new FieldMetadata(typeof(char), "a2", Array.Empty<Attribute>(), typeof(string));
            var fieldMetadata3 = new FieldMetadata(typeof(short), "a3", Array.Empty<Attribute>(), typeof(bool));

            var fieldDefinition1 = new FieldDefinition("a1".ToUpperInvariant(), "byte", 1, FieldRule.None);
            var fieldDefinition2 = new FieldDefinition("a2".ToUpperInvariant(), "char", 1, FieldRule.None);
            var fieldDefinition3 = new FieldDefinition("a3".ToUpperInvariant(), "short", 1, FieldRule.None);

            var nestedDataType1 = new DataTypeMetadata(typeof(int), new IFieldMetadata[] { fieldMetadata1 }, Array.Empty<IDataTypeMetadata>(), Array.Empty<IEnumTypeMetadata>());
            var nestedDataType2 = new DataTypeMetadata(typeof(string), new IFieldMetadata[] { fieldMetadata2 }, Array.Empty<IDataTypeMetadata>(), Array.Empty<IEnumTypeMetadata>());
            var nestedDataType3 = new DataTypeMetadata(typeof(bool), new IFieldMetadata[] { fieldMetadata3 }, new IDataTypeMetadata[] { nestedDataType2 }, new IEnumTypeMetadata[] { nestedEnum2 });

            var dataTypeMetadata = new DataTypeMetadata(type,
                                                        Array.Empty<IFieldMetadata>(),
                                                        new IDataTypeMetadata[] { nestedDataType1, nestedDataType3 },
                                                        new IEnumTypeMetadata[] { nestedEnum1, nestedEnum3 });

            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = new ProtoTypeMetadata(type.Name,
                                               "pac",
                                               $"pac.{type.Name}",
                                               "path1",
                                               nestedTypes: new HashSet<Type> { typeof(int), typeof(bool), typeof(long), typeof(short) }),
                [typeof(bool)] = new ProtoTypeMetadata("bool",
                                                       "pac",
                                                       "pac.bool",
                                                       "path2",
                                                       nestedTypes: new HashSet<Type> { typeof(string), typeof(char) }),

                [typeof(string)] = new ProtoTypeMetadata("string", "pac.pac2", "pac.pac2.string", "path1"),
                [typeof(int)] = new ProtoTypeMetadata("int", "pac", "pac.int", "path2"),

                // For the imports.
                [typeof(byte)] = new ProtoTypeMetadata("byte", "pac", "pac.byte", "import1"),
                [typeof(char)] = new ProtoTypeMetadata("char", "pac", "pac.char", "import2"),
                [typeof(short)] = new ProtoTypeMetadata("short", "pac", "pac.short", "import3"),
            };

            var expectedNestedEnum1 = new EnumDefinition(typeof(long).Name, "", Array.Empty<IEnumValueDefinition>());
            var expectedNestedEnum2 = new EnumDefinition(typeof(char).Name, "", Array.Empty<IEnumValueDefinition>());
            var expectedNestedEnum3 = new EnumDefinition(typeof(short).Name, "", Array.Empty<IEnumValueDefinition>());

            var expectedNestedMessage1 = new MessageDefinition("int",
                                                               "pac",
                                                               new string[] { "import1" },
                                                               new IFieldDefinition[] { fieldDefinition1 },
                                                               Array.Empty<IMessageDefinition>(),
                                                               Array.Empty<IEnumDefinition>());

            var expectedNestedMessage2 = new MessageDefinition("string",
                                                               "pac.pac2",
                                                               new string[] { "import2" },
                                                               new IFieldDefinition[] { fieldDefinition2 },
                                                               Array.Empty<IMessageDefinition>(),
                                                               Array.Empty<IEnumDefinition>());

            var expectedNestedMessage3 = new MessageDefinition("bool",
                                                               "pac",
                                                               new string[] { "import3", "import2" },
                                                               new IFieldDefinition[] { fieldDefinition3 },
                                                               new IMessageDefinition[] { expectedNestedMessage2 },
                                                               new IEnumDefinition[] { expectedNestedEnum2 });

            var expectedDefinition = new MessageDefinition(type.Name,
                                                           "pac",
                                                           new string[] { "import1", "import2", "import3" },
                                                           Array.Empty<IFieldDefinition>(),
                                                           new IMessageDefinition[] { expectedNestedMessage1, expectedNestedMessage3 },
                                                           new IEnumDefinition[] { expectedNestedEnum1, expectedNestedEnum3 });

            // Act
            var actualDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(dataTypeMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedDefinition, actualDefinition);
        }
    }
}
