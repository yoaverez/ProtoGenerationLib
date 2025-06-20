using ProtoGenerationLib.Mappers.Abstracts;
using System.Collections.Generic;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider for custom type mappers.
    /// </summary>
    public interface ICustomTypeMappersProvider
    {
        /// <summary>
        /// Get all the custom type mappers.
        /// </summary>
        /// <returns>All the custom type mappers.</returns>
        IEnumerable<ITypeMapper> GetCustomTypeMappers();
    }
}
