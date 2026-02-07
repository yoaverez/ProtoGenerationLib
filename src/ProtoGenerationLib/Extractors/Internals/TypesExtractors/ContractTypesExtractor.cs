using ProtoGenerationLib.CommonUtilities;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Types extractor for contract types.
    /// </summary>
    internal class ContractTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// Create new instance of the <see cref="ContractTypesExtractor"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        public ContractTypesExtractor(IProvider componentsProvider)
        {
            this.componentsProvider = componentsProvider;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
        {
            return type.IsProtoService(generationOptions.AnalysisOptions);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            var ignoreAttribute = generationOptions.AnalysisOptions.IgnoreMethodParametersAttribute;
            var parameterListNamingStrategy = componentsProvider.GetParameterListNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.ParameterListNamingStrategy);
            var methodSignatureExtractionStrategy = componentsProvider.GetMethodSignatureExtractionStrategy(generationOptions.AnalysisOptions.MethodSignatureExtractionStrategy);
            var types = new HashSet<Type>();
            var methods = type.ExtractRpcMethods(generationOptions.AnalysisOptions);

            foreach (var method in methods)
            {
                var (returnType, methodParameters) = methodSignatureExtractionStrategy.ExtractMethodSignature(method, ignoreAttribute);

                returnType = WrapIfNonePrimitiveMessageType(returnType, generationOptions.NewTypeNamingStrategiesOptions);
                types.Add(returnType);

                // Need to create a special type for the parameters list.
                if (methodParameters.Count() > 1)
                {
                    var parameters = methodParameters.Select(parameterInfo => (parameterInfo.Type, parameterInfo.Name));
                    var newTypeName = parameterListNamingStrategy.GetNewParametersListTypeName(method);
                    var newType = TypeCreator.CreateDataType(newTypeName, parameters, nameSpace: type.Namespace);
                    types.Add(newType);
                }
                else if (methodParameters.Count() == 1)
                {
                    var parameterType = WrapIfNonePrimitiveMessageType(methodParameters.First().Type, generationOptions.NewTypeNamingStrategiesOptions);

                    types.Add(parameterType);
                }
                // Method has no parameters.
                else
                {
                    types.Add(typeof(void));
                }
            }

            return types;
        }

        /// <summary>
        /// Wrap a type that is not a message type nor primitive type
        /// and can not be a part of rpc declaration.
        /// </summary>
        /// <param name="type">The type that is a part of rpc declaration.</param>
        /// <param name="newTypeNamingStrategiesOptions">The new type naming strategies options.</param>
        /// <returns>
        /// The given <paramref name="type"/> if that type can be a part of rpc declaration
        /// otherwise a new data type that wraps the given <paramref name="type"/>.
        /// </returns>
        private Type WrapIfNonePrimitiveMessageType(Type type, INewTypeNamingStrategiesOptions newTypeNamingStrategiesOptions)
        {
            if (!type.IsEnum)
                return type;

            var newTypeNamingStrategy = componentsProvider.GetNewTypeNamingStrategy(newTypeNamingStrategiesOptions.NewTypeNamingStrategy);
            var enumWrapperName = newTypeNamingStrategy.GetNewTypeName(type);
            var props = new List<(Type, string)> { (type, "Value") };
            var enumWrapper = TypeCreator.CreateDataType(enumWrapperName, props, type.Namespace);
            return enumWrapper;
        }
    }
}
