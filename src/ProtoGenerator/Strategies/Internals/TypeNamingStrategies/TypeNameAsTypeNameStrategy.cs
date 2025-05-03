using ProtoGenerator.Strategies.Abstracts;
using System;

namespace ProtoGenerator.Strategies.Internals.TypeNamingStrategies
{
    /// <summary>
    /// A type naming strategy where the proto name is the same as the type name.
    /// </summary>
    public class TypeNameAsTypeNameStrategy : ITypeNamingStrategy
    {
        /// <inheritdoc/>
        public string GetTypeName(Type type)
        {
            return type.Name;
        }
    }
}
