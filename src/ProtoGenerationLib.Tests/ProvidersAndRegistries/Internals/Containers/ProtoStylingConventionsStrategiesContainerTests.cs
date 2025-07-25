﻿using Moq;
using ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals.Containers
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

        #region GetProtoStylingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetProtoStylingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetProtoStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetProtoStylingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IProtoStylingStrategy>();
            container.RegisterProtoStylingStrategy("a", strategy.Object);

            // Act
            container.GetProtoStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetProtoStylingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            container.RegisterProtoStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetProtoStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetProtoStylingStrategy Tests

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
            var strategy = new Mock<IPackageStylingStrategy>();
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
            var expectedStrategy = new Mock<IPackageStylingStrategy>().Object;
            container.RegisterPackageStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetPackageStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetPackageStylingStrategy Tests

        #region GetFilePathStylingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFilePathStylingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetFilePathStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFilePathStylingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IFilePathStylingStrategy>();
            container.RegisterFilePathStylingStrategy("a", strategy.Object);

            // Act
            container.GetFilePathStylingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetFilePathStylingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IFilePathStylingStrategy>().Object;
            container.RegisterFilePathStylingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetFilePathStylingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetFilePathStylingStrategy Tests

        #endregion IProtoStylingConventionsStrategiesProvider Tests

        #region IProtoStylingConventionsStrategiesRegistry Tests

        #region RegisterProtoStylingStrategy Tests

        [TestMethod]
        public void RegisterProtoStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterProtoStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetProtoStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterProtoStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterProtoStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterProtoStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterProtoStylingStrategy Tests

        #region RegisterPackageStylingStrategy Tests

        [TestMethod]
        public void RegisterPackageStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IPackageStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterPackageStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualPackageStrategy = container.GetPackageStylingStrategy(strategyName);
            var actualProtoStrategy = container.GetProtoStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualPackageStrategy);
            Assert.AreSame(expectedStrategy, actualProtoStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterPackageStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IPackageStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterPackageStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterPackageStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterPackageStylingStrategy Tests

        #region RegisterFilePathStylingStrategy Tests

        [TestMethod]
        public void RegisterFilePathStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IFilePathStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterFilePathStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetFilePathStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterFilePathStylingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IFilePathStylingStrategy>().Object;
            var strategyName = "a";
            container.RegisterFilePathStylingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterFilePathStylingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterPackageStylingStrategy Tests

        #endregion IProtoStylingConventionsStrategiesRegistry Tests
    }
}
