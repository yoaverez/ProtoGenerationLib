using ProtoGenerationLib.Customizations.Abstracts;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Customizations.Internals
{
    /// <summary>
    /// A provider and adder of custom documentation of type and members.
    /// </summary>
    internal class DocumentationProviderAndAdder : IDocumentationAdder, IDocumentationProvider
    {
        /// <summary>
        /// A mapping between a type to its documentation.
        /// </summary>
        private Dictionary<Type, string> typesDocumentations;

        /// <summary>
        /// A mapping between field declaring type and the field name
        /// to its documentation.
        /// </summary>
        private Dictionary<(Type, string), string> fieldsDocumentations;

        /// <summary>
        /// A mapping between method declaring type, method name and method
        /// number of parameters to its documentation.
        /// </summary>
        private Dictionary<(Type, string, int), string> methodsDocumentations;

        /// <summary>
        /// A mapping between enum type and enum value to its documentation.
        /// </summary>
        private Dictionary<(Type, int), string> enumValuesDocumentations;

        /// <summary>
        /// Create new instance of the <see cref="DocumentationProviderAndAdder"/> class.
        /// </summary>
        public DocumentationProviderAndAdder()
        {
            typesDocumentations = new Dictionary<Type, string>();
            fieldsDocumentations = new Dictionary<(Type, string), string>();
            methodsDocumentations = new Dictionary<(Type, string, int), string>();
            enumValuesDocumentations = new Dictionary<(Type, int), string>();
        }

        #region IDocumentationAdder Implementation

        /// <inheritdoc/>
        public void AddTypeDocumentation<TType>(string documentation)
        {
            var type = typeof(TType);
            if (typesDocumentations.ContainsKey(type))
                throw new ArgumentException($"The given {nameof(TType)}: {type.Name} has already a documentation associated with it.");

            typesDocumentations.Add(type, documentation);
        }

        /// <inheritdoc/>
        public void AddFieldDocumentation<TFieldDeclaringType>(string fieldName, string documentation)
        {
            var fieldDeclaringType = typeof(TFieldDeclaringType);
            if (fieldsDocumentations.ContainsKey((fieldDeclaringType, fieldName)))
                throw new ArgumentException($"" +
                    $"The field with the given {nameof(fieldName)}: {fieldName} that was " +
                    $"declared in the given {nameof(TFieldDeclaringType)}: {fieldDeclaringType.Name}" +
                    $"has already a documentation associated with it.");

            fieldsDocumentations.Add((fieldDeclaringType, fieldName), documentation);
        }

        /// <inheritdoc/>
        public void AddMethodDocumentation<TMethodDeclaringType>(string methodName, int numOfParameters, string documentation)
        {
            var methodDeclaringType = typeof(TMethodDeclaringType);
            if (methodsDocumentations.ContainsKey((methodDeclaringType, methodName, numOfParameters)))
                throw new ArgumentException($"" +
                    $"The method with the given {nameof(methodName)}: {methodName} that was " +
                    $"declared in the given {nameof(TMethodDeclaringType)}: {methodDeclaringType.Name}" +
                    $"and has {numOfParameters} parameters, " +
                    $"has already a documentation associated with it.");

            methodsDocumentations.Add((methodDeclaringType, methodName, numOfParameters), documentation);
        }

        /// <inheritdoc/>
        public void AddEnumValueDocumentation<TEnumType>(int enumValue, string documentation) where TEnumType : Enum
        {
            var enumType = typeof(TEnumType);
            if (enumValuesDocumentations.ContainsKey((enumType, enumValue)))
                throw new ArgumentException($"" +
                    $"The enum value with the given {nameof(enumValue)}: {enumValue} that was " +
                    $"declared in the given {nameof(TEnumType)}: {enumType.Name}" +
                    $"has already a documentation associated with it.");

            enumValuesDocumentations.Add((enumType, enumValue), documentation);
        }

        #endregion IDocumentationAdder Implementation

        #region IDocumentationProvider Implementation

        /// <inheritdoc/>
        public bool TryGetEnumValueDocumentation(Type enumType, int enumValue, out string documentation)
        {
            return enumValuesDocumentations.TryGetValue((enumType, enumValue), out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetFieldDocumentation(Type fieldDeclaringType, string fieldName, out string documentation)
        {
            return fieldsDocumentations.TryGetValue((fieldDeclaringType, fieldName), out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetMethodDocumentation(Type methodDeclaringType, string methodName, int methodNumOfParams, out string documentation)
        {
            return methodsDocumentations.TryGetValue((methodDeclaringType, methodName, methodNumOfParams), out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetTypeDocumentation(Type type, out string documentation)
        {
            return typesDocumentations.TryGetValue(type, out documentation);
        }

        #endregion IDocumentationProvider Implementation
    }
}
