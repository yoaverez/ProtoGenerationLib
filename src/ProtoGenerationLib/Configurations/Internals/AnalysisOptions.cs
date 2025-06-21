using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Configurations.Delegates;
using System;

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
        public Type ProtoRpcAttribute { get; set; }

        /// <inheritdoc/>
        public Type OptionalFieldAttribute { get; set; }

        /// <inheritdoc/>
        public IsProtoService IsProtoServiceDelegate { get; set; }

        /// <inheritdoc/>
        public TryGetRpcType TryGetRpcTypeDelegate { get; set; }

        /// <summary>
        /// Create new instance of the <see cref="AnalysisOptions"/> class.
        /// </summary>
        public AnalysisOptions()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="AnalysisOptions"/> class.
        /// </summary>
        /// <param name="includeFields"><inheritdoc cref="IncludeFields" path="/node()"/></param>
        /// <param name="includePrivates"><inheritdoc cref="IncludePrivates" path="/node()"/></param>
        /// <param name="includeStatics"><inheritdoc cref="IncludeStatics" path="/node()"/></param>
        /// <param name="removeEmptyMembers"><inheritdoc cref="RemoveEmptyMembers" path="/node()"/></param>
        /// <param name="fieldsAndPropertiesExtractionStrategy"><inheritdoc cref="FieldsAndPropertiesExtractionStrategy" path="/node()"/></param>
        /// <param name="ignoreFieldOrPropertyAttribute"><inheritdoc cref="IgnoreFieldOrPropertyAttribute" path="/node()"/></param>
        /// <param name="dataTypeConstructorAttribute"><inheritdoc cref="DataTypeConstructorAttribute" path="/node()"/></param>
        /// <param name="protoServiceAttribute"><inheritdoc cref="ProtoServiceAttribute" path="/node()"/></param>
        /// <param name="protoRpcAttribute"><inheritdoc cref="ProtoRpcAttribute" path="/node()"/></param>
        /// <param name="optionalFieldAttribute"><inheritdoc cref="OptionalFieldAttribute" path="/node()"/></param>
        /// <param name="isProtoServiceDelegate"><inheritdoc cref="IsProtoServiceDelegate" path="/node()"/></param>
        /// <param name="tryGetRpcTypeDelegate"><inheritdoc cref="TryGetRpcTypeDelegate" path="/node()"/></param>
        public AnalysisOptions(bool includeFields,
                               bool includePrivates,
                               bool includeStatics,
                               bool removeEmptyMembers,
                               string fieldsAndPropertiesExtractionStrategy,
                               Type ignoreFieldOrPropertyAttribute,
                               Type dataTypeConstructorAttribute,
                               Type protoServiceAttribute,
                               Type protoRpcAttribute,
                               Type optionalFieldAttribute,
                               IsProtoService isProtoServiceDelegate,
                               TryGetRpcType tryGetRpcTypeDelegate)
        {
            IncludeFields = includeFields;
            IncludePrivates = includePrivates;
            IncludeStatics = includeStatics;
            RemoveEmptyMembers = removeEmptyMembers;
            FieldsAndPropertiesExtractionStrategy = fieldsAndPropertiesExtractionStrategy;
            IgnoreFieldOrPropertyAttribute = ignoreFieldOrPropertyAttribute;
            DataTypeConstructorAttribute = dataTypeConstructorAttribute;
            ProtoServiceAttribute = protoServiceAttribute;
            ProtoRpcAttribute = protoRpcAttribute;
            OptionalFieldAttribute = optionalFieldAttribute;
            IsProtoServiceDelegate = isProtoServiceDelegate;
            TryGetRpcTypeDelegate = tryGetRpcTypeDelegate;
        }
    }
}
