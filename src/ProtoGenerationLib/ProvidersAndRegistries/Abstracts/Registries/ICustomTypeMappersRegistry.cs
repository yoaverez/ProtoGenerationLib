using ProtoGenerationLib.Mappers.Abstracts;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// Registry for custom type mappers.
    /// </summary>
    public interface ICustomTypeMappersRegistry
    {
        /// <summary>
        /// Register the given <paramref name="typeMapper"/> to the
        /// type mappers collection.
        /// </summary>
        /// <param name="typeMapper">The type mapper to register.</param>
        void RegisterCustomTypeMapper(ITypeMapper typeMapper);
    }
}
