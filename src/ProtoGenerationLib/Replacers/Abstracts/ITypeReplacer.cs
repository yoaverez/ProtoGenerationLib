using ProtoGenerationLib.Configurations.Abstracts;
using System;

namespace ProtoGenerationLib.Replacers.Abstracts
{
    /// <summary>
    /// A contract for type replacers.
    /// </summary>
    public interface ITypeReplacer
    {
        /// <summary>
        /// Checks whether or not the given <paramref name="type"/> can be replaced
        /// by this replacer.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> can be
        /// replaced by this replacer.
        /// </returns>
        bool CanReplaceType(Type type);

        /// <summary>
        /// Create a new type for replacing the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to replace.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>
        /// A new type for replacing the given <paramref name="type"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="type"/> can not be replaced
        /// by this replacer.
        /// </exception>
        Type ReplaceType(Type type, IProtoGenerationOptions generationOptions);
    }
}
