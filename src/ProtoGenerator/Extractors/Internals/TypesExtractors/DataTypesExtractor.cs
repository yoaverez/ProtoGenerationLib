using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Types extractor for data types.
    /// </summary>
    public class DataTypesExtractor : BaseTypesExtractor
    {
        /// <summary>
        /// Extractors that are specialized for specific data types.
        /// </summary>
        private IEnumerable<ITypesExtractor> dataTypesExtractors;

        /// <param name="componentsProvider">A provider of the proto generator customizations.</param>
        /// <inheritdoc cref="DataTypesExtractor(IEnumerable{ITypesExtractor})"/>
        public DataTypesExtractor(IProvider componentsProvider)
        {
            this.dataTypesExtractors = DefaultTypesExtractorsCreator.CreateDefaultDataTypeTypesExtractors(componentsProvider);
        }

        /// <summary>
        /// Create new instance of the <see cref="DataTypesExtractor"/> class.
        /// </summary>
        /// <param name="dataTypesExtractors"><inheritdoc cref="dataTypesExtractors" path="/node()"/></param>
        public DataTypesExtractor(IEnumerable<ITypesExtractor> dataTypesExtractors)
        {
            this.dataTypesExtractors = dataTypesExtractors;
        }

        /// <inheritdoc/>
        public override bool CanHandle(Type type, IProtoGenerationOptions generationOptions)
        {
            return !type.IsEnum;
        }

        /// <inheritdoc/>
        protected override IEnumerable<Type> BaseExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            foreach(var extractor in dataTypesExtractors)
            {
                if (extractor.CanHandle(type, generationOptions))
                {
                    return extractor.ExtractUsedTypes(type, generationOptions);
                }
            }

            // Should not get here.
            // There should be a default extractor that can handle any type.
            throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by data types extractors.", nameof(type));
        }
    }
}
