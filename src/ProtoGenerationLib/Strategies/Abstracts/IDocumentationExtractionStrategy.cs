using System;
using System.Reflection;

namespace ProtoGenerationLib.Strategies.Abstracts
{
    /// <summary>
    /// A strategy for extracting documentation from csharp types.
    /// </summary>
    public interface IDocumentationExtractionStrategy
    {
        /// <summary>
        /// Try getting the documentation of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose documentation is requested.</param>
        /// <param name="documentation">The documentation of the given <paramref name="type"/> if found.</param>
        /// <returns>
        /// <see langword="true"/> if a documentation for the given
        /// <paramref name="type"/> was found otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetTypeDocumentation(Type type, out string documentation);

        /// <summary>
        /// Try getting the documentation of the given <paramref name="fieldInfo"/>.
        /// </summary>
        /// <param name="fieldInfo">The field whose documentation is requested.</param>
        /// <param name="documentation">The documentation of the given <paramref name="fieldInfo"/> if found.</param>
        /// <returns>
        /// <see langword="true"/> if a documentation for the given
        /// <paramref name="fieldInfo"/> was found otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetFieldDocumentation(FieldInfo fieldInfo, out string documentation);

        /// <summary>
        /// Try getting the documentation of the given <paramref name="propertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">The property whose documentation is requested.</param>
        /// <param name="documentation">The documentation of the given <paramref name="propertyInfo"/> if found.</param>
        /// <returns>
        /// <see langword="true"/> if a documentation for the given
        /// <paramref name="propertyInfo"/> was found otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetPropertyDocumentation(PropertyInfo propertyInfo, out string documentation);

        /// <summary>
        /// Try getting the documentation of the given <paramref name="methodBase"/>.
        /// </summary>
        /// <param name="methodBase">The method whose documentation is requested.</param>
        /// <param name="documentation">The documentation of the given <paramref name="methodBase"/> if found.</param>
        /// <returns>
        /// <see langword="true"/> if a documentation for the given
        /// <paramref name="methodBase"/> was found otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetMethodDocumentation(MethodBase methodBase, out string documentation);

        /// <summary>
        /// Try getting the documentation of the given <paramref name="MethodBase"/>
        /// parameter named <paramref name="parameterName"/>.
        /// </summary>
        /// <param name="MethodBase">The method whose parameter documentation is requested.</param>
        /// <param name="parameterName">The name of the parameter whose documentation is requested.</param>
        /// <param name="documentation">The documentation of the given <paramref name="MethodBase"/> parameter if found.</param>
        /// <returns>
        /// <see langword="true"/> if a documentation for the given
        /// <paramref name="MethodBase"/> parameter was found otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetMethodParameterDocumentation(MethodBase MethodBase, string parameterName, out string documentation);

        /// <summary>
        /// Try getting the documentation of the given <paramref name="enumValue"/>.
        /// </summary>
        /// <param name="enumType">The type of the enum whose value documentation is requested.</param>
        /// <param name="enumValue">The enum value whose documentation is requested.</param>
        /// <param name="documentation">The documentation of the given <paramref name="enumValue"/> if found.</param>
        /// <returns>
        /// <see langword="true"/> if a documentation for the given
        /// <paramref name="enumValue"/> was found otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetEnumValueDocumentation(Type enumType, int enumValue, out string documentation);

        /// <summary>
        /// Try getting the documentation of the given <paramref name="baseType"/>
        /// as a field in a concrete class.
        /// </summary>
        /// <param name="subClassType">The type that inherit from the given <paramref name="baseType"/>.</param>
        /// <param name="baseType">The type of the base type whose field documentation is requested.</param>
        /// <param name="documentation">The documentation of the given <paramref name="baseType"/> as field if found.</param>
        /// <returns>
        /// <see langword="true"/> if a documentation for the given
        /// <paramref name="baseType"/> as a field was found otherwise <see langword="false"/>.
        /// </returns>
        bool TryGetBaseTypeFieldDocumentation(Type subClassType, Type baseType, out string documentation);
    }
}
