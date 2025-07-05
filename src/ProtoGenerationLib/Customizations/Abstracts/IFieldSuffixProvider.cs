using System;

namespace ProtoGenerationLib.Customizations.Abstracts
{
    /// <summary>
    /// Provider for field names suffixes.
    /// </summary>
    public interface IFieldSuffixProvider
    {
        /// <summary>
        /// Try getting the suffix of the field with the given <paramref name="fieldName"/>
        /// of <paramref name="fieldType"/> that was declared in the given <paramref name="fieldDeclaringType"/>.
        /// </summary>
        /// <param name="fieldDeclaringType">The type that declared the field with the given <paramref name="fieldName"/> and <paramref name="fieldType"/>.</param>
        /// <param name="fieldType">The type of the field whose suffix is requested.</param>
        /// <param name="fieldName">The name of the field whose suffix is requested.</param>
        /// <param name="suffix">The suffix to append to the field name is this method return <see langword="true"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the field with the given <paramref name="fieldName"/>
        /// of <paramref name="fieldType"/> that was declared in the given
        /// <paramref name="fieldDeclaringType"/> should have a suffix addition
        /// otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetFieldSuffix(Type fieldDeclaringType, Type fieldType, string fieldName, out string suffix);
    }
}
