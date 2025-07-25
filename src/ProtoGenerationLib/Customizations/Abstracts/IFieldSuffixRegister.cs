﻿using System;

namespace ProtoGenerationLib.Customizations.Abstracts
{
    /// <summary>
    /// Register field suffixes.
    /// </summary>
    public interface IFieldSuffixRegister
    {
        /// <summary>
        /// Add the given <paramref name="suffix"/> to the
        /// field suffixes collection.
        /// This suffix will be appended to all the fields names of type <typeparamref name="TFieldType"/>.
        /// </summary>
        /// <typeparam name="TFieldType">The type of the fields that you want to append the <paramref name="suffix"/> for.</typeparam>
        /// <param name="suffix">The suffix to append to all the fields whom type is <typeparamref name="TFieldType"/>.</param>
        /// <remarks>
        /// The suffix will be appended only if there is no specific request for a suffix
        /// that include both the declaring type and the field type.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when the field suffixes collection already contains a
        /// suffix for the given <typeparamref name="TFieldType"/>.
        /// </exception>
        void AddFieldSuffix<TFieldType>(string suffix);

        /// <summary>
        /// Add the given <paramref name="suffix"/> to the
        /// field suffixes collection.
        /// This suffix will be appended to the field with the given <paramref name="fieldName"/> in
        /// the <typeparamref name="TFieldDeclaringType"/>.
        /// </summary>
        /// <typeparam name="TFieldDeclaringType">The type of the field's declaring type.</typeparam>
        /// <param name="fieldName">The field name to append the <paramref name="suffix"/> to.</param>
        /// <param name="suffix">The suffix to append to the field with the given <paramref name="fieldName"/> in the given <typeparamref name="TFieldDeclaringType"/>.</param>
        /// <remarks>
        /// The suffix will be appended only if there is no more specific constraint for a suffix
        /// that include both the declaring type and the field name.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when the field suffixes collection already contains a
        /// suffix for the given <paramref name="fieldName"/> that was declared
        /// in the given <typeparamref name="TFieldDeclaringType"/>.
        /// </exception>
        void AddFieldSuffix<TFieldDeclaringType>(string fieldName, string suffix);

        /// <summary>
        /// Add the given <paramref name="suffix"/> to the
        /// field suffixes collection.
        /// This suffix will be appended to all the fields names of type <typeparamref name="TFieldType"/>
        /// that are declared in the given <typeparamref name="TFieldDeclaringType"/>.
        /// </summary>
        /// <typeparam name="TFieldDeclaringType">The type of the field's declaring type.</typeparam>
        /// <typeparam name="TFieldType">The type of the fields that you want to append the <paramref name="suffix"/> for.</typeparam>
        /// <param name="suffix">The suffix to append to all the fields with <typeparamref name="TFieldType"/> in the <typeparamref name="TFieldDeclaringType"/>.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the field suffixes collection already contains an
        /// entry for the given <typeparamref name="TFieldType"/> that was
        /// declared in the given <typeparamref name="TFieldDeclaringType"/>.
        /// </exception>
        void AddFieldSuffix<TFieldDeclaringType, TFieldType>(string suffix);

        /// <summary>
        /// Add the given <paramref name="fieldName"/> to a collection excluded fields.
        /// i.e fields of type <typeparamref name="TFieldType"/> that was declared in the
        /// given <typeparamref name="TFieldDeclaringType"/> and should not get a suffix appended to them.
        /// </summary>
        /// <remarks>
        /// The use of this registry method is only relevant when there are some
        /// fields that was registered for suffix addition.
        /// </remarks>
        /// <typeparam name="TFieldDeclaringType">The type of the field's declaring type.</typeparam>
        /// <typeparam name="TFieldType">The type of the fields that you want to append suffix for but have some fields that you want to exclude.</typeparam>
        /// <param name="fieldName">The name of the field to exclude for the suffix addition.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the excluded field collection already contains an
        /// entry for the given <paramref name="fieldName"/> of type <typeparamref name="TFieldType"/> that was
        /// declared in the given <typeparamref name="TFieldDeclaringType"/>.
        /// </exception>
        void AddFieldThatShouldNotHaveSuffix<TFieldDeclaringType, TFieldType>(string fieldName);
    }
}
