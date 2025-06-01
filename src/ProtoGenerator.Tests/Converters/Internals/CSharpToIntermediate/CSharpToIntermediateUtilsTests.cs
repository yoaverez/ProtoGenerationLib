using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Converters.Internals.CSharpToIntermediate;

namespace ProtoGenerator.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpToIntermediateUtilsTests
    {
        private static IProtoGenerationOptions generationOptions;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            generationOptions = new ProtoGenerationOptions();
        }

        [TestMethod]
        public void TryConvertWithCustomConverters_ZeroConverters_ReturnFalse()
        {
            // Arrange
            var type = typeof(int);
            var customConverters = new List<ICSharpToIntermediateCustomConverter<string>>();

            // Act
            var actualResult = CSharpToIntermediateUtils.TryConvertWithCustomConverters(type, customConverters, generationOptions, out _);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void TryConvertWithCustomConverters_MultipleConvertersThatCanNotHandleTheType_ReturnFalse()
        {
            // Arrange
            var type = typeof(int);
            var customConverters = new List<ICSharpToIntermediateCustomConverter<string>>();

            for (int i = 0; i < 3; i++)
            {
                var mockConverter = new Mock<ICSharpToIntermediateCustomConverter<string>>();
                mockConverter.Setup(converter => converter.CanHandle(It.IsAny<Type>(), It.IsAny<IProtoGenerationOptions>()))
                             .Returns(false);
            }

            // Act
            var actualResult = CSharpToIntermediateUtils.TryConvertWithCustomConverters(type, customConverters, generationOptions, out _);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void TryConvertWithCustomConverters_MultipleConvertersAtLeastOneThatCanHandleTheType_ReturnTrue(int suitableCustomConverterIndex)
        {
            // Arrange
            var type = typeof(int);
            var customConverters = new List<ICSharpToIntermediateCustomConverter<int>>();

            for (int i = 0; i < 3; i++)
            {
                var mockConverter = new Mock<ICSharpToIntermediateCustomConverter<int>>();
                if (i < suitableCustomConverterIndex)
                {
                    mockConverter.Setup(converter => converter.CanHandle(It.IsAny<Type>(), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(false);
                }
                else
                {
                    mockConverter.Setup(converter => converter.CanHandle(It.IsAny<Type>(), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(true);
                    mockConverter.Setup(converter => converter.ConvertTypeToIntermediateRepresentation(It.IsAny<Type>(), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(i);
                }
                customConverters.Add(mockConverter.Object);
            }

            var expectedConvertedObject = suitableCustomConverterIndex;

            // Act
            var actualResult = CSharpToIntermediateUtils.TryConvertWithCustomConverters(type, customConverters, generationOptions, out var actualConvertedObject);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedConvertedObject, actualConvertedObject);
        }
    }
}
