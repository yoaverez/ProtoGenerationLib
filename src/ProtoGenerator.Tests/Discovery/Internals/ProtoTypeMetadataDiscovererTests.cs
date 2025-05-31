using Moq;
using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Internals;
using ProtoGenerator.Discovery.Internals;
using ProtoGenerator.Mappers.Abstracts;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Tests.Discovery.Internals.DummyTypes;

namespace ProtoGenerator.Tests.Discovery.Internals
{
    [TestClass]
    public class ProtoTypeMetadataDiscovererTests
    {
        private static ProtoGeneratorConfiguration generationOptions;

        private Mock<IProvider> mockIProvider;

        private List<ITypeNameMapper> customMappers;

        private Mock<ITypeNamingStrategy> mockITypeNamingStrategy;

        private Mock<IPackageNamingStrategy> mockIPackageNamingStrategy;

        private Mock<IFileNamingStrategy> mockIFileNamingStrategy;

        private Mock<IProtoStylingStrategy> mockMessageStylingStrategy;

        private Mock<IProtoStylingStrategy> mockServiceStylingStrategy;

        private Mock<IProtoStylingStrategy> mockEnumStylingStrategy;

        private Mock<IProtoStylingStrategy> mockPackageStylingStrategy;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            generationOptions = new ProtoGeneratorConfiguration
            {
                ProtoNamingStrategiesOptions = new ProtoNamingStrategiesOptions
                {
                    TypeNamingStrategy = "1",
                    PackageNamingStrategy = "2",
                    FileNamingStrategy = "3",
                },
                ProtoStylingConventionsStrategiesOptions = new ProtoStylingConventionsStrategiesOptions
                {
                    MessageStylingStrategy = "4",
                    ServiceStylingStrategy = "5",
                    EnumStylingStrategy = "6",
                    PackageStylingStrategy = "7",
                },
                AnalysisOptions = new AnalysisOptions
                {
                    ProtoServiceAttribute = typeof(ProtoServiceAttribute),
                }
            };
        }

        [TestInitialize]
        public void TestInitialize()
        {
            customMappers = new List<ITypeNameMapper>();
            mockIProvider = new Mock<IProvider>();
            mockIProvider.Setup(provider => provider.GetCustomTypeNameMappers())
                         .Returns(customMappers);

            // SetUp strategies.
            mockITypeNamingStrategy = new Mock<ITypeNamingStrategy>();
            mockIProvider.Setup(provider => provider.GetTypeNamingStrategy("1"))
                         .Returns(mockITypeNamingStrategy.Object);

            mockIPackageNamingStrategy = new Mock<IPackageNamingStrategy>();
            mockIProvider.Setup(provider => provider.GetPackageNamingStrategy("2"))
                         .Returns(mockIPackageNamingStrategy.Object);

            mockIFileNamingStrategy = new Mock<IFileNamingStrategy>();
            mockIProvider.Setup(provider => provider.GetFileNamingStrategy("3"))
                         .Returns(mockIFileNamingStrategy.Object);

            mockMessageStylingStrategy = new Mock<IProtoStylingStrategy>();
            mockIProvider.Setup(provider => provider.GetMessageStylingStrategy("4"))
                         .Returns(mockMessageStylingStrategy.Object);

            mockServiceStylingStrategy = new Mock<IProtoStylingStrategy>();
            mockIProvider.Setup(provider => provider.GetServiceStylingStrategy("5"))
                         .Returns(mockServiceStylingStrategy.Object);

            mockEnumStylingStrategy = new Mock<IProtoStylingStrategy>();
            mockIProvider.Setup(provider => provider.GetEnumStylingStrategy("6"))
                         .Returns(mockEnumStylingStrategy.Object);

            mockPackageStylingStrategy = new Mock<IProtoStylingStrategy>();
            mockIProvider.Setup(provider => provider.GetPackageStylingStrategy("7"))
                         .Returns(mockPackageStylingStrategy.Object);
        }

