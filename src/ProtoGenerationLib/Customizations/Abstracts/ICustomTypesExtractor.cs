using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Customizations.Abstracts
{
    /// <summary>
    /// Contract for user custom types extractor.
    /// </summary>
    public interface ICustomTypesExtractor
    {
        /// <summary>
        /// Check whether or not the given <paramref name="type"/> can be handled
        /// by the this object.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/>
        /// can be handled by this object otherwise <see langword="false"/>.
        /// </returns>
        bool CanHandle(Type type);

        /// <summary>
        /// Extract all the types that are used by the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to extract used types from.</param>
        /// <returns>All the types that are used by the given <paramref name="type"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="type"/>
        /// can not be handled by this custom extractor.
        /// </exception>
        IEnumerable<Type> ExtractUsedTypes(Type type);
    }
}
