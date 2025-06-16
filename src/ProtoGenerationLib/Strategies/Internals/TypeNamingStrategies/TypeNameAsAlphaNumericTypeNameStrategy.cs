using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Linq;

namespace ProtoGenerationLib.Strategies.Internals.TypeNamingStrategies
{
    /// <summary>
    /// A type naming strategy where the proto name is the same as the type name.
    /// </summary>
    public class TypeNameAsAlphaNumericTypeNameStrategy : ITypeNamingStrategy
    {
        /// <inheritdoc/>
        public string GetTypeName(Type type)
        {
            if (type.IsArray)
            {
                var elementType = type.GetArrayElementType();
                if (type.IsSingleDimensionalArray())
                {
                    return $"ArrayOf{GetTypeName(elementType)}";
                }

                return $"MultiDimensionalArrayOf{GetTypeName(elementType)}";
            }

            if (!type.IsGenericType)
                return type.Name;

            var genericArguments = type.GetGenericArguments();
            var genericArgumentsString = string.Join(string.Empty, genericArguments.Select(GetTypeName));
            return $"{type.GetTypeNameWithoutGenerics()}Of{genericArgumentsString}";
        }
    }
}
