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
    public class EnumTypeToEnumDefinitionConverterTests
    {
        [TestMethod]
        public void ConvertTypeToProtoDefinition_FlowIsCorrect()
        {
            // Arrange
            var type = typeof(int);
            var generationOptinos = new ProtoGenerationOptions();
            var metadatas = new Dictionary<Type, IProtoTypeMetadata>();

            var expectedIntermediate = new EnumTypeMetadata();
            var expectedProtoDefinition = new EnumDefinition();

            Type csharpToInterType = default;
            IProtoGenerationOptions csharpToInterOptions = default, interToProtoOptions = default;
            IReadOnlyDictionary<Type, IProtoTypeMetadata> interToProtoMetadatas = default;
            IEnumTypeMetadata actualIntermediate = default;

            var mockIProvider = new Mock<IProvider>();
            var mockCSharpToIntermediateConverter = new Mock<ICSharpToIntermediateConverter<IEnumTypeMetadata>>();
            mockCSharpToIntermediateConverter.Setup(toInterConveter => toInterConveter.ConvertTypeToIntermediateRepresentation(It.IsAny<Type>(), It.IsAny<IProtoGenerationOptions>()))
                .Callback((Type t, IProtoGenerationOptions o) =>
                {
                    csharpToInterType = t;
                    csharpToInterOptions = o;
                })
                .Returns(expectedIntermediate);

            var mockIntermediateToProtoConverter = new Mock<IIntermediateToProtoDefinitionConverter<IEnumTypeMetadata, IEnumDefinition>>();
            mockIntermediateToProtoConverter.Setup(toProtoConverter => toProtoConverter.ConvertIntermediateRepresentationToProtoDefinition(It.IsAny<IEnumTypeMetadata>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                .Callback((IEnumTypeMetadata inter, IReadOnlyDictionary<Type, IProtoTypeMetadata> m, IProtoGenerationOptions o) =>
                {
                    actualIntermediate = inter;
                    interToProtoMetadatas = m;
                    interToProtoOptions = o;
                })
                .Returns(expectedProtoDefinition);

            var converter = new EnumTypeToEnumDefinitionConverter(mockIProvider.Object,
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
