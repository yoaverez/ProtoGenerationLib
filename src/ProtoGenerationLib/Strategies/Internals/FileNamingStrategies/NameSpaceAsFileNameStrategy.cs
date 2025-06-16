using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.IO;

namespace ProtoGenerationLib.Strategies.Internals.FileNamingStrategies
{
    /// <summary>
    /// A file name strategy that groups types by their namespace.
    /// </summary>
    public class NameSpaceAsFileNameStrategy : IFileNamingStrategy
    {
        /// <inheritdoc/>
        public string GetFilePath(Type type)
        {
            var namespaceComponents = type.Namespace.Split('.');
            var lastComponent = namespaceComponents[namespaceComponents.Length - 1];
            namespaceComponents[namespaceComponents.Length - 1] = $"{lastComponent}.proto";
            return Path.Combine(namespaceComponents);
        }
    }
}
