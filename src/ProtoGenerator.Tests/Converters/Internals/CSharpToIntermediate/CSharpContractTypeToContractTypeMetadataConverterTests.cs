using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Converters.Internals.CSharpToIntermediate;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Tests.Converters.Internals.DummyTypes;
using static ProtoGenerator.Tests.Converters.Internals.ConvertersTestsUtils;

namespace ProtoGenerator.Tests.Converters.Internals.CSharpToIntermediate
{
    [TestClass]
    public class CSharpContractTypeToContractTypeMetadataConverterTests
    {
        private static IProtoGeneratorConfiguration generationOptions;

        private static CSharpContractTypeToContractTypeMetadataConverter converter;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            generationOptions = new ProtoGeneratorConfiguration
            {
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoServiceAttribute = typeof(ProtoServiceAttribute),
                    ProtoRpcAttribute = typeof(ProtoRpcAttribute),
                }
            };
            converter = new CSharpContractTypeToContractTypeMetadataConverter();
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
    }
}
