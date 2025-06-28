using ProtoGenerationLib.ProvidersAndRegistries.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for all the services. i.e. container that contains all
    /// the customizations.
    /// </summary>
    internal class ServicesContainer : IProviderAndRegister
    {
        /// <inheritdoc cref="ProtoStylingConventionsStrategiesContainer"/>
        private ProtoStylingConventionsStrategiesContainer protoStylingConventionsStrategiesContainer;

        /// <inheritdoc cref="ProtoNamingStrategiesContainer"/>
        private ProtoNamingStrategiesContainer protoNamingStrategiesContainer;

        /// <inheritdoc cref="NumberingStrategiesContainer"/>
        private NumberingStrategiesContainer numberingStrategiesContainer;

        /// <inheritdoc cref="ExtractionStrategiesContainer"/>
        private ExtractionStrategiesContainer extractionStrategiesContainer;

        /// <inheritdoc cref="NewTypeNamingStrategiesContainer"/>
        private NewTypeNamingStrategiesContainer newTypeNamingStrategiesContainer;

        /// <summary>
        /// Create new instance of the <see cref="ServicesContainer"/> class.
        /// </summary>
        public ServicesContainer()
        {
            protoStylingConventionsStrategiesContainer = new ProtoStylingConventionsStrategiesContainer();
            protoNamingStrategiesContainer = new ProtoNamingStrategiesContainer();
            numberingStrategiesContainer = new NumberingStrategiesContainer();
            extractionStrategiesContainer = new ExtractionStrategiesContainer();
            newTypeNamingStrategiesContainer = new NewTypeNamingStrategiesContainer();
        }

        #region IProvider Implementation

        #region IProtoStylingConventionsStrategiesProvider Implementation

        /// <inheritdoc/>
        public IProtoStylingStrategy GetProtoStylingStrategy(string strategyName)
        {
            return protoStylingConventionsStrategiesContainer.GetProtoStylingStrategy(strategyName);
        }

        /// <inheritdoc/>
        public IPackageStylingStrategy GetPackageStylingStrategy(string strategyName)
        {
            return protoStylingConventionsStrategiesContainer.GetPackageStylingStrategy(strategyName);
        }

        /// <inheritdoc/>
        public IFilePathStylingStrategy GetFilePathStylingStrategy(string strategyName)
        {
            return protoStylingConventionsStrategiesContainer.GetFilePathStylingStrategy(strategyName);
        }

        #endregion IProtoStylingConventionsStrategiesProvider Implementation

        #region INumberingStrategiesProvider Implementation

        /// <inheritdoc/>
        public IEnumValueNumberingStrategy GetEnumValueNumberingStrategy(string strategyName)
        {
            return numberingStrategiesContainer.GetEnumValueNumberingStrategy(strategyName);
        }

        /// <inheritdoc/>
        public IFieldNumberingStrategy GetFieldNumberingStrategy(string strategyName)
        {
            return numberingStrategiesContainer.GetFieldNumberingStrategy(strategyName);
        }

        #endregion INumberingStrategiesProvider Implementation

        #region IExtractionStrategiesProvider Implementation

        /// <inheritdoc/>
        public IFieldsAndPropertiesExtractionStrategy GetFieldsAndPropertiesExtractionStrategy(string strategyName)
        {
            return extractionStrategiesContainer.GetFieldsAndPropertiesExtractionStrategy(strategyName);
        }

        #endregion IExtractionStrategiesProvider Implementation

        #region IProtoNamingStrategiesProvider Implementation

        /// <inheritdoc/>
        public IFileNamingStrategy GetFileNamingStrategy(string strategyName)
        {
            return protoNamingStrategiesContainer.GetFileNamingStrategy(strategyName);
        }

        /// <inheritdoc/>
        public IPackageNamingStrategy GetPackageNamingStrategy(string strategyName)
        {
            return protoNamingStrategiesContainer.GetPackageNamingStrategy(strategyName);
        }

        /// <inheritdoc/>
        public ITypeNamingStrategy GetTypeNamingStrategy(string strategyName)
        {
            return protoNamingStrategiesContainer.GetTypeNamingStrategy(strategyName);
        }

        #endregion IProtoNamingStrategiesProvider Implementation

        #region INewTypeNamingStrategiesProvider Implementation

        /// <inheritdoc/>
        public IParameterListNamingStrategy GetParameterListNamingStrategy(string strategyName)
        {
            return newTypeNamingStrategiesContainer.GetParameterListNamingStrategy(strategyName);
        }

        /// <inheritdoc/>
        public INewTypeNamingStrategy GetNewTypeNamingStrategy(string strategyName)
        {
            return newTypeNamingStrategiesContainer.GetNewTypeNamingStrategy(strategyName);
        }

        #endregion INewTypeNamingStrategiesProvider Implementation

        #endregion IProvider Implementation

        #region IRegistry Implementation

        #region IProtoStylingConventionsStrategiesRegistry Implementation

        /// <inheritdoc/>
        public IRegistry RegisterProtoStylingStrategy(string strategyName, IProtoStylingStrategy strategy)
        {
            protoStylingConventionsStrategiesContainer.RegisterProtoStylingStrategy(strategyName, strategy);
            return this;
        }

        /// <inheritdoc/>
        public IRegistry RegisterPackageStylingStrategy(string strategyName, IPackageStylingStrategy strategy)
        {
            protoStylingConventionsStrategiesContainer.RegisterPackageStylingStrategy(strategyName, strategy);
            return this;
        }

        /// <inheritdoc/>
        public IRegistry RegisterFilePathStylingStrategy(string strategyName, IFilePathStylingStrategy strategy)
        {
            protoStylingConventionsStrategiesContainer.RegisterFilePathStylingStrategy(strategyName, strategy);
            return this;
        }

        #endregion IProtoStylingConventionsStrategiesRegistry Implementation

        #region INumberingStrategiesRegistry Implementation

        /// <inheritdoc/>
        public IRegistry RegisterEnumValueNumberingStrategy(string strategyName, IEnumValueNumberingStrategy strategy)
        {
            numberingStrategiesContainer.RegisterEnumValueNumberingStrategy(strategyName, strategy);
            return this;
        }

        /// <inheritdoc/>
        public IRegistry RegisterFieldNumberingStrategy(string strategyName, IFieldNumberingStrategy strategy)
        {
            numberingStrategiesContainer.RegisterFieldNumberingStrategy(strategyName, strategy);
            return this;
        }

        #endregion INumberingStrategiesRegistry Implementation

        #region IExtractionStrategiesRegistry Implementation

        /// <inheritdoc/>
        public IRegistry RegisterFieldsAndPropertiesExtractionStrategy(string strategyName, IFieldsAndPropertiesExtractionStrategy strategy)
        {
            extractionStrategiesContainer.RegisterFieldsAndPropertiesExtractionStrategy(strategyName, strategy);
            return this;
        }

        #endregion IExtractionStrategiesRegistry Implementation

        #region IProtoNamingStrategiesRegistry Implementation

        /// <inheritdoc/>
        public IRegistry RegisterFileNamingStrategy(string strategyName, IFileNamingStrategy strategy)
        {
            protoNamingStrategiesContainer.RegisterFileNamingStrategy(strategyName, strategy);
            return this;
        }

        /// <inheritdoc/>
        public IRegistry RegisterPackageNamingStrategy(string strategyName, IPackageNamingStrategy strategy)
        {
            protoNamingStrategiesContainer.RegisterPackageNamingStrategy(strategyName, strategy);
            return this;
        }

        /// <inheritdoc/>
        public IRegistry RegisterTypeNamingStrategy(string strategyName, ITypeNamingStrategy strategy)
        {
            protoNamingStrategiesContainer.RegisterTypeNamingStrategy(strategyName, strategy);
            return this;
        }

        #endregion IProtoNamingStrategiesRegistry Implementation

        #region INewTypeNamingStrategiesRegistry Implementation

        /// <inheritdoc/>
        public IRegistry RegisterParameterListNamingStrategy(string strategyName, IParameterListNamingStrategy strategy)
        {
            newTypeNamingStrategiesContainer.RegisterParameterListNamingStrategy(strategyName, strategy);
            return this;
        }

        /// <inheritdoc/>
        public IRegistry RegisterNewTypeNamingStrategy(string strategyName, INewTypeNamingStrategy strategy)
        {
            newTypeNamingStrategiesContainer.RegisterNewTypeNamingStrategy(strategyName, strategy);
            return this;
        }

        #endregion INewTypeNamingStrategiesRegistry Implementation

        #endregion IRegistry Implementation
    }
}
