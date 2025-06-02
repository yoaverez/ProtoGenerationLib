using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Extractors.Abstracts;
using ProtoGenerator.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoGenerator.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Extractor for needed proto types from fields.
    /// </summary>
    public class FieldsTypesExtractor : IFieldsTypesExtractor
    {
        /// <summary>
        /// Types extractors for wrapper types.
        /// </summary>
        public IEnumerable<ITypesExtractor> wrapperElementTypesExtractors;

        #region Singleton

        /// <summary>
        /// The only instance of the <see cref="FieldsTypesExtractor"/> class
        /// that will be initialize lazily in order to use mocks in tests.
        /// </summary>
        private static Lazy<FieldsTypesExtractor> instance;
        /// <summary>
        /// The only instance of the <see cref="FieldsTypesExtractor"/> class.
        /// </summary>
        public static FieldsTypesExtractor Instance => instance.Value;

        /// <summary>
        /// Initialize the static members of the <see cref="FieldsTypesExtractor"/> class.
        /// </summary>
        static FieldsTypesExtractor()
        {
            instance = new Lazy<FieldsTypesExtractor>(() => new FieldsTypesExtractor());
        }

        /// <summary>
        /// Create new instance of the <see cref="FieldsTypesExtractor"/> class.
        /// </summary>
        private FieldsTypesExtractor()
        {
            wrapperElementTypesExtractors = DefaultTypesExtractorsCreator.CreateDefaultWrapperElementTypesExtractors();
        }

        #endregion Singleton

        /// <inheritdoc/>
        public IEnumerable<Type> ExtractUsedTypesFromFields(IEnumerable<Type> fieldTypes, IProtoGenerationOptions generationOptions)
        {
            var neededTypes = fieldTypes.ToHashSet();

            // Extract element types from wrapper types like nullable or enumerable types.
            foreach (var fieldType in fieldTypes)
            {
                foreach (var wrapperElementTypesExtractor in wrapperElementTypesExtractors)
                {
                    if (wrapperElementTypesExtractor.CanHandle(fieldType, generationOptions))
                    {
                        var elementTypes = wrapperElementTypesExtractor.ExtractUsedTypes(fieldType, generationOptions);

                        // Remove the wrapper from the fieldTypes.
                        neededTypes.Remove(fieldType);

                        // Add the element types of the wrapper type.
                        neededTypes.AddRange(elementTypes);

                        // There is no need to keep looking for wrappers.
                        break;
                    }
                }
            }
            return neededTypes;
        }
    }
}
