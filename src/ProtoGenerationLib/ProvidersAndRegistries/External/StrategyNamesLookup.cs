using System.Collections.Generic;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;

namespace ProtoGenerationLib.ProvidersAndRegistries.External
{
    /// <summary>
    /// A class for looking all the project defined strategies names.
    /// </summary>
    public static class StrategyNamesLookup
    {
        /// <summary>
        /// A lookup table for enum value numbering strategies names.
        /// </summary>
        public static IReadOnlyDictionary<EnumValueNumberingStrategyKind, string> EnumValueNumberingStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for field numbering strategies names.
        /// </summary>
        public static IReadOnlyDictionary<FieldNumberingStrategyKind, string> FieldNumberingStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for fields and properties extraction strategies names.
        /// </summary>
        public static IReadOnlyDictionary<FieldsAndPropertiesExtractionStrategyKind, string> FieldsAndPropertiesExtractionStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for documentation extraction strategies names.
        /// </summary>
        public static IReadOnlyDictionary<DocumentationExtractionStrategyKind, string> DocumentationExtractionStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for file path strategies names.
        /// </summary>
        public static IReadOnlyDictionary<FilePathStrategyKind, string> FilePathStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for file path styling strategies names.
        /// </summary>
        public static IReadOnlyDictionary<FilePathStylingStrategyKind, string> FilePathStylingStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for new type naming strategies names.
        /// </summary>
        public static IReadOnlyDictionary<NewTypeNamingStrategyKind, string> NewTypeNamingStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for package naming strategies names.
        /// </summary>
        public static IReadOnlyDictionary<PackageNamingStrategyKind, string> PackageNamingStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for parameter list naming strategies names.
        /// </summary>
        public static IReadOnlyDictionary<ParameterListNamingStrategyKind, string> ParameterListNamingStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for proto styling strategies names.
        /// </summary>
        public static IReadOnlyDictionary<ProtoStylingStrategyKind, string> ProtoStylingStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for package styling strategies names.
        /// </summary>
        public static IReadOnlyDictionary<PackageStylingStrategyKind, string> PackageStylingStrategiesLookup { get; private set; }

        /// <summary>
        /// A lookup table for type naming strategies names.
        /// </summary>
        public static IReadOnlyDictionary<TypeNamingStrategyKind, string> TypeNamingStrategiesLookup { get; private set; }

        /// <summary>
        /// Initialize the static members of the <see cref="StrategyNamesLookup"/> class.
        /// </summary>
        static StrategyNamesLookup()
        {
            EnumValueNumberingStrategiesLookup = new Dictionary<EnumValueNumberingStrategyKind, string>
            {
                [EnumValueNumberingStrategyKind.SameAsEnumValue] = "SameAsEnumValue",
                [EnumValueNumberingStrategyKind.Sequential] = "Sequential",
            };

            FieldNumberingStrategiesLookup = new Dictionary<FieldNumberingStrategyKind, string>
            {
                [FieldNumberingStrategyKind.Sequential] = "Sequential",
            };

            FieldsAndPropertiesExtractionStrategiesLookup = new Dictionary<FieldsAndPropertiesExtractionStrategyKind, string>
            {
                [FieldsAndPropertiesExtractionStrategyKind.Composite] = "Composite",
                [FieldsAndPropertiesExtractionStrategyKind.Flatten] = "Flatten",
            };

            DocumentationExtractionStrategiesLookup = new Dictionary<DocumentationExtractionStrategyKind, string>
            {
                [DocumentationExtractionStrategyKind.None] = "None",
                [DocumentationExtractionStrategyKind.FromXmlFiles] = "FromXmlFiles",
            };

            FilePathStrategiesLookup = new Dictionary<FilePathStrategyKind, string>
            {
                [FilePathStrategyKind.NameSpace] = "FileNameAsNameSpace",
                [FilePathStrategyKind.SingleFileNamedProtos] = "SingleFileNamedProtos",
                [FilePathStrategyKind.TypeName] = "FileNameAsTypeName",
                [FilePathStrategyKind.NameSpaceAndTypeName] = "FileNameAsNameSpaceAndTypeName",
            };

            FilePathStylingStrategiesLookup = new Dictionary<FilePathStylingStrategyKind, string>
            {
                [FilePathStylingStrategyKind.ForwardSlashDelimitedSnakeCase] = "ForwardSlashDelimitedSnakeCase",
            };

            NewTypeNamingStrategiesLookup = new Dictionary<NewTypeNamingStrategyKind, string>
            {
                [NewTypeNamingStrategyKind.NameAsAlphaNumericTypeName] = "NewTypeNamingAsAlphaNumericTypeName",
            };

            PackageNamingStrategiesLookup = new Dictionary<PackageNamingStrategyKind, string>
            {
                [PackageNamingStrategyKind.SinglePackageNamedProtos] = "SinglePackageNamedProtos",
                [PackageNamingStrategyKind.NameSpaceAsPackageName] = "NameSpaceAsPackageName",
            };

            ParameterListNamingStrategiesLookup = new Dictionary<ParameterListNamingStrategyKind, string>
            {
                [ParameterListNamingStrategyKind.MethodNameAndParametersTypes] = "MethodNameAndParametersTypes",
            };

            ProtoStylingStrategiesLookup = new Dictionary<ProtoStylingStrategyKind, string>
            {
                [ProtoStylingStrategyKind.CamelCase] = "CamelCase",
                [ProtoStylingStrategyKind.DotDelimitedSnakeCase] = "DotDelimitedSnakeCase",
                [ProtoStylingStrategyKind.SnakeCase] = "SnakeCase",
                [ProtoStylingStrategyKind.UpperCamelCase] = "UpperCamelCase",
                [ProtoStylingStrategyKind.UpperSnakeCase] = "UpperSnakeCase",
            };

            PackageStylingStrategiesLookup = new Dictionary<PackageStylingStrategyKind, string>
            {
                [PackageStylingStrategyKind.DotDelimitedSnakeCase] = ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.DotDelimitedSnakeCase],
            };

            TypeNamingStrategiesLookup = new Dictionary<TypeNamingStrategyKind, string>
            {
                [TypeNamingStrategyKind.TypeNameAsAlphaNumericTypeName] = "TypeNameAsAlphaNumericTypeName",
            };
        }
    }
}
