using ProtoGenerationLib.Strategies.Abstracts;
using System;

namespace ProtoGenerationLib.Strategies.Internals.FileNamingStrategies
{
    /// <summary>
    /// A file name strategy that puts all types in a single proto file.
    /// </summary>
    public class SingleFileStrategy : IFileNamingStrategy
    {
        /// <summary>
        /// The proto file relative path.
        /// </summary>
        /// <remarks>Relative to the base directory that we generate the protos to.</remarks>
        private string relativeFilePath;

        /// <summary>
        /// Create new instance of the <see cref="SingleFileStrategy"/> class.
        /// </summary>
        /// <param name="relativeFilePath"><inheritdoc cref="relativeFilePath" path="/node()"/></param>
        public SingleFileStrategy(string relativeFilePath)
        {
            this.relativeFilePath = relativeFilePath;
        }

        /// <inheritdoc/>
        public string GetFilePath(Type type)
        {
            return relativeFilePath;
        }
    }
}
