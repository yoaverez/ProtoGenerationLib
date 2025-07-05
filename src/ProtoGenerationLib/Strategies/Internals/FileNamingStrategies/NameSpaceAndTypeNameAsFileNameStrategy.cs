using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.Linq;

namespace ProtoGenerationLib.Strategies.Internals.FileNamingStrategies
{
    /// <summary>
    /// A file naming strategy that groups types by their namespace and their names.
    /// </summary>
    public class NameSpaceAndTypeNameAsFileNameStrategy : IFileNamingStrategy
    {
        /// <inheritdoc cref="TypeNameAsFileNameStrategy"/>
        private TypeNameAsFileNameStrategy typeNameAsFileNameStrategy;

        /// <summary>
        /// Create new instance of the <see cref="NameSpaceAndTypeNameAsFileNameStrategy"/> class.
        /// </summary>
        public NameSpaceAndTypeNameAsFileNameStrategy()
        {
            typeNameAsFileNameStrategy = new TypeNameAsFileNameStrategy();
        }

        /// <inheritdoc/>
        public string GetFilePath(Type type)
        {
            var namespaceComponents = type.Namespace.Split('.');
            var typeNameAsFileName = typeNameAsFileNameStrategy.GetFilePath(type);
            return string.Join("/", namespaceComponents.Append(typeNameAsFileName));
        }
    }
}
