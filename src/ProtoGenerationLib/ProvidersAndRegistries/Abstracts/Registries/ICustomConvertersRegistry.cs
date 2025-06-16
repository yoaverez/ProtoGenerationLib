using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for custom converters.
    /// </summary>
    public interface ICustomConvertersRegistry
    {
        /// <summary>
        /// Register the given <paramref name="customConverter"/> to the
        /// <see cref="IDataTypeMetadata"/> custom converters collection.
        /// </summary>
        /// <param name="customConverter">The custom converter to register.</param>
        void RegisterDataTypeCustomConverter(ICSharpToIntermediateCustomConverter<IDataTypeMetadata> customConverter);

        /// <summary>
        /// Register the given <paramref name="customConverter"/> to the
        /// <see cref="IContractTypeMetadata"/> custom converters collection.
        /// </summary>
        /// <param name="customConverter">The custom converter to register.</param>
        void RegisterContractTypeCustomConverter(ICSharpToIntermediateCustomConverter<IContractTypeMetadata> customConverter);

        /// <summary>
        /// Register the given <paramref name="customConverter"/> to the
        /// <see cref="IEnumTypeMetadata"/> custom converters collection.
        /// </summary>
        /// <param name="customConverter">The custom converter to register.</param>
        void RegisterEnumTypeCustomConverter(ICSharpToIntermediateCustomConverter<IEnumTypeMetadata> customConverter);

    }
}
