using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Configurations.Internals;
using System.Reflection;
using ProtoGenerationLib.Customizations.Abstracts.CustomConverters;

namespace ProtoGenerationLib.Tests.Converters.CustomConverters
{
    [TestClass]
    public class CSharpContractTypeToContractTypeMetadataCustomConverterTests
    {
        private class CustomConverter : CSharpContractTypeToContractTypeMetadataCustomConverter
        {
            public Func<Type, bool> CanHandleType { get; set; }
            public Func<Type, IContractTypeMetadata> BaseConvertTypeToIntermediateRepresentationType { get; set; }

            public CustomConverter()
            {
                CanHandleType = (a) => default;
                BaseConvertTypeToIntermediateRepresentationType = (a) => default;
            }

            public override bool CanHandle(Type type)
            {
                return CanHandleType(type);
            }

            protected override IContractTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type)
            {
                return BaseConvertTypeToIntermediateRepresentationType(type);
            }
        }

        private CustomConverter customConverter;

        [TestInitialize]
        public void TestInitialize()
        {
            customConverter = new CustomConverter();
        }

        #region ConvertTypeToIntermediateRepresentation Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_CanNotHandleType_ThrowsArgumentException()
        {
            // Arrange
            customConverter.CanHandleType = (a) => false;
            var type = typeof(int);

            // Act
            customConverter.ConvertTypeToIntermediateRepresentation(type);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_CanHandleTypeAndTypeContainsMethodsWithMoreThanOneParameter_ThrowsException()
        {
            // Arrange
            customConverter.CanHandleType = (a) => true;
            customConverter.BaseConvertTypeToIntermediateRepresentationType = (a) =>
            {
                return CreateContractTypeMetadata(new List<IMethodMetadata>
                {
                    CreateMethodMetadata(1),
                    CreateMethodMetadata(0),
                    CreateMethodMetadata(2),
                    CreateMethodMetadata(0),
                });
            };
            var type = typeof(int);

            // Act
            customConverter.ConvertTypeToIntermediateRepresentation(type);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ConvertTypeToIntermediateRepresentation_CanHandleTypeAndTypeDoesNotContainMethodsWithMoreThanOneParameter_ReturnSameMetadataAsBaseMethod()
        {
            // Arrange
            customConverter.CanHandleType = (a) => true;

            var expectedMetadata = CreateContractTypeMetadata(new List<IMethodMetadata>
            {
                CreateMethodMetadata(1),
                CreateMethodMetadata(0),
                CreateMethodMetadata(1),
                CreateMethodMetadata(0),
            });
            customConverter.BaseConvertTypeToIntermediateRepresentationType = (a) =>
            {
                return expectedMetadata;
            };
            var type = typeof(int);

            // Act
            var actualMetadata = customConverter.ConvertTypeToIntermediateRepresentation(type);

            // Assert
            Assert.AreSame(expectedMetadata, actualMetadata);
        }

        #endregion ConvertTypeToIntermediateRepresentation Tests

        #region ExtractUsedTypes Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ExtractUsedTypes_CanNotHandleType_ThrowsArgumentException()
        {
            // Arrange
            customConverter.CanHandleType = (a) => false;
            var type = typeof(int);

            // Act
            customConverter.ExtractUsedTypes(type);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void ExtractUsedTypes_CanHandleTypeAndTypeContainsMethodsWithMoreThanOneParameter_ThrowsException()
        {
            // Arrange
            customConverter.CanHandleType = (a) => true;
            customConverter.BaseConvertTypeToIntermediateRepresentationType = (a) =>
            {
                return CreateContractTypeMetadata(new List<IMethodMetadata>
                {
                    CreateMethodMetadata(1),
                    CreateMethodMetadata(0),
                    CreateMethodMetadata(2),
                    CreateMethodMetadata(0),
                });
            };
            var type = typeof(int);

            // Act
            customConverter.ExtractUsedTypes(type);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [TestMethod]
        public void ExtractUsedTypes_CanHandleTypeAndTypeDoesNotContainMethodsWithMoreThanOneParameter_ReturnAllTheMethodsUsedTypes()
        {
            // Arrange
            customConverter.CanHandleType = (a) => true;

            var expectedMetadata = CreateContractTypeMetadata(new List<IMethodMetadata>
            {
                CreateMethodMetadata(1, typeof(int)),
                CreateMethodMetadata(0),
                CreateMethodMetadata(1, typeof(string)),
                CreateMethodMetadata(0),
            });
            customConverter.BaseConvertTypeToIntermediateRepresentationType = (a) =>
            {
                return expectedMetadata;
            };

            var type = typeof(int);
            var expectedUsedTypes = new List<Type>
            {
                typeof(int), typeof(string),
                typeof(void), // This is for the parameterless methods.
                typeof(bool), // This is for the methods return type.
            };

            // Act
            var actualUsedTypes = customConverter.ExtractUsedTypes(type);

            // Assert
            CollectionAssert.AreEquivalent(expectedUsedTypes, actualUsedTypes.ToList());
        }

        #endregion ExtractUsedTypes Tests

        #region Auxiliary Functions

        private static ContractTypeMetadata CreateContractTypeMetadata(IEnumerable<IMethodMetadata> methods)
        {
            return new ContractTypeMetadata
            {
                Methods = methods.ToList(),
            };
        }

        private static MethodMetadata CreateMethodMetadata(int numOfParameters, params Type[] parameterTypes)
        {
            parameterTypes = parameterTypes.Length != 0 ? parameterTypes : new Type[] { typeof(int) };
            var parameters = new List<IMethodParameterMetadata>();
            for (int i = 0; i < numOfParameters; i++)
            {
                var parameterType = parameterTypes[i % parameterTypes.Length];
                parameters.Add(new MethodParameterMetadata(parameterType, i.ToString()));
            }
            return new MethodMetadata()
            {
                Parameters = parameters,
                ReturnType = typeof(bool),
                MethodInfo = typeof(CSharpContractTypeToContractTypeMetadataCustomConverterTests).GetMethod(nameof(CreateMethodMetadata), BindingFlags.NonPublic | BindingFlags.Static),
            };
        }

        #endregion Auxiliary Functions
    }
}
