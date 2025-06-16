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
        /// The name of the strategy for extracting fields from csharp types.
        /// </summary>
        string FieldsAndPropertiesExtractionStrategy { get; }

        /// <summary>
        /// The type of the attribute that ask to ignore some
        /// fields or properties.
        /// </summary>
        Type IgnoreFieldOrPropertyAttribute { get; }

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
    }
}
