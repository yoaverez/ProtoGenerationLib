using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Replacers.Internals;

namespace ProtoGenerationLib.Tests.Replacers.Internals
{
    [TestClass]
    public class DefaultMethodSignatureTypeReplacersProviderTests
    {
        private static Type[] existingTypeReplacers;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            existingTypeReplacers = typeof(DefaultMethodSignatureTypeReplacersProvider).Assembly
                                                                       .GetTypes()
                                                                       .Where(t => t.IsClass && typeof(IMethodSignatureTypeReplacer).IsAssignableFrom(t))
                                                                       .ToArray();
        }

        [TestMethod]
        public void GetDefaultMethodSignatureTypeReplacers_AllReplacersExists()
        {
            // Act
            var actualReplacers = DefaultMethodSignatureTypeReplacersProvider.GetDefaultMethodSignatureTypeReplacers().Select(x => x.GetType()).ToArray();

            // Assert
            CollectionAssert.AreEqual(existingTypeReplacers, actualReplacers);
        }
    }
}