        [TestMethod]
        public void DiscoverProtosMetadata_TypesAreEmptyEnumerable_ReturnsEmptyEnumerables()
        {
            // Arrange
            var types = new List<Type>();
            var discoverer = CreateDiscoverer(mockIProvider.Object);

            // Act
            var typeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            Assert.AreEqual(0, typeToProtoMetadataMapping.Count);
        }

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void DiscoverProtosMetadata_TypeCanBeHandledByDefaultAndCustomTypeMapper_DefaultTypeMapperHandlesType(int mapperHandlerIndex)
        {
            // Arrange
            var type = typeof(int);
            var types = new List<Type> { type };

            var correctBaseMetadata = new ProtoTypeBaseMetadata("a", "b.c", "/path");
            var incorrectBaseMetadata = new ProtoTypeBaseMetadata("z", "x.y", "");

            var correctMetadata = new ProtoTypeMetadata(correctBaseMetadata, "b.c.a");
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = correctMetadata
            };

            var defaultTypeMappers = new List<ITypeNameMapper>();
            var numberOfDefaultHandlers = 3;
            for (int i = 0; i < numberOfDefaultHandlers; i++)
            {
                Mock<ITypeNameMapper> defaultTypeMapper;
                if (i < mapperHandlerIndex % numberOfDefaultHandlers)
                {
                    defaultTypeMapper = CreateAndSetUpITypeMapperMock(false);
                }
                else if (i == mapperHandlerIndex % numberOfDefaultHandlers)
                {
                    defaultTypeMapper = CreateAndSetUpITypeMapperMock(true, correctBaseMetadata);
                }
                else
                {
                    defaultTypeMapper = CreateAndSetUpITypeMapperMock(true, incorrectBaseMetadata);
                }
                defaultTypeMappers.Add(defaultTypeMapper.Object);
            }

            var customTypeMapper = CreateAndSetUpITypeMapperMock(true, incorrectBaseMetadata);
            customMappers.Add(customTypeMapper.Object);

            var discoverer = CreateDiscoverer(mockIProvider.Object, defaultTypeMappers);

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void DiscoverProtosMetadata_TypeCanBeHandledByCustomMapperOnly_FirstCustomTypeMapperHandlesType(int mapperHandlerIndex)
        {
            // Arrange
            var type = typeof(int);
            var types = new List<Type> { type };

            var correctBaseMetadata = new ProtoTypeBaseMetadata("a", "b.c", "/path");
            var incorrectBaseMetadata = new ProtoTypeBaseMetadata("z", "x.y", "");

            var correctMetadata = new ProtoTypeMetadata(correctBaseMetadata, "b.c.a");
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = correctMetadata
            };

            var numberOfCustomHandlers = 3;
            for (int i = 0; i < numberOfCustomHandlers; i++)
            {
                Mock<ITypeNameMapper> customTypeMapper;
                if (i < mapperHandlerIndex % numberOfCustomHandlers)
                {
                    customTypeMapper = CreateAndSetUpITypeMapperMock(false);
                }
                else if (i == mapperHandlerIndex % numberOfCustomHandlers)
                {
                    customTypeMapper = CreateAndSetUpITypeMapperMock(true, correctBaseMetadata);
                }
                else
                {
                    customTypeMapper = CreateAndSetUpITypeMapperMock(true, incorrectBaseMetadata);
                }
                customMappers.Add(customTypeMapper.Object);
            }

            var defaultTypeMapper = CreateAndSetUpITypeMapperMock(false, incorrectBaseMetadata);

