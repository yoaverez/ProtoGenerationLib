using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// A general extractor that can extract used types for all the possible data types.
    /// </summary>
    internal class DefaultDataTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// Provider of extraction strategies.
        /// </summary>
        private IExtractionStrategiesProvider extractionStrategiesProvider;

        /// <summary>
        /// Extractor for needed proto types from fields.
        /// </summary>
        private IFieldsTypesExtractor fieldsTypesExtractor;

        /// <summary>
        /// Create new instance of the <see cref="DefaultDataTypesExtractor"/> class.
        /// </summary>
        /// <param name="extractionStrategiesProvider"><inheritdoc cref="extractionStrategiesProvider" path="/node()"/></param>
        /// <param name="wrapperElementTypesExtractors"><inheritdoc cref="fieldsTypesExtractor" path="/node()"/></param>
        public DefaultDataTypesExtractor(IExtractionStrategiesProvider extractionStrategiesProvider,
                                         IFieldsTypesExtractor? fieldsTypesExtractor = null)
        {
            this.extractionStrategiesProvider = extractionStrategiesProvider;
            this.fieldsTypesExtractor = fieldsTypesExtractor ?? FieldsTypesExtractor.Instance;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            var fieldsAndPropertiesExtractionStrategy = extractionStrategiesProvider.GetFieldsAndPropertiesExtractionStrategy(generationOptions.AnalysisOptions.FieldsAndPropertiesExtractionStrategy);
            var fieldTypes = fieldsAndPropertiesExtractionStrategy.ExtractFieldsAndProperties(type, generationOptions.AnalysisOptions)
                                                                  .Select(member => member.Type)
                                                                  .ToHashSet();

            return fieldsTypesExtractor.ExtractUsedTypesFromFields(fieldTypes);
        }
    }
}
