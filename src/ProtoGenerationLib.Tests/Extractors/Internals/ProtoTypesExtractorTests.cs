using Moq;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Extractors.Internals;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Replacers.Abstracts;

namespace ProtoGenerationLib.Tests.Extractors.Internals
{
    [TestClass]
    public class ProtoTypesExtractorTests
    {
        private Mock<IProvider> mockIProvider;

        private IProtoGenerationOptions generationOptions;

        private Mock<IProtoGenerationOptions> mockProtoGenerationOptions;

        private IList<ICustomTypesExtractor> customTypesExtractors;

        [TestInitialize]
        public void TestInitialize()
        {
            customTypesExtractors = new List<ICustomTypesExtractor>();

            mockProtoGenerationOptions = new Mock<IProtoGenerationOptions>();
            mockProtoGenerationOptions.Setup(options => options.GetCustomTypesExtractors())
                                      .Returns(customTypesExtractors);

            generationOptions = mockProtoGenerationOptions.Object;

            mockIProvider = new Mock<IProvider>();
        }

        #region ExtractProtoTypes With Single Type Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExtractProtoTypes_TypeCouldNotBeHandled_ThrowsArgumentException()
        {
            // Arrange
            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor>());

            // Act
            var actualTypes = protoTypesExtractor.ExtractProtoTypes(typeof(int), generationOptions, out var _);

            // Assert
            // Noting to do. The ExpectedException attribute will assert the test.
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeIsAWellKnownType_ReturnTheWellKnownType()
        {
            // Arrange
            var testedType = typeof(int);

            var mockExtractor = new Mock<ITypesExtractor>();

            var expectedTypes = new List<Type> { testedType };

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor> { mockExtractor.Object }, wellKnownTypes: new HashSet<Type> { testedType });

