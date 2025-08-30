using ProtoGenerationLib.Strategies.Internals.DocumentationExtractionStrategies;
using System.Reflection;

namespace ProtoGenerationLib.Tests.Strategies.Internals.DocumentationExtractionStrategies
{
    [TestClass]
    public class XmlFileDocumentationExtractionStrategyTests
    {
        public static IEnumerable<object[]> Instances { get; set; }

        static XmlFileDocumentationExtractionStrategyTests()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var xmlPath = assembly.Location.Replace(".dll", ".xml");
            var assemblyToXmlFilePath = new Dictionary<Assembly, string>
            {
                [assembly] = xmlPath,
            };
            Instances = new List<object[]>
            {
                new object[] { new XmlFileDocumentationExtractionStrategy("./") },
                new object[] { new XmlFileDocumentationExtractionStrategy(assemblyToXmlFilePath) },
            };
        }

        [DynamicData(nameof(Instances), DynamicDataSourceType.Property)]
        [DataTestMethod]
        public void TryGetBaseTypeFieldDocumentation_ResultIsCorrect(XmlFileDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            // Arrange
            var subClassType = typeof(int);
            Type baseType = null;
            var expectedDocumentation = $"A field containing all the data of the {subClassType.Name} base class.";

            // Act
            var actualResult = documentationExtractionStrategy.TryGetBaseTypeFieldDocumentation(subClassType, baseType, out var actualDocumentation);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        [DynamicData(nameof(Instances), DynamicDataSourceType.Property)]
        [DataTestMethod]
        public void TryGetEnumValueDocumentation_ResultIsCorrect(XmlFileDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            // Arrange
            var enumType = typeof(EnumWithDocs);
            var enumValue = (int)EnumWithDocs.Value1;
            var expectedDocumentation = $"{EnumWithDocs.Value1} summary";

            // Act
            var actualResult = documentationExtractionStrategy.TryGetEnumValueDocumentation(enumType, enumValue, out var actualDocumentation);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        [DynamicData(nameof(Instances), DynamicDataSourceType.Property)]
        [DataTestMethod]
        public void TryGetEnumValueDocumentation_GivenTypeIsNotAnEnumType_ThrowsArgumentException(XmlFileDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            // Arrange
            var enumType = typeof(ClassWithDocs);
            var enumValue = 0;

            // Act + Assert
            Assert.ThrowsException<ArgumentException>(() => documentationExtractionStrategy.TryGetEnumValueDocumentation(enumType, enumValue, out var actualDocumentation));
        }

        [DynamicData(nameof(Instances), DynamicDataSourceType.Property)]
        [DataTestMethod]
        public void TryGetFieldDocumentation_ResultIsCorrect(XmlFileDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            // Arrange
            var testedType = typeof(ClassWithDocs);
            var testedMemberName = nameof(ClassWithDocs.field);
            var testedMember = testedType.GetField(testedMemberName)!;
            var expectedDocumentation = $"{testedMemberName} summary";

            // Act
            var actualResult = documentationExtractionStrategy.TryGetFieldDocumentation(testedMember, out var actualDocumentation);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        [DynamicData(nameof(Instances), DynamicDataSourceType.Property)]
        [DataTestMethod]
        public void TryGetMethodDocumentation_ResultIsCorrect(XmlFileDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            // Arrange
            var testedType = typeof(ClassWithDocs);
            var testedMemberName = nameof(ClassWithDocs.Method);
            var testedMember = testedType.GetMethod(testedMemberName)!;
            var expectedDocumentation = $"{testedMemberName} summary{Environment.NewLine}" +
                                        $"Remarks:{Environment.NewLine}" +
                                        $"{testedMemberName} remarks{Environment.NewLine}" +
                                        $"Returns:{Environment.NewLine}" +
                                        $"{testedMemberName} returns";

            // Act
            var actualResult = documentationExtractionStrategy.TryGetMethodDocumentation(testedMember, out var actualDocumentation);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        [DynamicData(nameof(Instances), DynamicDataSourceType.Property)]
        [DataTestMethod]
        public void TryGetMethodParameterDocumentation_ResultIsCorrect(XmlFileDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            // Arrange
            var testedType = typeof(ClassWithDocs);
            var testedMemberName = nameof(ClassWithDocs.Method);
            var testedMember = testedType.GetMethod(testedMemberName)!;
            var parameter1Name = "name";
            var parameter2Name = "b";
            var expectedParameter1Documentation = $"{parameter1Name} parameter";
            var expectedParameter2Documentation = $"{parameter2Name} parameter";

            // Act
            var actualResult1 = documentationExtractionStrategy.TryGetMethodParameterDocumentation(testedMember, parameter1Name, out var actualParameter1Documentation);
            var actualResult2 = documentationExtractionStrategy.TryGetMethodParameterDocumentation(testedMember, parameter2Name, out var actualParameter2Documentation);

            // Assert
            Assert.IsTrue(actualResult1);
            Assert.AreEqual(expectedParameter1Documentation, actualParameter1Documentation);

            Assert.IsTrue(actualResult2);
            Assert.AreEqual(expectedParameter2Documentation, actualParameter2Documentation);
        }

        [DynamicData(nameof(Instances), DynamicDataSourceType.Property)]
        [DataTestMethod]
        public void TryGetPropertyDocumentation_ResultIsCorrect(XmlFileDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            // Arrange
            var testedType = typeof(ClassWithDocs);
            var testedMemberName = nameof(ClassWithDocs.Prop);
            var testedMember = testedType.GetProperty(testedMemberName)!;
            var expectedDocumentation = $"{testedMemberName} summary{Environment.NewLine}" +
                                        $"Remarks:{Environment.NewLine}" +
                                        $"{testedMemberName} remarks";

            // Act
            var actualResult = documentationExtractionStrategy.TryGetPropertyDocumentation(testedMember, out var actualDocumentation);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }

        [DynamicData(nameof(Instances), DynamicDataSourceType.Property)]
        [DataTestMethod]
        public void TryGetTypeDocumentation_ResultIsCorrect(XmlFileDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            // Arrange
            var testedType = typeof(ClassWithDocs);
            var testedMemberName = nameof(ClassWithDocs);
            var testedMember = testedType;
            var expectedDocumentation = $"{testedMemberName} summary {nameof(XmlFileDocumentationExtractionStrategyTests)} " +
                                        $"true.";

            // Act
            var actualResult = documentationExtractionStrategy.TryGetTypeDocumentation(testedMember, out var actualDocumentation);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(expectedDocumentation, actualDocumentation);
        }
    }

    /// <summary>
    /// ClassWithDocs summary <see cref="XmlFileDocumentationExtractionStrategyTests"/> <see langword="true"/>.
    /// </summary>
    internal class ClassWithDocs
    {
        /// <summary>
        /// Prop summary
        /// </summary>
        /// <remarks>Prop remarks</remarks>
        public int Prop { get; set; }

        /// <summary>
        /// field summary
        /// </summary>
        /// <example>
        /// This shouldn't be seen.
        /// </example>
        public bool field;

        /// <remarks>Event remarks</remarks>
        public event Action Event;

        /// <summary>
        /// Method summary
        /// </summary>
        /// <param name="name">name parameter</param>
        /// <param name="b">b parameter</param>
        /// <returns>Method returns</returns>
        /// <remarks>Method remarks</remarks>
        public int Method(string name, bool b)
        {
            return 0;
        }
    }

    /// <summary>
    /// EnumWithDocs summary
    /// </summary>
    /// <remarks>EnumWithDocs remarks</remarks>
    internal enum EnumWithDocs
    {
        /// <summary>
        /// Value1 summary
        /// </summary>
        Value1,
    }
}
