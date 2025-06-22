using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Configurations.Delegates;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using System;
using ProtoGenerationLib.Attributes;
using System.Reflection;

namespace ProtoGenerationLib.Configurations.Internals
{
    /// <inheritdoc cref="IAnalysisOptions"/>
    public class AnalysisOptions : IAnalysisOptions
    {
        /// <inheritdoc/>
        public bool IncludeFields { get; set; }

        /// <inheritdoc/>
        public bool IncludePrivates { get; set; }

        /// <inheritdoc/>
        public bool IncludeStatics { get; set; }

        /// <inheritdoc/>
        public bool RemoveEmptyMembers { get; set; }

        /// <inheritdoc/>
        public string FieldsAndPropertiesExtractionStrategy { get; set; }

        /// <inheritdoc/>
        public Type IgnoreFieldOrPropertyAttribute { get; set; }

        /// <inheritdoc/>
        public Type DataTypeConstructorAttribute { get; set; }

        /// <inheritdoc/>
        public Type ProtoServiceAttribute { get; set; }

        /// <inheritdoc/>
        public Type ProtoRpcAttribute { get; internal set; }

        /// <inheritdoc/>
        public Type OptionalFieldAttribute { get; set; }

        /// <inheritdoc/>
        public IsProtoService IsProtoServiceDelegate { get; set; }

        /// <inheritdoc/>
        public TryGetRpcType TryGetRpcTypeDelegate { get; set; }

        /// <summary>
        /// Create new instance of the <see cref="AnalysisOptions"/> class.
        /// </summary>
        /// <param name="includeFields"><inheritdoc cref="IncludeFields" path="/node()"/><br/> Default to <see langword="false"/>.</param>
        /// <param name="includePrivates"><inheritdoc cref="IncludePrivates" path="/node()"/><br/> Default to <see langword="false"/>.</param>
        /// <param name="includeStatics"><inheritdoc cref="IncludeStatics" path="/node()"/><br/> Default to <see langword="false"/>.</param>
        /// <param name="removeEmptyMembers"><inheritdoc cref="RemoveEmptyMembers" path="/node()"/><br/> Default to <see langword="true"/>.</param>
        /// <param name="fieldsAndPropertiesExtractionStrategy"><inheritdoc cref="FieldsAndPropertiesExtractionStrategy" path="/node()"/><br/> Default to null converted to "Composite".</param>
        /// <param name="ignoreFieldOrPropertyAttribute"><inheritdoc cref="IgnoreFieldOrPropertyAttribute" path="/node()"/><br/> Default to null converted to the type of <see cref="ProtoIgnoreAttribute"/>.</param>
        /// <param name="dataTypeConstructorAttribute"><inheritdoc cref="DataTypeConstructorAttribute" path="/node()"/><br/> Default to null converted to the type of <see cref="ProtoMessageConstructorAttribute"/>.</param>
        /// <param name="protoServiceAttribute"><inheritdoc cref="ProtoServiceAttribute" path="/node()"/><br/> Default to null converted to the type of <see cref="Attributes.ProtoServiceAttribute"/>.</param>
        /// <param name="protoRpcAttribute"><inheritdoc cref="ProtoRpcAttribute" path="/node()"/><br/> Default to null converted to the type of <see cref="Attributes.ProtoRpcAttribute"/>.</param>
        /// <param name="optionalFieldAttribute"><inheritdoc cref="OptionalFieldAttribute" path="/node()"/><br/> Default to null converted to the type of <see cref="OptionalDataMemberAttribute"/>.</param>
        /// <param name="isProtoServiceDelegate"><inheritdoc cref="IsProtoServiceDelegate" path="/node()"/><br/> Default to null converted to a delegate that always return <see langword="false"/>.</param>
        /// <param name="tryGetRpcTypeDelegate"><inheritdoc cref="TryGetRpcTypeDelegate" path="/node()"/><br/> Default to null converted to a delegate that always return <see langword="false"/>.</param>
        public AnalysisOptions(bool includeFields = false,
                               bool includePrivates = false,
                               bool includeStatics = false,
                               bool removeEmptyMembers = true,
                               string? fieldsAndPropertiesExtractionStrategy = null,
                               Type? ignoreFieldOrPropertyAttribute = null,
                               Type? dataTypeConstructorAttribute = null,
                               Type? protoServiceAttribute = null,
                               Type? protoRpcAttribute = null,
                               Type? optionalFieldAttribute = null,
                               IsProtoService? isProtoServiceDelegate = null,
                               TryGetRpcType? tryGetRpcTypeDelegate = null)
        {
            IncludeFields = includeFields;
            IncludePrivates = includePrivates;
            IncludeStatics = includeStatics;
            RemoveEmptyMembers = removeEmptyMembers;
            FieldsAndPropertiesExtractionStrategy = fieldsAndPropertiesExtractionStrategy ??
                StrategyNamesLookup.FieldsAndPropertiesExtractionStrategiesLookup[FieldsAndPropertiesExtractionStrategyKind.Composite];
            IgnoreFieldOrPropertyAttribute = ignoreFieldOrPropertyAttribute ?? typeof(ProtoIgnoreAttribute);
            DataTypeConstructorAttribute = dataTypeConstructorAttribute ?? typeof(ProtoMessageConstructorAttribute);
            ProtoServiceAttribute = protoServiceAttribute ?? typeof(ProtoServiceAttribute);
            ProtoRpcAttribute = protoRpcAttribute ?? typeof(ProtoRpcAttribute);
            OptionalFieldAttribute = optionalFieldAttribute ?? typeof(OptionalDataMemberAttribute);
            IsProtoServiceDelegate = isProtoServiceDelegate ?? ((type) => false);
            TryGetRpcTypeDelegate = tryGetRpcTypeDelegate ?? ((Type serviceType, MethodInfo method, out ProtoRpcType rpcType) => { rpcType = ProtoRpcType.Unary; return false; });
        }
    }
}
