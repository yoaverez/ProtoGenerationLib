using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Strategies.Internals.TypeNamingStrategies;
using System;

namespace ProtoGenerator.Strategies.Internals.FileNamingStrategies
{
    /// <summary>
    /// A file naming strategy that
    /// in which the file name will be the type name.
    /// </summary>
    public class TypeNameAsFileNameStrategy : IFileNamingStrategy
    {
        /// <summary>
        /// The strategy that converts a type name to an alpha numeric
        /// type name.
        /// </summary>
        private ITypeNamingStrategy typeNamingStrategy;

        /// <summary>
        /// Create new instance of the <see cref="TypeNameAsFileNameStrategy"/> class.
        /// </summary>
        public TypeNameAsFileNameStrategy()
        {
            typeNamingStrategy = new TypeNameAsAlphaNumericTypeNameStrategy();
        }

        /// <inheritdoc/>
        public string GetFilePath(Type type)
        {
            return $"{typeNamingStrategy.GetTypeName(type)}.proto";
        }
    }
}
