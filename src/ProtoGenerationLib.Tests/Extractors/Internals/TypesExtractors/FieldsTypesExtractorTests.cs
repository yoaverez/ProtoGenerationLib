using HarmonyLib;
using Moq;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Configurations.Internals;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors
{
    [TestClass]
    public class FieldsTypesExtractorTests
    {
        public TestContext TestContext { get; set; }

        private static IProtoGenerationOptions generationOptions;

        private IFieldsTypesExtractor extractor;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            generationOptions = new ProtoGenerationOptions();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            extractor = FieldsTypesExtractor.Instance;
        }

        #region ExtractUsedTypesFromFields Tests

        [TestMethod]
        public void ExtractUsedTypesFromFields_TypeCanBeHandledByFirstExtractor_FirstExtractorHandlesType()
        {
            // Arrange
            var testedType = typeof(int);
            var expectedNeededTypes = new List<Type>
            {
                typeof(IEnumerable<>).MakeGenericType(testedType),
                testedType.MakeArrayType()
            };

            // Act
            var actualNeededTypes = RunExtractUsedTypesFromFieldsWithPath(new Type[] { testedType }, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedNeededTypes, actualNeededTypes.ToList());
        }

        [TestMethod]
        public void ExtractUsedTypesFromFields_TypeCanBeHandledByMiddleExtractor_MiddleExtractorHandlesType()
        {
            // Arrange
            var testedType = typeof(bool);
            var expectedNeededTypes = new List<Type>
            {
                typeof(IEnumerable<>).MakeGenericType(testedType),
                testedType.MakeArrayType()
            };

            // Act
            var actualNeededTypes = RunExtractUsedTypesFromFieldsWithPath(new Type[] { testedType }, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedNeededTypes, actualNeededTypes.ToList());
        }

        [TestMethod]
        public void ExtractUsedTypesFromFields_TypeCanBeHandledByLastExtractor_LastExtractorHandlesType()
        {
            // Arrange
            var testedType = typeof(char);
            var expectedNeededTypes = new List<Type>
            {
                typeof(IEnumerable<>).MakeGenericType(testedType),
                testedType.MakeArrayType()
            };

            // Act
            var actualNeededTypes = RunExtractUsedTypesFromFieldsWithPath(new Type[] { testedType }, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedNeededTypes, actualNeededTypes.ToList());
        }

        [TestMethod]
        public void ExtractUsedTypesFromFields_MultipleTypesSomeCanBeHandledByExtractors_ReturnAllTheNeededTypes()
        {
            // Arrange
            var testedTypes = new Type[]
            {
                typeof(char), typeof(object), typeof(string), typeof(int),
            };
            var expectedNeededTypes = new List<Type>
            {
                typeof(IEnumerable<>).MakeGenericType(typeof(char)),
                typeof(char).MakeArrayType(),

                typeof(object), typeof(string),

                typeof(IEnumerable<>).MakeGenericType(typeof(int)),
                typeof(int).MakeArrayType(),
            };

            // Act
            var actualNeededTypes = RunExtractUsedTypesFromFieldsWithPath(testedTypes, generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedNeededTypes, actualNeededTypes.ToList());
        }

        #endregion ExtractUsedTypesFromFields Tests

        private IEnumerable<Type> RunExtractUsedTypesFromFieldsWithPath(IEnumerable<Type> fieldTypes, IProtoGenerationOptions generationOptions)
        {
            var harmony = new Harmony($"{GetType().Name}.{TestContext.TestName}");

            var origin = typeof(DefaultTypesExtractorsCreator).GetMethod(nameof(DefaultTypesExtractorsCreator.CreateDefaultWrapperElementTypesExtractors));

            static bool Prefix(ref IEnumerable<ITypesExtractor> __result)
            {
                var wrapperElementTypesExtractors = new List<ITypesExtractor>
                {
                    CreateExtractorsMock(typeof(int)),
                    CreateExtractorsMock(typeof(bool)),
                    CreateExtractorsMock(typeof(char)),
                };
                __result = wrapperElementTypesExtractors;
                return false;
            }

            harmony.Patch(origin, new HarmonyMethod(Prefix));

            var result = extractor.ExtractUsedTypesFromFields(fieldTypes, generationOptions);

            harmony.UnpatchAll();
            return result;
        }

        private static ITypesExtractor CreateExtractorsMock(Type canHandleType)
        {
            var mockExtractor = new Mock<ITypesExtractor>();

            // Setup the CanHandle method.
            mockExtractor.Setup(x => x.CanHandle(It.Is<Type>(type => type.Equals(canHandleType)), generationOptions))
                         .Returns(true);
            mockExtractor.Setup(x => x.CanHandle(It.Is<Type>(type => !type.Equals(canHandleType)), generationOptions))
                         .Returns(false);

            // Setup the CanHandle method.
            mockExtractor.Setup(x => x.ExtractUsedTypes(It.IsAny<Type>(), generationOptions))
                         .Returns(new Type[] { typeof(IEnumerable<>).MakeGenericType(canHandleType), canHandleType.MakeArrayType() });

            return mockExtractor.Object;
        }
    }
}
