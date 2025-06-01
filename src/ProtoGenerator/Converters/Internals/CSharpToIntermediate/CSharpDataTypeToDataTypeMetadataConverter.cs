using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using System;

namespace ProtoGenerator.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp data types to <see cref="IDataTypeMetadata"/>.
    /// </summary>
    public class CSharpDataTypeToDataTypeMetadataConverter : ICSharpToIntermediateConverter<IDataTypeMetadata>
    {
        /// <summary>
        /// Provider of extraction strategies.
        /// </summary>
        private IExtractionStrategiesProvider extractionStrategiesProvider;

        /// <summary>
        /// Converter from csharp enum type to intermediate enum metadata.
        /// </summary>
        private ICSharpToIntermediateConverter<IEnumTypeMetadata> csharpEnumTypeToEnumMetaDataConverter;

        /// <summary>
        /// Create new instance of the <see cref="CSharpDataTypeToDataTypeMetadataConverter"/> class.
        /// </summary>
        /// <param name="extractionStrategiesProvider"><inheritdoc cref="extractionStrategiesProvider" path="/node()"/></param>
        /// <param name="csharpEnumTypeToEnumMetaDataConverter"><inheritdoc cref="extractionStrategiesProvider" path="/node()"/></param>
        public CSharpDataTypeToDataTypeMetadataConverter(IExtractionStrategiesProvider extractionStrategiesProvider, ICSharpToIntermediateConverter<IEnumTypeMetadata>? csharpEnumTypeToEnumMetaDataConverter = null)
        {
            this.extractionStrategiesProvider = extractionStrategiesProvider;
            this.csharpEnumTypeToEnumMetaDataConverter = csharpEnumTypeToEnumMetaDataConverter ?? new CSharpEnumTypeToEnumTypeMetadataConverter();
        }

        /// <inheritdoc/>
        public IDataTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
        {
            if (type.IsEnum)
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not a data type.", nameof(type));

            var dataTypeMetadata = new DataTypeMetadata();
            dataTypeMetadata.Type = type;

            // Extract the fields.
            var fieldsAndPropertiesExtractor = extractionStrategiesProvider.GetFieldsAndPropertiesExtractionStrategy(generationOptions.AnalysisOptions.FieldsAndPropertiesExtractionStrategy);
            var fields = fieldsAndPropertiesExtractor.ExtractFieldsAndProperties(type, generationOptions.AnalysisOptions);
            dataTypeMetadata.Fields.AddRange(fields);

            // Extract the nested types.
            var nestedTypes = type.GetNestedTypes();
            foreach (var nestedType in nestedTypes)
            {
                if (nestedType.IsEnum)
                {
                    dataTypeMetadata.NestedEnumTypes.Add(csharpEnumTypeToEnumMetaDataConverter.ConvertTypeToIntermediateRepresentation(nestedType, generationOptions));
                }
                else
                {
                    dataTypeMetadata.NestedDataTypes.Add(ConvertTypeToIntermediateRepresentation(nestedType, generationOptions));
                }
            }

            return dataTypeMetadata;
        }
    }
}
