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
        public static readonly IReadOnlyDictionary<EnumValueNumberingStrategyKind, string> EnumValueNumberingStrategiesLookup;

        /// <summary>
        /// A lookup table for field numbering strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<FieldNumberingStrategyKind, string> FieldNumberingStrategiesLookup;

        /// <summary>
        /// A lookup table for fields and properties extraction strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<FieldsAndPropertiesExtractionStrategyKind, string> FieldsAndPropertiesExtractionStrategiesLookup;

        /// <summary>
        /// A lookup table for file path strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<FilePathStrategyKind, string> FilePathStrategiesLookup;

        /// <summary>
        /// A lookup table for file path styling strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<FilePathStylingStrategyKind, string> FilePathStylingStrategiesLookup;

        /// <summary>
        /// A lookup table for new type naming strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<NewTypeNamingStrategyKind, string> NewTypeNamingStrategiesLookup;

        /// <summary>
        /// A lookup table for package naming strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<PackageNamingStrategyKind, string> PackageNamingStrategiesLookup;

        /// <summary>
        /// A lookup table for parameter list naming strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<ParameterListNamingStrategyKind, string> ParameterListNamingStrategiesLookup;

        /// <summary>
        /// A lookup table for proto styling strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<ProtoStylingStrategyKind, string> ProtoStylingStrategiesLookup;

        /// <summary>
        /// A lookup table for package styling strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<PackageStylingStrategyKind, string> PackageStylingStrategiesLookup;

        /// <summary>
        /// A lookup table for type naming strategies names.
        /// </summary>
        public static readonly IReadOnlyDictionary<TypeNamingStrategyKind, string> TypeNamingStrategiesLookup;

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
