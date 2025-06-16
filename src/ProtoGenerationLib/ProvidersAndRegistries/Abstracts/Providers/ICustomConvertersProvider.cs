using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using System.Collections.Generic;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider for custom converters.
    /// </summary>
    public interface ICustomConvertersProvider
    {
        /// <summary>
        /// Get all the data types custom converters.
        /// </summary>
        /// <returns>All the data types custom converters.</returns>
        IEnumerable<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> GetDataTypeCustomConverters();

        /// <summary>
        /// Get all the contract types custom converters.
        /// </summary>
        /// <returns>All the contract types custom converters.</returns>
        IEnumerable<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> GetContractTypeCustomConverters();

        /// <summary>
        /// Get all the enum types custom converters.
        /// </summary>
        /// <returns>All the enum types custom converters.</returns>
        IEnumerable<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> GetEnumTypeCustomConverters();

        /// <summary>
        /// Get all the custom types extractors.
        /// </summary>
        /// <returns>All the custom types extractors.</returns>
        IEnumerable<ITypesExtractor> GetCustomTypesExtractors();
    }
}
