using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;
using ProtoGenerationLib.ProvidersAndRegistries.External;

namespace ProtoGenerationLib.Configurations.Internals
{
    /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
    public class NewTypeNamingStrategiesOptions : INewTypeNamingStrategiesOptions
    {
        /// <inheritdoc/>
        public string ParameterListNamingStrategy { get; set; }

        /// <inheritdoc/>
        public string NewTypeNamingStrategy { get; set; }

        /// <summary>
        /// Create new instance of the <see cref="NewTypeNamingStrategiesOptions"/> class.
        /// </summary>
        /// <param name="parameterListNamingStrategy"><inheritdoc cref="ParameterListNamingStrategy" path="/node()"/><br/> Default to null converted to "MethodNameAndParametersTypes".</param>
        /// <param name="newTypeNamingStrategy"><inheritdoc cref="NewTypeNamingStrategy" path="/node()"/><br/> Default to null converted to "NewTypeNamingAsAlphaNumericTypeName".</param>
        public NewTypeNamingStrategiesOptions (string? parameterListNamingStrategy = null,
                                              string? newTypeNamingStrategy = null)
        {
            ParameterListNamingStrategy = parameterListNamingStrategy ??
                StrategyNamesLookup.ParameterListNamingStrategiesLookup[ParameterListNamingStrategyKind.MethodNameAndParametersTypes];
            NewTypeNamingStrategy = newTypeNamingStrategy ??
                StrategyNamesLookup.NewTypeNamingStrategiesLookup[NewTypeNamingStrategyKind.NameAsAlphaNumericTypeName];
        }
    }
}
