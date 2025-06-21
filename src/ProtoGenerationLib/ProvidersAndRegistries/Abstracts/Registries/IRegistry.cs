using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Mappers.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for all registrations.
    /// </summary>
    public interface IRegistry
    {
        #region Custom Converters Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="ICustomConvertersRegistry.RegisterDataTypeCustomConverter(ICSharpToIntermediateCustomConverter{IDataTypeMetadata})"/>
        IRegistry RegisterDataTypeCustomConverter(ICSharpToIntermediateCustomConverter<IDataTypeMetadata> customConverter);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="ICustomConvertersRegistry.RegisterContractTypeCustomConverter(ICSharpToIntermediateCustomConverter{IContractTypeMetadata})"/>
        IRegistry RegisterContractTypeCustomConverter(ICSharpToIntermediateCustomConverter<IContractTypeMetadata> customConverter);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="ICustomConvertersRegistry.RegisterEnumTypeCustomConverter(ICSharpToIntermediateCustomConverter{IEnumTypeMetadata})"/>
        IRegistry RegisterEnumTypeCustomConverter(ICSharpToIntermediateCustomConverter<IEnumTypeMetadata> customConverter);

        #endregion Custom Converters Registry

        #region Custom Type Mappers Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="ICustomTypeMappersRegistry.RegisterCustomTypeMapper(ITypeMapper)"/>
        IRegistry RegisterCustomTypeNameMapper(ITypeMapper typeNameMapper);

        #endregion Custom Type Mappers Registry

        #region Proto Styling Conventions Strategies Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterProtoStylingStrategy(string, IProtoStylingStrategy)"/>
        IRegistry RegisterProtoStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterPackageStylingStrategy(string, IPackageStylingStrategy)"/>
        IRegistry RegisterPackageStylingStrategy(string strategyName, IPackageStylingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterFilePathStylingStrategy(string, IFilePathStylingStrategy)"/>
        IRegistry RegisterFilePathStylingStrategy(string strategyName, IFilePathStylingStrategy strategy);

        #endregion Proto Styling Conventions Strategies Registry

        #region Proto Naming Strategies Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoNamingStrategiesRegistry.RegisterTypeNamingStrategy(string, ITypeNamingStrategy)"/>
        IRegistry RegisterTypeNamingStrategy(string strategyName, ITypeNamingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoNamingStrategiesRegistry.RegisterPackageNamingStrategy(string, IPackageNamingStrategy)"/>
        IRegistry RegisterPackageNamingStrategy(string strategyName, IPackageNamingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoNamingStrategiesRegistry.RegisterFileNamingStrategy(string, IFileNamingStrategy)"/>
        IRegistry RegisterFileNamingStrategy(string strategyName, IFileNamingStrategy strategy);

        #endregion Proto Naming Strategies Registry

        #region Extraction Strategies Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IExtractionStrategiesRegistry.RegisterFieldsAndPropertiesExtractionStrategy(string, IFieldsAndPropertiesExtractionStrategy)"/>
        IRegistry RegisterFieldsAndPropertiesExtractionStrategy(string strategyName, IFieldsAndPropertiesExtractionStrategy strategy);

        #endregion Extraction Strategies Registry

        #region Numbering Strategies Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="INumberingStrategiesRegistry.RegisterFieldNumberingStrategy(string, IFieldNumberingStrategy)"/>
        IRegistry RegisterFieldNumberingStrategy(string strategyName, IFieldNumberingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="INumberingStrategiesRegistry.RegisterEnumValueNumberingStrategy(string, IEnumValueNumberingStrategy)"/>
        IRegistry RegisterEnumValueNumberingStrategy(string strategyName, IEnumValueNumberingStrategy strategy);

        #endregion Numbering Strategies Registry

        #region Parameters List Naming Strategies Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="INewTypeNamingStrategiesRegistry.RegisterParameterListNamingStrategy(string, IParameterListNamingStrategy)"/>
        IRegistry RegisterParameterListNamingStrategy(string strategyName, IParameterListNamingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="INewTypeNamingStrategiesRegistry.RegisterNewTypeNamingStrategy(string, INewTypeNamingStrategy)"/>
        IRegistry RegisterNewTypeNamingStrategy(string strategyName, INewTypeNamingStrategy strategy);

        #endregion Parameters List Naming Strategies Registry
    }
}
