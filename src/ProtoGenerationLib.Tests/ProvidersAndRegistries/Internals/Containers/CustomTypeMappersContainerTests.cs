using Moq;
using ProtoGenerationLib.Mappers.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class CustomTypeMappersContainerTests
    {
        private CustomTypeMappersContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new CustomTypeMappersContainer();
        }

        #region ICustomTypeMappersProvider Tests

        #region GetCustomTypeMappers Tests

        [TestMethod]
        public void GetCustomTypeMappers_NoMappers_ReturnsEmptyEnumerable()
        {
            // Arrange
            var expectedMappers = new List<ITypeMapper>();

            // Act
            var actualMappers = container.GetCustomTypeMappers().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        [TestMethod]
        public void GetCustomTypeMappers_ThereAreMappers_ReturnsAllMappers()
        {
            // Arrange
            var mapper1 = new Mock<ITypeMapper>();
            var mapper2 = new Mock<ITypeMapper>();

            var expectedMappers = new List<ITypeMapper>
            {
                mapper1.Object, mapper2.Object
            };

            container.RegisterCustomTypeMapper(mapper1.Object);
            container.RegisterCustomTypeMapper(mapper2.Object);

            // Act
            var actualMappers = container.GetCustomTypeMappers().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        [TestMethod]
        public void GetCustomTypeMappers_SameMapperMultipleTimes_ReturnsAllMappersIncludingDuplicates()
        {
            // Arrange
            var mapper1 = new Mock<ITypeMapper>();
            var mapper2 = new Mock<ITypeMapper>();

            var expectedMappers = new List<ITypeMapper>
            {
                mapper1.Object, mapper2.Object, mapper1.Object
            };

            container.RegisterCustomTypeMapper(mapper1.Object);
            container.RegisterCustomTypeMapper(mapper2.Object);
            container.RegisterCustomTypeMapper(mapper1.Object);

            // Act
            var actualMappers = container.GetCustomTypeMappers().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        #endregion GetCustomTypeMappers Tests

        #endregion ICustomTypeMappersProvider Tests

        #region ICustomTypeMappersRegistry Tests

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
            container.RegisterCustomTypeMapper(mapper.Object);

            // Assert
            var actualMappers = container.GetCustomTypeMappers().ToList();
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

            container.RegisterCustomTypeMapper(mapper.Object);

            // Act
            container.RegisterCustomTypeMapper(mapper.Object);

            // Assert
            var actualMappers = container.GetCustomTypeMappers().ToList();
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
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

            container.RegisterCustomTypeMapper(mapper1.Object);

            // Act
            container.RegisterCustomTypeMapper(mapper2.Object);

            // Assert
            var actualMappers = container.GetCustomTypeMappers().ToList();
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        #endregion RegisterCustomTypeNameMapper Tests

        #endregion ICustomTypeMappersRegistry Tests
    }
}