            var discoverer = CreateDiscoverer(mockIProvider.Object, new List<ITypeNameMapper> { defaultTypeMapper.Object });

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [DynamicData(nameof(GetAllMissingPropsOptions), DynamicDataSourceType.Method)]
        [TestMethod]
        public void DiscoverProtosMetadata_TypeCanBeHandledByCustomMapperOnlyAndCustomMapperDidNotFillAllProps_MissingPropsAreFilled(IProtoTypeBaseMetadata protoTypeBaseMetadata)
        {
            // Arrange
            var type = typeof(int);
            var types = new List<Type> { type };

            var filledTypeName = "type name";
            var filledPackageName = new string[] { "package", "name" };
            var filledFilePath = "file path";
            SetUpStrategies(filledTypeName, filledPackageName, filledFilePath, str => str.ToUpperInvariant(), strs => string.Join(".", strs));

            var correctBaseMetadata = new ProtoTypeBaseMetadata(protoTypeBaseMetadata);
            if (protoTypeBaseMetadata.Name is null)
                correctBaseMetadata.Name = filledTypeName.ToUpperInvariant();

            if (protoTypeBaseMetadata.Package is null)
                correctBaseMetadata.Package = string.Join(".", filledPackageName);

            if (protoTypeBaseMetadata.FilePath is null)
                correctBaseMetadata.FilePath = filledFilePath;

            var correctMetadata = new ProtoTypeMetadata(correctBaseMetadata, $"{correctBaseMetadata.Package}.{correctBaseMetadata.Name}");
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = correctMetadata
            };

            customMappers.Add(CreateAndSetUpITypeMapperMock(true, protoTypeBaseMetadata).Object);

            var discoverer = CreateDiscoverer(mockIProvider.Object);

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [DynamicData(nameof(GetAllMissingPropsOptions), DynamicDataSourceType.Method)]
        [TestMethod]
        public void DiscoverProtosMetadata_TypeCanBeHandledByDefaultMapperOnlyAndCustomMapperDidNotFillAllProps_MissingPropsAreFilled(IProtoTypeBaseMetadata protoTypeBaseMetadata)
        {
            // Arrange
            var type = typeof(int);
            var types = new List<Type> { type };

            var filledTypeName = "type name";
            var filledPackageName = new string[] { "package", "name" };
            var filledFilePath = "file path";
            SetUpStrategies(filledTypeName, filledPackageName, filledFilePath, str => str.ToUpperInvariant(), strs => string.Join(".", strs));

            var correctBaseMetadata = new ProtoTypeBaseMetadata(protoTypeBaseMetadata);
            if (protoTypeBaseMetadata.Name is null)
                correctBaseMetadata.Name = filledTypeName.ToUpperInvariant();

            if (protoTypeBaseMetadata.Package is null)
                correctBaseMetadata.Package = string.Join(".", filledPackageName);

            if (protoTypeBaseMetadata.FilePath is null)
                correctBaseMetadata.FilePath = filledFilePath;

            var correctMetadata = new ProtoTypeMetadata(correctBaseMetadata, $"{correctBaseMetadata.Package}.{correctBaseMetadata.Name}");
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = correctMetadata
            };

            var defaultMapper = CreateAndSetUpITypeMapperMock(true, protoTypeBaseMetadata);

            var discoverer = CreateDiscoverer(mockIProvider.Object, new ITypeNameMapper[] { defaultMapper.Object });

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [TestMethod]
        public void DiscoverProtosMetadata_TypeCanNotBeHandledByDefaultOrCustomMapper_FillsAllTheDataUsingStrategies()
        {
            // Arrange
            var type = typeof(int);
            var types = new List<Type> { type };

            var filledTypeName = "type name";
            var filledPackageName = new string[] { "package", "name" };
            var filledFilePath = "file path";
            SetUpStrategies(filledTypeName, filledPackageName, filledFilePath, str => str.ToUpperInvariant(), strs => string.Join(".", strs));

            var correctBaseMetadata = new ProtoTypeBaseMetadata(filledTypeName.ToUpperInvariant(), string.Join(".", filledPackageName), filledFilePath);
            var correctMetadata = new ProtoTypeMetadata(correctBaseMetadata, $"{correctBaseMetadata.Package}.{correctBaseMetadata.Name}");
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = correctMetadata
            };

