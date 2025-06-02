using Moq;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Mappers.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.ProvidersAndRegistries.Internals.Containers;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class ServicesContainerTests
    {
        private ServicesContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new ServicesContainer();
        }

        #region IProvider Tests

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

        #region ICustomTypeNameMappersProvider Tests

        #region GetCustomTypeNameMappers Tests

        [TestMethod]
        public void GetCustomTypeNameMappers_NoMappers_ReturnsEmptyEnumerable()
        {
            // Arrange
            var expectedMappers = new List<ITypeMapper>();

            // Act
            var actualMappers = container.GetCustomTypeNameMappers().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        [TestMethod]
        public void GetCustomTypeNameMappers_ThereAreMappers_ReturnsAllMappers()
        {
            // Arrange
            var mapper1 = new Mock<ITypeMapper>();
            var mapper2 = new Mock<ITypeMapper>();

            var expectedMappers = new List<ITypeMapper>
            {
                mapper1.Object, mapper2.Object
            };

            container.RegisterCustomTypeNameMapper(mapper1.Object);
            container.RegisterCustomTypeNameMapper(mapper2.Object);

            // Act
            var actualMappers = container.GetCustomTypeNameMappers().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        [TestMethod]
        public void GetCustomTypeNameMappers_SameMapperMultipleTimes_ReturnsAllMappersIncludingDuplicates()
        {
            // Arrange
            var mapper1 = new Mock<ITypeMapper>();
            var mapper2 = new Mock<ITypeMapper>();

            var expectedMappers = new List<ITypeMapper>
            {
                mapper1.Object, mapper2.Object, mapper1.Object
            };

            container.RegisterCustomTypeNameMapper(mapper1.Object);
            container.RegisterCustomTypeNameMapper(mapper2.Object);
            container.RegisterCustomTypeNameMapper(mapper1.Object);

            // Act
            var actualMappers = container.GetCustomTypeNameMappers().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        #endregion GetCustomTypeNameMappers Tests

        #endregion ICustomTypeNameMappersProvider Tests

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

        #endregion IProtoStylingConventionsStrategiesProvider Tests

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

        #endregion IExtractionStrategiesProvider Tests

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

        #region INewTypeNamingStrategiesProvider Tests

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

        #region GetNewTypeNamingStrategy Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetNewTypeNamingStrategy_NoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Act
            container.GetNewTypeNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetNewTypeNamingStrategy_StrategiesExistsButNoStrategyWithWantedName_ThrowsArgumentException()
        {
            // Arrange
            var strategy = new Mock<INewTypeNamingStrategy>();
            container.RegisterNewTypeNamingStrategy("a", strategy.Object);

            // Act
            container.GetNewTypeNamingStrategy("sdfsdf");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        [TestMethod]
        public void GetNewTypeNamingStrategy_StrategyWithWantedNameExists_ReturnWantedStrategy()
        {
            // Arrange
            var expectedStrategy = new Mock<INewTypeNamingStrategy>().Object;
            container.RegisterNewTypeNamingStrategy("a", expectedStrategy);

            // Act
            var actualStrategy = container.GetNewTypeNamingStrategy("a");

            // Assert
            Assert.AreSame(expectedStrategy, actualStrategy);
        }

        #endregion GetNewTypeNamingStrategy Tests

        #endregion INewTypeNamingStrategiesProvider Tests

        #endregion IProvider Tests

        #region IRegistry Tests

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
            var registry = container.RegisterContractTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetContractTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterContractTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetContractTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterContractTypeCustomConverter(converter2.Object);

            // Assert
            var actualConverters = container.GetContractTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterDataTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetDataTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterDataTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetDataTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterDataTypeCustomConverter(converter2.Object);

            // Assert
            var actualConverters = container.GetDataTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterEnumTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
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

            var registry = container.RegisterEnumTypeCustomConverter(converter.Object);

            // Act
            container.RegisterEnumTypeCustomConverter(converter.Object);

            // Assert
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
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

            var registry = container.RegisterEnumTypeCustomConverter(converter1.Object);

            // Act
            container.RegisterEnumTypeCustomConverter(converter2.Object);

            // Assert
            var actualConverters = container.GetEnumTypeCustomConverters().ToList();
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
            Assert.AreSame(container, registry);
        }

        #endregion RegisterEnumTypeCustomConverter Tests

        #endregion ICustomConvertersRegistry Tests

        #region ICustomTypeNameMappersRegistry Tests

        #region RegisterCustomTypeNameMapper Tests

        [TestMethod]
        public void RegisterCustomTypeNameMapper_NoMappers_GetMappersReturnTheNewMapper()
        {
            // Arrange
            var mapper = new Mock<ITypeMapper>();
            var expectedMappers = new List<ITypeMapper>()
            {
                mapper.Object,
            };

            // Act
            var registry = container.RegisterCustomTypeNameMapper(mapper.Object);

            // Assert
            var actualMappers = container.GetCustomTypeNameMappers().ToList();
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        [TestMethod]
        public void RegisterCustomTypeNameMapper_SameMapperExists_GetMappersReturnTheAllMappers()
        {
            // Arrange
            var mapper = new Mock<ITypeMapper>();
            var expectedMappers = new List<ITypeMapper>()
            {
                mapper.Object, mapper.Object,
            };

            container.RegisterCustomTypeNameMapper(mapper.Object);

            // Act
            var registry = container.RegisterCustomTypeNameMapper(mapper.Object);

            // Assert
            var actualMappers = container.GetCustomTypeNameMappers().ToList();
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
            Assert.AreSame(container, registry);
        }

        [TestMethod]
        public void RegisterCustomTypeNameMapper_DifferentMapperExists_GetMappersReturnTheAllMappers()
        {
            // Arrange
            var mapper1 = new Mock<ITypeMapper>();
            var mapper2 = new Mock<ITypeMapper>();
            var expectedMappers = new List<ITypeMapper>()
            {
                mapper1.Object, mapper2.Object,
            };

            container.RegisterCustomTypeNameMapper(mapper1.Object);

            // Act
            var registry = container.RegisterCustomTypeNameMapper(mapper2.Object);

            // Assert
            var actualMappers = container.GetCustomTypeNameMappers().ToList();
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
            Assert.AreSame(container, registry);
        }

        #endregion RegisterCustomTypeNameMapper Tests

        #endregion ICustomTypeNameMappersRegistry Tests

        #region IProtoStylingConventionsStrategiesRegistry Tests

        #region RegisterProtoStylingStrategy Tests

        [TestMethod]
        public void RegisterProtoStylingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IProtoStylingStrategy>().Object;
            var strategyName = "a";

            // Act
            var registry = container.RegisterProtoStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetProtoStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterPackageStylingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualPackageStrategy = container.GetPackageStylingStrategy(strategyName);
            var actualProtoStrategy = container.GetProtoStylingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualPackageStrategy);
            Assert.AreSame(expectedStrategy, actualProtoStrategy);
            Assert.AreSame(container, registry);
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

        #endregion IProtoStylingConventionsStrategiesRegistry Tests

        #region INumberingStrategiesRegistry Tests

        #region RegisterEnumValueNumberingStrategy Tests

        [TestMethod]
        public void RegisterEnumValueNumberingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IEnumValueNumberingStrategy>().Object;
            var strategyName = "a";

            // Act
            var registry = container.RegisterEnumValueNumberingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetEnumValueNumberingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterFieldNumberingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetFieldNumberingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
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

        #region IExtractionStrategiesRegistry Tests

        #region RegisterFieldsAndPropertiesExtractionStrategy Tests

        [TestMethod]
        public void RegisterFieldsAndPropertiesExtractionStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IFieldsAndPropertiesExtractionStrategy>().Object;
            var strategyName = "a";

            // Act
            var registry = container.RegisterFieldsAndPropertiesExtractionStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetFieldsAndPropertiesExtractionStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
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

        #endregion IExtractionStrategiesRegistry Tests

        #region IProtoNamingStrategiesRegistry Tests

        #region RegisterFileNamingStrategy Tests

        [TestMethod]
        public void RegisterFileNamingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IFileNamingStrategy>().Object;
            var strategyName = "a";

            // Act
            var registry = container.RegisterFileNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetFileNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterPackageNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetPackageNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
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
            var registry = container.RegisterTypeNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetTypeNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
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

        #region INewTypeNamingStrategiesRegistry Tests

        #region RegisterParameterListNamingStrategy Tests

        [TestMethod]
        public void RegisterParameterListNamingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<IParameterListNamingStrategy>().Object;
            var strategyName = "a";

            // Act
            var registry = container.RegisterParameterListNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetParameterListNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
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

        #region RegisterNewTypeNamingStrategy Tests

        [TestMethod]
        public void RegisterNewTypeNamingStrategy_NoStrategyWithNewNameExists_TheNewStrategyIsRegistered()
        {
            // Arrange
            var expectedStrategy = new Mock<INewTypeNamingStrategy>().Object;
            var strategyName = "a";

            // Act
            var registry = container.RegisterNewTypeNamingStrategy(strategyName, expectedStrategy);

            // Assert
            var actualStrategy = container.GetNewTypeNamingStrategy(strategyName);
            Assert.AreSame(expectedStrategy, actualStrategy);
            Assert.AreSame(container, registry);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegisterNewTypeNamingStrategy_ThereExistsStrategyWithNewName_ThrowsArgumentException()
        {
            // Arrange
            var expectedStrategy = new Mock<INewTypeNamingStrategy>().Object;
            var strategyName = "a";
            container.RegisterNewTypeNamingStrategy(strategyName, expectedStrategy);

            // Act
            container.RegisterNewTypeNamingStrategy(strategyName, expectedStrategy);

            // Assert
            // Noting to do.
            // The ExpectedException attribute will do the assert.
        }

        #endregion RegisterNewTypeNamingStrategy Tests

        #endregion INewTypeNamingStrategiesRegistry Tests

        #endregion IRegistry Tests
    }
}
