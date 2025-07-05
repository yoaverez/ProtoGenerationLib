using Moq;
using ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class NumberingStrategiesContainerTests
    {
        private NumberingStrategiesContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new NumberingStrategiesContainer();
        }

        #region INumberingStrategiesProvider Tests

        #region GetEnumValueNumberingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEnumValueNumberingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetEnumValueNumberingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEnumValueNumberingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IEnumValueNumberingStrategy>();
            container.RegisterEnumValueNumberingStrategy("a", strategy.Object);

            // Act
            container.GetEnumValueNumberingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetEnumValueNumberingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IEnumValueNumberingStrategy>().Object;
            container.RegisterEnumValueNumberingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetEnumValueNumberingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetEnumValueNumberingStrategy Tests

        #region GetFieldNumberingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFieldNumberingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetFieldNumberingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFieldNumberingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IFieldNumberingStrategy>();
            container.RegisterFieldNumberingStrategy("a", strategy.Object);

            // Act
            container.GetFieldNumberingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetFieldNumberingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IFieldNumberingStrategy>().Object;
            container.RegisterFieldNumberingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetFieldNumberingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetFieldNumberingStrategy Tests

        #endregion INumberingStrategiesProvider Tests

        #region INumberingStrategiesRegistry Tests

        #region RegisterEnumValueNumberingStrategy Tests

        [TestMethod]
        public void RegisterEnumValueNumberingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IEnumValueNumberingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterEnumValueNumberingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetEnumValueNumberingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterEnumValueNumberingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IEnumValueNumberingStrategy>().Object;
            var strategyName = "a";
            container.RegisterEnumValueNumberingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterEnumValueNumberingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterEnumValueNumberingStrategy Tests

        #region RegisterFieldNumberingStrategy Tests

        [TestMethod]
        public void RegisterFieldNumberingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IFieldNumberingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterFieldNumberingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetFieldNumberingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterFieldNumberingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IFieldNumberingStrategy>().Object;
            var strategyName = "a";
            container.RegisterFieldNumberingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterFieldNumberingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterFieldNumberingStrategy Tests

        #endregion INumberingStrategiesRegistry Tests
    }
}
