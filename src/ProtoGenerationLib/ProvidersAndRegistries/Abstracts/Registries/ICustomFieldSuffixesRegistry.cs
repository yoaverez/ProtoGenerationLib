using ProtoGenerationLib.Models.Abstracts.CustomCollections;

namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Registries
{
    /// <summary>
    /// A registry for custom field suffixes.
    /// </summary>
    internal interface ICustomFieldSuffixesRegistry
    {
        /// <inheritdoc cref="IFieldSuffixRegister.RegisterFieldSuffix{TFieldType}(string)"/>
        void RegisterCustomFieldSuffix<TFieldType>(string suffix);

        /// <inheritdoc cref="IFieldSuffixRegister.RegisterFieldSuffix{TFieldDeclaringType}(string, string)"/>
        void RegisterCustomFieldSuffix<TFieldDeclaringType>(string fieldName, string suffix);

        /// <inheritdoc cref="IFieldSuffixRegister.RegisterFieldSuffix{TFieldDeclaringType, TFieldType}(string)"/>
        void RegisterCustomFieldSuffix<TFieldDeclaringType, TFieldType>(string suffix);

        /// <inheritdoc cref="IFieldSuffixRegister.RegisterFieldSuffix{TFieldDeclaringType, TFieldType}(string)"/>
        void RegisterCustomFieldThatShouldNotHaveSuffix<TFieldDeclaringType, TFieldType>(string fieldName);
    }
}
