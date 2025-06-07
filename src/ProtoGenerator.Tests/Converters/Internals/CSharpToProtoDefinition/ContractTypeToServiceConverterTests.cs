using Moq;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Converters.Internals.CSharpToProtoDefinition;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;

namespace ProtoGenerator.Tests.Converters.Internals.CSharpToProtoDefinition
{
    [TestClass]
    public class ContractTypeToServiceConverterTests
    {
        [TestMethod]
        public void ConvertTypeToProtoDefinition_FlowIsCorrect()
        {
            // Arrange
            var type = typeof(int);
            var generationOptinos = new ProtoGenerationOptions();
            var metadatas = new Dictionary<Type, IProtoTypeMetadata>();

            var expectedIntermediate = new ContractTypeMetadata();
            var expectedProtoDefinition = new ServiceDefinition();

            Type csharpToInterType = default;
            IProtoGenerationOptions csharpToInterOptions = default, interToProtoOptions = default;
            IReadOnlyDictionary<Type, IProtoTypeMetadata> interToProtoMetadatas = default;
            IContractTypeMetadata actualIntermediate = default;

            var mockIProvider = new Mock<IProvider>();
            var mockCSharpToIntermediateConverter = new Mock<ICSharpToIntermediateConverter<IContractTypeMetadata>>();
            mockCSharpToIntermediateConverter.Setup(toInterConveter => toInterConveter.ConvertTypeToIntermediateRepresentation(It.IsAny<Type>(), It.IsAny<IProtoGenerationOptions>()))
                .Callback((Type t, IProtoGenerationOptions o) =>
                {
                    csharpToInterType = t;
                    csharpToInterOptions = o;
                })
                .Returns(expectedIntermediate);

            var mockIntermediateToProtoConverter = new Mock<IIntermediateToProtoDefinitionConverter<IContractTypeMetadata, IServiceDefinition>>();
            mockIntermediateToProtoConverter.Setup(toProtoConverter => toProtoConverter.ConvertIntermediateRepresentationToProtoDefinition(It.IsAny<IContractTypeMetadata>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                .Callback((IContractTypeMetadata inter, IReadOnlyDictionary<Type, IProtoTypeMetadata> m, IProtoGenerationOptions o) =>
                {
                    actualIntermediate = inter;
                    interToProtoMetadatas = m;
                    interToProtoOptions = o;
                })
                .Returns(expectedProtoDefinition);

            var converter = new ContractTypeToServiceConverter(mockIProvider.Object,
                                                               mockCSharpToIntermediateConverter.Object,
                                                               mockIntermediateToProtoConverter.Object);

            // Act
            var actualProtoDefinition = converter.ConvertTypeToProtoDefinition(type, metadatas, generationOptinos);

            // Assert
            Assert.AreSame(type, csharpToInterType);
            Assert.AreSame(generationOptinos, csharpToInterOptions);
            Assert.AreSame(expectedIntermediate, actualIntermediate);

            Assert.AreSame(metadatas, interToProtoMetadatas);
            Assert.AreSame(generationOptinos, interToProtoOptions);
            Assert.AreSame(expectedProtoDefinition, actualProtoDefinition);
        }
    }
}
