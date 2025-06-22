using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using System;
using static ProtoGenerationLib.Converters.Internals.CSharpToIntermediate.CSharpToIntermediateUtils;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Converters.Abstracts;

namespace ProtoGenerationLib.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp data types to <see cref="IDataTypeMetadata"/>.
    /// </summary>
    internal class CSharpDataTypeToDataTypeMetadataConverter : ICSharpToIntermediateConverter<IDataTypeMetadata>
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// Converter from csharp enum type to intermediate enum metadata.
        /// </summary>
        private ICSharpToIntermediateConverter<IEnumTypeMetadata> csharpEnumTypeToEnumMetadataConverter;

        /// <summary>
        /// Create new instance of the <see cref="CSharpDataTypeToDataTypeMetadataConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        /// <param name="csharpEnumTypeToEnumMetadataConverter"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        public CSharpDataTypeToDataTypeMetadataConverter(IProvider componentsProvider, ICSharpToIntermediateConverter<IEnumTypeMetadata>? csharpEnumTypeToEnumMetadataConverter = null)
        {
            this.componentsProvider = componentsProvider;
            this.csharpEnumTypeToEnumMetadataConverter = csharpEnumTypeToEnumMetadataConverter ?? new CSharpEnumTypeToEnumTypeMetadataConverter(componentsProvider);
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
                        metadata.NestedEnumTypes.Add(csharpEnumTypeToEnumMetadataConverter.ConvertTypeToIntermediateRepresentation(nestedType, generationOptions));
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
