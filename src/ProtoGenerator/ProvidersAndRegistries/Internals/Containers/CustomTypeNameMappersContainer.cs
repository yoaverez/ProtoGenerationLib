using ProtoGenerator.Mappers.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Registries;
using System.Collections.Generic;

namespace ProtoGenerator.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for custom type name mappers.
    /// </summary>
    public class CustomTypeNameMappersContainer : ICustomTypeNameMappersRegistry, ICustomTypeNameMappersProvider
    {
        /// <summary>
        /// The collection of all the registered custom type name mappers.
        /// </summary>
        private List<ITypeNameMapper> customTypeNameMappers;

        /// <summary>
        /// Create new instance of the <see cref="CustomTypeNameMappersContainer"/> class.
        /// </summary>
        public CustomTypeNameMappersContainer()
        {
            customTypeNameMappers = new List<ITypeNameMapper>();
        }

        #region ICustomTypeNameMappersProvider Implementation

        /// <inheritdoc/>
        public IEnumerable<ITypeNameMapper> GetCustomTypeNameMappers()
        {
            return customTypeNameMappers;
        }

        #endregion ICustomTypeNameMappersProvider Implementation

        #region ICustomTypeNameMappersRegistry Implementation

        /// <inheritdoc/>
        public void RegisterCustomTypeNameMapper(ITypeNameMapper typeNameMapper)
        {
            customTypeNameMappers.Add(typeNameMapper);
        }

        #endregion ICustomTypeNameMappersRegistry Implementation
    }
}
