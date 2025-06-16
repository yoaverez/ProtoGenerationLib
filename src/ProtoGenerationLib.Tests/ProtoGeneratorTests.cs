using Moq;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using ProtoGenerationLib.Discovery.Abstracts;

namespace ProtoGenerationLib.Tests
{
    [TestClass]
    public class ProtoGeneratorTests
    {
        private Mock<IProtoTypesExtractor> mockProtoTypesExtractor;

        private Mock<IProtoTypeMetadataDiscoverer> mockProtoTypeMetadataDiscoverer;

        private Mock<ICSharpToProtoTypesConverter> mockCSharpToProtoTypesConverter;

        private HashSet<Type> wellKnownTypes;

        private ProtoGenerator protoGenerator;

        private ProtoGenerationOptions generationOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            mockProtoTypesExtractor = new Mock<IProtoTypesExtractor>();
            mockProtoTypeMetadataDiscoverer = new Mock<IProtoTypeMetadataDiscoverer>();
            mockCSharpToProtoTypesConverter = new Mock<ICSharpToProtoTypesConverter>();
            wellKnownTypes = new HashSet<Type>();

            var mockIProvider = new Mock<IProvider>();

            protoGenerator = new ProtoGenerator(mockIProvider.Object,
                                                mockProtoTypesExtractor.Object,
                                                mockProtoTypeMetadataDiscoverer.Object,
                                                mockCSharpToProtoTypesConverter.Object,
                                                wellKnownTypes);
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

            wellKnownTypes.AddRange(new Type[] { typeof(float), typeof(double) });
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
            };

            var neededProtoTypesWithoutWellKnownTypes = new Type[] { typeof(object), typeof(string) };

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

            CollectionAssert.AreEqual(neededProtoTypesWithoutWellKnownTypes, converterGivenTypes.ToArray());
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

            wellKnownTypes.AddRange(new Type[] { typeof(float), typeof(double) });
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
            };

            var neededProtoTypesWithoutWellKnownTypes = new Type[] { typeof(object), typeof(string) };

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

            CollectionAssert.AreEqual(neededProtoTypesWithoutWellKnownTypes, converterGivenTypes.ToArray());
            CollectionAssert.AreEquivalent(typesToMetadataAfterAddingOriginTypes, converterGivenMetadatas.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            Assert.AreSame(generationOptions, extractorGivenOptions);

            CollectionAssert.AreEqual(expectedFlow, actualFlow);
        }
    }
}
