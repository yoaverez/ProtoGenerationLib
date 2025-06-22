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
        /// Create a multi dimensional array type.
        /// </summary>
        /// <param name="elementType">The element type of the array.</param>
        /// <param name="newTypeName">The name of the resulted type.</param>
        /// <param name="nameSpace">
        /// The name space of the new type. It is recommended to give the name space
        /// of the type that caused this creation to prevent recursive references.
        /// </param>
        /// <returns>
        /// A new type representing a multi dimensions array of the given
        /// <paramref name="elementType"/> in a one dimensional manner.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the given <paramref name="elementType"/> is an array.</exception>
        public static Type CreateArrayType(Type elementType, string newTypeName, string nameSpace = DEFAULT_NAMESPACE_NAME)
        {
            if (elementType.IsArray)
                throw new ArgumentException($"The given {nameof(elementType)} is an array", nameof(elementType));

            if (CreatedTypes.TryGetValue(newTypeName, out Type value))
            {
                return value;
            }

            // Create a type builder.
            var typeBuilder = moduleBuilder.DefineType($"{nameSpace}.{newTypeName}", TypeAttributes.Public | TypeAttributes.Class);

            var arrayOfElementsType = elementType.MakeArrayType();

            DefinePublicProperty(typeBuilder, arrayOfElementsType, "Elements");
            DefinePublicProperty(typeBuilder, typeof(int[]), "Dimensions");

            var newType = typeBuilder.CreateTypeInfo();
            CreatedTypes[newTypeName] = newType;
            return newType;
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
    }
}
