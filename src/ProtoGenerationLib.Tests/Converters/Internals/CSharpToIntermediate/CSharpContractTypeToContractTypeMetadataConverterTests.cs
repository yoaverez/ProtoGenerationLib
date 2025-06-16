using Moq;
using ProtoGenerationLib.Configurations.Internals;
using static ProtoGenerationLib.Tests.Converters.Internals.ConvertersTestsUtils;
using ProtoGenerationLib.Tests.Converters.Internals.DummyTypes;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Converters.Internals.CSharpToIntermediate;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpContractTypeToContractTypeMetadataConverterTests
    {
        private static IProtoGenerationOptions generationOptions;

        private CSharpContractTypeToContractTypeMetadataConverter converter;

        private List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> customConverters;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            generationOptions = new ProtoGenerationOptions
            {
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoServiceAttribute = typeof(ProtoServiceAttribute),
                    ProtoRpcAttribute = typeof(ProtoRpcAttribute),
                }
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            customConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var mockIProvider = new Mock<IProvider>();
            mockIProvider.Setup(provider => provider.GetContractTypeCustomConverters())
                         .Returns(customConverters);

            converter = new CSharpContractTypeToContractTypeMetadataConverter(mockIProvider.Object);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsNotContract_ArgumentExceptionIsThrown()
        {
            // Arrange
            var type = typeof(CSharpContractTypeToContractTypeMetadataConverterTests);

            // Act
            converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            // Noting to do. The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsContract_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(IContractType1);
            var expectedMetadata = CreateContractTypeMetadata(type, new List<IMethodMetadata>
            {
                CreateMethodMetadata(type.GetMethod(nameof(IContractType1.Method1)), typeof(void), new List<IMethodParameterMetadata>
                {
                    CreateMethodParameterMetadata(typeof(int), "a"),
                }),

                CreateMethodMetadata(type.GetMethod(nameof(IContractType1.Method2)), typeof(double), new List<IMethodParameterMetadata>
                {
                    CreateMethodParameterMetadata(typeof(int), "a"),
                    CreateMethodParameterMetadata(typeof(bool), "b"),
                }),
            });

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsContractAndCouldBeHandledByCustomConverter_MetadataIsTheCustomConverterResult(int suitableCustomConverterIndex)
        {
            // Arrange
            var type = typeof(IContractType1);
            var expectedMetadata = CreateContractTypeMetadata(type, new List<IMethodMetadata>
            {
                CreateMethodMetadata(type.GetMethod(nameof(IContractType1.Method1)), typeof(void), new List<IMethodParameterMetadata>
                {
                    CreateMethodParameterMetadata(typeof(int), "a"),
                }),
            });

            for (int i = 0; i < 3; i++)
            {
                var mockConverter = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
                if (i != suitableCustomConverterIndex)
                {
                    mockConverter.Setup(customConverter => customConverter.CanHandle(It.Is<Type>((t) => t.Equals(type)), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(false);
                }
                else
                {
                    mockConverter.Setup(customConverter => customConverter.CanHandle(It.Is<Type>((t) => t.Equals(type)), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(true);
                    mockConverter.Setup(customConverter => customConverter.ConvertTypeToIntermediateRepresentation(It.Is<Type>((t) => t.Equals(type)), It.IsAny<IProtoGenerationOptions>()))
                                 .Returns(expectedMetadata);
                }
                customConverters.Add(mockConverter.Object);
            }

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreSame(expectedMetadata, actualMetadata);
        }
    }
}
