using ProtoGenerationLib.CommonUtilities;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Types extractor for contract types.
    /// </summary>
    internal class ContractTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// A provider of new type naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="ContractTypesExtractor"/> class.
        /// </summary>
        /// <param name="newTypeNamingStrategiesProvider"><inheritdoc cref="newTypeNamingStrategiesProvider" path="/node()"/></param>
        public ContractTypesExtractor(INewTypeNamingStrategiesProvider newTypeNamingStrategiesProvider)
        {
            this.newTypeNamingStrategiesProvider = newTypeNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
        {
            return type.IsProtoService(generationOptions.AnalysisOptions);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            var parameterListNamingStrategy = newTypeNamingStrategiesProvider.GetParameterListNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.ParameterListNamingStrategy);
            var types = new HashSet<Type>();
            var methods = type.ExtractRpcMethods(generationOptions.AnalysisOptions);

            foreach (var method in methods)
            {
                var returnType = WrapIfNonePrimitiveMessageType(method.ReturnType, generationOptions.NewTypeNamingStrategiesOptions);

                types.Add(returnType);
                var methodParameters = method.GetParameters();

                // Need to create a special type for the parameters list.
                if (methodParameters.Length > 1)
                {
                    var parameters = methodParameters.Select(parameterInfo => (parameterInfo.ParameterType, parameterInfo.Name));
                    var newTypeName = parameterListNamingStrategy.GetNewParametersListTypeName(method);
                    var newType = TypeCreator.CreateDataType(newTypeName, parameters, nameSpace: type.Namespace);
                    types.Add(newType);
                }
                else if (methodParameters.Length == 1)
                {
                    var parameterType = WrapIfNonePrimitiveMessageType(methodParameters[0].ParameterType, generationOptions.NewTypeNamingStrategiesOptions);

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

            var newTypeNamingStrategy = newTypeNamingStrategiesProvider.GetNewTypeNamingStrategy(newTypeNamingStrategiesOptions.NewTypeNamingStrategy);
            var enumWrapperName = newTypeNamingStrategy.GetNewTypeName(type);
            var props = new List<(Type, string)> { (type, "Value") };
            var enumWrapper = TypeCreator.CreateDataType(enumWrapperName, props, type.Namespace);
            return enumWrapper;
        }
    }
}