            var discoverer = CreateDiscoverer(mockIProvider.Object);

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [TestMethod]
        public void DiscoverProtosMetadata_TypeIsNestedButItDeclaringTypeDoesNotExists_TypeIsNotConsideredNested()
        {
            // Arrange
            var type = typeof(TypeWithNestedTypes.NestedType1.NestedType2);
            var types = new List<Type> { type };

            var correctBaseMetadata = new ProtoTypeBaseMetadata("a", "b", "c");
            var correctMetadata = new ProtoTypeMetadata(correctBaseMetadata,
                                                        $"{correctBaseMetadata.Package}.{correctBaseMetadata.Name}",
                                                        isNested: false,
                                                        nestedTypes: new HashSet<Type>());
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type] = correctMetadata
            };

            customMappers.Add(CreateAndSetUpITypeMapperMock(true, correctBaseMetadata).Object);

            var discoverer = CreateDiscoverer(mockIProvider.Object);

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEqual(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [TestMethod]
        public void DiscoverProtosMetadata_TypeIsNestedAndItDeclaringTypeDoesExists_TypeIsConsideredNested()
        {
            // Arrange
            var type1 = typeof(TypeWithNestedTypes);
            var type2 = typeof(TypeWithNestedTypes.NestedType1);
            var types = new List<Type> { type1, type2 };

            var correctBaseMetadata1 = new ProtoTypeBaseMetadata("a", "b", "c");
            var correctMetadata1 = new ProtoTypeMetadata(correctBaseMetadata1,
                                                         $"{correctBaseMetadata1.Package}.{correctBaseMetadata1.Name}",
                                                         isNested: false,
                                                         nestedTypes: new HashSet<Type> { type2 });

            var correctBaseMetadata2 = new ProtoTypeBaseMetadata("aa", "b", "c");
            var correctMetadata2 = new ProtoTypeMetadata(correctBaseMetadata2,
                                                         $"{correctBaseMetadata2.Package}.{correctBaseMetadata1.Name}.{correctBaseMetadata2.Name}",
                                                         isNested: true,
                                                         nestedTypes: new HashSet<Type>());
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type1] = correctMetadata1,
                [type2] = correctMetadata2,
            };

            customMappers.Add(CreateAndSetUpITypeMapperMock(type1, correctBaseMetadata1).Object);
            customMappers.Add(CreateAndSetUpITypeMapperMock(type2, correctBaseMetadata2).Object);

            var discoverer = CreateDiscoverer(mockIProvider.Object);

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [TestMethod]
        public void DiscoverProtosMetadata_TypeIsNested2DegreeAndItDeclaringTypeDoesExists_TypeIsConsideredNested()
        {
            // Arrange
            var type1 = typeof(TypeWithNestedTypes);
            var type2 = typeof(TypeWithNestedTypes.NestedType1);
            var type3 = typeof(TypeWithNestedTypes.NestedType1.NestedType2);
            var types = new List<Type> { type1, type2, type3 };

            var correctBaseMetadata1 = new ProtoTypeBaseMetadata("a", "b", "c");
            var correctMetadata1 = new ProtoTypeMetadata(correctBaseMetadata1,
                                                         $"{correctBaseMetadata1.Package}.{correctBaseMetadata1.Name}",
                                                         isNested: false,
                                                         nestedTypes: new HashSet<Type> { type2 });

            var correctBaseMetadata2 = new ProtoTypeBaseMetadata("aa", "b", "c");
            var correctMetadata2 = new ProtoTypeMetadata(correctBaseMetadata2,
                                                         $"{correctBaseMetadata2.Package}.{correctBaseMetadata1.Name}.{correctBaseMetadata2.Name}",
                                                         isNested: true,
                                                         nestedTypes: new HashSet<Type> { type3 });

            var correctBaseMetadata3 = new ProtoTypeBaseMetadata("aaa", "b", "c");
            var correctMetadata3 = new ProtoTypeMetadata(correctBaseMetadata3,
                                                         $"{correctBaseMetadata3.Package}.{correctBaseMetadata1.Name}.{correctBaseMetadata2.Name}.{correctBaseMetadata3.Name}",
                                                         isNested: true,
                                                         nestedTypes: new HashSet<Type>());
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type1] = correctMetadata1,
                [type2] = correctMetadata2,
                [type3] = correctMetadata3,
            };

            customMappers.Add(CreateAndSetUpITypeMapperMock(type1, correctBaseMetadata1).Object);
            customMappers.Add(CreateAndSetUpITypeMapperMock(type2, correctBaseMetadata2).Object);
            customMappers.Add(CreateAndSetUpITypeMapperMock(type3, correctBaseMetadata3).Object);

            var discoverer = CreateDiscoverer(mockIProvider.Object);

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [TestMethod]
        public void DiscoverProtosMetadata_TypeIsNestedButItsDeclaringTypePackageIsDifferent_TypeIsNotConsideredNested()
        {
            // Arrange
            var type1 = typeof(TypeWithNestedTypes);
            var type2 = typeof(TypeWithNestedTypes.NestedType1);
            var types = new List<Type> { type1, type2 };

            var correctBaseMetadata1 = new ProtoTypeBaseMetadata("a", "b", "c");
            var correctMetadata1 = new ProtoTypeMetadata(correctBaseMetadata1,
                                                         $"{correctBaseMetadata1.Package}.{correctBaseMetadata1.Name}",
                                                         isNested: false,
                                                         nestedTypes: new HashSet<Type>());

            var correctBaseMetadata2 = new ProtoTypeBaseMetadata("aa", "bb", "c");
            var correctMetadata2 = new ProtoTypeMetadata(correctBaseMetadata2,
                                                         $"{correctBaseMetadata2.Package}.{correctBaseMetadata2.Name}",
                                                         isNested: false,
                                                         nestedTypes: new HashSet<Type>());
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type1] = correctMetadata1,
                [type2] = correctMetadata2,
            };

            customMappers.Add(CreateAndSetUpITypeMapperMock(type1, correctBaseMetadata1).Object);
            customMappers.Add(CreateAndSetUpITypeMapperMock(type2, correctBaseMetadata2).Object);

            var discoverer = CreateDiscoverer(mockIProvider.Object);

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        [TestMethod]
        public void DiscoverProtosMetadata_TypeIsNestedButItsDeclaringTypeFileIsDifferent_TypeIsNotConsideredNested()
        {
            // Arrange
            var type1 = typeof(TypeWithNestedTypes);
            var type2 = typeof(TypeWithNestedTypes.NestedType1);
            var types = new List<Type> { type1, type2 };

            var correctBaseMetadata1 = new ProtoTypeBaseMetadata("a", "b", "c");
            var correctMetadata1 = new ProtoTypeMetadata(correctBaseMetadata1,
                                                         $"{correctBaseMetadata1.Package}.{correctBaseMetadata1.Name}",
                                                         isNested: false,
                                                         nestedTypes: new HashSet<Type>());

            var correctBaseMetadata2 = new ProtoTypeBaseMetadata("aa", "b", "cc");
            var correctMetadata2 = new ProtoTypeMetadata(correctBaseMetadata2,
                                                         $"{correctBaseMetadata2.Package}.{correctBaseMetadata2.Name}",
                                                         isNested: false,
                                                         nestedTypes: new HashSet<Type>());
            var expectedTypeToProtoMetadataMapping = new Dictionary<Type, IProtoTypeMetadata>
            {
                [type1] = correctMetadata1,
                [type2] = correctMetadata2,
            };

            customMappers.Add(CreateAndSetUpITypeMapperMock(type1, correctBaseMetadata1).Object);
            customMappers.Add(CreateAndSetUpITypeMapperMock(type2, correctBaseMetadata2).Object);

            var discoverer = CreateDiscoverer(mockIProvider.Object);

            // Act
            var actualTypeToProtoMetadataMapping = discoverer.DiscoverProtosMetadata(types, generationOptions);

            // Assert
            CollectionAssert.AreEquivalent(expectedTypeToProtoMetadataMapping, actualTypeToProtoMetadataMapping.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        #region Auxiliary Methods

        private ProtoTypeMetadataDiscoverer CreateDiscoverer(IProvider provider, IEnumerable<ITypeNameMapper>? defaultTypeMappers = null)
        {
            return new ProtoTypeMetadataDiscoverer(provider, defaultTypeMappers ?? new List<ITypeNameMapper>());
        }

        private Mock<ITypeNameMapper> CreateAndSetUpITypeMapperMock(bool canHandle, IProtoTypeBaseMetadata? protoTypeBaseMetadata = null)
        {
            var mock = new Mock<ITypeNameMapper>();
            mock.Setup(mapper => mapper.CanHandle(It.IsAny<Type>()))
                .Returns(canHandle);

            if (protoTypeBaseMetadata is not null)
            {
                mock.Setup(mapper => mapper.MapTypeToProtoMetaData(It.IsAny<Type>()))
                    .Returns(protoTypeBaseMetadata);
            }

            return mock;
        }

        private Mock<ITypeNameMapper> CreateAndSetUpITypeMapperMock(Type canHandle, IProtoTypeBaseMetadata? protoTypeBaseMetadata = null)
        {
            var mock = new Mock<ITypeNameMapper>();
            mock.Setup(mapper => mapper.CanHandle(canHandle))
                .Returns(true);

            if (protoTypeBaseMetadata is not null)
            {
                mock.Setup(mapper => mapper.MapTypeToProtoMetaData(It.IsAny<Type>()))
                    .Returns(protoTypeBaseMetadata);
            }

            return mock;
        }

        private void SetUpStrategies(string typeName,
                                     string[] PackageComponents,
                                     string filePath,
                                     Func<string, string> namesStylingStrategy,
                                     Func<string[], string> packageStylingStrategy)
        {
            mockITypeNamingStrategy.Setup(strategy => strategy.GetTypeName(It.IsAny<Type>()))
                                   .Returns(typeName);

            mockIPackageNamingStrategy.Setup(strategy => strategy.GetPackageComponents(It.IsAny<Type>()))
                                      .Returns(PackageComponents);

            mockIFileNamingStrategy.Setup(strategy => strategy.GetFilePath(It.IsAny<Type>()))
                                   .Returns(filePath);

            mockMessageStylingStrategy.Setup(strategy => strategy.ToProtoStyle(It.IsAny<string>()))
                                      .Returns<string>(name => namesStylingStrategy(name));

            mockServiceStylingStrategy.Setup(strategy => strategy.ToProtoStyle(It.IsAny<string>()))
                                      .Returns<string>(name => namesStylingStrategy(name));

            mockEnumStylingStrategy.Setup(strategy => strategy.ToProtoStyle(It.IsAny<string>()))
                                   .Returns<string>(name => namesStylingStrategy(name));

            mockPackageStylingStrategy.Setup(strategy => strategy.ToProtoStyle(It.IsAny<string[]>()))
                                      .Returns<string[]>(name => packageStylingStrategy(name));
        }

        #endregion Auxiliary Methods

        #region Tests Methods Parameters Suppliers

        private static IEnumerable<object[]> GetAllMissingPropsOptions()
        {
            var baseMetadataType = typeof(DummyIProtoTypeBaseMetadata);
            var props = baseMetadataType.GetProperties();
            var numOfProps = props.Length;

            // Since each prop can be either missing or not missing,
            // the number of options is 2^numOfProps.
            var numOfOptions = Convert.ToInt32(Math.Pow(2, numOfProps));

            string? nullValue = null;
            var value = "a";

            Func<string?, string?> ChangeValue = val => val is null ? value : nullValue;
            var propsValues = new string?[] { value, value, value };

            var baseMetadatas = new List<IProtoTypeBaseMetadata>();
            for (int i = 0; i < numOfOptions; i++)
            {
                var baseMetadata = new DummyIProtoTypeBaseMetadata();
                for (var propIdx = 0; propIdx < numOfProps; propIdx++)
                {
                    props[propIdx].SetValue(baseMetadata, propsValues[propIdx]);

                    var twoPowerOfIdx = Convert.ToInt32(Math.Pow(2, propIdx));
                    if (i % twoPowerOfIdx == 0)
                    {
                        propsValues[propIdx] = ChangeValue(propsValues[propIdx]);
                    }
                }
                baseMetadatas.Add(baseMetadata);
            }
            return baseMetadatas.Select(x => new object[] { x }).ToArray();
        }

        #endregion Tests Methods Parameters Suppliers
    }
}
