using ProtoGenerationLib.Mappers.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using System.Collections.Generic;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for custom type name mappers.
    /// </summary>
    public class CustomTypeNameMappersContainer : ICustomTypeNameMappersRegistry, ICustomTypeNameMappersProvider
    {
        /// <summary>
        /// The collection of all the registered custom type name mappers.
        /// </summary>
        private List<ITypeMapper> customTypeNameMappers;

        /// <summary>
        /// Create new instance of the <see cref="CustomTypeNameMappersContainer"/> class.
        /// </summary>
        public CustomTypeNameMappersContainer()
        {
            customTypeNameMappers = new List<ITypeMapper>();
        }

        #region ICustomTypeNameMappersProvider Implementation

        /// <inheritdoc/>
        public IEnumerable<ITypeMapper> GetCustomTypeNameMappers()
        {
            return customTypeNameMappers;
        }

        #endregion ICustomTypeNameMappersProvider Implementation

        #region ICustomTypeNameMappersRegistry Implementation

        /// <inheritdoc/>
        public void RegisterCustomTypeNameMapper(ITypeMapper typeNameMapper)
        {
            customTypeNameMappers.Add(typeNameMapper);
        }

        #endregion ICustomTypeNameMappersRegistry Implementation
    }
}
