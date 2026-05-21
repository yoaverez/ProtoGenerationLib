using ProtoGenerationLib.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors;
using ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.DummyTypes;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors
{
    [TestClass]
    public class DictionaryElementTypesExtractorTests
    {
        private DictionaryElementTypesExtractor extractor;

        [TestInitialize]
        public void TestInitialize()
        {
            extractor = new DictionaryElementTypesExtractor();
        }

        #region CanHandle Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanNotBeHandled_ReturnFalse(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanNotBeHandled_ReturnFalse(extractor, type);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanBeHandled_ReturnTrue(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanBeHandled_ReturnTrue(extractor, type);
        }

        #endregion CanHandle Tests

        #region ExtractUsedTypes Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(extractor, type);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandledAndTheirUsedTypes), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(Type type, IEnumerable<Type> expectedUsedTypes)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, type, expectedUsedTypes);
        }

        #endregion ExtractUsedTypes Tests

        private static IEnumerable<object[]> GetTypesThatCanNotBeHandled()
        {
            var typesThatCanNotBeHandled = new Type[]
            {
                typeof(IEnumerable<>),
                typeof(int),
                typeof(object),
                typeof(bool[]),
            };

            return typesThatCanNotBeHandled.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandledAndTheirUsedTypes()
        {
            return new List<object[]>
            {
                new object[] { typeof(Dictionary<int, string>), new List<Type> { typeof(int), typeof(string) } },
                new object[] { typeof(Dictionary<char, string>), new List<Type> { typeof(char), typeof(string) } },
                new object[] { typeof(Dictionary<bool, string>), new List<Type> { typeof(bool), typeof(string) } },
                new object[] { typeof(Dictionary<byte, string>), new List<Type> { typeof(byte), typeof(string) } },
                new object[] { typeof(Dictionary<short, string>), new List<Type> { typeof(short), typeof(string) } },
                new object[] { typeof(IDictionary<char, bool>), new List<Type> { typeof(char), typeof(bool) } },
                new object[] { typeof(IEnumerable<KeyValuePair<double, object>>), new List<Type> { typeof(KeyValuePair<double, object>) } },
                new object[] { typeof(Dictionary<DummyEnum1, Dictionary<int, string>>), new List<Type> { typeof(KeyValuePair<DummyEnum1, Dictionary<int, string>>) } },
                new object[] { typeof(Dictionary<DummyDataType1, string>), new List<Type> { typeof(KeyValuePair<DummyDataType1, string>) } },
                new object[] { typeof(Dictionary<IEnumerable<DummyDataType1>, string>), new List<Type> { typeof(KeyValuePair<IEnumerable<DummyDataType1>, string>) } },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            var typeAndResult = GetTypesThatCanBeHandledAndTheirUsedTypes();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
