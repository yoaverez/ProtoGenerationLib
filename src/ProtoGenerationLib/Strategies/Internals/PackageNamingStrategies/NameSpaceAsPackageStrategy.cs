using ProtoGenerationLib.Strategies.Abstracts;
using System;

namespace ProtoGenerationLib.Strategies.Internals.PackageNamingStrategies
{
    /// <summary>
    /// A package naming strategy where the package of a type is
    /// its namespace.
    /// </summary>
    public class NameSpaceAsPackageStrategy : IPackageNamingStrategy
    {
        /// <inheritdoc/>
        public string[] GetPackageComponents(Type type)
        {
            return type.Namespace.Split('.');
        }
    }
}
