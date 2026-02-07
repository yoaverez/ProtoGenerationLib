using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Delegates;
using ProtoGenerationLib.Customizations.Abstracts;
using System;

namespace ProtoGenerationLib.Configurations.Abstracts
{
    /// <summary>
    /// Options for the csharp types analysis.
    /// </summary>
    public interface IAnalysisOptions
    {
        /// <summary>
        /// Whether or not to include csharp fields in the proto message fields.
        /// </summary>
        /// <remarks>Properties will always be included in the proto message fields.</remarks>
        bool IncludeFields { get; }

        /// <summary>
        /// Whether or not to include private properties (and private fields
        /// if <see cref="IncludeFields"/> is <see langword="true"/>).
        /// </summary>
        bool IncludePrivates { get; }

        /// <summary>
        /// Whether or not to include static properties (and static fields
        /// if <see cref="IncludeFields"/> is <see langword="true"/>).
        /// </summary>
        bool IncludeStatics { get; }

        /// <summary>
        /// Whether or not to remove properties or fields with types
        /// that are considered empty.
        /// e.g. If Type A contains Property with type B and type B contains no properties,
        /// then Type B is considered empty and will be removes from Type A Members list.
        /// </summary>
        bool RemoveEmptyMembers { get; }

        /// <summary>
        /// The name of the strategy for extracting fields from csharp types.
        /// </summary>
        string FieldsAndPropertiesExtractionStrategy { get; }

        /// <summary>
        /// The name of the strategy for extracting documentation from
        /// csharp entities.
        /// </summary>
        string DocumentationExtractionStrategy { get; }

        /// <summary>
        /// The name of the strategy for extracting method signature from
        /// csharp entities.
        /// </summary>
        string MethodSignatureExtractionStrategy { get; }

        /// <summary>
        /// The type of the attribute that ask to ignore some
        /// fields or properties.
        /// </summary>
        Type IgnoreFieldOrPropertyAttribute { get; }

        /// <summary>
        /// The type of the attribute that ask to ignore some
        /// method parameters.
        /// </summary>
        Type IgnoreMethodParametersAttribute { get; }

        /// <summary>
        /// The type of the constructor attribute that tells that
        /// the constructor contains all the wanted fields
        /// and properties of the type.
        /// </summary>
        Type DataTypeConstructorAttribute { get; }

        /// <summary>
        /// The type of the attribute that tells that the class/interface/struct
        /// represents a proto service.
        /// </summary>
        Type ProtoServiceAttribute { get; }

        /// <summary>
        /// The type of the attribute that tells that the method
        /// represents a proto rpc method.
        /// </summary>
        Type ProtoRpcAttribute { get; }

        /// <summary>
        /// The type of the attribute that tells that the field/property
        /// represents an optional proto field.
        /// </summary>
        Type OptionalFieldAttribute { get; }

        /// <summary>
        /// A delegate for checking if a type is a proto service.
        /// </summary>
        /// <remarks>
        /// This delegate comes in addition to the <see cref="ProtoServiceAttribute"/>.
        /// So in order for a type to be considered a proto service it can either have the
        /// <see cref="ProtoServiceAttribute"/> or this delegate returns <see langword="true"/>.
        /// </remarks>
        IsProtoService IsProtoServiceDelegate { get; }

        /// <summary>
        /// A delegate for trying to get the <see cref="ProtoRpcType"/> from a service method.
        /// </summary>
        /// <remarks>
        /// This delegate comes in addition to the <see cref="ProtoRpcAttribute"/>.
        /// So in order for a method to be considered rpc it can either have the
        /// <see cref="ProtoRpcAttribute"/> or this delegate returns <see langword="true"/>.
        /// </remarks>
        TryGetRpcType TryGetRpcTypeDelegate { get; }

        /// <summary>
        /// A provider for user documentation customization.
        /// </summary>
        IDocumentationProvider DocumentationProvider { get; }
    }
}
