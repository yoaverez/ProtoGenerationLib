using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProtoGenerationLib.CommonUtilities
{
    /// <summary>
    /// Common <see cref="Type"/> extensions methods for the proto generator components
    /// to use.
    /// </summary>
    /// <remarks>
    /// This is separated from the utilities since these methods used the project types
    /// and therefor are decoupled to this project while the utilities are standalones.
    /// </remarks>
    public static class CommonTypeExtensions
    {
        /// <summary>
        /// Checks whether or not the given <paramref name="type"/> is
        /// a proto service.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> is a proto service
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool IsProtoService(this Type type, IAnalysisOptions analysisOptions)
        {
            var serviceAttribute = analysisOptions.ProtoServiceAttribute;
            var doesHaveServiceAttribute = () => type.IsDefined(serviceAttribute, serviceAttribute.IsAttributeInherited());
            var isTypeService = () => analysisOptions.IsProtoServiceDelegate(type);

            return doesHaveServiceAttribute() || isTypeService();
        }

        /// <summary>
        /// Get the given <paramref name="method"/>'s <see cref="ProtoRpcType"/>.
        /// </summary>
        /// <param name="method">The method whose <see cref="ProtoRpcType"/> is requested.</param>
        /// <param name="methodDeclaringType">The type that declare the given <paramref name="method"/>.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <returns>
        /// The given <paramref name="method"/>'s <see cref="ProtoRpcType"/> if the
        /// <paramref name="method"/> is an rpc method otherwise throws exception.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="method"/> is not rpc method.
        /// </exception>
        public static ProtoRpcType GetMethodRpcType(this MethodInfo method, Type methodDeclaringType, IAnalysisOptions analysisOptions)
        {
            var rpcAttribute = analysisOptions.ProtoRpcAttribute;

            if (analysisOptions.TryGetRpcTypeDelegate(methodDeclaringType, method, out var rpcType))
                return rpcType;

            var attribute = method.GetCustomAttribute<ProtoRpcAttribute>(rpcAttribute.IsAttributeInherited());
            if (attribute != null)
                return attribute.RpcType;

            // If we got here it means that the method can be rpc if and only if
            // an there is implemented interface of the type in which declared the same
            // method and the method is rpc.
            // Since interface attributes is not inherit we need to take all the interfaces methods.
            if (!methodDeclaringType.IsInterface)
            {
                var implementedInterfaces = methodDeclaringType.GetAllImplementedInterfaces();
                var interfacesMethods = implementedInterfaces.SelectMany(interfaceType => interfaceType.GetMethods());

                var interfaceMethodsDictionary = GetImplementedInterfacesMethodsSearchDictonary(methodDeclaringType, interfacesMethods);

                if (TryGetSameImplementedInterfaceRpcMethod(method, interfaceMethodsDictionary, analysisOptions, out var sameMethodInInterface))
                {
                    return sameMethodInInterface.GetMethodRpcType(sameMethodInInterface.ReflectedType, analysisOptions);
                }
            }

            throw new ArgumentException($"The given {method}: {method.Name} is not an rpc method.", nameof(method));
        }

        /// <summary>
        /// Extract all the public rpc methods of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose methods to extract.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <returns>
        /// All the public methods of the given <paramref name="type"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="analysisOptions"/> does not contain
        /// an <see cref="Attribute"/> type in the <see cref="IAnalysisOptions.ProtoRpcAttribute"/>
        /// property.
        /// </exception>
        public static IEnumerable<MethodInfo> ExtractRpcMethods(this Type type, IAnalysisOptions analysisOptions)
        {
            var rpcAttribute = analysisOptions.ProtoRpcAttribute;
            if (!typeof(Attribute).IsAssignableFrom(rpcAttribute))
                throw new ArgumentException("Type must be an Attribute", nameof(IAnalysisOptions.ProtoRpcAttribute));

            var typeMethods = new HashSet<MethodInfo>(type.GetMethods());

            // Since interface attributes is not inherit we need to take all the interfaces methods.
            var implementedInterfaces = type.GetAllImplementedInterfaces();
            var interfacesMethods = implementedInterfaces.SelectMany(interfaceType => interfaceType.GetMethods());

            // An interface only contains its own declared types
            // so add all the implemented interfaces methods.
            if (type.IsInterface)
                typeMethods.AddRange(interfacesMethods);

            var interfaceMethodsDictionary = new Dictionary<string, Dictionary<int, List<(Type[] ParameterTypes, MethodInfo Method)>>>();

            // If the type is an interface, we don't need the interface methods dictionary.
            // The dictionary is for fast searching of a specific method.
            if (!type.IsInterface)
            {
                interfaceMethodsDictionary = GetImplementedInterfacesMethodsSearchDictonary(type, interfacesMethods);
            }

            var filteredMethods = new List<MethodInfo>();

            Func<MethodInfo, bool> doesMethodHaveRpcAttribute = (method) => method.IsDefined(rpcAttribute, rpcAttribute.IsAttributeInherited());
            Func<MethodInfo, Type, bool> isRpcByDelegate = (method, declaringType) => analysisOptions.TryGetRpcTypeDelegate(declaringType, method, out _);

            foreach (var method in typeMethods)
            {
                if (doesMethodHaveRpcAttribute(method) || isRpcByDelegate(method, type))
                    filteredMethods.Add(method);

                // If the type is not an interface then check if there exists a method
                // in any of the implemented interfaces that represent the current method
                // and is rpc method.
                else if (!type.IsInterface)
                {
                    if (TryGetSameImplementedInterfaceRpcMethod(method, interfaceMethodsDictionary, analysisOptions, out _))
                        filteredMethods.Add(method);
                }
            }

            return filteredMethods;
        }

        /// <summary>
        /// Try getting an implemented interface method that is the same as the given <paramref name="method"/>
        /// and is rpc method.
        /// </summary>
        /// <param name="method">The original non rpc method.</param>
        /// <param name="implementedInterfacesMethodsDictionary">A search dictionary for implemented interfaces methods.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <param name="sameMethodInInterface">
        /// The same method of an implemented interface if found otherwise
        /// the <see langword="default"/> of <see cref="MethodInfo"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the there is a method in the <paramref name="implementedInterfacesMethodsDictionary"/>
        /// that is rpc method and it has the same signature as the given <paramref name="method"/>
        /// otherwise <see langword="false"/>.
        /// </returns>
        private static bool TryGetSameImplementedInterfaceRpcMethod(MethodInfo method,
                                                                    Dictionary<string, Dictionary<int, List<(Type[] ParameterTypes, MethodInfo Method)>>> implementedInterfacesMethodsDictionary,
                                                                    IAnalysisOptions analysisOptions,
                                                                    out MethodInfo sameMethodInInterface)
        {
            sameMethodInInterface = default;

            var rpcAttribute = analysisOptions.ProtoRpcAttribute;
            Func<MethodInfo, bool> doesMethodHaveRpcAttribute = (method) => method.IsDefined(rpcAttribute, rpcAttribute.IsAttributeInherited());
            Func<MethodInfo, Type, bool> isRpcByDelegate = (method, declaringType) => analysisOptions.TryGetRpcTypeDelegate(declaringType, method, out _);

            var name = method.Name;
            var paramtersTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
            var numOfParameters = paramtersTypes.Length;

            // Check if there is interface method that represents
            // the current iteration method and it is an rpc method.
            if (implementedInterfacesMethodsDictionary.ContainsKey(name))
            {
                if (implementedInterfacesMethodsDictionary[name].ContainsKey(numOfParameters))
                {
                    var possibleInterfaceMethods = implementedInterfacesMethodsDictionary[name][numOfParameters];
                    var matchInterfaceMethod = possibleInterfaceMethods.FirstOrDefault(paramtersAndInterfaceMethod =>
                    {
                        return paramtersTypes.SequenceEqual(paramtersAndInterfaceMethod.ParameterTypes);
                    });

                    if (matchInterfaceMethod != default((Type[], MethodInfo)))
                    {
                        var interfaceMethod = matchInterfaceMethod.Method;
                        var interfaceType = interfaceMethod.ReflectedType;
                        if (doesMethodHaveRpcAttribute(interfaceMethod) || isRpcByDelegate(interfaceMethod, interfaceType))
                        {
                            sameMethodInInterface = interfaceMethod;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get a search dictionary containing all the given <paramref name="implementedInterfacesMethods"/>
        /// in a manner that makes it easy to search a specific method by its name and parameters types.
        /// </summary>
        /// <param name="type">The types whose implemented interfaces methods you want in a search dictionary.</param>
        /// <param name="implementedInterfacesMethods">The implemented interfaces methods.</param>
        /// <returns>
        /// A search dictionary containing all the given <paramref name="implementedInterfacesMethods"/>
        /// in a manner that makes it easy to search a specific method by its name and parameters types.
        /// </returns>
        private static Dictionary<string, Dictionary<int, List<(Type[] ParameterTypes, MethodInfo Method)>>> GetImplementedInterfacesMethodsSearchDictonary(Type type, IEnumerable<MethodInfo> implementedInterfacesMethods)
        {
            var interfaceMethodsDictionary = new Dictionary<string, Dictionary<int, List<(Type[] ParameterTypes, MethodInfo Method)>>>();

            foreach (var interfaceMethod in implementedInterfacesMethods)
            {
                var name = interfaceMethod.Name;
                var paramtersTypes = interfaceMethod.GetParameters().Select(p => p.ParameterType).ToArray();
                var numOfParameters = paramtersTypes.Length;

                if (!interfaceMethodsDictionary.ContainsKey(name))
                    interfaceMethodsDictionary.Add(name, new Dictionary<int, List<(Type[], MethodInfo)>>());

                if (!interfaceMethodsDictionary[name].ContainsKey(numOfParameters))
                    interfaceMethodsDictionary[name].Add(numOfParameters, new List<(Type[], MethodInfo)>());

                interfaceMethodsDictionary[name][numOfParameters].Add((paramtersTypes, interfaceMethod));
            }

            return interfaceMethodsDictionary;
        }
    }
}
