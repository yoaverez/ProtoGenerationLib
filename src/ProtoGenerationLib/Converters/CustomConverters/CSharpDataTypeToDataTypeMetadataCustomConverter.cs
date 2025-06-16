using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Extractors.Internals.TypesExtractors;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Converters.CustomConverters
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
        public IDataTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!CanHandle(type, generationOptions))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by this custom converter.");

            return BaseConvertTypeToIntermediateRepresentation(type, generationOptions);
        }

        /// <inheritdoc/>
        /// <inheritdoc cref="ConvertTypeToIntermediateRepresentation(Type, IProtoGenerationOptions)" path="/exception"/>
        public virtual IEnumerable<Type> ExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            var metadata = ConvertTypeToIntermediateRepresentation(type, generationOptions);
            var fieldTypes = metadata.Fields.Select(fieldMetadata => fieldMetadata.Type).ToHashSet();
            return FieldsTypesExtractor.Instance.ExtractUsedTypesFromFields(fieldTypes, generationOptions);
        }

        /// <inheritdoc/>
        public abstract bool CanHandle(Type type, IProtoGenerationOptions generationOptions);

        /// <inheritdoc cref="ICSharpToIntermediateConverter{TIntermediate}.ConvertTypeToIntermediateRepresentation(Type, IProtoGenerationOptions)"/>
        protected abstract IDataTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions);
    }
}
