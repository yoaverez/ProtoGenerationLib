using Moq;
using ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class ProtoNamingStrategiesContainerTests
    {
        private ProtoNamingStrategiesContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new ProtoNamingStrategiesContainer();
        }

        #region IProtoNamingStrategiesProvider Tests

        #region GetFileNamingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileNamingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetFileNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileNamingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IFileNamingStrategy>();
            container.RegisterFileNamingStrategy("a", strategy.Object);

            // Act
            container.GetFileNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetFileNamingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IFileNamingStrategy>().Object;
            container.RegisterFileNamingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetFileNamingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetFileNamingStrategy Tests

        #region GetPackageNamingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPackageNamingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetPackageNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPackageNamingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IPackageNamingStrategy>();
            container.RegisterPackageNamingStrategy("a", strategy.Object);

            // Act
            container.GetPackageNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetPackageNamingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IPackageNamingStrategy>().Object;
            container.RegisterPackageNamingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetPackageNamingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetPackageNamingStrategy Tests

        #region GetTypeNamingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTypeNamingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetTypeNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTypeNamingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<ITypeNamingStrategy>();
            container.RegisterTypeNamingStrategy("a", strategy.Object);

            // Act
            container.GetTypeNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetTypeNamingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<ITypeNamingStrategy>().Object;
            container.RegisterTypeNamingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetTypeNamingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetTypeNamingStrategy Tests

        #endregion IProtoNamingStrategiesProvider Tests

        #region IProtoNamingStrategiesRegistry Tests

        #region RegisterFileNamingStrategy Tests

        [TestMethod]
        public void RegisterFileNamingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IFileNamingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterFileNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetFileNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterFileNamingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IFileNamingStrategy>().Object;
            var strategyName = "a";
            container.RegisterFileNamingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterFileNamingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterFileNamingStrategy Tests

        #region RegisterPackageNamingStrategy Tests

        [TestMethod]
        public void RegisterPackageNamingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IPackageNamingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterPackageNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetPackageNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterPackageNamingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IPackageNamingStrategy>().Object;
            var strategyName = "a";
            container.RegisterPackageNamingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterPackageNamingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterPackageNamingStrategy Tests

        #region RegisterTypeNamingStrategy Tests

        [TestMethod]
        public void RegisterTypeNamingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<ITypeNamingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterTypeNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetTypeNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterTypeNamingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<ITypeNamingStrategy>().Object;
            var strategyName = "a";
            container.RegisterTypeNamingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterTypeNamingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterTypeNamingStrategy Tests

        #endregion IProtoNamingStrategiesRegistry Tests
    }
}
