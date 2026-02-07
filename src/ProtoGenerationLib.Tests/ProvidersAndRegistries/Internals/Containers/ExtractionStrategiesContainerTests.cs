using Moq;
using ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class ExtractionStrategiesContainerTests
    {
        private ExtractionStrategiesContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new ExtractionStrategiesContainer();
        }

        #region IExtractionStrategiesProvider Tests

        #region GetFieldsAndPropertiesExtractionStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFieldsAndPropertiesExtractionStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetFieldsAndPropertiesExtractionStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFieldsAndPropertiesExtractionStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IFieldsAndPropertiesExtractionStrategy>();
            container.RegisterFieldsAndPropertiesExtractionStrategy("a", strategy.Object);

            // Act
            container.GetFieldsAndPropertiesExtractionStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetFieldsAndPropertiesExtractionStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>().Object;
            container.RegisterFieldsAndPropertiesExtractionStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetFieldsAndPropertiesExtractionStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetFieldsAndPropertiesExtractionStrategy Tests

        #region GetDocumentationExtractionStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDocumentationExtractionStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetDocumentationExtractionStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDocumentationExtractionStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IDocumentationExtractionStrategy>();
            container.RegisterDocumentationExtractionStrategy("a", strategy.Object);

            // Act
            container.GetDocumentationExtractionStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetDocumentationExtractionStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IDocumentationExtractionStrategy>().Object;
            container.RegisterDocumentationExtractionStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetDocumentationExtractionStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetDocumentationExtractionStrategy Tests

        #region GetMethodSignatureExtractionStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMethodSignatureExtractionStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetMethodSignatureExtractionStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMethodSignatureExtractionStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<IMethodSignatureExtractionStrategy>();
            container.RegisterMethodSignatureExtractionStrategy("a", strategy.Object);

            // Act
            container.GetMethodSignatureExtractionStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetMethodSignatureExtractionStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<IMethodSignatureExtractionStrategy>().Object;
            container.RegisterMethodSignatureExtractionStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetMethodSignatureExtractionStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetMethodSignatureExtractionStrategy Tests

        #endregion IExtractionStrategiesProvider Tests

        #region IExtractionStrategiesRegistry Tests

        #region RegisterFieldsAndPropertiesExtractionStrategy Tests

        [TestMethod]
        public void RegisterFieldsAndPropertiesExtractionStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterFieldsAndPropertiesExtractionStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetFieldsAndPropertiesExtractionStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterFieldsAndPropertiesExtractionStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>().Object;
            var strategyName = "a";
            container.RegisterFieldsAndPropertiesExtractionStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterFieldsAndPropertiesExtractionStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterFieldsAndPropertiesExtractionStrategy Tests

        #region RegisterDocumentationExtractionStrategy Tests

        [TestMethod]
        public void RegisterDocumentationExtractionStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IDocumentationExtractionStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterDocumentationExtractionStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetDocumentationExtractionStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterDocumentationExtractionStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IDocumentationExtractionStrategy>().Object;
            var strategyName = "a";
            container.RegisterDocumentationExtractionStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterDocumentationExtractionStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterDocumentationExtractionStrategy Tests

        #region RegisterMethodSignatureExtractionStrategy Tests

        [TestMethod]
        public void RegisterMethodSignatureExtractionStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IMethodSignatureExtractionStrategy>().Object;
            var strategyName = "a";

            // Act
            container.RegisterMethodSignatureExtractionStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetMethodSignatureExtractionStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterMethodSignatureExtractionStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<IMethodSignatureExtractionStrategy>().Object;
            var strategyName = "a";
            container.RegisterMethodSignatureExtractionStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterMethodSignatureExtractionStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterMethodSignatureExtractionStrategy Tests

        #endregion IExtractionStrategiesRegistry Tests
    }
}
