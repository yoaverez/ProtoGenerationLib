﻿using ProtoGenerationLib.Strategies.Internals.TypeNamingStrategies;
using System;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.Strategies.Internals.NewTypeNamingStrategy
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
            var newTypeName = typeNamingStrategy.GetTypeName(type);

            // The only reason to create new enum type is
            // to wrap it in a message.
            if (type.IsEnum)
                newTypeName = $"{newTypeName}Wrapper";

            return newTypeName;
        }
    }
}
