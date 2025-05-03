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
    }
}
