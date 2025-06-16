using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for custom converters.
    /// </summary>
    public class CustomConvertersContainer : ICustomConvertersRegistry, ICustomConvertersProvider
    {
        /// <summary>
        /// The collection of all the registered data type converters.
        /// </summary>
        private List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> dataTypeCustomConverters;

        /// <summary>
        /// The collection of all the registered contract type converters.
        /// </summary>
        private List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> contractTypeCustomConverters;

        /// <summary>
        /// The collection of all the registered enum type converters.
        /// </summary>
        private List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> enumTypeCustomConverters;

        /// <summary>
        /// Create new instance of the <see cref="CustomConvertersContainer"/> class.
        /// </summary>
        public CustomConvertersContainer()
        {
            dataTypeCustomConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            contractTypeCustomConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            enumTypeCustomConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
        }

        #region ICustomConvertersProvider Implementation

        /// <inheritdoc/>
        public IEnumerable<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> GetContractTypeCustomConverters()
        {
            return contractTypeCustomConverters;
        }

        /// <inheritdoc/>
        public IEnumerable<ITypesExtractor> GetCustomTypesExtractors()
        {
            return dataTypeCustomConverters.Cast<ITypesExtractor>()
                                           .Concat(contractTypeCustomConverters)
                                           .Concat(enumTypeCustomConverters)
                                           .ToArray();
        }

        /// <inheritdoc/>
        public IEnumerable<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> GetDataTypeCustomConverters()
        {
            return dataTypeCustomConverters;
        }

        /// <inheritdoc/>
        public IEnumerable<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> GetEnumTypeCustomConverters()
        {
            return enumTypeCustomConverters;
        }

        #endregion ICustomConvertersProvider Implementation

        #region ICustomConvertersRegistry Implementation

        /// <inheritdoc/>
        public void RegisterContractTypeCustomConverter(ICSharpToIntermediateCustomConverter<IContractTypeMetadata> customConverter)
        {
            contractTypeCustomConverters.Add(customConverter);
        }

        /// <inheritdoc/>
        public void RegisterDataTypeCustomConverter(ICSharpToIntermediateCustomConverter<IDataTypeMetadata> customConverter)
        {
            dataTypeCustomConverters.Add(customConverter);
        }

        /// <inheritdoc/>
        public void RegisterEnumTypeCustomConverter(ICSharpToIntermediateCustomConverter<IEnumTypeMetadata> customConverter)
        {
            enumTypeCustomConverters.Add(customConverter);
        }

        #endregion ICustomConvertersRegistry Implementation
    }
}
