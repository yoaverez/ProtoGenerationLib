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
    public class ContractTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// A provider of parameter list naming strategies.
        /// </summary>
        private INewTypeNamingStrategiesProvider parameterListNamingStrategiesProvider;

        /// <summary>
        /// Create new instance of the <see cref="ContractTypesExtractor"/> class.
        /// </summary>
        /// <param name="parameterListNamingStrategiesProvider"><inheritdoc cref="parameterListNamingStrategiesProvider" path="/node()"/></param>
        public ContractTypesExtractor(INewTypeNamingStrategiesProvider parameterListNamingStrategiesProvider)
        {
            this.parameterListNamingStrategiesProvider = parameterListNamingStrategiesProvider;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
        {
            return type.IsProtoService(generationOptions.AnalysisOptions);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            var parameterListNamingStrategy = parameterListNamingStrategiesProvider.GetParameterListNamingStrategy(generationOptions.NewTypeNamingStrategiesOptions.ParameterListNamingStrategy);
            var types = new HashSet<Type>();
            var methods = type.ExtractRpcMethods(generationOptions.AnalysisOptions);

            foreach (var method in methods)
            {
                types.Add(method.ReturnType);
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
                    types.Add(methodParameters[0].ParameterType);
                }
                // Method has no parameters.
                else
                {
                    types.Add(typeof(void));
                }
            }

            return types;
        }
    }
}
