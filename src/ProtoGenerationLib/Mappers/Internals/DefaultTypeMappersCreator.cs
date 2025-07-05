using ProtoGenerationLib.Mappers.Internals.TypeMappers;
using System.Collections.Generic;
using ProtoGenerationLib.Mappers.Abstracts;

namespace ProtoGenerationLib.Mappers.Internals
{
    /// <summary>
    /// Creator of the proto generation default type mappers.
    /// </summary>
    internal static class DefaultTypeMappersCreator
    {
        /// <summary>
        /// Create the proto generation default type mappers.
        /// </summary>
        /// <returns>All the proto generation default type mappers.</returns>
        public static IEnumerable<ITypeMapper> CreateDefaultTypeMappers()
        {
            return new ITypeMapper[]
            {
                new WellKnownTypesMapper(),
            };
        }
    }
}
