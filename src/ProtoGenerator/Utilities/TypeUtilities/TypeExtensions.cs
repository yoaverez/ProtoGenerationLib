using ProtoGenerator.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProtoGenerator.Utilities.TypeUtilities
{
    /// <summary>
    /// Extension methods for <see cref="Type"/>s.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Try getting the base type of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose base type you want.</param>
        /// <param name="baseType">The base type if exists otherwise the default of <see cref="Type"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/>
        /// has a base type that is not <see cref="object"/>
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryGetBase(this Type type, out Type baseType)
        {
            baseType = default;
            if (type == null || type.BaseType == null || type.BaseType.Equals(typeof(object)))
                return false;

            baseType = type.BaseType;
            return true;
        }

        /// <summary>
        /// Retrieves all the interfaces (direct and indirect)
        /// that the given <paramref name="type"/> implements.
        /// </summary>
        /// <param name="type">The type whose interfaces to retrieve.</param>
        /// <returns>
        /// An array of all the interfaces (direct and indirect)
        /// that the given <paramref name="type"/> implements.
        /// </returns>
        public static Type[] GetAllImplementedInterfaces(this Type type)
        {
            var interfaces = new HashSet<Type>();
            foreach (var interfaceType in type.GetInterfaces())
            {
                interfaces.Add(interfaceType);
                interfaces.AddRange(interfaceType.GetAllImplementedInterfaces());
            }
            return interfaces.ToArray();
        }

        /// <summary>
        /// Check if the given <paramref name="attributeType"/> is inherited
        /// attribute.
        /// </summary>
        /// <param name="attributeType">The attribute type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="attributeType"/>
        /// is inherited attribute otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="attributeType"/>
        /// is not an <see cref="Attribute"/>
        /// </exception>
        public static bool IsAttributeInherited(this Type attributeType)
        {
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException("Type must be an Attribute", nameof(attributeType));

            // Get the AttributeUsage attribute applied to the attribute class.
            AttributeUsageAttribute usage = attributeType.GetCustomAttribute<AttributeUsageAttribute>(false);

            // If AttributeUsage isn't specified, the default is Inherited = true.
            if (usage == null)
                return true;

            // Return the Inherited property value.
            return usage.Inherited;
        }

        /// <summary>
        /// Extract all the public methods of the given <paramref name="type"/>
        /// that have the attribute type of the given <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="type">The type whose methods to extract.</param>
        /// <param name="attributeType">The type of the attribute of the wanted methods.</param>
        /// <returns>
        /// All the public methods of the given <paramref name="type"/>
        /// that have the attribute type of the given <paramref name="attributeType"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="attributeType"/>
        /// is not an <see cref="Attribute"/>
        /// </exception>
        public static IEnumerable<MethodInfo> ExtractMethods(this Type type, Type attributeType)
        {
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException("Type must be an Attribute", nameof(attributeType));

            // Find all the methods of the given type.
            var allMethods = new HashSet<MethodInfo>();

            // Since interface attributes is not inherit we need to take all the interfaces methods.
            var implementedInterfaces = type.GetAllImplementedInterfaces();
            allMethods.AddRange(implementedInterfaces.SelectMany(interfaceType => interfaceType.GetMethods()));

            // Add all the methods of the current type.
            allMethods.AddRange(type.GetMethods());

            // Filter all the methods with the given attribute.
            var filteredMethods = allMethods.Where(method => method.IsDefined(attributeType, attributeType.IsAttributeInherited()));
            return filteredMethods;
        }

        /// <summary>
        /// Checks whether or not the given <paramref name="type"/> is
        /// a single dimensional array.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/>
        /// is a single dimensional array otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsSingleDimensionalArray(this Type type)
        {
            // Type is not an array or it is multi dimensional array.
            if (!type.IsArray || type.GetArrayRank() > 1)
            {
                return false;
            }
            // Type is a single dimensional array of a jagged array.
            else
            {
                // If the element type is not an array than it
                // is a single dimensional array and not a jagged array.
                return !(type.GetElementType()!.IsArray);
            }
        }

        /// <summary>
        /// Retrieves the element type of the given <paramref name="arrayType"/>.
        /// </summary>
        /// <param name="arrayType">The array type whose element type you want.</param>
        /// <returns>The element type of the given <paramref name="arrayType"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the given <paramref name="arrayType"/> is not an array.</exception>
        public static Type GetArrayElementType(this Type arrayType)
        {
            if (!arrayType.IsArray)
                throw new ArgumentException($"The given {nameof(arrayType)} is not an array.", nameof(arrayType));

            var elementType = arrayType.GetElementType();
            while (elementType.IsArray)
            {
                elementType = elementType.GetElementType();
            }

            return elementType;
        }

        /// <summary>
        /// Check whether or not the given <paramref name="type"/> is a <see cref="Nullable{T}"/> type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given type is <see cref="Nullable{T}"/> otherwise
        /// <see langword="false"/>.
        /// </returns>
        public static bool IsNullable(this Type type)
        {
            return type.TryGetElementOfNullableType(out _);
        }

        /// <summary>
        /// Try getting the element type of a <see cref="Nullable{T}"/> type.
        /// </summary>
        /// <param name="type">The type that is allegedly <see cref="Nullable{T}"/>.</param>
        /// <param name="elementType">
        /// The element type of the <see cref="Nullable{T}"/> if given
        /// <paramref name="type"/> is <see cref="Nullable{T}"/> otherwise
        /// <see langword="default"/> of <see cref="Type"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> is
        /// an <see cref="Nullable{T}"/> otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryGetElementOfNullableType(this Type type, out Type elementType)
        {
            elementType = default;
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                elementType = type.GetGenericArguments().Single();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether or not the given <paramref name="type"/> is a <see cref="IEnumerable{T}"/> type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given type is <see cref="IEnumerable{T}"/> otherwise
        /// <see langword="false"/>.
        /// </returns>
        public static bool IsEnumerableType(this Type type)
        {
            return type.TryGetElementOfEnumerableType(out _);
        }

        /// <summary>
        /// Try getting the element type of an <see cref="IEnumerable{T}"/> type.
        /// </summary>
        /// <param name="type">The alleged <see cref="IEnumerable{T}"/>.</param>
        /// <param name="elementType">
        /// The element type of the <see cref="IEnumerable{T}"/> if given
        /// <paramref name="type"/> is <see cref="IEnumerable{T}"/> otherwise
        /// <see langword="default"/> of <see cref="Type"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> is
        /// an <see cref="IEnumerable{T}"/> otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryGetElementOfEnumerableType(this Type type, out Type elementType)
        {
            elementType = default;

            var possibleTypes = type.GetAllImplementedInterfaces().Append(type).ToArray();
            foreach (var possibleType in possibleTypes)
            {
                if (possibleType.IsGenericType && possibleType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    elementType = possibleType.GetGenericArguments().Single();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check whether or not the given <paramref name="type"/> is an <see cref="IEnumerable{T}"/> of
        /// <see cref="KeyValuePair{TKey, TValue}"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given type is an <see cref="IEnumerable{T}"/> of
        /// <see cref="KeyValuePair{TKey, TValue}"/> otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsKeyValuePairEnumerableType(this Type type)
        {
            return TryGetElementsOfKeyValuePairEnumerableType(type, out _, out _);
        }

        /// <summary>
        /// Try getting the keyType and the valueType of the given <paramref name="type"/>.
        /// If the given is not an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/>,
        /// it will return the default value of <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type from which to try getting the keyType and the valueType.</param>
        /// <param name="keyType">
        /// If the given type is an <see cref="IEnumerable{T}"/> of
        /// <see cref="KeyValuePair{TKey, TValue}"/> then this will contain the key type of the
        /// <see cref="KeyValuePair{TKey, TValue}"/> other wise this will contain the <see langword="default"/>
        /// of <see cref="Type"/>.
        /// </param>
        /// <param name="valueType">
        /// If the given type is an <see cref="IEnumerable{T}"/> of
        /// <see cref="KeyValuePair{TKey, TValue}"/> then this will contain the value type of the
        /// <see cref="KeyValuePair{TKey, TValue}"/> other wise this will contain the <see langword="default"/>
        /// of <see cref="Type"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> is an
        /// <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/>
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryGetElementsOfKeyValuePairEnumerableType(this Type type, out Type keyType, out Type valueType)
        {
            keyType = default;
            valueType = default;

            var possibleTypes = type.GetAllImplementedInterfaces().Append(type).ToArray();
            foreach (var possibleType in possibleTypes)
            {
                if (possibleType.IsGenericType && possibleType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    var elements = possibleType.GetGenericArguments();
                    var element = elements[0];
                    if (element.IsGenericType && element.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                    {
                        var genericArguments = element.GetGenericArguments();
                        keyType = genericArguments[0];
                        valueType = genericArguments[1];
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks whether or not the given type is a <see cref="ValueTuple"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/>
        /// is a <see cref="ValueTuple"/> otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsValueTuple(this Type type)
        {
            return type.GetTypeNameWithoutGenerics().Equals(typeof(ValueTuple<>).GetTypeNameWithoutGenerics());
        }

        /// <summary>
        /// Checks whether or not the given type is a <see cref="Tuple"/>.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/>
        /// is a <see cref="Tuple"/> otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsTuple(this Type type)
        {
            return type.GetTypeNameWithoutGenerics().Equals(typeof(Tuple<>).GetTypeNameWithoutGenerics());
        }

        /// <summary>
        /// Get the name of the given <paramref name="type"/> without the
        /// generic suffix.<br/>
        /// e.g. List&lt;int&gt; name is "List`1[System.Int32]" → "List"
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// The name of the given <paramref name="type"/> without the
        /// generic suffix.
        /// </returns>
        public static string GetTypeNameWithoutGenerics(this Type type)
        {
            return string.Join("", type.Name.TakeWhile(c => c != '`'));
        }
    }
}
