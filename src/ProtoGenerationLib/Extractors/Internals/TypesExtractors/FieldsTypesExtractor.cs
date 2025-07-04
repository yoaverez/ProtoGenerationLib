using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Extractors.Abstracts;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Extractors.Internals.TypesExtractors
{
    /// <summary>
    /// Extractor for needed proto types from fields.
    /// </summary>
    internal class FieldsTypesExtractor : IFieldsTypesExtractor
    {
        /// <summary>
        /// Types extractors for wrapper types.
        /// </summary>
        private IEnumerable<IWrapperElementTypeExtractor> wrapperElementTypesExtractors => lazyWrapperElementTypesExtractors.Value;

        /// <summary>
        /// The lazy initialization of the <see cref="wrapperElementTypesExtractors"/> property.
        /// </summary>
        private Lazy<IEnumerable<IWrapperElementTypeExtractor>> lazyWrapperElementTypesExtractors;

        #region Singleton

        /// <summary>
        /// The only instance of the <see cref="FieldsTypesExtractor"/> class.
        /// </summary>
        public static FieldsTypesExtractor Instance { get; }

        /// <summary>
        /// Initialize the static members of the <see cref="FieldsTypesExtractor"/> class.
        /// </summary>
        static FieldsTypesExtractor()
        {
            Instance = new FieldsTypesExtractor();
        }

        /// <summary>
        /// Create new instance of the <see cref="FieldsTypesExtractor"/> class.
        /// </summary>
        private FieldsTypesExtractor()
        {
            lazyWrapperElementTypesExtractors = new Lazy<IEnumerable<IWrapperElementTypeExtractor>>(DefaultTypesExtractorsCreator.CreateDefaultWrapperElementTypesExtractors);
        }

        #endregion Singleton

        /// <inheritdoc/>
        public IEnumerable<Type> ExtractUsedTypesFromFields(IEnumerable<Type> fieldTypes)
        {
            var neededTypes = fieldTypes.ToHashSet();

            // Extract element types from wrapper types like nullable or enumerable types.
            foreach (var fieldType in fieldTypes)
            {
                foreach (var wrapperElementTypesExtractor in wrapperElementTypesExtractors)
                {
                    if (wrapperElementTypesExtractor.CanHandle(fieldType))
                    {
                        var elementTypes = wrapperElementTypesExtractor.ExtractUsedTypes(fieldType);

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
