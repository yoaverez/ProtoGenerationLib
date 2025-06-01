using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.Extractors.Internals.TypesExtractors.WrapperElementTypesExtractors
{
    [TestClass]
    public class ArrayElementTypeExtractorTests
    {
        private ArrayElementTypeExtractor extractor;

        private IProtoGeneratorConfiguration generationOptions;

        private Mock<INewTypeNamingStrategy> mockINewTypeNamingStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            extractor = new ArrayElementTypeExtractor();
        }

        #region CanHandle Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanNotBeHandled_ReturnFalse(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanNotBeHandled_ReturnFalse(extractor, type, generationOptions);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanHandle_TypeCanBeHandled_ReturnTrue(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.CanHandle_TypeCanBeHandled_ReturnTrue(extractor, type, generationOptions);
        }

        #endregion CanHandle Tests

        #region ExtractUsedTypes Tests

        [DynamicData(nameof(GetTypesThatCanNotBeHandled), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(Type type)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanNotBeHandled_ThrowsArgumentException(extractor, type, generationOptions);
        }

        [DynamicData(nameof(GetTypesThatCanBeHandledAndTheirUsedTypes), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(Type type, IEnumerable<Type> expectedUsedTypes)
        {
            // Act + Assert
            TypesExtractorsCommonTests.ExtractUsedTypes_TypeCanBeHandled_ReturnAllTheUsedTypes(extractor, type, generationOptions, expectedUsedTypes);
        }

        #endregion ExtractUsedTypes Tests

        private static IEnumerable<object[]> GetTypesThatCanNotBeHandled()
        {
            var typesThatCanNotBeHandled = new Type[]
            {
                typeof(IEnumerable<>),
                typeof(int),
                typeof(object),
            };

            return typesThatCanNotBeHandled.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandledAndTheirUsedTypes()
        {
            return new List<object[]>
            {
                new object[] { typeof(int[]), new List<Type> { typeof(int) } },
                new object[] { typeof(string[][][]), new List<Type> { typeof(string), typeof(string[][][]) } },
                new object[] { typeof(bool[,,,,]), new List<Type> { typeof(bool), typeof(bool[,,,,]) } },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeHandled()
        {
            var typeAndResult = GetTypesThatCanBeHandledAndTheirUsedTypes();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
