using Moq;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class CustomConvertersContainerTests
    {
        private CustomConvertersContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new CustomConvertersContainer();
        }

        #region ICustomConvertersProvider Tests

        #region GetContractTypeCustomConverters Tests

        [TestMethod]
        public void GetContractTypeCustomConverters_NoConverters_ReturnsEmptyEnumerable()
        {
            // Arrange
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();

            // Act
            var actualConverters = container.GetContractTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void GetContractTypeCustomConverters_ThereAreConverters_ReturnsAllConverters()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> { converter1.Object, converter2.Object };

            container.RegisterContractTypeCustomConverter(converter1.Object);
            container.RegisterContractTypeCustomConverter(converter2.Object);

            // Act
            var actualConverters = container.GetContractTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void GetContractTypeCustomConverters_SameConverterMultipleTimes_ReturnsAllConvertersIncludingDuplicates()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> { converter1.Object, converter2.Object, converter1.Object };

            container.RegisterContractTypeCustomConverter(converter1.Object);
            container.RegisterContractTypeCustomConverter(converter2.Object);
            container.RegisterContractTypeCustomConverter(converter1.Object);

            // Act
            var actualConverters = container.GetContractTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion GetContractTypeCustomConverters Tests

        #region GetDataTypeCustomConverters Tests

        [TestMethod]
        public void GetDataTypeCustomConverters_NoConverters_ReturnsEmptyEnumerable()
        {
            // Arrange
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();

            // Act
            var actualConverters = container.GetDataTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void GetDataTypeCustomConverters_ThereAreConverters_ReturnsAllConverters()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> { converter1.Object, converter2.Object };

            container.RegisterDataTypeCustomConverter(converter1.Object);
            container.RegisterDataTypeCustomConverter(converter2.Object);

            // Act
            var actualConverters = container.GetDataTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void GetDataTypeCustomConverters_SameConverterMultipleTimes_ReturnsAllConvertersIncludingDuplicates()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> { converter1.Object, converter2.Object, converter1.Object };

            container.RegisterDataTypeCustomConverter(converter1.Object);
            container.RegisterDataTypeCustomConverter(converter2.Object);
            container.RegisterDataTypeCustomConverter(converter1.Object);

            // Act
            var actualConverters = container.GetDataTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion GetDataTypeCustomConverters Tests

        #region GetEnumTypeCustomConverters Tests

        [TestMethod]
        public void GetEnumTypeCustomConverters_NoConverters_ReturnsEmptyEnumerable()
        {
            // Arrange
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();

            // Act
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void GetEnumTypeCustomConverters_ThereAreConverters_ReturnsAllConverters()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> { converter1.Object, converter2.Object };

            container.RegisterEnumTypeCustomConverter(converter1.Object);
            container.RegisterEnumTypeCustomConverter(converter2.Object);

            // Act
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void GetEnumTypeCustomConverters_SameConverterMultipleTimes_ReturnsAllConvertersIncludingDuplicates()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> { converter1.Object, converter2.Object, converter1.Object };

            container.RegisterEnumTypeCustomConverter(converter1.Object);
            container.RegisterEnumTypeCustomConverter(converter2.Object);
            container.RegisterEnumTypeCustomConverter(converter1.Object);

            // Act
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion GetEnumTypeCustomConverters Tests

        #region GetCustomTypesExtractors Tests

        [TestMethod]
        public void GetCustomTypesExtractors_NoConverters_ReturnsEmptyEnumerable()
        {
            // Arrange
            var expectedConverters = new List<ITypesExtractor>();

            // Act
            var actualConverters = container.GetCustomTypesExtractors().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void GetCustomTypesExtractors_MultipleConvertersFromDifferentKinds_ReturnsAllConverters()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var converter3 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();

            container.RegisterContractTypeCustomConverter(converter1.Object);
            container.RegisterContractTypeCustomConverter(converter1.Object);
            container.RegisterDataTypeCustomConverter(converter2.Object);
            container.RegisterEnumTypeCustomConverter(converter3.Object);

            var expectedConverters = new List<ITypesExtractor>()
            {
                converter1.Object, converter1.Object, converter2.Object, converter3.Object,
            };

            // Act
            var actualConverters = container.GetCustomTypesExtractors().ToList();

            // Assert
            CollectionAssert.AreEquivalent(expectedConverters, actualConverters);
        }

        #endregion GetCustomTypesExtractors Tests

        #endregion ICustomConvertersProvider Tests

        #region ICustomConvertersRegistry Tests

        #region RegisterContractTypeCustomConverter Tests

        [TestMethod]
        public void RegisterContractTypeCustomConverter_NoConverters_GetConvertersReturnTheNewConverter()
        {
            // Arrange
            var converter = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>()
            {
                converter.Object,
            };

            // Act
            container.RegisterContractTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetContractTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void RegisterContractTypeCustomConverter_SameConverterExists_GetConvertersReturnTheAllConverters()
        {
            // Arrange
            var converter = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>()
            {
                converter.Object, converter.Object,
            };

            container.RegisterContractTypeCustomConverter(converter.Object);

            // Act
            container.RegisterContractTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetContractTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void RegisterContractTypeCustomConverter_DifferentConverterExists_GetConvertersReturnTheAllConverters()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>()
            {
                converter1.Object, converter2.Object,
            };

            container.RegisterContractTypeCustomConverter(converter1.Object);

            // Act
            container.RegisterContractTypeCustomConverter(converter2.Object);

            // Assert
            var actualConverters = container.GetContractTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion RegisterContractTypeCustomConverter Tests

        #region RegisterDataTypeCustomConverter Tests

        [TestMethod]
        public void RegisterDataTypeCustomConverter_NoConverters_GetConvertersReturnTheNewConverter()
        {
            // Arrange
            var converter = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>()
            {
                converter.Object,
            };

            // Act
            container.RegisterDataTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetDataTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void RegisterDataTypeCustomConverter_SameConverterExists_GetConvertersReturnTheAllConverters()
        {
            // Arrange
            var converter = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>()
            {
                converter.Object, converter.Object,
            };

            container.RegisterDataTypeCustomConverter(converter.Object);

            // Act
            container.RegisterDataTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetDataTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void RegisterDataTypeCustomConverter_DifferentConverterExists_GetConvertersReturnTheAllConverters()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>()
            {
                converter1.Object, converter2.Object,
            };

            container.RegisterDataTypeCustomConverter(converter1.Object);

            // Act
            container.RegisterDataTypeCustomConverter(converter2.Object);

            // Assert
            var actualConverters = container.GetDataTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion RegisterDataTypeCustomConverter Tests

        #region RegisterEnumTypeCustomConverter Tests

        [TestMethod]
        public void RegisterEnumTypeCustomConverter_NoConverters_GetConvertersReturnTheNewConverter()
        {
            // Arrange
            var converter = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>()
            {
                converter.Object,
            };

            // Act
            container.RegisterEnumTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void RegisterEnumTypeCustomConverter_SameConverterExists_GetConvertersReturnTheAllConverters()
        {
            // Arrange
            var converter = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>()
            {
                converter.Object, converter.Object,
            };

            container.RegisterEnumTypeCustomConverter(converter.Object);

            // Act
            container.RegisterEnumTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        [TestMethod]
        public void RegisterEnumTypeCustomConverter_DifferentConverterExists_GetConvertersReturnTheAllConverters()
        {
            // Arrange
            var converter1 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            var converter2 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>()
            {
                converter1.Object, converter2.Object,
            };

            container.RegisterEnumTypeCustomConverter(converter1.Object);

            // Act
            container.RegisterEnumTypeCustomConverter(converter2.Object);

            // Assert
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion RegisterEnumTypeCustomConverter Tests

        #endregion ICustomConvertersRegistry Tests
    }
}
