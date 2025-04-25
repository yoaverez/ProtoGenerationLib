using ProtoGenerator.Mappers.Abstracts;
using System.Collections.Generic;

namespace ProtoGenerator.ProvidersAndRegistries.Providers
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
        IEnumerable<ITypeNameMapper> GetCustomTypeNameMappers();
    }
}
