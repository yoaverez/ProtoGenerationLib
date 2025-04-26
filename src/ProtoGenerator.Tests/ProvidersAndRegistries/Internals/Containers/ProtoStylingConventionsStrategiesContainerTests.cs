using Moq;
using ProtoGenerator.ProvidersAndRegistries.Internals.Containers;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class ProtoStylingConventionsStrategiesContainerTests
    {
        private ProtoStylingConventionsStrategiesContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new ProtoStylingConventionsStrategiesContainer();
        }

        #region IProtoStylingConventionsStrategiesProvider Tests

        #region GetEnumStylingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEnumStylingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetEnumStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEnumStylingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IProtoStylingStrategy>();
            container.RegisterEnumStylingStrategy("a", strategy.Object);

            // Act
            container.GetEnumStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetEnumStylingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            container.RegisterEnumStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetEnumStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetEnumStylingStrategy Tests

        #region GetEnumValueStylingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEnumValueStylingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetEnumValueStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetEnumValueStylingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IProtoStylingStrategy>();
            container.RegisterEnumValueStylingStrategy("a", strategy.Object);

            // Act
            container.GetEnumValueStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetEnumValueStylingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            container.RegisterEnumValueStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetEnumValueStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetEnumValueStylingStrategy Tests

        #region GetFieldStylingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFieldStylingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetFieldStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFieldStylingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IProtoStylingStrategy>();
            container.RegisterFieldStylingStrategy("a", strategy.Object);

            // Act
            container.GetFieldStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetFieldStylingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            container.RegisterFieldStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetFieldStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetFieldStylingStrategy Tests

        #region GetMessageStylingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMessageStylingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetMessageStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMessageStylingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IProtoStylingStrategy>();
            container.RegisterMessageStylingStrategy("a", strategy.Object);

            // Act
            container.GetMessageStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetMessageStylingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            container.RegisterMessageStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetMessageStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetMessageStylingStrategy Tests

        #region GetPackageStylingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPackageStylingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetPackageStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPackageStylingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IProtoStylingStrategy>();
            container.RegisterPackageStylingStrategy("a", strategy.Object);

            // Act
            container.GetPackageStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetPackageStylingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            container.RegisterPackageStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetPackageStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetPackageStylingStrategy Tests

        #region GetServiceStylingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetServiceStylingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetServiceStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetServiceStylingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IProtoStylingStrategy>();
            container.RegisterServiceStylingStrategy("a", strategy.Object);

            // Act
            container.GetServiceStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetServiceStylingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            container.RegisterServiceStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetServiceStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetServiceStylingStrategy Tests

        #endregion IProtoStylingConventionsStrategiesProvider Tests

        #region IProtoStylingConventionsStrategiesRegistry Tests

        #region RegisterEnumStylingStrategy Tests

        [TestMethod]
        public void RegisterEnumStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterEnumStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetEnumStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterEnumStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterEnumStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterEnumStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterEnumStylingStrategy Tests

        #region RegisterEnumValueStylingStrategy Tests

        [TestMethod]
        public void RegisterEnumValueStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterEnumValueStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetEnumValueStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterEnumValueStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterEnumValueStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterEnumValueStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterEnumValueStylingStrategy Tests

        #region RegisterFieldStylingStrategy Tests

        [TestMethod]
        public void RegisterFieldStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterFieldStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetFieldStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterFieldStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterFieldStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterFieldStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterFieldStylingStrategy Tests

        #region RegisterMessageStylingStrategy Tests

        [TestMethod]
        public void RegisterMessageStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterMessageStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetMessageStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterMessageStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterMessageStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterMessageStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterMessageStylingStrategy Tests

        #region RegisterPackageStylingStrategy Tests

        [TestMethod]
        public void RegisterPackageStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterPackageStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetPackageStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterPackageStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterPackageStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterPackageStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterPackageStylingStrategy Tests

        #region RegisterServiceStylingStrategy Tests

        [TestMethod]
        public void RegisterServiceStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterServiceStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetServiceStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterServiceStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterServiceStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterServiceStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterServiceStylingStrategy Tests

        #endregion IProtoStylingConventionsStrategiesRegistry Tests
    }
}
