using Moq;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Discovery.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts;

namespace ProtoGenerationLib.Tests
{
    [TestClass]
    public class ProtoGeneratorTests
    {
        private Mock<IProtoTypesExtractor> mockProtoTypesExtractor;

        private Mock<IProtoTypeMetadataDiscoverer> mockProtoTypeMetadataDiscoverer;

        private Mock<ICSharpToProtoTypesConverter> mockCSharpToProtoTypesConverter;

        private ProtoGenerator protoGenerator;

        private ProtoGenerationOptions generationOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            mockProtoTypesExtractor = new Mock<IProtoTypesExtractor>();
            mockProtoTypeMetadataDiscoverer = new Mock<IProtoTypeMetadataDiscoverer>();
            mockCSharpToProtoTypesConverter = new Mock<ICSharpToProtoTypesConverter>();

            var mockIProviderAndRegister = new Mock<IProviderAndRegister>();

            protoGenerator = new ProtoGenerator(mockIProviderAndRegister.Object,
                                                mockProtoTypesExtractor.Object,
                                                mockProtoTypeMetadataDiscoverer.Object,
                                                mockCSharpToProtoTypesConverter.Object);
            generationOptions = new ProtoGenerationOptions();
        }

        [TestMethod]
        public void GenerateProtos_SingleType_FlowIsCorrect()
        {
            // Arrange
            var actualFlow = new List<string>();
            var expectedFlow = new List<string>
            {
                nameof(IProtoTypesExtractor.ExtractProtoTypes),
                nameof(IProtoTypeMetadataDiscoverer.DiscoverProtosMetadata),
                nameof(ICSharpToProtoTypesConverter.Convert),
            };

            //wellKnownTypes.AddRange(new Type[] { typeof(float), typeof(double) });
            IReadOnlyDictionary<Type, Type> originToNewTypeMapping = new Dictionary<Type, Type>
            {
                [typeof(int)] = typeof(uint),
            };

            var extractorResult = new Type[] { typeof(object), typeof(double), typeof(string) };
            IEnumerable<Type> extractorGivenTypes = null;
            IProtoGenerationOptions extractorGivenOptions = null;
            mockProtoTypesExtractor.Setup(x => x.ExtractProtoTypes(It.IsAny<IEnumerable<Type>>(), It.IsAny<IProtoGenerationOptions>(), out originToNewTypeMapping))
                                   .Callback(() => actualFlow.Add(nameof(IProtoTypesExtractor.ExtractProtoTypes)))
                                   .Returns<IEnumerable<Type>, IProtoGenerationOptions, IReadOnlyDictionary<Type, Type>>((types, options, x) =>
                                   {
                                       extractorGivenTypes = types;
                                       extractorGivenOptions = options;
                                       return extractorResult;
                                   });

            var discovererResult = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(uint)] = new ProtoTypeMetadata("a", "pac", "pac.a", "path"),
                [typeof(double)] = new ProtoTypeMetadata("b", "pac", "pac.b", "path", shouldCreateProtoType: false),
                [typeof(object)] = new ProtoTypeMetadata("c", "pac", "pac.c", "path"),
                [typeof(string)] = new ProtoTypeMetadata("d", "pac", "pac.d", "path"),
            };
            IEnumerable<Type> discovererGivenTypes = null;
            IProtoGenerationOptions discovererGivenOptions = null;
            mockProtoTypeMetadataDiscoverer.Setup(x => x.DiscoverProtosMetadata(It.IsAny<IEnumerable<Type>>(), It.IsAny<IProtoGenerationOptions>()))
                                           .Callback(() => actualFlow.Add(nameof(IProtoTypeMetadataDiscoverer.DiscoverProtosMetadata)))
                                           .Returns<IEnumerable<Type>, IProtoGenerationOptions>((types, options) =>
                                           {
                                               discovererGivenTypes = types;
                                               discovererGivenOptions = options;
                                               return discovererResult;
                                           });

            var typesToMetadataAfterAddingOriginTypes = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(uint)] = new ProtoTypeMetadata("a", "pac", "pac.a", "path"),
                [typeof(int)] = new ProtoTypeMetadata("a", "pac", "pac.a", "path"),
                [typeof(double)] = new ProtoTypeMetadata("b", "pac", "pac.b", "path", shouldCreateProtoType: false),
                [typeof(object)] = new ProtoTypeMetadata("c", "pac", "pac.c", "path"),
                [typeof(string)] = new ProtoTypeMetadata("d", "pac", "pac.d", "path"),
            };

            // Remove the double since in its meta data this should not be created.
            var typesThatNeedsToBeCreated = new Type[] { typeof(object), typeof(string) };

            var converterResult = new Dictionary<string, IProtoDefinition>
            {
                ["path"] = new ProtoDefinition(),
            };
            IEnumerable<Type> converterGivenTypes = null;
            IReadOnlyDictionary<Type, IProtoTypeMetadata> converterGivenMetadatas = null;
            IProtoGenerationOptions converterGivenOptions = null;
            mockCSharpToProtoTypesConverter.Setup(x => x.Convert(It.IsAny<IEnumerable<Type>>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                           .Callback(() => actualFlow.Add(nameof(ICSharpToProtoTypesConverter.Convert)))
                                           .Returns<IEnumerable<Type>, IReadOnlyDictionary<Type, IProtoTypeMetadata>, IProtoGenerationOptions>((types, metadatas, options) =>
                                           {
                                               converterGivenTypes = types;
                                               converterGivenMetadatas = metadatas;
                                               converterGivenOptions = options;
                                               return converterResult;
                                           });

            var testedType = typeof(byte);

            // Act
            var actualDefinitions = protoGenerator.GenerateProtos(testedType, generationOptions);

            // Assert
            CollectionAssert.AreEqual(new Type[] { testedType }, extractorGivenTypes.ToArray());
            Assert.AreSame(generationOptions, extractorGivenOptions);

            CollectionAssert.AreEqual(extractorResult, discovererGivenTypes.ToArray());
            Assert.AreSame(generationOptions, discovererGivenOptions);

            CollectionAssert.AreEqual(typesThatNeedsToBeCreated, converterGivenTypes.ToArray());
            CollectionAssert.AreEquivalent(typesToMetadataAfterAddingOriginTypes, converterGivenMetadatas.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            Assert.AreSame(generationOptions, extractorGivenOptions);

            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }

        [TestMethod]
        public void GenerateProtos_MultipleTypes_FlowIsCorrect()
        {
            // Arrange
            var actualFlow = new List<string>();
            var expectedFlow = new List<string>
            {
                nameof(IProtoTypesExtractor.ExtractProtoTypes),
                nameof(IProtoTypeMetadataDiscoverer.DiscoverProtosMetadata),
                nameof(ICSharpToProtoTypesConverter.Convert),
            };

            IReadOnlyDictionary<Type, Type> originToNewTypeMapping = new Dictionary<Type, Type>
            {
                [typeof(int)] = typeof(uint),
            };

            var extractorResult = new Type[] { typeof(object), typeof(double), typeof(string) };
            IEnumerable<Type> extractorGivenTypes = null;
            IProtoGenerationOptions extractorGivenOptions = null;
            mockProtoTypesExtractor.Setup(x => x.ExtractProtoTypes(It.IsAny<IEnumerable<Type>>(), It.IsAny<IProtoGenerationOptions>(), out originToNewTypeMapping))
                                   .Callback(() => actualFlow.Add(nameof(IProtoTypesExtractor.ExtractProtoTypes)))
                                   .Returns<IEnumerable<Type>, IProtoGenerationOptions, IReadOnlyDictionary<Type, Type>>((types, options, x) =>
                                   {
                                       extractorGivenTypes = types;
                                       extractorGivenOptions = options;
                                       return extractorResult;
                                   });

            var discovererResult = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(uint)] = new ProtoTypeMetadata("a", "pac", "pac.a", "path"),
                [typeof(double)] = new ProtoTypeMetadata("b", "pac", "pac.b", "path", shouldCreateProtoType: false),
                [typeof(object)] = new ProtoTypeMetadata("c", "pac", "pac.c", "path"),
                [typeof(string)] = new ProtoTypeMetadata("d", "pac", "pac.d", "path"),
            };
            IEnumerable<Type> discovererGivenTypes = null;
            IProtoGenerationOptions discovererGivenOptions = null;
            mockProtoTypeMetadataDiscoverer.Setup(x => x.DiscoverProtosMetadata(It.IsAny<IEnumerable<Type>>(), It.IsAny<IProtoGenerationOptions>()))
                                           .Callback(() => actualFlow.Add(nameof(IProtoTypeMetadataDiscoverer.DiscoverProtosMetadata)))
                                           .Returns<IEnumerable<Type>, IProtoGenerationOptions>((types, options) =>
                                           {
                                               discovererGivenTypes = types;
                                               discovererGivenOptions = options;
                                               return discovererResult;
                                           });

            var typesToMetadataAfterAddingOriginTypes = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(uint)] = new ProtoTypeMetadata("a", "pac", "pac.a", "path"),
                [typeof(int)] = new ProtoTypeMetadata("a", "pac", "pac.a", "path"),
                [typeof(double)] = new ProtoTypeMetadata("b", "pac", "pac.b", "path", shouldCreateProtoType: false),
                [typeof(object)] = new ProtoTypeMetadata("c", "pac", "pac.c", "path"),
                [typeof(string)] = new ProtoTypeMetadata("d", "pac", "pac.d", "path"),
            };

            // Remove the double since in its meta data this should not be created.
            var typesThatNeedsToBeCreated = new Type[] { typeof(object), typeof(string) };

            var converterResult = new Dictionary<string, IProtoDefinition>
            {
                ["path"] = new ProtoDefinition(),
            };
            IEnumerable<Type> converterGivenTypes = null;
            IReadOnlyDictionary<Type, IProtoTypeMetadata> converterGivenMetadatas = null;
            IProtoGenerationOptions converterGivenOptions = null;
            mockCSharpToProtoTypesConverter.Setup(x => x.Convert(It.IsAny<IEnumerable<Type>>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                                           .Callback(() => actualFlow.Add(nameof(ICSharpToProtoTypesConverter.Convert)))
                                           .Returns<IEnumerable<Type>, IReadOnlyDictionary<Type, IProtoTypeMetadata>, IProtoGenerationOptions>((types, metadatas, options) =>
                                           {
                                               converterGivenTypes = types;
                                               converterGivenMetadatas = metadatas;
                                               converterGivenOptions = options;
                                               return converterResult;
                                           });

            var testedTypes = new Type[] { typeof(byte), typeof(sbyte) };

            // Act
            var actualDefinitions = protoGenerator.GenerateProtos(testedTypes, generationOptions);

            // Assert
            CollectionAssert.AreEqual(testedTypes, extractorGivenTypes.ToArray());
            Assert.AreSame(generationOptions, extractorGivenOptions);

            CollectionAssert.AreEqual(extractorResult, discovererGivenTypes.ToArray());
            Assert.AreSame(generationOptions, discovererGivenOptions);

            CollectionAssert.AreEqual(typesThatNeedsToBeCreated, converterGivenTypes.ToArray());
            CollectionAssert.AreEquivalent(typesToMetadataAfterAddingOriginTypes, converterGivenMetadatas.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            Assert.AreSame(generationOptions, extractorGivenOptions);

            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }
    }
}
