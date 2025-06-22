using ProtoGenerationLib.CommonUtilities;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Constants;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using static ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition.IntermediateToProtoDefinitionUtils;

namespace ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition
{
    /// <summary>
    /// A converter between contract intermediate representation to its proto representation.
    /// </summary>
    internal class ContractMetadataToServiceDefinitionConverter : IIntermediateToProtoDefinitionConverter<IContractTypeMetadata, IServiceDefinition>
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// A mapping between a csharp primitive type to its
        /// protobuf wrapper message type metadata.
        /// </summary>
        private IReadOnlyDictionary<Type, IProtoTypeMetadata> primitiveTypesWrappers;

        /// <summary>
        /// Create new instance of the <see cref="ContractMetadataToServiceDefinitionConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider"/>.</param>
        /// <param name="primitiveTypesWrappers"><inheritdoc cref="primitiveTypesWrappers"/>.</param>
        public ContractMetadataToServiceDefinitionConverter(IProvider componentsProvider, IReadOnlyDictionary<Type, IProtoTypeMetadata>? primitiveTypesWrappers = null)
        {
            this.componentsProvider = componentsProvider;
            this.primitiveTypesWrappers = primitiveTypesWrappers ?? WellKnownTypesConstants.PrimitiveTypesWrappers;
        }

        /// <inheritdoc/>
        /// <inheritdoc cref="CreateRpcFromMethodMetadata(IMethodMetadata, string, IProtoGenerationOptions, IReadOnlyDictionary{Type, IProtoTypeMetadata}, out ISet{string})" path="/exception"/>
        public IServiceDefinition ConvertIntermediateRepresentationToProtoDefinition(IContractTypeMetadata intermediateType,
                                                                                     IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                                                     IProtoGenerationOptions generationOptions)
        {
            var typeProtoMetadata = protoTypesMetadatas[intermediateType.Type];
            var imports = new HashSet<string>();
            var rpcMethods = new List<IRpcDefinition>();
            foreach (var methodMetadata in intermediateType.Methods)
            {
                rpcMethods.Add(CreateRpcFromMethodMetadata(methodMetadata, intermediateType.Type, typeProtoMetadata.FullName!, generationOptions, protoTypesMetadatas, out var rpcNeededImports));
                imports.AddRange(rpcNeededImports);
            }

            return new ServiceDefinition(typeProtoMetadata.Name!, typeProtoMetadata.Package!, imports, rpcMethods);
        }

        /// <summary>
        /// Convert the given <paramref name="methodMetadata"/> to a <see cref="IRpcDefinition"/>.
        /// </summary>
        /// <param name="methodMetadata">The method meta data to convert.</param>
        /// <param name="declaringType">The type that declare the method.</param>
        /// <param name="serviceFullName">The full name of the service in which the rpc is defined.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <param name="protoTypesMetadatas">Mapping between type to its proto type metadata.</param>
        /// <param name="neededImports">The imports needed in the file for the rpc.</param>
        /// <returns>An <see cref="IRpcDefinition"/> that represents the given <paramref name="methodMetadata"/>.</returns>
        /// <exception cref="Exception">
        /// Thrown if the method have more than one parameter and there is no
        /// new type that represents the method parameter.
        /// </exception>
        private IRpcDefinition CreateRpcFromMethodMetadata(IMethodMetadata methodMetadata,
                                                           Type declaringType,
                                                           string serviceFullName,
                                                           IProtoGenerationOptions generationOptions,
                                                           IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                           out ISet<string> neededImports)
        {
            neededImports = new HashSet<string>();

            var numOfParameters = methodMetadata.Parameters.Count();
            Type requestType;
            if (numOfParameters == 0)
            {
                requestType = typeof(void);
            }
            else if (numOfParameters == 1)
            {
                requestType = methodMetadata.Parameters.Single().Type;
            }
            else
            {
                var parameterListNamingStrategy = componentsProvider.GetParameterListNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.ParameterListNamingStrategy);
                var typeName = parameterListNamingStrategy.GetNewParametersListTypeName(methodMetadata.MethodInfo);
                if (!TypeCreator.TryGetCreatedType(typeName, out requestType))
                    throw new Exception($"The service method: {methodMetadata.MethodInfo} " +
                        $"contains more than one parameter and yet no new type was created " +
                        $"for the parameter list.");
            }

            var packageComponentsSeparator = componentsProvider.GetPackageStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.PackageStylingStrategy).PackageComponentsSeparator;

            var requestTypeMetadata = GetTypeMetadata(requestType, protoTypesMetadatas);
            neededImports.Add(requestTypeMetadata.FilePath!);
            var requestTypeName = GetTypeShortName(requestTypeMetadata.FullName, serviceFullName, packageComponentsSeparator);

            var returnTypeMetadata = GetTypeMetadata(methodMetadata.ReturnType, protoTypesMetadatas);
            neededImports.Add(returnTypeMetadata.FilePath!);
            var responseTypeName = GetTypeShortName(returnTypeMetadata.FullName, serviceFullName, packageComponentsSeparator);

            var rpcStylingStrategy = componentsProvider.GetProtoStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.RpcStylingStrategy);
            var rpcName = rpcStylingStrategy.ToProtoStyle(methodMetadata.MethodInfo.Name);

            var rpcType = methodMetadata.MethodInfo.GetMethodRpcType(declaringType, generationOptions.AnalysisOptions);

            return new RpcDefinition(rpcName, responseTypeName, requestTypeName, rpcType);
        }

        /// <summary>
        /// Get the proto type metadata of rpc return or request type.
        /// </summary>
        /// <param name="type">The type that is used as the rpc return or request type.</param>
        /// <param name="protoTypesMetadatas">The mapping between types to their metadatas.</param>
        /// <returns>The proto type metadata of rpc return or request type.</returns>
        private IProtoTypeMetadata GetTypeMetadata(Type type, IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas)
        {
            if (type.DoesTypeHaveProtobufWrapperType())
            {
                return primitiveTypesWrappers[type];
            }

            return protoTypesMetadatas[type];
        }
    }
}
