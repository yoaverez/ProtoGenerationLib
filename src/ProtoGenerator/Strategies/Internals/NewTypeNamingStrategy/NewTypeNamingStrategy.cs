using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Strategies.Internals.TypeNamingStrategies;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoGenerator.Strategies.Internals.NewTypeNamingStrategy
{
    /// <summary>
    /// Naming strategy for new types that used the name of the type if
    /// the type is not generic otherwise uses the name without generics
    /// and a combination of the generic arguments names.
    /// </summary>
    public class NewTypeNamingStrategy : INewTypeNamingStrategy
    {
        /// <summary>
        /// The strategy to use to name the types.
        /// </summary>
        ITypeNamingStrategy typeNamingStrategy;

        /// <summary>
        /// Create new instance of the <see cref="NewTypeNamingStrategy"/> class.
        /// </summary>
        public NewTypeNamingStrategy()
        {
            typeNamingStrategy = new TypeNameAsAlphaNumericTypeNameStrategy();
        }

        /// <inheritdoc/>
        public string GetNewTypeName(Type type)
        {
            return typeNamingStrategy.GetTypeName(type);
        }
    }
}
