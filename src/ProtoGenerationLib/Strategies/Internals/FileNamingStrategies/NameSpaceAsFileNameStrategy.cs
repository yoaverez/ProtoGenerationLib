using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.IO;

namespace ProtoGenerationLib.Strategies.Internals.FileNamingStrategies
{
    /// <summary>
    /// A file name strategy that groups types by their namespace.
    /// </summary>
    /// <remarks>
    /// This is a risky strategy that if the user dtos are not structured correctly,
    /// may get a recursive import to happen inside the proto files.
    /// </remarks>
    public class NameSpaceAsFileNameStrategy : IFileNamingStrategy
    {
        /// <inheritdoc/>
        public string GetFilePath(Type type)
        {
            var namespaceComponents = type.Namespace.Split('.');
            var lastComponent = namespaceComponents[namespaceComponents.Length - 1];
            namespaceComponents[namespaceComponents.Length - 1] = $"{lastComponent}.proto";
            return string.Join("/", namespaceComponents);
        }
    }
}
