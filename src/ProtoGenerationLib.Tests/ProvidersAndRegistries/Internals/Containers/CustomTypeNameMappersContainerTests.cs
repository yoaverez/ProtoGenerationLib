using Moq;
using ProtoGenerationLib.Mappers.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals.Containers
{
    [TestClass]
    public class CustomTypeNameMappersContainerTests
    {
        private CustomTypeNameMappersContainer container;

        [TestInitialize]
        public void TestInitialize()
        {
            container = new CustomTypeNameMappersContainer();
        }

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
            container.RegisterCustomTypeNameMapper(mapper.Object);

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
            container.RegisterCustomTypeNameMapper(mapper.Object);

            // Assert
            var actualMappers = container.GetCustomTypeNameMappers().ToList();
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

            container.RegisterCustomTypeNameMapper(mapper1.Object);

            // Act
            container.RegisterCustomTypeNameMapper(mapper2.Object);

            // Assert
            var actualMappers = container.GetCustomTypeNameMappers().ToList();
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        #endregion RegisterCustomTypeNameMapper Tests

        #endregion ICustomTypeNameMappersRegistry Tests
    }
}
