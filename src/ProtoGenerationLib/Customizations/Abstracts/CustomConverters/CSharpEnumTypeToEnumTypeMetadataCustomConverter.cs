using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Customizations.Abstracts.CustomConverters
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
        public IEnumTypeMetadata ConvertTypeToIntermediateRepresentation(Type type)
        {
            if (!CanHandle(type))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by this custom converter.");

            return BaseConvertTypeToIntermediateRepresentation(type);
        }

        /// <inheritdoc/>
        /// <inheritdoc cref="ConvertTypeToIntermediateRepresentation(Type)" path="/exception"/>
        public virtual IEnumerable<Type> ExtractUsedTypes(Type type)
        {
            if (!CanHandle(type))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by this custom converter.");

            // Enum types does not need another types.
            return Array.Empty<Type>();
        }

        /// <inheritdoc/>
        public abstract bool CanHandle(Type type);

        /// <inheritdoc cref="ICSharpToIntermediateCustomConverter{TIntermediate}.ConvertTypeToIntermediateRepresentation(Type)"/>
        protected abstract IEnumTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type);
    }
}
