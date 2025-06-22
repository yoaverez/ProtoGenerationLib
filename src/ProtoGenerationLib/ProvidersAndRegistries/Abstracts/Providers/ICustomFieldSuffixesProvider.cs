using ProtoGenerationLib.Models.Abstracts.CustomCollections;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider for custom fields suffixes.
    /// </summary>
    public interface ICustomFieldSuffixesProvider
    {
        /// <summary>
        /// Get an object that can provide suffixes to field names.
        /// </summary>
        /// <returns>An object that can provide suffixes to field names.</returns>
        IFieldSuffixProvider GetFieldSuffixProvider();
    }
}
