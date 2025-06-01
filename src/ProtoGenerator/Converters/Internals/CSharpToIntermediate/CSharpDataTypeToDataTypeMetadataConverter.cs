using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using System;
using static ProtoGenerator.Converters.Internals.CSharpToIntermediate.CSharpToIntermediateUtils;

namespace ProtoGenerator.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp data types to <see cref="IDataTypeMetadata"/>.
    /// </summary>
    public class CSharpDataTypeToDataTypeMetadataConverter : ICSharpToIntermediateConverter<IDataTypeMetadata>
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// Converter from csharp enum type to intermediate enum metadata.
        /// </summary>
        private ICSharpToIntermediateConverter<IEnumTypeMetadata> csharpEnumTypeToEnumMetaDataConverter;

        /// <summary>
        /// Create new instance of the <see cref="CSharpDataTypeToDataTypeMetadataConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        /// <param name="csharpEnumTypeToEnumMetaDataConverter"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        public CSharpDataTypeToDataTypeMetadataConverter(IProvider componentsProvider, ICSharpToIntermediateConverter<IEnumTypeMetadata>? csharpEnumTypeToEnumMetaDataConverter = null)
        {
            this.componentsProvider = componentsProvider;
            this.csharpEnumTypeToEnumMetaDataConverter = csharpEnumTypeToEnumMetaDataConverter ?? new CSharpEnumTypeToEnumTypeMetadataConverter(componentsProvider);
        }

        /// <inheritdoc/>
        public IDataTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
        {
            if (type.IsEnum)
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not a data type.", nameof(type));

            var customConverters = componentsProvider.GetDataTypeCustomConverters();

            IDataTypeMetadata dataTypeMetadata;
            if (!TryConvertWithCustomConverters(type, customConverters, generationOptions, out dataTypeMetadata))
            {
                var metadata = new DataTypeMetadata();
                metadata.Type = type;

                // Extract the fields.
                var fieldsAndPropertiesExtractor = componentsProvider.GetFieldsAndPropertiesExtractionStrategy(generationOptions.AnalysisOptions.FieldsAndPropertiesExtractionStrategy);
                var fields = fieldsAndPropertiesExtractor.ExtractFieldsAndProperties(type, generationOptions.AnalysisOptions);
                metadata.Fields.AddRange(fields);

                // Extract the nested types.
                var nestedTypes = type.GetNestedTypes();
                foreach (var nestedType in nestedTypes)
                {
                    if (nestedType.IsEnum)
                    {
                        metadata.NestedEnumTypes.Add(csharpEnumTypeToEnumMetaDataConverter.ConvertTypeToIntermediateRepresentation(nestedType, generationOptions));
                    }
                    else
                    {
                        metadata.NestedDataTypes.Add(ConvertTypeToIntermediateRepresentation(nestedType, generationOptions));
                    }
                }
                dataTypeMetadata = metadata;
            }

            return dataTypeMetadata;
        }
    }
}
