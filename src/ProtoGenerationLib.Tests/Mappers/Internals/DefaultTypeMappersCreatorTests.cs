using ProtoGenerationLib.Mappers.Internals;

namespace ProtoGenerationLib.Tests.Mappers.Internals
{
    [TestClass]
    public class DefaultTypeMappersCreatorTests
    {
        [TestMethod]
        public void CreateDefaultTypeMappers_NumberOfMappersIsCorrect()
        {
            // Arrange
            var expectedCount = 1;

            // Act
            var actualCount = DefaultTypeMappersCreator.CreateDefaultTypeMappers().Count();

            // Assert
            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
