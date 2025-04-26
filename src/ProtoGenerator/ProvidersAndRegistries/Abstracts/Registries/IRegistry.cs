using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Mappers.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Strategies.Abstracts;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries
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

        #region Custom Type Name Mappers Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="ICustomTypeNameMappersRegistry.RegisterCustomTypeNameMapper(ITypeNameMapper)"/>
        IRegistry RegisterCustomTypeNameMapper(ITypeNameMapper typeNameMapper);

        #endregion Custom Type Name Mappers Registry

        #region Proto Styling Conventions Strategies Registry

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterMessageStylingStrategy(string, IProtoStylingStrategy)"/>
        IRegistry RegisterMessageStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterEnumStylingStrategy(string, IProtoStylingStrategy)"/>
        IRegistry RegisterEnumStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterEnumValueStylingStrategy(string, IProtoStylingStrategy)"/>
        IRegistry RegisterEnumValueStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterServiceStylingStrategy(string, IProtoStylingStrategy)"/>
        IRegistry RegisterServiceStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterFieldStylingStrategy(string, IProtoStylingStrategy)"/>
        IRegistry RegisterFieldStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

        /// <returns>This instance of the <see cref="IRegistry"/> in order to allow method chaining.</returns>
        /// <inheritdoc cref="IProtoStylingConventionsStrategiesRegistry.RegisterPackageStylingStrategy(string, IProtoStylingStrategy)"/>
        IRegistry RegisterPackageStylingStrategy(string strategyName, IProtoStylingStrategy strategy);

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
    }
}
