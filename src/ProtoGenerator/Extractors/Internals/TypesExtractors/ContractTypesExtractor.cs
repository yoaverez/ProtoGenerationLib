using ProtoGenerator.Attributes;
using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors
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
        public override bool CanHandle(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var protoServiceAttribute = typeExtractionOptions.AnalysisOptions.ProtoServiceAttribute;
            return type.IsDefined(protoServiceAttribute, protoServiceAttribute.IsAttributeInherited());
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var parameterListNamingStrategy = parameterListNamingStrategiesProvider.GetParameterListNamingStrategy(typeExtractionOptions.NewTypeNamingStrategiesOptions.ParameterListNamingStrategy);
            var types = new HashSet<Type>();
            var methods = type.ExtractMethods(typeExtractionOptions.AnalysisOptions.ProtoRpcAttribute);

            foreach (var method in methods)
            {
                types.Add(method.ReturnType);
                var methodParameters = method.GetParameters();

                // Need to create a special type for the parameters list.
                if (methodParameters.Length > 1)
                {
                    var parameters = methodParameters.Select(parameterInfo => (parameterInfo.ParameterType, parameterInfo.Name ?? parameterInfo.ParameterType.Name));
                    var newTypeName = parameterListNamingStrategy.GetNewParametersListTypeName(method);
                    var newType = TypeCreator.CreateDataType(newTypeName, parameters);
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
