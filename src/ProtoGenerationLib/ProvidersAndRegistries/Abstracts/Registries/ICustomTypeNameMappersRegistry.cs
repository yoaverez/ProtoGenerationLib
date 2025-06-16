using ProtoGenerationLib.Mappers.Abstracts;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for custom type name mappers.
    /// </summary>
    public interface ICustomTypeNameMappersRegistry
    {
        /// <summary>
        /// Register the given <paramref name="typeNameMapper"/> to the
        /// type name mappers collection.
        /// </summary>
        /// <param name="typeNameMapper">The type name mapper to register.</param>
        void RegisterCustomTypeNameMapper(ITypeMapper typeNameMapper);
    }
}
