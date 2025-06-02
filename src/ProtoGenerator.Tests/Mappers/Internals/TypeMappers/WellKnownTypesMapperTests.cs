using ProtoGenerator.Mappers.Internals.TypeMappers;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.ProtoDefinitions;

namespace ProtoGenerator.Tests.Mappers.Internals.TypeMappers
{
    [TestClass]
    public class WellKnownTypesMapperTests
    {
        private static Dictionary<Type, IProtoTypeMetadata> wellKnownTypesProtoMetadatas;

        private static WellKnownTypesMapper mapper;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            wellKnownTypesProtoMetadatas = new Dictionary<Type, IProtoTypeMetadata>
            {
                [typeof(int)] = new ProtoTypeMetadata() { Name = "a" },
                [typeof(object)] = new ProtoTypeMetadata() { Name = "b" },
            };

            mapper = new WellKnownTypesMapper(wellKnownTypesProtoMetadatas);
        }

        #region CanHandle Tests

        [TestMethod]
        public void CanHandle_CanNotHandleType_ReturnFalse()
        {
            // Arrange
            var type = GetType();

            // Act
            var actualResult = mapper.CanHandle(type);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [DataRow(typeof(int))]
        [DataRow(typeof(object))]
        [TestMethod]
        public void CanHandle_CanHandleType_ReturnTrue(Type type)
        {
            // Act
            var actualResult = mapper.CanHandle(type);

            // Assert
            Assert.IsTrue(actualResult);
        }

        #endregion CanHandle Tests

        #region MapTypeToProtoMetaData Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void MapTypeToProtoMetaData_CanNotHandleType_ThrownArgumentException()
        {
            // Arrange
            var type = GetType();

            // Act
            mapper.MapTypeToProtoMetaData(type);

            // Assert
            // Noting to do.
            // The ExpectedException will do the assert.
        }

        [DataRow(typeof(int))]
        [DataRow(typeof(object))]
        [TestMethod]
        public void MapTypeToProtoMetaData_CanHandleType_ReturnSameMetadata(Type type)
        {
            // Arrange
            var expectedMetadata = wellKnownTypesProtoMetadatas[type];

            // Act
            var actualMetadata = mapper.MapTypeToProtoMetaData(type);

            // Assert
            Assert.AreSame(expectedMetadata, actualMetadata);
        }

        #endregion MapTypeToProtoMetaData Tests
    }
}
