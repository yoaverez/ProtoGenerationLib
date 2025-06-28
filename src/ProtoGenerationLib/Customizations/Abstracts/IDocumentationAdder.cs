using System;

namespace ProtoGenerationLib.Customizations.Abstracts
{
    /// <summary>
    /// A contract for adding documentation to types and members.
    /// </summary>
    public interface IDocumentationAdder
    {
        /// <summary>
        /// Associate the given <paramref name="documentation"/>
        /// with the given <typeparamref name="TType"/>.
        /// </summary>
        /// <typeparam name="TType">The type to associate the <paramref name="documentation"/> with.</typeparam>
        /// <param name="documentation">The documentation to add to the given <typeparamref name="TType"/>.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already a documentation that is associated
        /// with the given <typeparamref name="TType"/>.
        /// </exception>
        void AddDocumentation<TType>(string documentation);

        /// <summary>
        /// Associate the given <paramref name="documentation"/>
        /// with the field with the given <paramref name="fieldName"/> that
        /// is declared in the given <typeparamref name="TFieldDeclaringType"/>.
        /// </summary>
        /// <typeparam name="TFieldDeclaringType">The type that declares the requested field.</typeparam>
        /// <param name="fieldName">The name of the field to associate the <paramref name="documentation"/> with.</param>
        /// <param name="documentation">The documentation to add to the field.</param>
        /// <remarks>
        /// The method also works for properties.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already a documentation that is associated
        /// with the field with the given <paramref name="fieldName"/> that was declared
        /// in the given <typeparamref name="TFieldDeclaringType"/>.
        /// </exception>
        void AddDocumentation<TFieldDeclaringType>(string fieldName, string documentation);

        /// <summary>
        /// Associate the given <paramref name="documentation"/>
        /// with the method with the given <paramref name="methodName"/> that
        /// is declared in the given <typeparamref name="TMethodDeclaringType"/>.
        /// </summary>
        /// <typeparam name="TMethodDeclaringType">The type that declares the requested method.</typeparam>
        /// <param name="methodName">The name of the method to associate the <paramref name="documentation"/> with.</param>
        /// <param name="numOfParameters">
        /// The number of the parameters of the method.
        /// Needed in order to associate the documentation the right method.
        /// </param>
        /// <param name="documentation">The documentation to add to the method.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already a documentation that is associated
        /// with the method with the given <paramref name="methodName"/> that was declared
        /// in the given <typeparamref name="TMethodDeclaringType"/>.
        /// </exception>
        void AddDocumentation<TMethodDeclaringType>(string methodName, int numOfParameters, string documentation);

        /// <summary>
        /// Associate the given <paramref name="documentation"/>
        /// with the given <paramref name="enumValue"/>.
        /// </summary>
        /// <typeparam name="TEnumType">The type of the enum whose value documentation is requested.</typeparam>
        /// <param name="enumValue">The value of the enum.</param>
        /// <param name="documentation">The documentation to add to the given <paramref name="enumValue"/>.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when there is already a documentation that is associated
        /// with the given <paramref name="enumValue"/>.
        /// </exception>
        void AddDocumentation<TEnumType>(int enumValue, string documentation) where TEnumType : Enum;
    }
}
