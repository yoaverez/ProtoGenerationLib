using Moq;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Tests.Converters.Internals.DummyTypes;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;

namespace ProtoGenerationLib.Tests.Converters.Internals.IntermediateToProtoDefinition
{
    [TestClass]
    public class EnumTypeMetadataToEnumDefinitionConverterTests
    {
        private Mock<IProvider> mockIProvider;

        private Mock<IEnumValueNumberingStrategy> mockIEnumValueNumberingStrategy;

        private Mock<IProtoStylingStrategy> mockIProtoStylingStrategy;

        private EnumTypeMetadataToEnumDefinitionConverter converter;

        private static ProtoGenerationOptions generationOptions;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            generationOptions = new ProtoGenerationOptions
            {
                NumberingStrategiesOptions = new NumberingStrategiesOptions
                {
                    EnumValueNumberingStrategy = "1",
                },
                ProtoStylingConventionsStrategiesOptions = new ProtoStylingConventionsStrategiesOptions
                {
                    EnumValueStylingStrategy = "2",
                },
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            mockIEnumValueNumberingStrategy = new Mock<IEnumValueNumberingStrategy>();
            mockIEnumValueNumberingStrategy.Setup(strategy => strategy.GetEnumValueNumber(It.IsAny<IEnumTypeMetadata>(),
                                                                                          It.IsAny<IEnumValueMetadata>(),
                                                                                          It.IsAny<int>(),
                                                                                          It.IsAny<int>()))
                                           .Returns<IEnumTypeMetadata, IEnumValueMetadata, int, int>((a, enumValue, b, c) => enumValue.Value);

            mockIProtoStylingStrategy = new Mock<IProtoStylingStrategy>();
            mockIProtoStylingStrategy.Setup(strategy => strategy.ToProtoStyle(It.IsAny<string>()))
                                     .Returns<string>((name) => name.ToUpperInvariant());

            mockIProvider = new Mock<IProvider>();
            mockIProvider.Setup(provider => provider.GetEnumValueNumberingStrategy("1"))
                         .Returns(mockIEnumValueNumberingStrategy.Object);
            mockIProvider.Setup(provider => provider.GetProtoStylingStrategy("2"))
                         .Returns(mockIProtoStylingStrategy.Object);

            converter = new EnumTypeMetadataToEnumDefinitionConverter(mockIProvider.Object);
        }

        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_EnumValuesAreLegalAndContainsZero_EnumDefinitionIsCorrect()
        {
            // Arrange
            var enumType = typeof(Enum1);
            var enumValues = new List<IEnumValueMetadata>
            {
                new EnumValueMetadata("a", 1, "a docs"),
                new EnumValueMetadata("b", 0, "b docs"),
            };
            var enumMetadata = new EnumTypeMetadata(enumType, enumValues);

            var enumName = "enum1";
            var enumPackage = "pac";
            var enumProtoMetadata = new ProtoTypeMetadata(enumName, enumPackage, $"{enumPackage}.{enumName}", "path");
            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [enumType] = enumProtoMetadata,
            };

            var valueDefinitions = new List<IEnumValueDefinition>
            {
                new EnumValueDefinition($"{enumName}_a".ToUpperInvariant(), 1, "a docs"),
                new EnumValueDefinition($"{enumName}_b".ToUpperInvariant(), 0, "b docs"),
            };
            var expectedEnumDefinition = new EnumDefinition(enumName, enumPackage, valueDefinitions);

            // Act
            var actualEnumDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(enumMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedEnumDefinition, actualEnumDefinition);
        }

        [DataRow("UNKNOWN", "a")]
        [DataRow("UNSPECIFIED", "UNKNOWN")]
        [DataRow("THERE_IS_NO_FUCKING_WAY_THAT_YOU_HAVE_THIS_ENUM_NAME", "UNKNOWN", "UNSPECIFIED")]
        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_EnumValuesDoesContainsZero_ZeroValueIsAddedWithCorrectNameAndValue(string expectedZeroName, params string[] existingEnumValueNames)
        {
            // Arrange
            var enumType = typeof(Enum1);
            var enumValues = existingEnumValueNames.Select((name, idx) => new EnumValueMetadata(name, idx + 1)).Cast<IEnumValueMetadata>().ToList();
            var enumMetadata = new EnumTypeMetadata(enumType, enumValues);

            var enumName = "enum1";
            var enumPackage = "pac";
            var enumProtoMetadata = new ProtoTypeMetadata(enumName, enumPackage, $"{enumPackage}.{enumName}", "path");
            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [enumType] = enumProtoMetadata,
            };

            var valueDefinitions = existingEnumValueNames.Select((name, idx) => new EnumValueDefinition($"{enumName}_{name}".ToUpperInvariant(), idx + 1)).Cast<IEnumValueDefinition>().ToList();
            valueDefinitions.Add(new EnumValueDefinition($"{enumName}_{expectedZeroName}".ToUpperInvariant(), 0));
            var expectedEnumDefinition = new EnumDefinition(enumName, enumPackage, valueDefinitions);

            // Act
            var actualEnumDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(enumMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            Assert.AreEqual(expectedEnumDefinition, actualEnumDefinition);
        }

        [DataRow("THERE_IS_NO_FUCKING_WAY_THAT_YOU_HAVE_THIS_ENUM_NAME", "UNKNOWN", "UNSPECIFIED")]
        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_EnumValuesDoesContainsZeroAndAReplacementWasNotFound_ThrowsAnException(params string[] existingEnumValueNames)
        {
            // Arrange
            var enumType = typeof(Enum1);
            var enumValues = existingEnumValueNames.Select((name, idx) => new EnumValueMetadata(name, idx + 1)).Cast<IEnumValueMetadata>().ToList();
            var enumMetadata = new EnumTypeMetadata(enumType, enumValues);

            var enumName = "enum1";
            var enumPackage = "pac";
            var enumProtoMetadata = new ProtoTypeMetadata(enumName, enumPackage, $"{enumPackage}.{enumName}", "path");
            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [enumType] = enumProtoMetadata,
            };

            // Act
            var actualEnumDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(enumMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            // Noting to do. The ExpectedException attribute
            // will do the assert.
        }

        [DataRow("a", "b")]
        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ConvertIntermediateRepresentationToProtoDefinition_SameEnumNumericValueExists_ThrowsAnException(params string[] existingEnumValueNames)
        {
            // Arrange
            var enumType = typeof(Enum1);
            var enumValues = existingEnumValueNames.Select((name) => new EnumValueMetadata(name, 0)).Cast<IEnumValueMetadata>().ToList();
            var enumMetadata = new EnumTypeMetadata(enumType, enumValues);

            var enumName = "enum1";
            var enumPackage = "pac";
            var enumProtoMetadata = new ProtoTypeMetadata(enumName, enumPackage, $"{enumPackage}.{enumName}", "path");
            var protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [enumType] = enumProtoMetadata,
            };

            // Act
            var actualEnumDefinition = converter.ConvertIntermediateRepresentationToProtoDefinition(enumMetadata, protoTypesMetadatas, generationOptions);

            // Assert
            // Noting to do. The ExpectedException attribute
            // will do the assert.
        }
    }
}
