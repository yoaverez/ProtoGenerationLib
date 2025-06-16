using Moq;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Converters.Internals.CSharpToProtoDefinition;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;

namespace ProtoGenerationLib.Tests.Converters.Internals.CSharpToProtoDefinition
{
    [TestClass]
    public class DataTypeToMessageConverterTests
    {
        [TestMethod]
        public void ConvertTypeToProtoDefinition_FlowIsCorrect()
        {
            // Arrange
            var type = typeof(int);
            var generationOptinos = new ProtoGenerationOptions();
            var metadatas = new Dictionary<Type, IProtoTypeMetadata>();

            var expectedIntermediate = new DataTypeMetadata();
            var expectedProtoDefinition = new MessageDefinition();

            Type csharpToInterType = default;
            IProtoGenerationOptions csharpToInterOptions = default, interToProtoOptions = default;
            IReadOnlyDictionary<Type, IProtoTypeMetadata> interToProtoMetadatas = default;
            IDataTypeMetadata actualIntermediate = default;

            var mockIProvider = new Mock<IProvider>();
            var mockCSharpToIntermediateConverter = new Mock<ICSharpToIntermediateConverter<IDataTypeMetadata>>();
            mockCSharpToIntermediateConverter.Setup(toInterConveter => toInterConveter.ConvertTypeToIntermediateRepresentation(It.IsAny<Type>(), It.IsAny<IProtoGenerationOptions>()))
                .Callback((Type t, IProtoGenerationOptions o) =>
                {
                    csharpToInterType = t;
                    csharpToInterOptions = o;
                })
                .Returns(expectedIntermediate);

            var mockIntermediateToProtoConverter = new Mock<IIntermediateToProtoDefinitionConverter<IDataTypeMetadata, IMessageDefinition>>();
            mockIntermediateToProtoConverter.Setup(toProtoConverter => toProtoConverter.ConvertIntermediateRepresentationToProtoDefinition(It.IsAny<IDataTypeMetadata>(), It.IsAny<IReadOnlyDictionary<Type, IProtoTypeMetadata>>(), It.IsAny<IProtoGenerationOptions>()))
                .Callback((IDataTypeMetadata inter, IReadOnlyDictionary<Type, IProtoTypeMetadata> m, IProtoGenerationOptions o) =>
                {
                    actualIntermediate = inter;
                    interToProtoMetadatas = m;
                    interToProtoOptions = o;
                })
                .Returns(expectedProtoDefinition);

            var converter = new DataTypeToMessageConverter(mockIProvider.Object,
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
