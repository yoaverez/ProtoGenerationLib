using Moq;
using ProtoGenerator.ProvidersAndRegistries.Internals.Containers;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class ParameterListNamingStrategiesContainerTests
    {
        private ParameterListNamingStrategiesContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new ParameterListNamingStrategiesContainer();
        }

        #region IParameterListNamingStrategiesProvider Tests

        #region GetParameterListNamingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetParameterListNamingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetParameterListNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetParameterListNamingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IParameterListNamingStrategy>();
            container.RegisterParameterListNamingStrategy("a", strategy.Object);

            // Act
            container.GetParameterListNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetParameterListNamingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IParameterListNamingStrategy>().Object;
            container.RegisterParameterListNamingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetParameterListNamingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetParameterListNamingStrategy Tests

        #endregion IParameterListNamingStrategiesProvider Tests

        #region IParameterListNamingStrategiesRegistry Tests

        #region RegisterParameterListNamingStrategy Tests

        [TestMethod]
        public void RegisterParameterListNamingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IParameterListNamingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterParameterListNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetParameterListNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterParameterListNamingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IParameterListNamingStrategy>().Object;
            var strategyName = "a";
            container.RegisterParameterListNamingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterParameterListNamingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterParameterListNamingStrategy Tests

        #endregion IParameterListNamingStrategiesRegistry Tests
    }
}
