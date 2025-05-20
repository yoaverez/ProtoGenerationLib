using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Extractors.Internals;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Replacers.Abstracts;

namespace ProtoGenerator.Tests.Extractors.Internals
{
    [TestClass]
    public class ProtoTypesExtractorTests
    {
        private Mock<IProvider> mockIProvider;

        private static ITypeExtractionOptions typeExtractionOptions;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            typeExtractionOptions = new ProtoGeneratorConfiguration();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            mockIProvider = new Mock<IProvider>();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExtractProtoTypes_TypeCouldNotBeHandled_ThrowsArgumentException()
        {
            // Arrange
            mockIProvider.Setup(componentsProvider => componentsProvider.GetCustomTypesExtractors())
                         .Returns(new List<ITypesExtractor>());
            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor>());

            // Act
            var actualTypes = protoTypesExtractor.ExtractProtoTypes(typeof(int), typeExtractionOptions);

            // Assert
            // Noting to do. The ExpectedException attribute will assert the test.
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeIsAWellKnownType_ReturnTheWellKnownType()
        {
            // Arrange
            var testedType = typeof(int);
            mockIProvider.Setup(componentsProvider => componentsProvider.GetCustomTypesExtractors())
                         .Returns(new List<ITypesExtractor>());

            var mockExtractor = new Mock<ITypesExtractor>();

            var expectedTypes = new List<Type> { testedType };

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor> { mockExtractor.Object }, wellKnownTypes: new Dictionary<Type, string> { [testedType] = testedType.Name });

