using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;

namespace ProtoGenerationLib.Configurations.Internals
{
    /// <inheritdoc cref="IProtoGenerationOptions"/>
    public class ProtoGenerationOptions : IProtoGenerationOptions
    {
        /// <summary>
        /// An instance of the <see cref="ProtoGenerationOptions"/> class
        /// that contains the default configurations.
        /// </summary>
        public static ProtoGenerationOptions Default;

        /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
        public ProtoStylingConventionsStrategiesOptions ProtoStylingConventionsStrategiesOptions { get; set; }
        IProtoStylingConventionsStrategiesOptions IProtoGenerationOptions.ProtoStylingConventionsStrategiesOptions => ProtoStylingConventionsStrategiesOptions;

        /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
        public ProtoNamingStrategiesOptions ProtoNamingStrategiesOptions { get; set; }
        IProtoNamingStrategiesOptions IProtoGenerationOptions.ProtoNamingStrategiesOptions => ProtoNamingStrategiesOptions;

        /// <inheritdoc cref="INumberingStrategiesOptions"/>
        public NumberingStrategiesOptions NumberingStrategiesOptions { get; set; }
        INumberingStrategiesOptions IProtoGenerationOptions.NumberingStrategiesOptions => NumberingStrategiesOptions;

        /// <inheritdoc cref="IAnalysisOptions"/>
        public AnalysisOptions AnalysisOptions { get; set; }
        IAnalysisOptions IProtoGenerationOptions.AnalysisOptions => AnalysisOptions;

        /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
        public NewTypeNamingStrategiesOptions NewTypeNamingStrategiesOptions { get; set; }
        INewTypeNamingStrategiesOptions IProtoGenerationOptions.NewTypeNamingStrategiesOptions => NewTypeNamingStrategiesOptions;

        /// <inheritdoc/>
        public string ProtoFileSyntax { get; set; }

        /// <summary>
        /// Initialize the static members of the <see cref="ProtoGenerationOptions"/> class.
        /// </summary>
        static ProtoGenerationOptions()
        {
            Default = CreateDefaultProtoGenerationOptions();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoGenerationOptions"/> class.
        /// </summary>
        public ProtoGenerationOptions()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoGenerationOptions"/> class.
        /// </summary>
        /// <param name="protoStylingConventionsStrategiesOptions"><inheritdoc cref="ProtoStylingConventionsStrategiesOptions" path="/node()"/></param>
        /// <param name="protoNamingStrategiesOptions"><inheritdoc cref="ProtoNamingStrategiesOptions" path="/node()"/></param>
        /// <param name="numberingStrategiesOptions"><inheritdoc cref="NumberingStrategiesOptions" path="/node()"/></param>
        /// <param name="analysisOptions"><inheritdoc cref="AnalysisOptions" path="/node()"/></param>
        /// <param name="newTypeNamingStrategiesOptions"><inheritdoc cref="NewTypeNamingStrategiesOptions" path="/node()"/></param>
        /// <param name="protoFileSyntax"><inheritdoc cref="ProtoFileSyntax" path="/node()"/></param>
        public ProtoGenerationOptions(ProtoStylingConventionsStrategiesOptions protoStylingConventionsStrategiesOptions,
                                      ProtoNamingStrategiesOptions protoNamingStrategiesOptions,
                                      NumberingStrategiesOptions numberingStrategiesOptions,
                                      AnalysisOptions analysisOptions,
                                      NewTypeNamingStrategiesOptions newTypeNamingStrategiesOptions,
                                      string protoFileSyntax)
        {
            ProtoStylingConventionsStrategiesOptions = protoStylingConventionsStrategiesOptions;
            ProtoNamingStrategiesOptions = protoNamingStrategiesOptions;
            NumberingStrategiesOptions = numberingStrategiesOptions;
            AnalysisOptions = analysisOptions;
            NewTypeNamingStrategiesOptions = newTypeNamingStrategiesOptions;
            ProtoFileSyntax = protoFileSyntax;
        }

        /// <summary>
        /// Create the default <see cref="ProtoGenerationOptions"/> object.
        /// </summary>
        /// <returns>
        /// A new <see cref="ProtoGenerationOptions"/> object that contains the default options.
        /// </returns>
        public static ProtoGenerationOptions CreateDefaultProtoGenerationOptions()
        {
            var stylingOptions = new ProtoStylingConventionsStrategiesOptions(
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase],
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase],
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperSnakeCase],
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase],
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.SnakeCase],
                StrategyNamesLookup.PackageStylingStrategiesLookup[PackageStylingStrategyKind.DotDelimitedSnakeCase],
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase],
                StrategyNamesLookup.FilePathStylingStrategiesLookup[FilePathStylingStrategyKind.ForwardSlashDelimitedSnakeCase]);

            var namingOptions = new ProtoNamingStrategiesOptions(
                StrategyNamesLookup.TypeNamingStrategiesLookup[TypeNamingStrategyKind.TypeNameAsAlphaNumericTypeName],
                StrategyNamesLookup.PackageNamingStrategiesLookup[PackageNamingStrategyKind.SinglePackageNamedProtos],
                StrategyNamesLookup.FilePathStrategiesLookup[FilePathStrategyKind.SingleFileNamedProtos]);

            var numberingStrategiesOptions = new NumberingStrategiesOptions(
                StrategyNamesLookup.FieldNumberingStrategiesLookup[FieldNumberingStrategyKind.Sequential],
                StrategyNamesLookup.EnumValueNumberingStrategiesLookup[EnumValueNumberingStrategyKind.Sequential]);

            var analysisOptions = new AnalysisOptions(
                includeFields: false,
                includePrivates: false,
                includeStatics: false,
                removeEmptyMembers: true,
                fieldsAndPropertiesExtractionStrategy: StrategyNamesLookup.FieldsAndPropertiesExtractionStrategiesLookup[FieldsAndPropertiesExtractionStrategyKind.Composite],
                ignoreFieldOrPropertyAttribute: typeof(ProtoIgnoreAttribute),
                dataTypeConstructorAttribute: typeof(ProtoMessageConstructorAttribute),
                protoServiceAttribute: typeof(ProtoServiceAttribute),
                protoRpcAttribute: typeof(ProtoRpcAttribute),
                optionalFieldAttribute: typeof(OptionalDataMemberAttribute));

            var newTypeNamingStrategiesOptions = new NewTypeNamingStrategiesOptions(
                StrategyNamesLookup.ParameterListNamingStrategiesLookup[ParameterListNamingStrategyKind.MethodNameAndParametersTypes],
                StrategyNamesLookup.NewTypeNamingStrategiesLookup[NewTypeNamingStrategyKind.NameAsAlphaNumericTypeName]);

            return new ProtoGenerationOptions(stylingOptions,
                                              namingOptions,
                                              numberingStrategiesOptions,
                                              analysisOptions,
                                              newTypeNamingStrategiesOptions,
                                              "proto3");
        }
    }
}
