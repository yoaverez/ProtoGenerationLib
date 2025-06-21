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
using System.Reflection;
using ProtoGenerationLib.Tests.CommonUtilities.DummyTypes;

namespace ProtoGenerationLib.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpContractTypeToContractTypeMetadataConverterTests
    {
        private ProtoGenerationOptions generationOptions;

        private CSharpContractTypeToContractTypeMetadataConverter converter;

        private List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> customConverters;

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGenerationOptions
            {
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoServiceAttribute = typeof(ProtoServiceAttribute),
                    IsProtoServiceDelegate = (type) => false,
                    ProtoRpcAttribute = typeof(ProtoRpcAttribute),
                    TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) => { rpcType = ProtoRpcType.Unary; return false; },
                }
            };

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
        public void ConvertTypeToIntermediateRepresentation_TypeIsContractByAttribute_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(IContractType1);

            generationOptions.AnalysisOptions.TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) =>
            {
                rpcType = ProtoRpcType.ServerStreaming;

                if (method.Name.Equals(nameof(IContractType1.Method3)))
                {
                    rpcType = ProtoRpcType.BidirectionalStreaming;
                    return true;
                }

                return false;
            };

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

                CreateMethodMetadata(type.GetMethod(nameof(IContractType1.Method3)), typeof(double), new List<IMethodParameterMetadata>
                {
                    CreateMethodParameterMetadata(typeof(int), "a"),
                }),
            });

            // Act
            var actualMetadata = converter.ConvertTypeToIntermediateRepresentation(type, generationOptions);

            // Assert
            Assert.AreEqual(expectedMetadata, actualMetadata);
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_TypeIsContractByDelegate_MetadataIsCorrect()
        {
            // Arrange
            var type = typeof(IContractType2);

            generationOptions.AnalysisOptions.IsProtoServiceDelegate = (type) =>
            {
                if (type.Equals(typeof(IContractType2)))
                    return true;
                return false;
            };

            generationOptions.AnalysisOptions.TryGetRpcTypeDelegate = (Type declaringType, MethodInfo method, out ProtoRpcType rpcType) =>
            {
                rpcType = ProtoRpcType.ServerStreaming;

                if (method.Name.Equals(nameof(IContractType2.Method4)))
                {
                    rpcType = ProtoRpcType.BidirectionalStreaming;
                    return true;
                }

                return false;
            };

            var expectedMetadata = CreateContractTypeMetadata(type, new List<IMethodMetadata>
            {
                CreateMethodMetadata(type.GetMethod(nameof(IContractType2.Method1)), typeof(void), new List<IMethodParameterMetadata>
                {
                }),

                CreateMethodMetadata(type.GetMethod(nameof(IContractType2.Method2)), typeof(void), new List<IMethodParameterMetadata>
                {
                    CreateMethodParameterMetadata(typeof(int), "a"),
                }),

                CreateMethodMetadata(type.GetMethod(nameof(IContractType2.Method3)), typeof(double), new List<IMethodParameterMetadata>
                {
                    CreateMethodParameterMetadata(typeof(int), "a"),
                    CreateMethodParameterMetadata(typeof(bool), "b"),
                }),

                CreateMethodMetadata(type.GetMethod(nameof(IContractType2.Method4)), typeof(double), new List<IMethodParameterMetadata>
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
