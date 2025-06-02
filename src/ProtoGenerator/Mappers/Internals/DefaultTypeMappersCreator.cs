using ProtoGenerator.Mappers.Abstracts;
using ProtoGenerator.Mappers.Internals.TypeMappers;
using System.Collections.Generic;

namespace ProtoGenerator.Mappers.Internals
{
    /// <summary>
    /// Creator of the proto generation default type mappers.
    /// </summary>
    public static class DefaultTypeMappersCreator
    {
        /// <summary>
        /// Create the proto generation default type mappers.
        /// </summary>
        /// <returns>All the proto generation default type mappers.</returns>
        public static IEnumerable<ITypeNameMapper> CreateDefaultTypeMappers()
        {
            return new ITypeNameMapper[]
            {
                new WellKnownTypesMapper(),
            };
        }
    }
}
