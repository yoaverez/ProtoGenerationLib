using ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers;
using ProtoGenerationLib.Strategies.Internals.EnumValueNumberingStrategies;
using ProtoGenerationLib.Strategies.Internals.FieldNumberingStrategies;
using ProtoGenerationLib.Strategies.Internals.FieldsAndPropertiesExtractionStrategies;
using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;
using ProtoGenerationLib.Strategies.Internals.NewTypeNamingStrategy;
using ProtoGenerationLib.Strategies.Internals.PackageNamingStrategies;
using ProtoGenerationLib.Strategies.Internals.PatameterListNamingStrategies;
using ProtoGenerationLib.Strategies.Internals.ProtoStylingStrategies;
using ProtoGenerationLib.Strategies.Internals.TypeNamingStrategies;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals
{
    /// <summary>
    /// Holder of the default services container
    /// for the proto generation project.
    /// </summary>
    public static class DefaultServicesContainer
    {
        /// <summary>
        /// The default services container.
        /// </summary>
        public static IProviderAndRegister Instance { get; }

        /// <summary>
        /// Initialize the static members of the <see cref="DefaultServicesContainer"/> class.
        /// </summary>
        static DefaultServicesContainer()
        {
            Instance = CreateDefaultServicesContainer();
        }

        /// <summary>
        /// Create the default services container which contains
        /// all the project defined strategies.
        /// </summary>
        /// <returns>
        /// A new instance of <see cref="IProviderAndRegister"/>
        /// in which all the project defined strategies are registered.
        /// </returns>
        private static IProviderAndRegister CreateDefaultServicesContainer()
        {
            var container = new ServicesContainer();

            // Register enum value numbering strategies.
            container.RegisterEnumValueNumberingStrategy(StrategyNamesLookup.EnumValueNumberingStrategiesLookup[EnumValueNumberingStrategyKind.SameAsEnumValue], new EnumNumberSameAsEnumValueStrategy());
            container.RegisterEnumValueNumberingStrategy(StrategyNamesLookup.EnumValueNumberingStrategiesLookup[EnumValueNumberingStrategyKind.Sequential], new SequentialEnumValueNumberingStrategy());

            // Register field numbering strategies.
            container.RegisterFieldNumberingStrategy(StrategyNamesLookup.FieldNumberingStrategiesLookup[FieldNumberingStrategyKind.Sequential], new SequentialFieldNumberingStrategy());

            // Register fields and properties extraction strategies.
            container.RegisterFieldsAndPropertiesExtractionStrategy(StrategyNamesLookup.FieldsAndPropertiesExtractionStrategiesLookup[FieldsAndPropertiesExtractionStrategyKind.Composite], new CompositeFieldsAndPropertiesExtractionStrategy());
            container.RegisterFieldsAndPropertiesExtractionStrategy(StrategyNamesLookup.FieldsAndPropertiesExtractionStrategiesLookup[FieldsAndPropertiesExtractionStrategyKind.Flatten], new FlattenedFieldsAndPropertiesExtractionStrategy());

            // Register file naming strategies.
            container.RegisterFileNamingStrategy(StrategyNamesLookup.FilePathStrategiesLookup[FilePathStrategyKind.NameSpace], new NameSpaceAsFileNameStrategy());
            container.RegisterFileNamingStrategy(StrategyNamesLookup.FilePathStrategiesLookup[FilePathStrategyKind.SingleFileNamedProtos], new SingleFileStrategy("protos.proto"));
            container.RegisterFileNamingStrategy(StrategyNamesLookup.FilePathStrategiesLookup[FilePathStrategyKind.TypeName], new TypeNameAsFileNameStrategy());
            container.RegisterFileNamingStrategy(StrategyNamesLookup.FilePathStrategiesLookup[FilePathStrategyKind.NameSpaceAndTypeName], new NameSpaceAndTypeNameAsFileNameStrategy());

            // Register new type naming strategies.
            container.RegisterNewTypeNamingStrategy(StrategyNamesLookup.NewTypeNamingStrategiesLookup[NewTypeNamingStrategyKind.NameAsAlphaNumericTypeName], new NewTypeNamingStrategy());

            // Register packages naming strategies.
            container.RegisterPackageNamingStrategy(StrategyNamesLookup.PackageNamingStrategiesLookup[PackageNamingStrategyKind.SinglePackageNamedProtos], new ConstNameAsPackageStrategy("protos"));
            container.RegisterPackageNamingStrategy(StrategyNamesLookup.PackageNamingStrategiesLookup[PackageNamingStrategyKind.NameSpaceAsPackageName], new NameSpaceAsPackageStrategy());

            // Register the parameter list naming strategies.
            container.RegisterParameterListNamingStrategy(StrategyNamesLookup.ParameterListNamingStrategiesLookup[ParameterListNamingStrategyKind.MethodNameAndParametersTypes], new ParameterListNamingStrategy());

            // Register proto styling strategies.
            container.RegisterProtoStylingStrategy(StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.CamelCase], new CamelCaseStrategy());
            container.RegisterProtoStylingStrategy(StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.SnakeCase], new SnakeCaseStrategy());
            container.RegisterProtoStylingStrategy(StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase], new UpperCamelCaseStrategy());
            container.RegisterProtoStylingStrategy(StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperSnakeCase], new UpperSnakeCaseStrategy());

            // Register file styling strategies.
            container.RegisterFilePathStylingStrategy(StrategyNamesLookup.FilePathStylingStrategiesLookup[FilePathStylingStrategyKind.ForwardSlashDelimitedSnakeCase], new ForwardSlashDelimitedSnakeCaseStrategy());

            // Register package styling strategies.
            container.RegisterPackageStylingStrategy(StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.DotDelimitedSnakeCase], new DotDelimitedSnakeCaseStrategy());

            // Register type naming strategies.
            container.RegisterTypeNamingStrategy(StrategyNamesLookup.TypeNamingStrategiesLookup[TypeNamingStrategyKind.TypeNameAsAlphaNumericTypeName], new TypeNameAsAlphaNumericTypeNameStrategy());

            return container;
        }
    }
}
