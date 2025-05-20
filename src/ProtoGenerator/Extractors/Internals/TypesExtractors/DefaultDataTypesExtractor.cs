using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// A general extractor that can extract used types for all the possible data types.
    /// </summary>
    public class DefaultDataTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// Provider of extraction strategies.
        /// </summary>
        private IExtractionStrategiesProvider extractionStrategiesProvider;

        /// <summary>
        /// Types extractors for wrapper types.
        /// </summary>
        private IEnumerable<ITypesExtractor> wrapperElementTypesExtractors;

        /// <summary>
        /// Create new instance of the <see cref="DefaultDataTypesExtractor"/> class.
        /// </summary>
        /// <param name="extractionStrategiesProvider"><inheritdoc cref="extractionStrategiesProvider" path="/node()"/></param>
        /// <param name="wrapperElementTypesExtractors"><inheritdoc cref="wrapperElementTypesExtractors" path="/node()"/></param>
        public DefaultDataTypesExtractor(IExtractionStrategiesProvider extractionStrategiesProvider,
                                         IEnumerable<ITypesExtractor> wrapperElementTypesExtractors)
        {
            this.extractionStrategiesProvider = extractionStrategiesProvider;
            this.wrapperElementTypesExtractors = wrapperElementTypesExtractors;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, ITypeExtractionOptions typeExtractionOptions)
        {
            var fieldsAndPropertiesExtractionStrategy = extractionStrategiesProvider.GetFieldsAndPropertiesExtractionStrategy(typeExtractionOptions.AnalysisOptions.FieldsAndPropertiesExtractionStrategy);
            var fieldTypes = fieldsAndPropertiesExtractionStrategy.ExtractFieldsAndProperties(type, typeExtractionOptions.AnalysisOptions)
                                                                  .Select(member => member.Type)
                                                                  .ToHashSet();

            // Extract element types from wrapper types like nullable or enumerable types.
            // The ToArray is so that the fieldTypes set could be changed inside the loop.
            foreach (var fieldType in fieldTypes.ToArray())
            {
                foreach (var wrapperElementTypesExtractor in wrapperElementTypesExtractors)
                {
                    if (wrapperElementTypesExtractor.CanHandle(fieldType, typeExtractionOptions))
                    {
                        var elementTypes = wrapperElementTypesExtractor.ExtractUsedTypes(fieldType, typeExtractionOptions);

                        // Remove the wrapper from the fieldTypes.
                        fieldTypes.Remove(fieldType);

                        // Add the element types of the wrapper type.
                        fieldTypes.AddRange(elementTypes);

                        // There is no need to keep looking for wrappers.
                        break;
                    }
                }
            }
            return fieldTypes;
        }
    }
}
