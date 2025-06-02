using ProtoGenerator.Mappers.Abstracts;
using System.Collections.Generic;

namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider for custom type mappers.
    /// </summary>
    public interface ICustomTypeNameMappersProvider
    {
        /// <summary>
        /// Get all the custom type name mappers.
        /// </summary>
        /// <returns>All the custom type name mappers.</returns>
        IEnumerable<ITypeMapper> GetCustomTypeNameMappers();
    }
}
