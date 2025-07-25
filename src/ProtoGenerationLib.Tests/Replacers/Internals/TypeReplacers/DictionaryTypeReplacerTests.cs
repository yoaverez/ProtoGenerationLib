﻿using Moq;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Replacers.Internals.TypeReplacers;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using ProtoGenerationLib.Configurations.Internals;

namespace ProtoGenerationLib.Tests.Replacers.Internals.TypeReplacers
{
    [TestClass]
    public class DictionaryTypeReplacerTests
    {
        private DictionaryTypeReplacer replacer;

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
            replacer = new DictionaryTypeReplacer(mockINewTypeNamingStrategiesProvider.Object);
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
            type.TryGetElementsOfKeyValuePairEnumerableType(out var keyType, out var valueType);
            var unifiedDictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

            mockINewTypeNamingStrategy.Setup(x => x.GetNewTypeName(It.Is<Type>(t => t.Equals(unifiedDictionaryType))))
                                      .Returns(expectedNewType);

            // Act + Assert
            TypeReplacersCommonTests.ReplaceType_TypeCanBeReplaced_ReturnNewType(replacer, type, generationOptions, expectedNewType);
        }

        #endregion ReplaceType Tests

        private static IEnumerable<object[]> GetTypesThatCanNotBeReplaced()
        {
            var typesThatCanNotBeReplaced = new Type[]
            {
                typeof(IEnumerable<>),
                typeof(int),
                typeof(object),
                typeof(bool[]),
            };

            return typesThatCanNotBeReplaced.Select(x => new object[] { x }).ToArray();
        }

        private static IEnumerable<object[]> GetTypesThatCanBeReplacedAndTheirNewType()
        {
            var testClassName = nameof(DictionaryTypeReplacerTests);
            return new List<object[]>
            {
                new object[] { typeof(Dictionary<int, string>), $"{testClassName}1" },
                new object[] { typeof(IDictionary<char, bool>), $"{testClassName}2" },
                new object[] { typeof(IEnumerable<KeyValuePair<double, object>>), $"{testClassName}3" },
            };
        }

        private static IEnumerable<object[]> GetTypesThatCanBeReplaced()
        {
            var typeAndResult = GetTypesThatCanBeReplacedAndTheirNewType();
            return typeAndResult.Select(x => new object[] { x[0] });
        }
    }
}