            // Act
            var actualTypes = protoTypesExtractor.ExtractProtoTypes(testedType, generationOptions, out var originTypeToNewTypeMapping).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedTypes, actualTypes);
            Assert.AreEqual(0, originTypeToNewTypeMapping.Count);
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeCanBeReplaced_ReturnTheReplacedTypeAlongWithAllItsUsedTypes()
        {
            // Arrange
            var testedType = typeof(int);
            var replacingType = typeof(uint);

            var mockReplacer = new Mock<ITypeReplacer>();
            mockReplacer.Setup(replacer => replacer.CanReplaceType(It.Is<Type>(x => x.Equals(testedType))))
                        .Returns(true);
            mockReplacer.Setup(replacer => replacer.CanReplaceType(It.Is<Type>(x => !x.Equals(testedType))))
                        .Returns(false);
            mockReplacer.Setup(replacer => replacer.ReplaceType(It.Is<Type>(x => x.Equals(testedType)), generationOptions))
                        .Returns(replacingType);

            var mockExtractor = new Mock<ITypesExtractor>();
            mockExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(x => x.Equals(replacingType)), generationOptions))
                         .Returns(true);
            mockExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(x => !x.Equals(replacingType)), generationOptions))
                         .Returns(false);
            mockExtractor.Setup(extractor => extractor.ExtractUsedTypes(It.IsAny<Type>(), generationOptions)).
                Returns(new List<Type> { typeof(bool), typeof(object) });

            var wellKnownTypes = new HashSet<Type>
            {
                typeof(bool),
                typeof(object),
                replacingType,
            };
            var expectedTypes = new List<Type> { replacingType, typeof(bool), typeof(object) };
            var expectedOriginToNewType = new Dictionary<Type, Type>
            {
                [testedType] = replacingType,
            };

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor> { mockExtractor.Object },
                                                                new List<ITypeReplacer> { mockReplacer.Object },
                                                                wellKnownTypes);

            // Act
            var actualTypes = protoTypesExtractor.ExtractProtoTypes(testedType, generationOptions, out var originTypeToNewTypeMapping).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedTypes, actualTypes);
            CollectionAssert.AreEquivalent(expectedOriginToNewType, originTypeToNewTypeMapping.ToDictionary(x => x.Key, x => x.Value));
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeCouldBeHandledByCustomExtractorAndADefaultOne_TheCustomExtractorWillHandleTheType()
        {
            // Arrange
            var expectedResult = new List<Type>
            {
                typeof(int), typeof(bool)
            };
            var mockCustomExtractor = new Mock<ICustomTypesExtractor>();
            mockCustomExtractor.Setup(customExtractor => customExtractor.CanHandle(It.IsAny<Type>()))
                               .Returns(true);
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.IsAny<Type>()))
                               .Returns(expectedResult.ToList());

            var mockDefaultExtractor = new Mock<ITypesExtractor>();
            mockDefaultExtractor.Setup(defaultExtractor => defaultExtractor.CanHandle(It.IsAny<Type>(), generationOptions))
                                .Returns(true);
            mockDefaultExtractor.Setup(defaultExtractor => defaultExtractor.ExtractUsedTypes(It.IsAny<Type>(), generationOptions))
                                .Returns(new List<Type>());

            customTypesExtractors.Add(mockCustomExtractor.Object);

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor> { mockDefaultExtractor.Object });

            // Act
            var actualResult = protoTypesExtractor.ExtractProtoTypes(typeof(bool), generationOptions, out var originTypeToNewTypeMapping).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedResult, actualResult);
            Assert.AreEqual(0, originTypeToNewTypeMapping.Count);
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeUsesItself_NoStackOverflowExceptionIsThrown()
        {
            // Arrange
            var expectedResult = new List<Type>
            {
                typeof(int)
            };
            var mockCustomExtractor = new Mock<ICustomTypesExtractor>();
            mockCustomExtractor.Setup(customExtractor => customExtractor.CanHandle(It.IsAny<Type>()))
                               .Returns(true);
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.IsAny<Type>()))
                               .Returns(expectedResult.ToList());

            customTypesExtractors.Add(mockCustomExtractor.Object);

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor>());

            // Act
            var actualResult = protoTypesExtractor.ExtractProtoTypes(typeof(int), generationOptions, out var originTypeToNewTypeMapping).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedResult, actualResult);
            Assert.AreEqual(0, originTypeToNewTypeMapping.Count);
        }

        [TestMethod]
        public void ExtractProtoTypes_TypeUsesTypesThatUsesTypes_AllUsedTypesAreReturned()
        {
            // Arrange
            var expectedResult = new List<Type>
            {
                typeof(int), typeof(bool), typeof(object), typeof(double), typeof(float)
            };
            var mockCustomExtractor = new Mock<ICustomTypesExtractor>();
            mockCustomExtractor.Setup(customExtractor => customExtractor.CanHandle(It.IsAny<Type>()))
                               .Returns(true);
            // Mock for int.
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.Is<Type>(type => type.Equals(typeof(int)))))
                               .Returns(new List<Type> { typeof(bool), typeof(object) });

            // Mock for bool.
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.Is<Type>(type => type.Equals(typeof(bool)))))
                               .Returns(new List<Type> { typeof(int), typeof(object), typeof(double) });

            // Mock for object.
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.Is<Type>(type => type.Equals(typeof(object)))))
                               .Returns(new List<Type> { typeof(double), typeof(float) });

            // Mock for anything else.
            var mockedTypes = new HashSet<Type> { typeof(int), typeof(bool), typeof(object) };
            mockCustomExtractor.Setup(customExtractor => customExtractor.ExtractUsedTypes(It.Is<Type>(type => !mockedTypes.Contains(type))))
                               .Returns(new List<Type>());

            customTypesExtractors.Add(mockCustomExtractor.Object);

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor>());

            // Act
            var actualResult = protoTypesExtractor.ExtractProtoTypes(typeof(int), generationOptions, out var originTypeToNewTypeMapping).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedResult, actualResult);
            Assert.AreEqual(0, originTypeToNewTypeMapping.Count);
        }

        #endregion ExtractProtoTypes With Single Type Tests

        #region ExtractProtoTypes With Multiple Type Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExtractProtoTypes_MiddleTypeCouldNotBeHandled_ThrowsArgumentException()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(int), typeof(bool), typeof(object) };

            var mockTypeExtractor = new Mock<ITypesExtractor>();
            mockTypeExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(type => !type.Equals(testedTypes[1])), It.IsAny<IProtoGenerationOptions>()))
                             .Returns(true);
            mockTypeExtractor.Setup(extractor => extractor.ExtractUsedTypes(It.Is<Type>(type => !type.Equals(testedTypes[1])), It.IsAny<IProtoGenerationOptions>()))
                             .Returns(Array.Empty<Type>());

            // Make sure that the middle type could not be handled by the mock extractor.
            mockTypeExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(type => type.Equals(testedTypes[1])), It.IsAny<IProtoGenerationOptions>()))
                             .Returns(false);

            var protoTypesExtractor = CreateProtoTypesExtractor(new List<ITypesExtractor>() { mockTypeExtractor.Object });

            // Act
            var actualTypes = protoTypesExtractor.ExtractProtoTypes(testedTypes, generationOptions, out var _);

            // Assert
            // Noting to do. The ExpectedException attribute will assert the test.
        }

        [TestMethod]
        public void ExtractProtoTypes_MultipleTypesThatCanBeReplaced_ReturnTheReplacedTypesAlongWithAllItsUsedTypes()
        {
            // Arrange
            var testedTypes = new Type[] { typeof(int), typeof(short), typeof(long), typeof(string) };
            var replacingTypes = new Type[] { typeof(uint), typeof(ushort), typeof(ulong) };
            var usedTypes = new Type[] { typeof(float), typeof(double), typeof(decimal), typeof(string) };

            var customType = typeof(short);
            var customTypeUsedType = typeof(double);
            var mockCustomExtractor = new Mock<ICustomTypesExtractor>();
            mockCustomExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(x => x.Equals(customType))))
                         .Returns(true);
            mockCustomExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(x => !x.Equals(customType))))
                         .Returns(false);
            mockCustomExtractor.Setup(extractor => extractor.ExtractUsedTypes(It.IsAny<Type>()))
                               .Returns(new List<Type> { customTypeUsedType });

            customTypesExtractors.Add(mockCustomExtractor.Object);

            var replacers = new List<ITypeReplacer>();
            var extractors = new List<ITypesExtractor>();
            for (int i = 0; i < replacingTypes.Length; i++)
            {
                if (i != 1)
                {
                    var replacingType = replacingTypes[i];
                    var testedType = testedTypes[i];
                    var usedType = usedTypes[i];
                    var mockExtractor = new Mock<ITypesExtractor>();
                    mockExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(x => x.Equals(replacingType)), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(true);
                    mockExtractor.Setup(extractor => extractor.CanHandle(It.Is<Type>(x => !x.Equals(replacingType)), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(false);
                    mockExtractor.Setup(extractor => extractor.ExtractUsedTypes(It.Is<Type>(x => x.Equals(replacingType)), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(new Type[] { usedType });
                    extractors.Add(mockExtractor.Object);

                    var mockReplacer = new Mock<ITypeReplacer>();
                    mockReplacer.Setup(replacer => replacer.CanReplaceType(It.Is<Type>(x => x.Equals(testedType))))
                                .Returns(true);
                    mockReplacer.Setup(replacer => replacer.CanReplaceType(It.Is<Type>(x => !x.Equals(testedType))))
                                .Returns(false);
                    mockReplacer.Setup(replacer => replacer.ReplaceType(It.Is<Type>(x => x.Equals(testedType)), generationOptions))
                                .Returns(replacingType);
                    replacers.Add(mockReplacer.Object);
                }
            }

            var wellKnownTypes = new HashSet<Type>(usedTypes);
            var expectedOriginToNewType = new Dictionary<Type, Type>
            {
                [testedTypes[0]] = replacingTypes[0],
                [testedTypes[2]] = replacingTypes[2],
            };

            // The index 1 is for convenience.
            var realReplacingTypes = replacingTypes.Where((x, idx) => idx != 1);
            var expectedTypes = usedTypes.Concat(realReplacingTypes).Append(customType).ToArray();

            var protoTypesExtractor = CreateProtoTypesExtractor(extractors,
                                                                replacers,
                                                                wellKnownTypes);

            // Act
            var actualTypes = protoTypesExtractor.ExtractProtoTypes(testedTypes, generationOptions, out var originTypeToNewTypeMapping).ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedTypes, actualTypes);
            CollectionAssert.AreEquivalent(expectedOriginToNewType, originTypeToNewTypeMapping.ToDictionary(x => x.Key, x => x.Value));
        }

        #endregion ExtractProtoTypes With Multiple Type Tests

        private ProtoTypesExtractor CreateProtoTypesExtractor(IEnumerable<ITypesExtractor> typesExtractors,
                                                              IEnumerable<ITypeReplacer>? typeReplacers = null,
                                                              ISet<Type>? wellKnownTypes = null)
        {
            return new ProtoTypesExtractor(mockIProvider.Object,
                                           typesExtractors,
                                           typeReplacers ?? new List<ITypeReplacer>(),
                                           wellKnownTypes ?? new HashSet<Type>());
        }
    }
}
