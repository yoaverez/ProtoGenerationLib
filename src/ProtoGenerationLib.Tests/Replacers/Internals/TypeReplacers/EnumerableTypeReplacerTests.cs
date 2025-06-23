using Moq;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Replacers.Internals.TypeReplacers;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using ProtoGenerationLib.Configurations.Internals;

namespace ProtoGenerationLib.Tests.Replacers.Internals.TypeReplacers
{
    [TestClass]
    public class EnumerableTypeReplacerTests
    {
        private EnumerableTypeReplacer replacer;

        private IProtoGenerationOptions generationOptions;

        private Mock<INewTypeNamingStrategy> mockINewTypeNamingStrategy;

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGenerationOptions()
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
            replacer = new EnumerableTypeReplacer(mockINewTypeNamingStrategiesProvider.Object);
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
            type.TryGetElementOfEnumerableType(out var elementType);
            Type arrayType;

            if (elementType.IsArray)
            {
                var i = 1;
                while (elementType.IsArray)
                {
                    mockINewTypeNamingStrategy.Setup(x => x.GetNewTypeName(elementType))
                                              .Returns($"{expectedNewType}_{1}");
                    i++;
                    elementType = elementType.GetElementType()!;
                }

                mockINewTypeNamingStrategy.Setup(x => x.GetNewTypeName(It.Is<Type>(t => t.IsArray)))
                                          .Returns($"{expectedNewType}_{i}");

                mockINewTypeNamingStrategy.Setup(x => x.GetNewTypeName(It.Is<Type>(t => t.Name.Equals($"{expectedNewType}_{i}[]"))))
                                              .Returns(expectedNewType);
            }
            else
            {
                arrayType = elementType.MakeArrayType();
                mockINewTypeNamingStrategy.Setup(x => x.GetNewTypeName(arrayType))
                                          .Returns(expectedNewType);
            }

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
            };

            return typesThatCanNotBeReplaced.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeReplacedAndTheirNewType()
        {
            var testClassName = nameof(EnumerableTypeReplacerTests);
            return new List<object[]>
            {
                //new object[] { typeof(Dictionary<int, string>), $"{testClassName}1" },
                //new object[] { typeof(List<char>), $"{testClassName}2" },
                //new object[] { typeof(IEnumerable<double>), $"{testClassName}3" },
                new object[] { typeof(IEnumerable<int[,]>), $"{testClassName}4" },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeReplaced()
        {
            var typeAndResult = GetTypesThatCanBeReplacedAndTheirNewType();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
