using Moq;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Converters.Internals;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using ProtoGenerationLib.Tests.Converters.Internals.DummyTypes;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Converters.Internals
{
    [TestClass]
    public class CSharpToProtoConverterTests
    {
        private Mock<ICSharpToProtoTypeConverter<IServiceDefinition>> mockContractToServiceConverter;

        private Mock<ICSharpToProtoTypeConverter<IMessageDefinition>> mockDataTypeToMessageConverter;

        private Mock<ICSharpToProtoTypeConverter<IEnumDefinition>> mockEnumToEnumDefinitionConverter;

        private ICSharpToProtoTypesConverter converter;

        private IProtoGenerationOptions generationOptions;

        private HashSet<Type> wellKnownTypes;

        private Dictionary<Type, IProtoTypeMetadata> protoTypesMetadatas;

        [TestInitialize]
        public void TestInitialize()
        {
            mockContractToServiceConverter = new Mock<ICSharpToProtoTypeConverter<IServiceDefinition>>();
            mockDataTypeToMessageConverter = new Mock<ICSharpToProtoTypeConverter<IMessageDefinition>>();
            mockEnumToEnumDefinitionConverter = new Mock<ICSharpToProtoTypeConverter<IEnumDefinition>>();

            var mockIProvider = new Mock<IProvider>();

            generationOptions = new ProtoGenerationOptions
            {
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoServiceAttribute = typeof(ProtoServiceAttribute),
                },
                ProtoFileSyntax = "",
            };

            wellKnownTypes = new HashSet<Type>();

            protoTypesMetadatas = new Dictionary<Type, IProtoTypeMetadata>();

            converter = new CSharpToProtoConverter(mockIProvider.Object,
                                                   mockContractToServiceConverter.Object,
                                                   mockDataTypeToMessageConverter.Object,
                                                   mockEnumToEnumDefinitionConverter.Object);
        }

        [TestMethod]
        public void Convert_SingleTypeAndTypeIsWellKnown_ReturnEmptyDictionary()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(int) };
            wellKnownTypes.AddRange(testedTypes);

            var expectedProtoDefinitions = new Dictionary<string, IProtoDefinition>();

            // Act
            var actualProtoDefinitions = converter.Convert(testedTypes,
                                                           protoTypesMetadatas,
                                                           generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedProtoDefinitions, actualProtoDefinitions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [TestMethod]
        public void Convert_SingleTypeAndTypeIsNested_ReturnEmptyDictionary()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(int) };
            protoTypesMetadatas.Add(testedTypes[0], new ProtoTypeMetadata { IsNested = true });

            var expectedProtoDefinitions = new Dictionary<string, IProtoDefinition>();

            // Act
            var actualProtoDefinitions = converter.Convert(testedTypes,
                                                           protoTypesMetadatas,
                                                           generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedProtoDefinitions, actualProtoDefinitions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [TestMethod]
        public void Convert_SingleTypeAndTypeIsEnum_ReturnCorrectDefinitions()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(Enum1) };
            protoTypesMetadatas.Add(testedTypes[0], new ProtoTypeMetadata("a", "pac", "pac.a", "path"));

            var expectedEnumDefinition = new EnumDefinition("a", "pac", Array.Empty<IEnumValueDefinition>());
            var isEnumConverterCalled = false;
            mockEnumToEnumDefinitionConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(It.IsAny<Type>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                             .Callback(() => isEnumConverterCalled = true)
                                             .Returns(expectedEnumDefinition);

            var expectedProtoDefinitions = new Dictionary<string, IProtoDefinition>
            {
                ["path"] = new ProtoDefinition()
                {
                    Package = "pac",
                    Enums = new List<IEnumDefinition> { expectedEnumDefinition },
                }
            };

            // Act
            var actualProtoDefinitions = converter.Convert(testedTypes,
                                                           protoTypesMetadatas,
                                                           generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedProtoDefinitions, actualProtoDefinitions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            Assert.IsTrue(isEnumConverterCalled);
        }

        [TestMethod]
        public void Convert_SingleTypeAndTypeIsContract_ReturnCorrectDefinitions()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(IContractType1) };
            protoTypesMetadatas.Add(testedTypes[0], new ProtoTypeMetadata("a", "pac", "pac.a", "path"));

            var imports = new string[] { "import1", "import2" };
            var expectedServiceDefinition = new ServiceDefinition("a", "pac", imports, Array.Empty<IRpcDefinition>());
            var isContractConverterCalled = false;
            mockContractToServiceConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(It.IsAny<Type>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                          .Callback(() => isContractConverterCalled = true)
                                          .Returns(expectedServiceDefinition);

            var expectedProtoDefinitions = new Dictionary<string, IProtoDefinition>
            {
                ["path"] = new ProtoDefinition()
                {
                    Package = "pac",
                    Services = new List<IServiceDefinition> { expectedServiceDefinition },
                    Imports = new HashSet<string>(imports),
                }
            };

            // Act
            var actualProtoDefinitions = converter.Convert(testedTypes,
                                                           protoTypesMetadatas,
                                                           generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedProtoDefinitions, actualProtoDefinitions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            Assert.IsTrue(isContractConverterCalled);
        }

        [TestMethod]
        public void Convert_SingleTypeAndTypeIsDataType_ReturnCorrectDefinitions()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(DataType1) };
            protoTypesMetadatas.Add(testedTypes[0], new ProtoTypeMetadata("a", "pac", "pac.a", "path"));

            var imports = new string[] { "import1", "import2" };
            var expectedMessageDefinition = new MessageDefinition("a",
                                                                  "pac",
                                                                  imports,
                                                                  Array.Empty<IFieldDefinition>(),
                                                                  Array.Empty<IMessageDefinition>(),
                                                                  Array.Empty<IEnumDefinition>());
            var isDataTypeConverterCalled = false;
            mockDataTypeToMessageConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(It.IsAny<Type>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                          .Callback(() => isDataTypeConverterCalled = true)
                                          .Returns(expectedMessageDefinition);

            var expectedProtoDefinitions = new Dictionary<string, IProtoDefinition>
            {
                ["path"] = new ProtoDefinition()
                {
                    Package = "pac",
                    Messages = new List<IMessageDefinition> { expectedMessageDefinition },
                    Imports = new HashSet<string>(imports),
                }
            };

            // Act
            var actualProtoDefinitions = converter.Convert(testedTypes,
                                                           protoTypesMetadatas,
                                                           generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedProtoDefinitions, actualProtoDefinitions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            Assert.IsTrue(isDataTypeConverterCalled);
        }

        [TestMethod]
        public void Convert_MultipleTypesThatShouldBeInDifferentFiles_ReturnCorrectDefinitions()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(Enum1), typeof(Enum2) };
            protoTypesMetadatas.Add(testedTypes[0], new ProtoTypeMetadata("a", "pac", "pac.a", "path1"));
            protoTypesMetadatas.Add(testedTypes[1], new ProtoTypeMetadata("b", "pac", "pac.b", "path2"));

            var expectedEnum1Definition = new EnumDefinition("a", "pac", Array.Empty<IEnumValueDefinition>());
            var expectedEnum2Definition = new EnumDefinition("b", "pac", Array.Empty<IEnumValueDefinition>());
            var numOfTimesEnumConverterIsCalled = 0;
            mockEnumToEnumDefinitionConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(Enum1), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                             .Callback(() => numOfTimesEnumConverterIsCalled++)
                                             .Returns(expectedEnum1Definition);

            mockEnumToEnumDefinitionConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(Enum2), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                             .Callback(() => numOfTimesEnumConverterIsCalled++)
                                             .Returns(expectedEnum2Definition);

            var expectedProtoDefinitions = new Dictionary<string, IProtoDefinition>
            {
                ["path1"] = new ProtoDefinition()
                {
                    Package = "pac",
                    Enums = new List<IEnumDefinition> { expectedEnum1Definition },
                },

                ["path2"] = new ProtoDefinition()
                {
                    Package = "pac",
                    Enums = new List<IEnumDefinition> { expectedEnum2Definition },
                }
            };

            // Act
            var actualProtoDefinitions = converter.Convert(testedTypes,
                                                           protoTypesMetadatas,
                                                           generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedProtoDefinitions, actualProtoDefinitions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            Assert.AreEqual(numOfTimesEnumConverterIsCalled, 2);
        }

        [TestMethod]
        public void Convert_MultipleTypesThatShouldBeInSameFile_ReturnCorrectDefinitions()
        {
            // Arrange
            var testedTypes = new Type[]
            {
                typeof(Enum1), typeof(Enum2),
                typeof(IContractType1), typeof(IContractType2),
                typeof(DataType1), typeof(DataType1.DataType2),
            };

            // For the enums.
            protoTypesMetadatas.Add(testedTypes[0], new ProtoTypeMetadata("a", "pac", "pac.a", "path"));
            protoTypesMetadatas.Add(testedTypes[1], new ProtoTypeMetadata("b", "pac", "pac.b", "path"));

            var expectedEnum1Definition = new EnumDefinition("a", "pac", Array.Empty<IEnumValueDefinition>());
            var expectedEnum2Definition = new EnumDefinition("b", "pac", Array.Empty<IEnumValueDefinition>());
            mockEnumToEnumDefinitionConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(Enum1), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                             .Returns(expectedEnum1Definition);
            mockEnumToEnumDefinitionConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(Enum2), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                             .Returns(expectedEnum2Definition);

            // For the contracts.
            protoTypesMetadatas.Add(testedTypes[2], new ProtoTypeMetadata("c", "pac", "pac.c", "path"));
            protoTypesMetadatas.Add(testedTypes[3], new ProtoTypeMetadata("d", "pac", "pac.d", "path"));

            var contractImports = new string[] { "contract-import1", "contract-import2" };
            var expectedService1Definition = new ServiceDefinition("c", "pac", contractImports.Take(1), Array.Empty<IRpcDefinition>());
            var expectedService2Definition = new ServiceDefinition("d", "pac", contractImports.Skip(1), Array.Empty<IRpcDefinition>());
            mockContractToServiceConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(IContractType1), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                          .Returns(expectedService1Definition);
            mockContractToServiceConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(IContractType2), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                          .Returns(expectedService2Definition);

            // For the data types.
            protoTypesMetadatas.Add(testedTypes[4], new ProtoTypeMetadata("e", "pac", "pac.e", "path"));
            protoTypesMetadatas.Add(testedTypes[5], new ProtoTypeMetadata("f", "pac", "pac.f", "path"));

            // Lets add the path file import since all the types are in the same
            // file and probably need other types that are defined in the same file.
            var dataImports = new string[] { "data-import1", "data-import2", "path" };
            var expectedMessage1Definition = new MessageDefinition("e", "pac", dataImports.Take(1), Array.Empty<IFieldDefinition>(), Array.Empty<IMessageDefinition>(), Array.Empty<IEnumDefinition>());
            var expectedMessage2Definition = new MessageDefinition("f", "pac", dataImports.Skip(1), Array.Empty<IFieldDefinition>(), Array.Empty<IMessageDefinition>(), Array.Empty<IEnumDefinition>());
            mockDataTypeToMessageConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(DataType1), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                          .Returns(expectedMessage1Definition);
            mockDataTypeToMessageConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(DataType1.DataType2), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                          .Returns(expectedMessage2Definition);

            var expectedProtoDefinitions = new Dictionary<string, IProtoDefinition>
            {
                ["path"] = new ProtoDefinition()
                {
                    Package = "pac",
                    Enums = new List<IEnumDefinition> { expectedEnum1Definition, expectedEnum2Definition },
                    Services = new List<IServiceDefinition> { expectedService1Definition, expectedService2Definition },
                    Messages = new List<IMessageDefinition> { expectedMessage1Definition, expectedMessage2Definition },
                    Imports = new HashSet<string>(contractImports.Concat(dataImports).Where(x => !x.Equals("path")))
                },
            };

            // Act
            var actualProtoDefinitions = converter.Convert(testedTypes,
                                                           protoTypesMetadatas,
                                                           generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedProtoDefinitions, actualProtoDefinitions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void Convert_MultipleTypesThatShouldBeInSameFileButHaveDifferentPackage_ThrowsException()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(Enum1), typeof(Enum2) };
            protoTypesMetadatas.Add(testedTypes[0], new ProtoTypeMetadata("a", "pac1", "pac1.a", "path"));
            protoTypesMetadatas.Add(testedTypes[1], new ProtoTypeMetadata("b", "pac2", "pac2.b", "path"));

            var expectedEnum1Definition = new EnumDefinition("a", "pac1", Array.Empty<IEnumValueDefinition>());
            var expectedEnum2Definition = new EnumDefinition("b", "pac2", Array.Empty<IEnumValueDefinition>());
            mockEnumToEnumDefinitionConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(Enum1), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                             .Returns(expectedEnum1Definition);

            mockEnumToEnumDefinitionConverter.Setup(mockConverter => mockConverter.ConvertTypeToProtoDefinition(typeof(Enum2), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                             .Returns(expectedEnum2Definition);

            // Act
            converter.Convert(testedTypes, protoTypesMetadatas, generationOptions);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will assert the test.
        }
    }
}
