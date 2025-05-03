using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.Strategies.Internals.FileNamingStrategies
{
    /// <summary>
    /// A file name strategy that groups types by their namespace.
    /// </summary>
    public class NameSpaceAsFileNameStrategy : IFileNamingStrategy
    {
        /// <inheritdoc/>
        public string GetFilePath(Type type)
        {
            return type.Namespace;
        }
    }
}
