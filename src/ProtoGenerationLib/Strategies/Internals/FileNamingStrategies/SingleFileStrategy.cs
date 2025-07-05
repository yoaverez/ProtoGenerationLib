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
        /// <remarks>
        /// Relative to the path from the proto root directory in which to generate the protos to.<br/>
        /// <b>Note that the path components should be separated by forward slash (/).</b>
        /// </remarks>
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
