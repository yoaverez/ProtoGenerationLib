using ProtoGenerationLib.Models.Abstracts.CustomCollections;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Models.Internals.CustomCollections
{
    /// <summary>
    /// Provider and register for field name suffixes.
    /// </summary>
    internal class FieldSuffixProviderAndRegister : IFieldSuffixRegister, IFieldSuffixProvider
    {
        /// <summary>
        /// A mapping between field type to its suffix.
        /// </summary>
        private Dictionary<Type, string> fieldTypesSuffixes;

        /// <summary>
        /// A mapping between field declaring type to a mapping between field type to its suffix.
        /// </summary>
        private Dictionary<Type, Dictionary<Type, string>> declaringTypeToFieldTypesSuffixes;

        /// <summary>
        /// A mapping between field declaring type to a mapping between field type to its
        /// excluded fields i.e. fields that should not have a suffix.
        /// </summary>
        private Dictionary<Type, Dictionary<Type, ISet<string>>> declaringTypeToExcludedFieldTypes;

        /// <summary>
        /// Create new instance of the <see cref="FieldSuffixProviderAndRegister"/> class.
        /// </summary>
        public FieldSuffixProviderAndRegister()
        {
            fieldTypesSuffixes = new Dictionary<Type, string>();
            declaringTypeToFieldTypesSuffixes = new Dictionary<Type, Dictionary<Type, string>>();
            declaringTypeToExcludedFieldTypes = new Dictionary<Type, Dictionary<Type, ISet<string>>>();
        }

        #region IFieldSuffixRegister Implementation

        /// <inheritdoc/>
        public void RegisterFieldSuffix<TFieldType>(string suffix)
        {
            var fieldType = typeof(TFieldType);
            if (fieldTypesSuffixes.ContainsKey(fieldType))
                throw new ArgumentException($"The given {nameof(TFieldType)}: {fieldType.Name} already has register suffix.", nameof(TFieldType));

            fieldTypesSuffixes.Add(fieldType, suffix);
        }

        /// <inheritdoc/>
        public void RegisterFieldSuffix<TFieldDeclaringType, TFieldType>(string suffix)
        {
            var fieldDeclaringType = typeof(TFieldDeclaringType);
            var fieldType = typeof(TFieldType);

            if (declaringTypeToFieldTypesSuffixes.ContainsKey(fieldDeclaringType))
            {
                if (declaringTypeToFieldTypesSuffixes[fieldDeclaringType].ContainsKey(fieldType))
                    throw new ArgumentException($"The given {nameof(TFieldType)}: {fieldType.Name} already has register suffix " +
                        $"for the given {nameof(TFieldDeclaringType)}: {fieldDeclaringType.Name}.", nameof(TFieldType));

            }
            else
            {
                declaringTypeToFieldTypesSuffixes.Add(fieldDeclaringType, new Dictionary<Type, string>());
            }

            declaringTypeToFieldTypesSuffixes[fieldDeclaringType].Add(fieldType, suffix);
        }

        /// <inheritdoc/>
        public void RegisterFieldThatShouldNotHaveSuffix<TFieldDeclaringType, TFieldType>(string fieldName)
        {
            var fieldDeclaringType = typeof(TFieldDeclaringType);
            var fieldType = typeof(TFieldType);

            if (declaringTypeToExcludedFieldTypes.ContainsKey(fieldDeclaringType))
            {
                if (declaringTypeToExcludedFieldTypes[fieldDeclaringType].ContainsKey(fieldType))
                {
                    if (declaringTypeToExcludedFieldTypes[fieldDeclaringType][fieldType].Contains(fieldName))
                        throw new ArgumentException($"The given {nameof(fieldName)}: {fieldName} is already excluded for " +
                            $"the fields of type {fieldType.Name} that was declared in {fieldDeclaringType.Name}", nameof(fieldName));
                }
                else
                {
                    declaringTypeToExcludedFieldTypes[fieldDeclaringType].Add(fieldType, new HashSet<string>());
                }
            }
            else
            {
                declaringTypeToExcludedFieldTypes.Add(fieldDeclaringType, new Dictionary<Type, ISet<string>>());
                declaringTypeToExcludedFieldTypes[fieldDeclaringType].Add(fieldType, new HashSet<string>());
            }

            declaringTypeToExcludedFieldTypes[fieldDeclaringType][fieldType].Add(fieldName);
        }

        #endregion IFieldSuffixRegister Implementation

        #region IFieldSuffixProvider Implementation

        /// <inheritdoc/>
        public bool TryGetFieldSuffix(Type fieldDeclaringType, Type fieldType, string fieldName, out string suffix)
        {
            suffix = string.Empty;

            // Check if the field should specifically don't need to have the suffix addition.
            if (declaringTypeToExcludedFieldTypes.ContainsKey(fieldDeclaringType))
            {
                if (declaringTypeToExcludedFieldTypes[fieldDeclaringType].ContainsKey(fieldType))
                {
                    var excludedFieldNames = declaringTypeToExcludedFieldTypes[fieldDeclaringType][fieldType];
                    if (excludedFieldNames.Contains(fieldName))
                    {
                        return false;
                    }
                }
            }

            // Check if the field should have the suffix addition due to its declaring
            // type and its type.
            if (declaringTypeToFieldTypesSuffixes.ContainsKey(fieldDeclaringType))
            {
                if (declaringTypeToFieldTypesSuffixes[fieldDeclaringType].ContainsKey(fieldType))
                {
                    suffix = declaringTypeToFieldTypesSuffixes[fieldDeclaringType][fieldType];
                    return true;
                }
            }

            // Check if the field should have the suffix addition due to its type.
            if (fieldTypesSuffixes.ContainsKey(fieldType))
            {
                suffix = fieldTypesSuffixes[fieldType];
                return true;
            }

            return false;
        }

        #endregion IFieldSuffixProvider Implementation
    }
}
