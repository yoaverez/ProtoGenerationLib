using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;
using ProtoGenerationLib.ProvidersAndRegistries.External;

namespace ProtoGenerationLib.Configurations.Internals
{
    /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
    public class ProtoNamingStrategiesOptions : IProtoNamingStrategiesOptions
    {
        /// <inheritdoc/>
        public string TypeNamingStrategy { get; set; }

        /// <inheritdoc/>
        public string PackageNamingStrategy { get; set; }

        /// <inheritdoc/>
        public string FileNamingStrategy { get; set; }

        /// <summary>
        /// Create new instance of the <see cref="ProtoNamingStrategiesOptions"/> class.
        /// </summary>
        /// <param name="typeNamingStrategy"><inheritdoc cref="TypeNamingStrategy" path="/node()"/><br/> Default to null converted to "TypeNameAsAlphaNumericTypeName".</param>
        /// <param name="packageNamingStrategy"><inheritdoc cref="PackageNamingStrategy" path="/node()"/><br/> Default to null converted to "SinglePackageNamedProtos".</param>
        /// <param name="fileNamingStrategy"><inheritdoc cref="FileNamingStrategy" path="/node()"/><br/> Default to null converted to "SingleFileNamedProtos".</param>
        public ProtoNamingStrategiesOptions(string? typeNamingStrategy = null,
                                            string? packageNamingStrategy = null,
                                            string? fileNamingStrategy = null)
        {
            TypeNamingStrategy = typeNamingStrategy ??
                StrategyNamesLookup.TypeNamingStrategiesLookup[TypeNamingStrategyKind.TypeNameAsAlphaNumericTypeName];
            PackageNamingStrategy = packageNamingStrategy ??
                StrategyNamesLookup.PackageNamingStrategiesLookup[PackageNamingStrategyKind.SinglePackageNamedProtos];
            FileNamingStrategy = fileNamingStrategy ??
                StrategyNamesLookup.FilePathStrategiesLookup[FilePathStrategyKind.SingleFileNamedProtos];
        }
    }
}
