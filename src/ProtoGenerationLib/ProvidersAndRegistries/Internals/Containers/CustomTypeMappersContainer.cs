using ProtoGenerationLib.Mappers.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries;
using System.Collections.Generic;

namespace ProtoGenerationLib.ProvidersAndRegistries.Internals.Containers
{
    /// <summary>
    /// Container for custom type mappers.
    /// </summary>
    internal class CustomTypeMappersContainer : ICustomTypeMappersRegistry, ICustomTypeMappersProvider
    {
        /// <summary>
        /// The collection of all the registered custom type mappers.
        /// </summary>
        private List<ITypeMapper> customTypeMappers;

        /// <summary>
        /// Create new instance of the <see cref="CustomTypeMappersContainer"/> class.
        /// </summary>
        public CustomTypeMappersContainer()
        {
            customTypeMappers = new List<ITypeMapper>();
        }

        #region ICustomTypeMappersProvider Implementation

        /// <inheritdoc/>
        public IEnumerable<ITypeMapper> GetCustomTypeMappers()
        {
            return customTypeMappers;
        }

        #endregion ICustomTypeMappersProvider Implementation

        #region ICustomTypeMappersRegistry Implementation

        /// <inheritdoc/>
        public void RegisterCustomTypeMapper(ITypeMapper typeMapper)
        {
            customTypeMappers.Add(typeMapper);
        }

        #endregion ICustomTypeMappersRegistry Implementation
    }
}
