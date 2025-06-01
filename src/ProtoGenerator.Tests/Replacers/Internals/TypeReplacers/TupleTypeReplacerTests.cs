using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Replacers.Internals.TypeReplacers;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.Replacers.Internals.TypeReplacers
{
    [TestClass]
    public class TupleTypeReplacerTests
    {
        private TupleTypeReplacer replacer;

        private IProtoGeneratorConfiguration generationOptions;

        private Mock<INewTypeNamingStrategy> mockINewTypeNamingStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGeneratorConfiguration()
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
            replacer = new TupleTypeReplacer(mockINewTypeNamingStrategiesProvider.Object);
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
            TypeReplacersCommonTests.ReplaceType_TypeCanNotBeReplaced_ThrowsArgumentException(replacer, type, generationOptions);
        }

        [DynamicData(nameof(GetTypesThatCanBeReplacedAndTheirNewType), DynamicDataSourceType.Method)]
        [TestMethod]
        public void ReplaceType_TypeCanBeReplaced_ReturnNewType(Type type, string expectedNewType)
        {
            // Arrange
            mockINewTypeNamingStrategy.Setup(x => x.GetNewTypeName(It.Is<Type>(t => t.Equals(type))))
                                      .Returns(expectedNewType);

            // Act + Assert
            TypeReplacersCommonTests.ReplaceType_TypeCanBeReplaced_ReturnNewType(replacer, type, generationOptions, expectedNewType);
        }

        #endregion ReplaceType Tests

        private static IEnumerable<object[]> GetTypesThatCanNotBeReplaced()
        {
            var typesThatCanNotBeReplaced = new Type[]
            {
                typeof(int),
                typeof(object),
                typeof(IEnumerable<int>),
            };

            return typesThatCanNotBeReplaced.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeReplacedAndTheirNewType()
        {
            var testClassName = nameof(TupleTypeReplacerTests);
            return new List<object[]>
            {
                new object[] { typeof((bool a, string b)), $"{testClassName}1" },
                new object[] { typeof((int, object)), $"{testClassName}2" },
                new object[] { typeof(ValueTuple<char, bool, string>), $"{testClassName}3" },
                new object[] { typeof(Tuple<char, bool, object>), $"{testClassName}4" },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeReplaced()
        {
            var typeAndResult = GetTypesThatCanBeReplacedAndTheirNewType();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
