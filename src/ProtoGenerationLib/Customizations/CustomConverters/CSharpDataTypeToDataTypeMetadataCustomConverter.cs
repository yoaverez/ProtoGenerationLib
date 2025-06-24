using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Customizations.CustomConverters
{
    /// <summary>
    /// Abstract custom converter for data types.
    /// </summary>
    /// <remarks>
    /// Implementing this will make the customization more safe
    /// and easy for the user, however this allows less flexibility
    /// then just implementing the <see cref="ICSharpToIntermediateCustomConverter{TIntermediate}"/>.
    /// </remarks>
    public abstract class CSharpDataTypeToDataTypeMetadataCustomConverter : ICSharpToIntermediateCustomConverter<IDataTypeMetadata>
    {
        /// <inheritdoc/>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="type"/> can not be handled by this custom converter.
        /// </exception>
        public IDataTypeMetadata ConvertTypeToIntermediateRepresentation(Type type)
        {
            if (!CanHandle(type))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by this custom converter.");

            return BaseConvertTypeToIntermediateRepresentation(type);
        }

        /// <inheritdoc/>
        /// <inheritdoc cref="ConvertTypeToIntermediateRepresentation(Type)" path="/exception"/>
        public virtual IEnumerable<Type> ExtractUsedTypes(Type type)
        {
            var metadata = ConvertTypeToIntermediateRepresentation(type);
            var fieldTypes = metadata.Fields.Select(fieldMetadata => fieldMetadata.Type).ToHashSet();
            return FieldsTypesExtractor.Instance.ExtractUsedTypesFromFields(fieldTypes);
        }

        /// <inheritdoc/>
        public abstract bool CanHandle(Type type);

        /// <inheritdoc cref="ICSharpToIntermediateCustomConverter{TIntermediate}.ConvertTypeToIntermediateRepresentation(Type)"/>
        protected abstract IDataTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type);
    }
}
