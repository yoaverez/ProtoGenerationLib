using System.Collections.Generic;
using System;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Converters.Abstracts;

namespace ProtoGenerationLib.Converters.CustomConverters
{
    /// <summary>
    /// Abstract custom converter for enum types.
    /// </summary>
    /// <remarks>
    /// Implementing this will make the customization more safe
    /// and easy for the user, however this allows less flexibility
    /// then just implementing the <see cref="ICSharpToIntermediateCustomConverter{TIntermediate}"/>.
    /// </remarks>
    public abstract class CSharpEnumTypeToEnumTypeMetadataCustomConverter : ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>
    {
        /// <inheritdoc/>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="type"/> can not be handled by this custom converter.
        /// </exception>
        public IEnumTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!CanHandle(type, generationOptions))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by this custom converter.");

            return BaseConvertTypeToIntermediateRepresentation(type, generationOptions);
        }

        /// <inheritdoc/>
        /// <inheritdoc cref="ConvertTypeToIntermediateRepresentation(Type, IProtoGenerationOptions)" path="/exception"/>
        public virtual IEnumerable<Type> ExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!CanHandle(type, generationOptions))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by this custom converter.");

            // Enum types does not need another types.
            return Array.Empty<Type>();
        }

        /// <inheritdoc/>
        public abstract bool CanHandle(Type type, IProtoGenerationOptions generationOptions);

        /// <inheritdoc cref="ICSharpToIntermediateConverter{TIntermediate}.ConvertTypeToIntermediateRepresentation(Type, IProtoGenerationOptions)"/>
        protected abstract IEnumTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions);
    }
}
