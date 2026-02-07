using Moq;
using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Strategies.Internals.MethodSignatureExtractionStrategies;

namespace ProtoGenerationLib.Tests.Strategies.Internals.MethodSignatureExtractionStrategies
{
    [TestClass]
    public class DefaultMethodSignatureExtractionStrategyTests
    {
        private DefaultMethodSignatureExtractionStrategy strategy;

        private List<IMethodSignatureTypeReplacer> typeReplacers;

        [TestInitialize]
        public void TestInitialize()
        {
            typeReplacers = new List<IMethodSignatureTypeReplacer>();
            strategy = new DefaultMethodSignatureExtractionStrategy(typeReplacers);
        }

        [TestMethod]
        public void ExtractMethodSignature_SomeParametersHasIgnoreAttribute_IngoreCorrectParameters()
        {
            // Arrange
            var method = GetType().GetMethod(nameof(Method1ForTesting));
            (Type, IEnumerable<IMethodParameterMetadata>) expectedSignature = (typeof(int), [new MethodParameterMetadata(typeof(bool), "b")]);

            // Act
            var actualSignature = strategy.ExtractMethodSignature(method, typeof(ProtoIgnoreAttribute));

            // Assert
            Assert.AreEqual(expectedSignature.Item1, actualSignature.ReturnType);
            CollectionAssert.AreEqual(expectedSignature.Item2.ToArray(), actualSignature.Parameters.ToArray());
        }

        [TestMethod]
        public void ExtractMethodSignature_TypesNeedsToBeReplaced_TypesAreReplaced()
        {
            // Arrange
            var mockTypeReplacer1 = new Mock<IMethodSignatureTypeReplacer>();
            mockTypeReplacer1.Setup(replacer => replacer.CanReplace(typeof(int), true))
                                                        .Returns(true);
            mockTypeReplacer1.Setup(replacer => replacer.ReplaceType(typeof(int), true))
                                                        .Returns(typeof(long));

            var mockTypeReplacer2 = new Mock<IMethodSignatureTypeReplacer>();
            mockTypeReplacer2.Setup(replacer => replacer.CanReplace(typeof(TimeSpan), false))
                                                        .Returns(true);
            mockTypeReplacer2.Setup(replacer => replacer.ReplaceType(typeof(TimeSpan), false))
                                                        .Returns(typeof(DateTime));

            typeReplacers.Add(mockTypeReplacer1.Object);
            typeReplacers.Add(mockTypeReplacer2.Object);
            var method = GetType().GetMethod(nameof(Method2ForTesting));
            (Type, IEnumerable<IMethodParameterMetadata>) expectedSignature = (typeof(long), [new MethodParameterMetadata(typeof(bool), "a"), new MethodParameterMetadata(typeof(DateTime), "b")]);

            // Act
            var actualSignature = strategy.ExtractMethodSignature(method, typeof(ProtoIgnoreAttribute));

            // Assert
            Assert.AreEqual(expectedSignature.Item1, actualSignature.ReturnType);
            CollectionAssert.AreEqual(expectedSignature.Item2.ToArray(), actualSignature.Parameters.ToArray());
        }

        public int Method1ForTesting([ProtoIgnore] int a, bool b, [ProtoIgnore] TimeSpan c)
        {
            return 0;
        }

        public int Method2ForTesting(bool a, TimeSpan b)
        {
            return 0;
        }
    }
}
