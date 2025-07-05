using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtoGenerationLib.Utilities.TypeUtilities
{
    /// <summary>
    /// Class for creating types.
    /// </summary>
    internal class TypeCreator
    {
        /// <summary>
        /// The default namespace name of the newly created types.
        /// </summary>
        public const string DEFAULT_NAMESPACE_NAME = "ProtoGenerator.DynamicTypes";

        /// <summary>
        /// The assembly name of the newly created types.
        /// </summary>
        private const string DYNAMIC_TYPE_ASSEMBLY_NAME = "DynamicAssembly";

        /// <summary>
        /// The module name of the newly created types.
        /// </summary>
        private const string DYNAMIC_TYPE_MODULE_NAME = "DynamicModule";

        /// <summary>
        /// A builder that can create type builders.
        /// </summary>
        private static readonly ModuleBuilder moduleBuilder;

        /// <summary>
        /// A mapping between the name of a created type to the created type.
        /// </summary>
        private static readonly Dictionary<string, Type> CreatedTypes;

        /// <summary>
        /// Initialize the class static members.
        /// </summary>
        static TypeCreator()
        {
            // Create assembly name.
            var assemblyName = new AssemblyName(DYNAMIC_TYPE_ASSEMBLY_NAME);

            // Create the assembly builder.
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            // Create a module builder.
            moduleBuilder = assemblyBuilder.DefineDynamicModule(DYNAMIC_TYPE_MODULE_NAME);

            CreatedTypes = new Dictionary<string, Type>();
        }

        /// <summary>
        /// Crete a data type i.e. a type with only properties.
        /// </summary>
        /// <param name="typeName">The name of the new type.</param>
        /// <param name="Properties">The properties of the new type.</param>
        /// <param name="nameSpace">
        /// The name space of the new type. It is recommended to give the name space
        /// of the type that caused this creation to prevent recursive references.
        /// </param>
        /// <returns>A new type that contains all the given <paramref name="Properties"/>.</returns>
        public static Type CreateDataType(string typeName, IEnumerable<(Type Type, string Name)> Properties, string nameSpace = DEFAULT_NAMESPACE_NAME)
        {
            if (CreatedTypes.TryGetValue(typeName, out var value))
            {
                return value;
            }

            // Create a type builder.
            var typeBuilder = moduleBuilder.DefineType($"{nameSpace}.{typeName}", TypeAttributes.Public | TypeAttributes.Class);

            // Add fields.
            foreach (var fieldData in Properties)
            {
                DefinePublicProperty(typeBuilder, fieldData.Type, fieldData.Name);
            }

            // Create the type.
            var createdType = typeBuilder.CreateTypeInfo();
            CreatedTypes[typeName] = createdType;
            return createdType;
        }

        /// <summary>
        /// Create a new array type that can be converted to proto from the given <paramref name="arrayType"/>.
        /// </summary>
        /// <param name="arrayType">The csharp array to convert to a new type that can be converted to proto.</param>
        /// <param name="newTypeNamingFunction">A function for choosing names to all the needed newly created types.</param>
        /// <param name="nameSpace">
        /// The name space of the new type. It is recommended to give the name space
        /// of the type that caused this creation to prevent recursive references.
        /// </param>
        /// <returns>A new array type that can be converted to proto from the given <paramref name="arrayType"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="arrayType"/> is not an array.
        /// </exception>
        public static Type CreateProtoArrayType(Type arrayType, Func<Type, string> newTypeNamingFunction, string nameSpace = DEFAULT_NAMESPACE_NAME)
        {
            if (!arrayType.IsArray)
                throw new ArgumentException($"The given {nameof(arrayType)} is not an array", nameof(arrayType));

            var newTypeName = newTypeNamingFunction(arrayType);
            if (CreatedTypes.TryGetValue(newTypeName, out Type value))
            {
                return value;
            }

            if (arrayType.IsMultiDimensionalArray())
                return CreateMultiDimensionalArrayType(arrayType, newTypeNamingFunction, nameSpace);

            else if (arrayType.IsJaggedArray())
                return CreateJaggedArrayType(arrayType, newTypeNamingFunction, nameSpace);

            else
            {
                // The array is a single dimensional array.
                // So just wrap the array.
                var props = new List<(Type, string)> { (arrayType, "Elements") };
                return CreateDataType(newTypeName, props, nameSpace);
            }
        }

        /// <summary>
        /// Try getting a type that was already created.
        /// </summary>
        /// <param name="typeName">The name of the requested type.</param>
        /// <param name="type">The result type if found otherwise null.</param>
        /// <returns>
        /// <see langword="true"/> if a type with a name of the given <paramref name="typeName"/>
        /// was created otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryGetCreatedType(string typeName, out Type type)
        {
            type = null;
            return CreatedTypes.TryGetValue(typeName, out type);
        }

        /// <summary>
        /// Define a public property in the given <paramref name="typeBuilder"/>.
        /// </summary>
        /// <param name="typeBuilder">The type builder in which to define new property.</param>
        /// <param name="propertyType">The type of the new property.</param>
        /// <param name="propertyName">The name of the new property.</param>
        /// <remarks>
        /// For some reason it is impossible to create a public property with
        /// a default get and set.
        /// So in order to create public property, a get and set methods must also be defined
        /// along with a backfield.
        /// </remarks>
        private static void DefinePublicProperty(TypeBuilder typeBuilder, Type propertyType, string propertyName)
        {
            var fieldBuilder = typeBuilder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

            // Define the getter method.
            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(
                $"get_{propertyName}",            // Method name
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,              // Return type
                Type.EmptyTypes);            // Parameter types

            // Generate IL code for the getter.
            ILGenerator getIL = getMethodBuilder.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);                 // Load 'this'
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);     // Load the private field
            getIL.Emit(OpCodes.Ret);                     // Return the value

            // Define the setter method.
            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(
                $"set_{propertyName}",            // Method name
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,                        // Return type (void)
                new Type[] { propertyType });  // Parameter type

            // Generate IL code for the setter.
            ILGenerator setIL = setMethodBuilder.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);                 // Load 'this'
            setIL.Emit(OpCodes.Ldarg_1);                 // Load the value to set
            setIL.Emit(OpCodes.Stfld, fieldBuilder);     // Store the value in the private field
            setIL.Emit(OpCodes.Ret);                     // Return

            // Associate the property with getter and setter methods.
            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }

        /// <summary>
        /// Create a new multi dimensional array type.
        /// </summary>
        /// <param name="multiDimensionalArrayType">The type of the multidimensional array.</param>
        /// <param name="newTypeNamingFunction">A function for choosing names to all the needed newly created types.</param>
        /// <param name="nameSpace">
        /// The name space of the new type. It is recommended to give the name space
        /// of the type that caused this creation to prevent recursive references.
        /// </param>
        /// <returns>
        /// A new type representing a multi dimensions array suitable for proto of the given
        /// <paramref name="multiDimensionalArrayType"/> in a one dimensional manner.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the given <paramref name="multiDimensionalArrayType"/> is not a multidimensional array.</exception>
        private static Type CreateMultiDimensionalArrayType(Type multiDimensionalArrayType, Func<Type, string> newTypeNamingFunction, string nameSpace = DEFAULT_NAMESPACE_NAME)
        {
            if (!multiDimensionalArrayType.IsMultiDimensionalArray())
                throw new ArgumentException($"The given {nameof(multiDimensionalArrayType)} is not a multidimensional array", nameof(multiDimensionalArrayType));

            var newTypeName = newTypeNamingFunction(multiDimensionalArrayType);
            if (CreatedTypes.TryGetValue(newTypeName, out Type value))
            {
                return value;
            }

            // Create a type builder.
            var typeBuilder = moduleBuilder.DefineType($"{nameSpace}.{newTypeName}", TypeAttributes.Public | TypeAttributes.Class);

            var elementType = multiDimensionalArrayType.GetElementType();
            if (elementType.IsArray)
                elementType = CreateProtoArrayType(elementType, newTypeNamingFunction, nameSpace);

            var arrayOfElementsType = elementType.MakeArrayType();
            DefinePublicProperty(typeBuilder, arrayOfElementsType, "Elements");
            DefinePublicProperty(typeBuilder, typeof(int[]), "Dimensions");

            var newType = typeBuilder.CreateTypeInfo();
            CreatedTypes[newTypeName] = newType;
            return newType;
        }

        /// <summary>
        /// Create a multi dimensional array type.
        /// </summary>
        /// <param name="jaggedArrayType">The type of the jagged array.</param>
        /// <param name="newTypeNamingFunction">A function for choosing names to all the needed newly created types.</param>
        /// <param name="nameSpace">
        /// The name space of the new type. It is recommended to give the name space
        /// of the type that caused this creation to prevent recursive references.
        /// </param>
        /// <returns>
        /// A new type representing a jagged array that is suitable for protos of the given
        /// <paramref name="jaggedArrayType"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the given <paramref name="jaggedArrayType"/> is not a jagged array.</exception>
        private static Type CreateJaggedArrayType(Type jaggedArrayType, Func<Type, string> newTypeNamingFunction, string nameSpace = DEFAULT_NAMESPACE_NAME)
        {
            if (!jaggedArrayType.IsJaggedArray())
                throw new ArgumentException($"The given {nameof(jaggedArrayType)} is not a jagged array", nameof(jaggedArrayType));

            var newTypeName = newTypeNamingFunction(jaggedArrayType);
            if (CreatedTypes.TryGetValue(newTypeName, out Type value))
            {
                return value;
            }

            var arrayElementType = jaggedArrayType.GetElementType();

            arrayElementType = CreateProtoArrayType(arrayElementType, newTypeNamingFunction, nameSpace);

            var arrayOfArrayElementType = arrayElementType.MakeArrayType();

            // Create a type builder.
            var typeBuilder = moduleBuilder.DefineType($"{nameSpace}.{newTypeName}", TypeAttributes.Public | TypeAttributes.Class);

            DefinePublicProperty(typeBuilder, arrayOfArrayElementType, "arrays");

            var newType = typeBuilder.CreateTypeInfo();
            CreatedTypes[newTypeName] = newType;
            return newType;
        }
    }
}
