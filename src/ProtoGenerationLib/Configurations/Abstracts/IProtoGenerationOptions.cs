using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using System.Collections.Generic;

namespace ProtoGenerationLib.Configurations.Abstracts
{
    /// <summary>
    /// The proto generator configurations.
    /// </summary>
    public interface IProtoGenerationOptions : IFieldSuffixProvider
    {
        /// <inheritdoc cref="IAnalysisOptions"/>
        IAnalysisOptions AnalysisOptions { get; }

        /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
        INewTypeNamingStrategiesOptions NewTypeNamingStrategiesOptions { get; }

        /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
        IProtoStylingConventionsStrategiesOptions ProtoStylingConventionsStrategiesOptions { get; }

        /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
        IProtoNamingStrategiesOptions ProtoNamingStrategiesOptions { get; }

        /// <inheritdoc cref="INumberingStrategiesOptions"/>
        INumberingStrategiesOptions NumberingStrategiesOptions { get; }

        /// <summary>
        /// The proto file syntax (The line in the head of a proto file).
        /// Should be either "proto2" or "proto3".
        /// </summary>
        string ProtoFileSyntax { get; }

        /// <summary>
        /// Get all the user defined types extractors.
        /// </summary>
        /// <returns>All the user defined types extractors.</returns>
        IEnumerable<ICustomTypesExtractor> GetCustomTypesExtractors();

        /// <summary>
        /// Get all the user defined contract types converters.
        /// </summary>
        /// <returns>All the user defined contract types converters.</returns>
        IEnumerable<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> GetContractTypeCustomConverters();

        /// <summary>
        /// Get all the user defined data types converters.
        /// </summary>
        /// <returns>All the user defined data types converters.</returns>
        IEnumerable<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> GetDataTypeCustomConverters();

        /// <summary>
        /// Get all the user defined enum types converters.
        /// </summary>
        /// <returns>All the user defined enum types converters.</returns>
        IEnumerable<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> GetEnumTypeCustomConverters();

        /// <summary>
        /// Get all the user defined type mappers.
        /// </summary>
        /// <returns>All the user defined type mappers.</returns>
        IEnumerable<ICustomTypeMapper> GetCustomTypeMappers();
    }
}