            // Act
            var actualTypes = protoTypesExtractor.ExtractProtoTypes(testedType, typeExtractionOptions).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedTypes, actualTypes);
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeCanBeReplaced_ReturnTheReplacedTypeAlongWithAllItsUsedTypes()
        {
            // Arrange
            var testedType = typeof(int);
            var replacingType = typeof(uint);
            mockIProvider.Setup(componentsProvider => componentsProvider.GetCustomTypesExtractors())
                         .Returns(new List<ITypesExtractor>());

            var mockReplacer = new Mock<ITypeReplacer>();
            mockReplacer.Setup(extractor => extractor.CanReplaceType(It.Is<Type>(x => x.Equals(testedType))))
                        .Returns(true);
            mockReplacer.Setup(extractor => extractor.CanReplaceType(It.Is<Type>(x => !x.Equals(testedType))))
                        .Returns(false);
            mockReplacer.Setup(extractor => extractor.ReplaceType(It.Is<Type>(x => x.Equals(testedType)), It.IsAny<ITypeExtractionOptions>()))
                        .Returns(replacingType);

            var mockExtractor = new Mock<ITypesExtractor>();
            mockExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(x => x.Equals(replacingType)), It.IsAny<ITypeExtractionOptions>()))
                         .Returns(true);
            mockExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(x => !x.Equals(replacingType)), It.IsAny<ITypeExtractionOptions>()))
                         .Returns(false);
            mockExtractor.Setup(extractor => extractor.ExtractUsedTypes(It.IsAny<Type>(), It.IsAny<ITypeExtractionOptions>())).
                Returns(new List<Type> { typeof(bool), typeof(object) });

            var wellKnownTypes = new Dictionary<Type, string>
            {
                [typeof(bool)] = typeof(bool).Name,
                [typeof(object)] = typeof(object).Name,
                [replacingType] = replacingType.Name,
            };
            var expectedTypes = new List<Type> { replacingType, typeof(bool), typeof(object) };

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor> { mockExtractor.Object },
                                                                new List<ITypeReplacer> { mockReplacer.Object },
                                                                wellKnownTypes);

            // Act
            var actualTypes = protoTypesExtractor.ExtractProtoTypes(testedType, typeExtractionOptions).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedTypes, actualTypes);
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeCouldBeHandledByCustomExtractorAndADefaultOne_TheCustomExtractorWillHandleTheType()
        {
            // Arrange
            var expectedResult = new List<Type>
            {
                typeof(int), typeof(bool)
            };
            var mockCustomExtractor = new Mock<ITypesExtractor>();
            mockCustomExtractor.Setup(customExtractor => customExtractor.CanHandle(It.IsAny<Type>(), It.IsAny<ITypeExtractionOptions>()))
                               .Returns(true);
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.IsAny<Type>(), It.IsAny<ITypeExtractionOptions>()))
                               .Returns(expectedResult.ToList());

            var mockDefaultExtractor = new Mock<ITypesExtractor>();
            mockDefaultExtractor.Setup(defaultExtractor => defaultExtractor.CanHandle(It.IsAny<Type>(), typeExtractionOptions))
                                .Returns(true);
            mockDefaultExtractor.Setup(defaultExtractor => defaultExtractor.ExtractUsedTypes(It.IsAny<Type>(), It.IsAny<ITypeExtractionOptions>()))
                                .Returns(new List<Type>());

            mockIProvider.Setup(componentsProvider => componentsProvider.GetCustomTypesExtractors())
                         .Returns(new List<ITypesExtractor> { mockCustomExtractor.Object });

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor> { mockDefaultExtractor.Object });

            // Act
            var actualResult = protoTypesExtractor.ExtractProtoTypes(typeof(bool), typeExtractionOptions).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedResult, actualResult);
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeUsesItself_NoStackOverflowExceptionIsThrown()
        {
            // Arrange
            var expectedResult = new List<Type>
            {
                typeof(int)
            };
            var mockCustomExtractor = new Mock<ITypesExtractor>();
            mockCustomExtractor.Setup(customExtractor => customExtractor.CanHandle(It.IsAny<Type>(), typeExtractionOptions))
                               .Returns(true);
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.IsAny<Type>(), It.IsAny<ITypeExtractionOptions>()))
                               .Returns(expectedResult.ToList());

            mockIProvider.Setup(componentsProvider => componentsProvider.GetCustomTypesExtractors())
                         .Returns(new List<ITypesExtractor> { mockCustomExtractor.Object });

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor>());

            // Act
            var actualResult = protoTypesExtractor.ExtractProtoTypes(typeof(int), typeExtractionOptions).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedResult, actualResult);
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeUsesTypesThatUsesTypes_AllUsedTypesAreReturned()
        {
            // Arrange
            var expectedResult = new List<Type>
            {
                typeof(int), typeof(bool), typeof(object), typeof(double), typeof(float)
            };
            var mockCustomExtractor = new Mock<ITypesExtractor>();
            mockCustomExtractor.Setup(customExtractor => customExtractor.CanHandle(It.IsAny<Type>(), typeExtractionOptions))
                               .Returns(true);
            // Mock for int.
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.Is<Type>(type => type.Equals(typeof(int))), It.IsAny<ITypeExtractionOptions>()))
                               .Returns(new List<Type> { typeof(bool), typeof(object) });

            // Mock for bool.
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.Is<Type>(type => type.Equals(typeof(bool))), It.IsAny<ITypeExtractionOptions>()))
                               .Returns(new List<Type> { typeof(int), typeof(object), typeof(double) });

            // Mock for object.
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.Is<Type>(type => type.Equals(typeof(object))), It.IsAny<ITypeExtractionOptions>()))
                               .Returns(new List<Type> { typeof(double), typeof(float) });

            // Mock for anything else.
            var mockedTypes = new HashSet<Type> { typeof(int), typeof(bool), typeof(object) };
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.Is<Type>(type => !mockedTypes.Contains(type)), It.IsAny<ITypeExtractionOptions>()))
                               .Returns(new List<Type>());

            mockIProvider.Setup(componentsProvider => componentsProvider.GetCustomTypesExtractors())
                         .Returns(new List<ITypesExtractor> { mockCustomExtractor.Object });

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor>());

            // Act
            var actualResult = protoTypesExtractor.ExtractProtoTypes(typeof(int), typeExtractionOptions).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedResult, actualResult);
        }

        private ProtoTypesExtractor CreateProtoTypesExtractor(IEnumerable<ITypesExtractor> typesExtractors,
                                                              IEnumerable<ITypeReplacer>? typeReplacers = null,
                                                              IReadOnlyDictionary<Type, string>? wellKnownTypes = null)
        {
            return new ProtoTypesExtractor(mockIProvider.Object,
                                           typesExtractors,
                                           typeReplacers ?? new List<ITypeReplacer>(),
                                           wellKnownTypes ?? new Dictionary<Type, string>());
        }
    }
}
