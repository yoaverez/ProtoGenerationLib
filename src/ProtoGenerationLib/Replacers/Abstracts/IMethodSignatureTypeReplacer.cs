using System;

namespace ProtoGenerationLib.Replacers.Abstracts
{
    /// <summary>
    /// A contract for a method signature type replacer.
    /// </summary>
    public interface IMethodSignatureTypeReplacer
    {
        /// <summary>
        /// Checks whether or not this replacer can replace the
        /// given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="isReturnType">Whether or not the given type is a method return type.</param>
        /// <returns>
        /// <see langword="true"/> if this replacer can replace the given <paramref name="type"/>
        /// otherwise <see langword="false"/>.
        /// </returns>
        bool CanReplace(Type type, bool isReturnType);

        /// <summary>
        /// Replace the given method signature <paramref name="type"/>
        /// with another type.
        /// </summary>
        /// <param name="type">The type to replace.</param>
        /// <param name="isReturnType">Whether or not the given type is a method return type.</param>
        /// <returns>
        /// The type that should replace the given <paramref name="type"/>.
        /// </returns>
        Type ReplaceType(Type type, bool isReturnType);
    }
}
