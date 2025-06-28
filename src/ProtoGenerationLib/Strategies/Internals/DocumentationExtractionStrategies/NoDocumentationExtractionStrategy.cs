using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.Reflection;

namespace ProtoGenerationLib.Strategies.Internals.DocumentationExtractionStrategies
{
    /// <summary>
    /// A documentation strategy that returns false for each try get documentation
    /// method.
    /// </summary>
    public class NoDocumentationExtractionStrategy : IDocumentationExtractionStrategy
    {
        /// <inheritdoc/>
        public bool TryGetBaseTypeFieldDocumentation(Type subClassType, Type baseType, out string documentation)
        {
            return TryGetDocumentation(out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetEnumValueDocumentation(Type enumType, int enumValue, out string documentation)
        {
            return TryGetDocumentation(out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetFieldDocumentation(FieldInfo fieldInfo, out string documentation)
        {
            return TryGetDocumentation(out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetMethodDocumentation(MethodInfo methodInfo, out string documentation)
        {
            return TryGetDocumentation(out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetPropertyDocumentation(PropertyInfo propertyInfo, out string documentation)
        {
            return TryGetDocumentation(out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetTypeDocumentation(Type type, out string documentation)
        {
            return TryGetDocumentation(out documentation);
        }

        /// <summary>
        /// A method the puts <see cref="string.Empty"/> in the given
        /// <paramref name="documentation"/> and return <see langword="false"/>.
        /// </summary>
        private bool TryGetDocumentation(out string documentation)
        {
            documentation = string.Empty;
            return false;
        }
    }
}
