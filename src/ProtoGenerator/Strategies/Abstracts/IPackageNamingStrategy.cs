using System;

namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// Naming strategy for getting a csharp type protobuf package.
    /// </summary>
    public interface IPackageNamingStrategy
    {
        /// <summary>
        /// Get the components of the protobuf package for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose protobuf package components you want.</param>
        /// <returns>The components of the protobuf package for the given <paramref name="type"/>.</returns>
        /// <remarks>
        /// Package components are the strings that are separated by `.`
        /// e.g. abs_dfd.gg.wer -> 3 components: abs_dfd, gg, wer.
        /// </remarks>
        string[] GetPackageComponents(Type type);
    }
}
