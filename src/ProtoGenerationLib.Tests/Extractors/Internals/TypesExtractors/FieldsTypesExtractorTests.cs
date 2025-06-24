using HarmonyLib;
using Moq;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors
{
    [TestClass]
    public class FieldsTypesExtractorTests
    {
        public TestContext TestContext { get; set; }

        private IFieldsTypesExtractor extractor;

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
            var actualNeededTypes = RunExtractUsedTypesFromFieldsWithPath(new Type[] { testedType });

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
            var actualNeededTypes = RunExtractUsedTypesFromFieldsWithPath(new Type[] { testedType });

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
            var actualNeededTypes = RunExtractUsedTypesFromFieldsWithPath(new Type[] { testedType });

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
            var actualNeededTypes = RunExtractUsedTypesFromFieldsWithPath(testedTypes);

            // Assert
            CollectionAssert.AreEquivalent(expectedNeededTypes, actualNeededTypes.ToList());
        }

        #endregion ExtractUsedTypesFromFields Tests

        private IEnumerable<Type> RunExtractUsedTypesFromFieldsWithPath(IEnumerable<Type> fieldTypes)
        {
            var harmony = new Harmony($"{GetType().Name}.{TestContext.TestName}");

            var origin = typeof(DefaultTypesExtractorsCreator).GetMethod(nameof(DefaultTypesExtractorsCreator.CreateDefaultWrapperElementTypesExtractors));

            static bool Prefix(ref IEnumerable<IWrapperElementTypeExtractor> __result)
            {
                var wrapperElementTypesExtractors = new List<IWrapperElementTypeExtractor>
                {
                    CreateExtractorsMock(typeof(int)),
                    CreateExtractorsMock(typeof(bool)),
                    CreateExtractorsMock(typeof(char)),
                };
                __result = wrapperElementTypesExtractors;
                return false;
            }

            harmony.Patch(origin, new HarmonyMethod(Prefix));

            var result = extractor.ExtractUsedTypesFromFields(fieldTypes);

            harmony.UnpatchAll();
            return result;
        }

        private static IWrapperElementTypeExtractor CreateExtractorsMock(Type canHandleType)
        {
            var mockExtractor = new Mock<IWrapperElementTypeExtractor>();

            // Setup the CanHandle method.
            mockExtractor.Setup(x => x.CanHandle(It.Is<Type>(type => type.Equals(canHandleType))))
                         .Returns(true);
            mockExtractor.Setup(x => x.CanHandle(It.Is<Type>(type => !type.Equals(canHandleType))))
                         .Returns(false);

            // Setup the CanHandle method.
            mockExtractor.Setup(x => x.ExtractUsedTypes(It.IsAny<Type>()))
                         .Returns(new Type[] { typeof(IEnumerable<>).MakeGenericType(canHandleType), canHandleType.MakeArrayType() });

            return mockExtractor.Object;
        }
    }
}
