using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.Strategies.Internals.FileNamingStrategies
{
    /// <summary>
    /// A file naming strategy that
    /// in which the file name will be the type name.
    /// </summary>
    public class TypeNameAsFileNameStrategy : IFileNamingStrategy
    {
        /// <inheritdoc/>
        public string GetFilePath(Type type)
        {
            return type.Name;
        }
    }
}
