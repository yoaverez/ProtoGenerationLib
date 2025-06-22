using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;
using ProtoGenerationLib.ProvidersAndRegistries.External;

namespace ProtoGenerationLib.Configurations.Internals
{
    /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
    public class ProtoStylingConventionsStrategiesOptions : IProtoStylingConventionsStrategiesOptions
    {
        /// <inheritdoc/>
        public string MessageStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string EnumStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string EnumValueStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string ServiceStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string FieldStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string PackageStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string RpcStylingStrategy { get; set; }

        /// <inheritdoc/>
        public string FilePathStylingStrategy { get; set; }

        /// <summary>
        /// Create new instance of the <see cref="ProtoStylingConventionsStrategiesOptions"/> class.
        /// </summary>
        /// <param name="messageStylingStrategy"><inheritdoc cref="MessageStylingStrategy" path="/node()"/><br/> Default to null converted to "UpperCamelCase".</param>
        /// <param name="enumStylingStrategy"><inheritdoc cref="EnumStylingStrategy" path="/node()"/><br/> Default to null converted to "UpperCamelCase".</param>
        /// <param name="enumValueStylingStrategy"><inheritdoc cref="EnumValueStylingStrategy" path="/node()"/><br/> Default to null converted to "UpperSnakeCase".</param>
        /// <param name="serviceStylingStrategy"><inheritdoc cref="ServiceStylingStrategy" path="/node()"/><br/> Default to null converted to "UpperCamelCase".</param>
        /// <param name="fieldStylingStrategy"><inheritdoc cref="FieldStylingStrategy" path="/node()"/><br/> Default to null converted to "SnakeCase".</param>
        /// <param name="packageStylingStrategy"><inheritdoc cref="PackageStylingStrategy" path="/node()"/><br/> Default to null converted to "DotDelimitedSnakeCase".</param>
        /// <param name="rpcStylingStrategy"><inheritdoc cref="RpcStylingStrategy" path="/node()"/><br/> Default to null converted to "UpperCamelCase".</param>
        /// <param name="filePathStylingStrategy"><inheritdoc cref="FilePathStylingStrategy" path="/node()"/><br/> Default to null converted to "ForwardSlashDelimitedSnakeCase".</param>
        public ProtoStylingConventionsStrategiesOptions(string? messageStylingStrategy = null,
                                                        string? enumStylingStrategy = null,
                                                        string? enumValueStylingStrategy = null,
                                                        string? serviceStylingStrategy = null,
                                                        string? fieldStylingStrategy = null,
                                                        string? packageStylingStrategy = null,
                                                        string? rpcStylingStrategy = null,
                                                        string? filePathStylingStrategy = null)
        {
            MessageStylingStrategy = messageStylingStrategy ??
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase];
            EnumStylingStrategy = enumStylingStrategy ??
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase];
            EnumValueStylingStrategy = enumValueStylingStrategy ??
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperSnakeCase];
            ServiceStylingStrategy = serviceStylingStrategy ??
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase];
            FieldStylingStrategy = fieldStylingStrategy ??
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.SnakeCase];
            PackageStylingStrategy = packageStylingStrategy ??
                StrategyNamesLookup.PackageStylingStrategiesLookup[PackageStylingStrategyKind.DotDelimitedSnakeCase];
            RpcStylingStrategy = rpcStylingStrategy ??
                StrategyNamesLookup.ProtoStylingStrategiesLookup[ProtoStylingStrategyKind.UpperCamelCase];
            FilePathStylingStrategy = filePathStylingStrategy ??
                StrategyNamesLookup.FilePathStylingStrategiesLookup[FilePathStylingStrategyKind.ForwardSlashDelimitedSnakeCase];
        }
    }
}
