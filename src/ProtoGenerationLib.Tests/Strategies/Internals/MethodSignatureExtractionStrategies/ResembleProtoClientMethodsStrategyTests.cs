using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Internals.MethodSignatureExtractionStrategies;

namespace ProtoGenerationLib.Tests.Strategies.Internals.MethodSignatureExtractionStrategies
{
    [TestClass]
    public class ResembleProtoClientMethodsStrategyTests
    {
        private ResembleProtoClientMethodsStrategy strategy;

        [TestInitialize]
        public void TestInitialize()
        {
            strategy = new ResembleProtoClientMethodsStrategy();
        }

        [DynamicData(nameof(GetTestParameters), DynamicDataSourceType.Method)]
        [DataTestMethod]
        public void ExtractMethodSignature_DifferentCombinationsOfAdditionalParameters_ResultParametersDoesNotContainsAdditionals(string methodName, Type expectedReturnType, IEnumerable<IMethodParameterMetadata> expectedParameters)
        {
            // Arrange
            var method = GetType().GetMethod(methodName);

            // Act
            var actualSignature = strategy.ExtractMethodSignature(method, typeof(ProtoIgnoreAttribute));

            // Assert
            Assert.AreEqual(expectedReturnType, actualSignature.ReturnType);
            CollectionAssert.AreEqual(expectedParameters.ToArray(), actualSignature.Parameters.ToArray());
        }

        public static IEnumerable<object[]> GetTestParameters()
        {
            return new List<object[]>
            {
                new object[] { nameof(Method1ForTesting), typeof(int), new IMethodParameterMetadata[0]},
                new object[] { nameof(Method2ForTesting), typeof(int), new IMethodParameterMetadata[0]},
                new object[] { nameof(Method3ForTesting), typeof(int), new IMethodParameterMetadata[0]},
                new object[] { nameof(Method4ForTesting), typeof(void), new IMethodParameterMetadata[0]},
                new object[] { nameof(Method5ForTesting), typeof(void), new IMethodParameterMetadata[] { new MethodParameterMetadata(typeof(int), "a"), new MethodParameterMetadata(typeof(bool), "b") }},
                new object[] { nameof(Method6ForTesting), typeof(void), new IMethodParameterMetadata[] { new MethodParameterMetadata(typeof(DateTime?), "dead") }},
                new object[] { nameof(Method7ForTesting), typeof(void), new IMethodParameterMetadata[] { new MethodParameterMetadata(typeof(char), "deadline") }},
                new object[] { nameof(Method8ForTesting), typeof(char), new IMethodParameterMetadata[] { new MethodParameterMetadata(typeof(int), "a") }},
            };
        }

        #region Methods For Testing

        public Task<int> Method1ForTesting(DateTime deadline, Metadata headers, CancellationToken? cancellationToken)
        {
            return Task.FromResult<int>(0);
        }

        public Task<int> Method2ForTesting(DateTime? deadline)
        {
            return Task.FromResult<int>(0);
        }

        public Task<int> Method3ForTesting(CancellationToken? cancellationToken, DateTime? deadline)
        {
            return Task.FromResult<int>(0);
        }

        public Task Method4ForTesting(CancellationToken? cancellationToken, DateTime? deadline)
        {
            return Task.CompletedTask;
        }

        public Task Method5ForTesting(int a, bool b, CancellationToken? cancellationToken, DateTime? deadline)
        {
            return Task.CompletedTask;
        }

        public Task Method6ForTesting(DateTime? dead)
        {
            return Task.CompletedTask;
        }

        public Task Method7ForTesting(char deadline)
        {
            return Task.CompletedTask;
        }

        public char Method8ForTesting(int a, DateTime deadline)
        {
            return 'a';
        }

        #endregion Methods For Testing

        public class Metadata { }
    }
}
