using ProtoGenerationLib.Strategies.Abstracts;
using System;

namespace ProtoGenerationLib.Strategies.Internals.PackageNamingStrategies
{
    /// <summary>
    /// A package naming strategy where all types
    /// share the same package.
    /// </summary>
    public class ConstNameAsPackageStrategy : IPackageNamingStrategy
    {
        /// <summary>
        /// The name of the package.
        /// </summary>
        private string package;

        /// <summary>
        /// Create new instance of the <see cref="ConstNameAsPackageStrategy"/> class.
        /// </summary>
        /// <param name="package"><inheritdoc cref="package" path="/node()"/></param>
        public ConstNameAsPackageStrategy(string package)
        {
            this.package = package;
        }

        /// <inheritdoc/>
        public string[] GetPackageComponents(Type type)
        {
            return new[] { package };
        }
    }
}
