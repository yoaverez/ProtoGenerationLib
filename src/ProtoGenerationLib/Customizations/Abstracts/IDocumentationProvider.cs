using System;

namespace ProtoGenerationLib.Customizations.Abstracts
{
    /// <summary>
    /// A provider for types and members documentation.
    /// </summary>
    public interface IDocumentationProvider
    {
        /// <summary>
        /// Try getting the documentation of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose documentation is requested.</param>
        /// <param name="documentation">The documentation if found.</param>
        /// <returns>
        /// <see langword="true"/> if the given type has documentation associated with
        /// it otherwise <see langword="false"/>
        /// </returns>
        bool TryGetTypeDocumentation(Type type, out string documentation);

        /// <summary>
        /// Try getting the documentation of the field with the
        /// given <paramref name="fieldName"/> that is declared inside the given
        /// <paramref name="fieldDeclaringType"/>.
        /// </summary>
        /// <param name="fieldDeclaringType">The type that declared the field.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <param name="documentation">The documentation of the field if found.</param>
        /// <returns>
        /// <see langword="true"/> if the field with the given <paramref name="fieldName"/> has
        /// documentation associated with it otherwise <see langword="false"/>
        /// </returns>
        bool TryGetFieldDocumentation(Type fieldDeclaringType, string fieldName, out string documentation);

        /// <summary>
        /// Try getting the documentation of the method with the
        /// given <paramref name="methodName"/> that is declared inside the given
        /// <paramref name="methodDeclaringType"/>.
        /// </summary>
        /// <param name="methodDeclaringType">The type that declared the method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="methodNumOfParams">The number of method parameters.</param>
        /// <param name="documentation">The method documentation if found.</param>
        /// <returns>
        /// <see langword="true"/> if the method with the given <paramref name="methodName"/> and
        /// <paramref name="methodNumOfParams"/> has documentation associated with it
        /// otherwise <see langword="false"/>
        /// </returns>
        bool TryGetMethodDocumentation(Type methodDeclaringType, string methodName, int methodNumOfParams, out string documentation);

        /// <summary>
        /// Try getting the documentation of the enum value with the
        /// given <paramref name="enumValue"/> that is declared inside the given
        /// <paramref name="enumType"/>.
        /// </summary>
        /// <param name="enumType">The enum types that declares the enum value.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="documentation">The documentation of the enum value if found.</param>
        /// <returns>
        /// <see langword="true"/> if the enum value with the given <paramref name="enumValue"/>
        /// has documentation associated with it otherwise <see langword="false"/>
        /// </returns>
        bool TryGetEnumValueDocumentation(Type enumType, int enumValue, out string documentation);
    }
}
