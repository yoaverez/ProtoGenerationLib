using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Replacers.Internals.TypeReplacers;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.Replacers.Internals.TypeReplacers
{
    [TestClass]
    public class ArrayTypeReplacerTests
    {
        private ArrayTypeReplacer replacer;

        private ITypeExtractionOptions extractionOptions;

        private Mock<INewTypeNamingStrategy> mockINewTypeNamingStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            extractionOptions = new ProtoGeneratorConfiguration()
            {
                NewTypeNamingStrategiesOptions = new NewTypeNamingStrategiesOptions
                {
                    NewTypeNamingStrategy = "a"
                }
            };

            mockINewTypeNamingStrategy = new Mock<INewTypeNamingStrategy>();

            var mockINewTypeNamingStrategiesProvider = new Mock<INewTypeNamingStrategiesProvider>();
            mockINewTypeNamingStrategiesProvider.Setup(provider => provider.GetNewTypeNamingStrategy(It.IsAny<string>()))
                                                .Returns(mockINewTypeNamingStrategy.Object);
            replacer = new ArrayTypeReplacer(mockINewTypeNamingStrategiesProvider.Object);
        }

        #region CanReplaceType Tests

        [DynamicData(nameof(GetTypesThatCanNotBeReplaced), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanReplaceType_TypeCanNotBeReplaced_ReturnFalse(Type type)
        {
            // Act + Assert
            TypeReplacersCommonTests.CanReplaceType_TypeCanNotBeReplaced_ReturnFalse(replacer, type);
        }

        [DynamicData(nameof(GetTypesThatCanBeReplaced), DynamicDataSourceType.Method)]
        [TestMethod]
        public void CanReplaceType_TypeCanBeReplaced_ReturnTrue(Type type)
        {
            // Act + Assert
            TypeReplacersCommonTests.CanReplaceType_TypeCanBeReplaced_ReturnTrue(replacer, type);
        }

        #endregion CanReplaceType Tests

        #region ReplaceType Tests

        [DynamicData(nameof(GetTypesThatCanNotBeReplaced), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ReplaceType_TypeCanNotBeReplaced_ThrowsArgumentException(Type type)
        {
            // Act + Assert
            TypeReplacersCommonTests.ReplaceType_TypeCanNotBeReplaced_ThrowsArgumentException(replacer, type, extractionOptions);
        }

        [DynamicData(nameof(GetTypesThatCanBeReplacedAndTheirNewType), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ReplaceType_TypeCanBeReplaced_ReturnNewType(Type type, string expectedNewType)
        {
            // Arrange
            mockINewTypeNamingStrategy.Setup(x => x.GetNewTypeName(It.Is<Type>(t => t.Equals(type))))
                                      .Returns(expectedNewType);

            // Act + Assert
            TypeReplacersCommonTests.ReplaceType_TypeCanBeReplaced_ReturnNewType(replacer, type, extractionOptions, expectedNewType);
        }

        #endregion ReplaceType Tests

        private static IEnumerable<object[]> GetTypesThatCanNotBeReplaced()
        {
            var typesThatCanNotBeReplaced = new Type[]
            {
                typeof(IEnumerable<>),
                typeof(int),
                typeof(object),
            };

            return typesThatCanNotBeReplaced.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeReplacedAndTheirNewType()
        {
            var testClassName = nameof(ArrayTypeReplacerTests);
            return new List<object[]>
            {
                new object[] { typeof(int[]), $"{testClassName}1" },
                new object[] { typeof(string[][][]), $"{testClassName}2" },
                new object[] { typeof(bool[,,,,]), $"{testClassName}3" },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeReplaced()
        {
            var typeAndResult = GetTypesThatCanBeReplacedAndTheirNewType();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
