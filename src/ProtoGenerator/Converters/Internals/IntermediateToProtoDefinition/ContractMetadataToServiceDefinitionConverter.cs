using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Strategies.Internals.PatameterListNamingStrategies;
using ProtoGenerator.Utilities.CollectionUtilities;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ProtoGenerator.Converters.Internals.IntermediateToProtoDefinition.IntermediateToProtoDefinitionUtils;

namespace ProtoGenerator.Converters.Internals.IntermediateToProtoDefinition
{
    /// <summary>
    /// A converter between contract intermediate representation to its proto representation.
    /// </summary>
    public class ContractMetadataToServiceDefinitionConverter : IIntermediateToProtoDefinitionConverter<IContractTypeMetadata, IServiceDefinition>
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// Create new instance of the <see cref="ContractMetadataToServiceDefinitionConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider"/>.</param>
        public ContractMetadataToServiceDefinitionConverter(IProvider componentsProvider)
        {
            this.componentsProvider = componentsProvider;
        }

        /// <inheritdoc/>
        /// <inheritdoc cref="CreateRpcFromMethodMetadata(IMethodMetadata, string, IConversionOptions, IReadOnlyDictionary{Type, IProtoTypeMetadata}, out ISet{string})" path="/exception"/>
        public IServiceDefinition ConvertIntermediateRepresentationToProtoDefinition(IContractTypeMetadata intermediateType,
                                                                                     IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                                                     IConversionOptions conversionOptions)
        {
            var typeProtoMetadata = protoTypesMetadatas[intermediateType.Type];
            var imports = new HashSet<string>();
            var rpcMethods = new List<IRpcDefinition>();
            foreach(var methodMetadata in intermediateType.Methods)
            {
                rpcMethods.Add(CreateRpcFromMethodMetadata(methodMetadata, typeProtoMetadata.Package!, conversionOptions, protoTypesMetadatas, out var rpcNeededImports));
                imports.AddRange(rpcNeededImports);
            }

            return new ServiceDefinition(rpcMethods, typeProtoMetadata.Name!, typeProtoMetadata.Package!, imports);
        }

        /// <summary>
        /// Convert the given <paramref name="methodMetadata"/> to a <see cref="IRpcDefinition"/>.
        /// </summary>
        /// <param name="methodMetadata">The method meta data to convert.</param>
        /// <param name="filePackage">The package of the file in which the rpc is defined.</param>
        /// <param name="conversionOptions">The generation options.</param>
        /// <param name="protoTypesMetadatas">Mapping between type to its proto type metadata.</param>
        /// <param name="neededImports">The imports needed in the file for the rpc.</param>
        /// <returns>An <see cref="IRpcDefinition"/> that represents the given <paramref name="methodMetadata"/>.</returns>
        /// <exception cref="Exception">
        /// Thrown if the method have more than one parameter and there is no
        /// new type that represents the method parameter.
        /// </exception>
        private IRpcDefinition CreateRpcFromMethodMetadata(IMethodMetadata methodMetadata,
                                                           string filePackage,
                                                           IConversionOptions conversionOptions,
                                                           IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                           out ISet<string> neededImports)
        {
            neededImports = new HashSet<string>();

            var numOfParameters = methodMetadata.Parameters.Count();
            Type requestType;
            if(numOfParameters == 0)
            {
                requestType = typeof(void);
            }
            else if(numOfParameters == 1)
            {
                requestType = methodMetadata.Parameters.Single().Type;
            }
            else
            {
                var parameterListNamingStrategy = componentsProvider.GetParameterListNamingStrategy(conversionOptions.NewTypeNamingStrategiesOptions.ParameterListNamingStrategy);
                var typeName = parameterListNamingStrategy.GetNewParametersListTypeName(methodMetadata.MethodInfo);
                if (!TypeCreator.TryGetCreatedType(typeName, out requestType))
                    throw new Exception($"The service method: {methodMetadata.MethodInfo} " +
                        $"contains more than one parameter and yet no new type was created " +
                        $"for the parameter list.");
            }

            var packageComponentsSeparator = componentsProvider.GetPackageStylingStrategy(conversionOptions.ProtoStylingConventionsStrategiesOptions.PackageStylingStrategy).PackageComponentsSeparator;

            neededImports.Add(protoTypesMetadatas[requestType].FilePath!);
            var requestTypeName = GetTypeShortName(protoTypesMetadatas[requestType].FullName, filePackage, packageComponentsSeparator);

            neededImports.Add(protoTypesMetadatas[methodMetadata.ReturnType].FilePath!);
            var responseTypeName = GetTypeShortName(protoTypesMetadatas[methodMetadata.ReturnType].FullName, filePackage, packageComponentsSeparator);

            var rpcAttributeType = conversionOptions.AnalysisOptions.ProtoRpcAttribute;
            var attribute = methodMetadata.MethodInfo.GetCustomAttribute<ProtoRpcAttribute>(rpcAttributeType.IsAttributeInherited());

            var rpcStylingStrategy = componentsProvider.GetProtoStylingStrategy(conversionOptions.ProtoStylingConventionsStrategiesOptions.RpcStylingStrategy);
            var rpcName = rpcStylingStrategy.ToProtoStyle(methodMetadata.MethodInfo.Name);

            return new RpcDefinition(rpcName, responseTypeName, requestTypeName, attribute.RpcType);
        }
    }
}
