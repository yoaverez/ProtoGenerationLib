using Moq;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Tests.Configurations.Internals.DummyTypes;
using ProtoGenerationLib.Tests.ProvidersAndRegistries.Internals.Containers.DummyTypes;

namespace ProtoGenerationLib.Tests.Configurations.Internals
{
    [TestClass]
    public class ProtoGenerationOptionsTests
    {
        private ProtoGenerationOptions generationOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            generationOptions = new ProtoGenerationOptions();
        }

        #region GetCustomTypesExtractors Tests

        [TestMethod]
        public void GetCustomTypesExtractors_NoCustomExtractors_ReturnEmptyEnumerable()
        {
            // Assert
            var expectedExtractors = new List<ICustomTypesExtractor>();

            // Act
            var actualExtractors = generationOptions.GetCustomTypesExtractors().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedExtractors, actualExtractors);
        }

        [TestMethod]
        public void GetCustomTypesExtractors_MultipleCustomConvertersExists_ReturnCustomExtractorsInCorrectOrder()
        {
            // Assert
            var mockContractCustomExtractor1 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var mockContractCustomExtractor2 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            generationOptions.ContractTypeCustomConverters.Add(mockContractCustomExtractor1.Object);
            generationOptions.ContractTypeCustomConverters.Add(mockContractCustomExtractor2.Object);

            var mockDataCustomExtractor1 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var mockDataCustomExtractor2 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            generationOptions.DataTypeCustomConverters.Add(mockDataCustomExtractor1.Object);
            generationOptions.DataTypeCustomConverters.Add(mockDataCustomExtractor2.Object);

            var mockEnumCustomExtractor1 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            var mockEnumCustomExtractor2 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            generationOptions.EnumTypeCustomConverters.Add(mockEnumCustomExtractor1.Object);
            generationOptions.EnumTypeCustomConverters.Add(mockEnumCustomExtractor2.Object);

            var expectedExtractors = new List<ICustomTypesExtractor>
            {
                mockContractCustomExtractor1.Object,
                mockContractCustomExtractor2.Object,
                mockDataCustomExtractor1.Object,
                mockDataCustomExtractor2.Object,
                mockEnumCustomExtractor1.Object,
                mockEnumCustomExtractor2.Object,
            };

            // Act
            var actualExtractors = generationOptions.GetCustomTypesExtractors().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedExtractors, actualExtractors);
        }

        #endregion GetCustomTypesExtractors Tests

        #region GetContractTypeCustomConverters Tests

        [TestMethod]
        public void GetContractTypeCustomConverters_NoCustomConverters_ReturnEmptyEnumerable()
        {
            // Assert
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();

            // Act
            var actualConventers = generationOptions.GetContractTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConventers);
        }

        [TestMethod]
        public void GetContractTypeCustomConverters_MultipleCustomConvertersExists_ReturnCustomConvertersInCorrectOrder()
        {
            // Assert
            var mockCustomExtractor1 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            var mockCustomExtractor2 = new Mock<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            generationOptions.ContractTypeCustomConverters.Add(mockCustomExtractor1.Object);
            generationOptions.ContractTypeCustomConverters.Add(mockCustomExtractor2.Object);

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>
            {
                mockCustomExtractor1.Object,
                mockCustomExtractor2.Object,
            };

            // Act
            var actualConverters = generationOptions.GetContractTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion GetContractTypeCustomConverters Tests

        #region GetDataTypeCustomConverters Tests

        [TestMethod]
        public void GetDataTypeCustomConverters_NoCustomConverters_ReturnEmptyEnumerable()
        {
            // Assert
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();

            // Act
            var actualConventers = generationOptions.GetDataTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConventers);
        }

        [TestMethod]
        public void GetDataTypeCustomConverters_MultipleCustomConvertersExists_ReturnCustomConvertersInCorrectOrder()
        {
            // Assert
            var mockCustomExtractor1 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            var mockCustomExtractor2 = new Mock<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            generationOptions.DataTypeCustomConverters.Add(mockCustomExtractor1.Object);
            generationOptions.DataTypeCustomConverters.Add(mockCustomExtractor2.Object);

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>
            {
                mockCustomExtractor1.Object,
                mockCustomExtractor2.Object,
            };

            // Act
            var actualConverters = generationOptions.GetDataTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion GetDataTypeCustomConverters Tests

        #region GetEnumTypeCustomConverters Tests

        [TestMethod]
        public void GetEnumTypeCustomConverters_NoCustomConverters_ReturnEmptyEnumerable()
        {
            // Assert
            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();

            // Act
            var actualConventers = generationOptions.GetEnumTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConventers);
        }

        [TestMethod]
        public void GetEnumTypeCustomConverters_MultipleCustomConvertersExists_ReturnCustomConvertersInCorrectOrder()
        {
            // Assert
            var mockCustomExtractor1 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            var mockCustomExtractor2 = new Mock<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            generationOptions.EnumTypeCustomConverters.Add(mockCustomExtractor1.Object);
            generationOptions.EnumTypeCustomConverters.Add(mockCustomExtractor2.Object);

            var expectedConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>
            {
                mockCustomExtractor1.Object,
                mockCustomExtractor2.Object,
            };

            // Act
            var actualConverters = generationOptions.GetEnumTypeCustomConverters().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConverters);
        }

        #endregion GetEnumTypeCustomConverters Tests

        #region GetCustomTypeMappers Tests

        [TestMethod]
        public void GetCustomTypeMappers_NoCustomMappers_ReturnEmptyEnumerable()
        {
            // Assert
            var expectedConverters = new List<ICustomTypeMapper>();

            // Act
            var actualConventers = generationOptions.GetCustomTypeMappers().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedConverters, actualConventers);
        }

        [TestMethod]
        public void GetCustomTypeMappers_MultipleCustomMappersExists_ReturnCustomMappersInCorrectOrder()
        {
            // Assert
            var mockCustomMapper1 = new Mock<ICustomTypeMapper>();
            var mockCustomMapper2 = new Mock<ICustomTypeMapper>();
            generationOptions.CustomTypeMappers.Add(mockCustomMapper1.Object);
            generationOptions.CustomTypeMappers.Add(mockCustomMapper2.Object);

            var expectedMappers = new List<ICustomTypeMapper>
            {
                mockCustomMapper1.Object,
                mockCustomMapper2.Object,
            };

            // Act
            var actualMappers = generationOptions.GetCustomTypeMappers().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedMappers, actualMappers);
        }

        #endregion GetCustomTypeMappers Tests

        #region ISuffixesProviderAndRegister Tests

        [TestMethod]
        public void GetFieldSuffixProviderAndAllTheRegisterMethods_ProvideSuffixesCorrecly()
        {
            // Arrange
            generationOptions.AddFieldSuffix<int>("AllIntSuffix");
            generationOptions.AddFieldSuffix<DummyType1, int>("SpecificTypeIntSuffix");
            generationOptions.AddFieldThatShouldNotHaveSuffix<DummyType1, int>(nameof(DummyType1.IntProp2));

            generationOptions.AddFieldSuffix<DummyType1, string>("SpecificTypeStringSuffix");

            generationOptions.AddFieldSuffix<bool>("AllBoolSuffix");

            generationOptions.AddFieldSuffix<DummyType1>(nameof(DummyType1.CharProp1), "CharProp1Suffix");

            // Act
            var doesIntProp1hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType1), typeof(int), nameof(DummyType1.IntProp1), out var intProp1Suffix);
            var doesIntProp2hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType1), typeof(int), nameof(DummyType1.IntProp2), out _);
            var doesStringProp1hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType1), typeof(string), nameof(DummyType1.StringProp1), out var stringProp1Suffix);
            var doesStringProp2hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType1), typeof(string), nameof(DummyType1.StringProp2), out var stringProp2Suffix);
            var doesBoolProp1hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType1), typeof(bool), nameof(DummyType1.BoolProp1), out var boolProp1Suffix);
            var doesBoolProp2hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType1), typeof(bool), nameof(DummyType1.BoolProp2), out var boolProp2Suffix);
            var doesCharProp1hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType1), typeof(char), nameof(DummyType1.CharProp1), out var charProp1Suffix);
            var doesCharProp2hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType1), typeof(char), nameof(DummyType1.CharProp2), out _);

            // Check Dummy2
            var doesDummy2IntProp1hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType2), typeof(int), nameof(DummyType2.IntProp1), out var dummy2IntProp1Suffix);
            var doesDummy2IntProp2hasSuffix = generationOptions.TryGetFieldSuffix(typeof(DummyType2), typeof(int), nameof(DummyType2.IntProp2), out var dummy2IntProp2Suffix);

            // Assert
            Assert.IsTrue(doesIntProp1hasSuffix);
            Assert.AreEqual("SpecificTypeIntSuffix", intProp1Suffix);

            Assert.IsFalse(doesIntProp2hasSuffix);

            Assert.IsTrue(doesStringProp1hasSuffix);
            Assert.AreEqual("SpecificTypeStringSuffix", stringProp1Suffix);

            Assert.IsTrue(doesStringProp2hasSuffix);
            Assert.AreEqual("SpecificTypeStringSuffix", stringProp2Suffix);

            Assert.IsTrue(doesBoolProp1hasSuffix);
            Assert.AreEqual("AllBoolSuffix", boolProp1Suffix);

            Assert.IsTrue(doesBoolProp2hasSuffix);
            Assert.AreEqual("AllBoolSuffix", boolProp2Suffix);

            Assert.IsTrue(doesCharProp1hasSuffix);
            Assert.AreEqual("CharProp1Suffix", charProp1Suffix);

            Assert.IsFalse(doesCharProp2hasSuffix);

            Assert.IsTrue(doesDummy2IntProp1hasSuffix);
            Assert.AreEqual("AllIntSuffix", dummy2IntProp1Suffix);

            Assert.IsTrue(doesDummy2IntProp2hasSuffix);
            Assert.AreEqual("AllIntSuffix", dummy2IntProp2Suffix);
        }

        #endregion ISuffixesProviderAndRegister Tests

        #region IDocumentationAdder Tests

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AddTypeDocumentation_ThereIsAlreadyDocumentationForType_ThrowsArgumentException()
        {
            // Arrange
            generationOptions.AddDocumentation<int>("abc");

            // Act
            generationOptions.AddDocumentation<int>("");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will assert the test.
        }

        [TestMethod]
        public void AddTypeDocumentation_ThereIsNotAlreadyDocumentationForType_TypeIsAssociatedWithDocumentation()
        {
            // Arrange
            var expectedDocs = "abc";

            // Act
            generationOptions.AddDocumentation<int>(expectedDocs);

            // Assert
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetTypeDocumentation(typeof(int), out var actualDocs);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDocs, actualDocs);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AddFieldDocumentation_ThereIsAlreadyDocumentationForField_ThrowsArgumentException()
        {
            // Arrange
            generationOptions.AddDocumentation<DummyDataType>(nameof(DummyDataType.Prop1), "abc");

            // Act
            generationOptions.AddDocumentation<DummyDataType>(nameof(DummyDataType.Prop1), "");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will assert the test.
        }

        [TestMethod]
        public void AddFieldDocumentation_ThereIsNotAlreadyDocumentationForField_FieldIsAssociatedWithDocumentation()
        {
            // Arrange
            var expectedDocs = "abc";

            // Act
            generationOptions.AddDocumentation<DummyDataType>(nameof(DummyDataType.Prop1) ,expectedDocs);

            // Assert
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetFieldDocumentation(typeof(DummyDataType), nameof(DummyDataType.Prop1), out var actualDocs);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDocs, actualDocs);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AddMethodDocumentation_ThereIsAlreadyDocumentationForMethod_ThrowsArgumentException()
        {
            // Arrange
            generationOptions.AddDocumentation<IDummyContractType>(nameof(IDummyContractType.Method), 2, "abc");

            // Act
            generationOptions.AddDocumentation<IDummyContractType>(nameof(IDummyContractType.Method), 2, "");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will assert the test.
        }

        [TestMethod]
        public void AddMethodDocumentation_ThereIsNotAlreadyDocumentationForMethod_MethodIsAssociatedWithDocumentation()
        {
            // Arrange
            var expectedDocs = "abc";

            // Act
            generationOptions.AddDocumentation<IDummyContractType>(nameof(IDummyContractType.Method), 2, expectedDocs);

            // Assert
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetMethodDocumentation(typeof(IDummyContractType), nameof(IDummyContractType.Method), 2, out var actualDocs);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDocs, actualDocs);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AddEnumValueDocumentation_ThereIsAlreadyDocumentationForEnumValue_ThrowsArgumentException()
        {
            // Arrange
            generationOptions.AddDocumentation<DummyEnum>((int)DummyEnum.Value1, "abc");

            // Act
            generationOptions.AddDocumentation<DummyEnum>((int)DummyEnum.Value1, "");

            // Assert
            // Noting to do.
            // The ExpectedException attribute will assert the test.
        }

        [TestMethod]
        public void AddEnumValueDocumentation_ThereIsNotAlreadyDocumentationForEnumValue_EnumValueIsAssociatedWithDocumentation()
        {
            // Arrange
            var expectedDocs = "abc";

            // Act
            generationOptions.AddDocumentation<DummyEnum>((int)DummyEnum.Value1, expectedDocs);

            // Assert
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetEnumValueDocumentation(typeof(DummyEnum), (int)DummyEnum.Value1, out var actualDocs);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDocs, actualDocs);
        }

        #endregion IDocumentationAdder Tests

        #region IDocumentationProvider Tests

        [TestMethod]
        public void TryGetTypeDocumentation_TypeHasNoAssociatedDocumentation_ReturnFalse()
        {
            // Act
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetTypeDocumentation(typeof(int), out _);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryGetTypeDocumentation_TypeHasAssociatedDocumentation_ReturnTrueAndAssociatedDocumentation()
        {
            // Arrange
            var expectedDocumentation = "abc";
            generationOptions.AddDocumentation<int>(expectedDocumentation);

            // Act
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetTypeDocumentation(typeof(int), out var actualDocumentation);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        [TestMethod]
        public void TryGetFieldDocumentation_FieldHasNoAssociatedDocumentation_ReturnFalse()
        {
            // Act
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetFieldDocumentation(typeof(DummyDataType), nameof(DummyDataType.Prop1), out _);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryGetFieldDocumentation_FieldHasAssociatedDocumentation_ReturnTrueAndAssociatedDocumentation()
        {
            // Arrange
            var expectedDocumentation = "abc";
            generationOptions.AddDocumentation<DummyDataType>(nameof(DummyDataType.Prop1), expectedDocumentation);

            // Act
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetFieldDocumentation(typeof(DummyDataType), nameof(DummyDataType.Prop1), out var actualDocumentation);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        [TestMethod]
        public void TryGetMethodDocumentation_MethodHasNoAssociatedDocumentation_ReturnFalse()
        {
            // Act
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetMethodDocumentation(typeof(IDummyContractType), nameof(IDummyContractType.Method), 2, out _);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryGetMethodDocumentation_MethodHasAssociatedDocumentation_ReturnTrueAndAssociatedDocumentation()
        {
            // Arrange
            var expectedDocumentation = "abc";
            generationOptions.AddDocumentation<IDummyContractType>(nameof(IDummyContractType.Method), 2, expectedDocumentation);

            // Act
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetMethodDocumentation(typeof(IDummyContractType), nameof(IDummyContractType.Method), 2, out var actualDocumentation);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        [TestMethod]
        public void TryGetEnumValueDocumentation_EnumValueHasNoAssociatedDocumentation_ReturnFalse()
        {
            // Act
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetEnumValueDocumentation(typeof(DummyEnum), (int)DummyEnum.Value1, out _);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryGetEnumValueDocumentation_EnumValueHasAssociatedDocumentation_ReturnTrueAndAssociatedDocumentation()
        {
            // Arrange
            var expectedDocumentation = "abc";
            generationOptions.AddDocumentation<DummyEnum>((int)DummyEnum.Value1, expectedDocumentation);

            // Act
            var result = generationOptions.AnalysisOptions.DocumentationProvider.TryGetEnumValueDocumentation(typeof(DummyEnum), (int)DummyEnum.Value1, out var actualDocumentation);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        #endregion IDocumentationProvider Tests
    }
}
