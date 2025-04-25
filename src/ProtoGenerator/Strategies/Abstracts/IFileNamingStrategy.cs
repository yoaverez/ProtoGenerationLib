using System;

namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// Naming strategy for getting proto file path that will contain the type.
    /// </summary>
    public interface IFileNamingStrategy
    {
        /// <summary>
        /// Gets the file path (should be relative) of the proto file that will
        /// contains the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose proto file path you want.</param>
        /// <returns>
        /// The file path (should be relative) of the proto file that will
        /// contains the given <paramref name="type"/>.
        /// </returns>
        /// <remarks>
        /// This strategy should make sure that if two types have the same file path,
        /// they should also have the same package in the <see cref="IPackageNamingStrategy"/>.
        /// </remarks>
        string GetFilePath(Type type);
    }
}
