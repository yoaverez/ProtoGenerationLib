using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;

namespace ProtoGenerationLib.Tests.ProvidersAndRegistries.Externals
{
    [TestClass]
    public class StrategyNamesLookupTests
    {
        [TestMethod]
        public void EnumValueNumberingStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<EnumValueNumberingStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.EnumValueNumberingStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void FieldNumberingStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<FieldNumberingStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.FieldNumberingStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void FieldsAndPropertiesExtractionStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<FieldsAndPropertiesExtractionStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.FieldsAndPropertiesExtractionStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void FilePathStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<FilePathStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.FilePathStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void NewTypeNamingStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<NewTypeNamingStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.NewTypeNamingStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void PackageNamingStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<PackageNamingStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.PackageNamingStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void ParameterListNamingStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<ParameterListNamingStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.ParameterListNamingStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void ProtoStylingStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<ProtoStylingStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.ProtoStylingStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void PackageStylingStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<PackageStylingStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.PackageStylingStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }

        [TestMethod]
        public void TypeNamingStrategiesLookup_ContainsAllPossibleValues()
        {
            // Arrange
            var enumValues = Enum.GetValues<TypeNamingStrategyKind>();

            // Act + Assert
            foreach (var enumValue in enumValues)
            {
                if (!StrategyNamesLookup.TypeNamingStrategiesLookup.ContainsKey(enumValue))
                    Assert.Fail($"The lookup does not contains the enum value: {enumValue}");
            }
        }
    }
}
