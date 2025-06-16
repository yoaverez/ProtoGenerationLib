using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Converters.CustomConverters
{
    /// <summary>
    /// Abstract custom converter for contract types.
    /// </summary>
    /// <remarks>
    /// Implementing this will make the customization more safe
    /// and easy for the user, however this allows less flexibility
    /// then just implementing the <see cref="ICSharpToIntermediateCustomConverter{TIntermediate}"/>.
    /// </remarks>
    public abstract class CSharpContractTypeToContractTypeMetadataCustomConverter : ICSharpToIntermediateCustomConverter<IContractTypeMetadata>
    {
        /// <inheritdoc/>
        /// <exception cref="Exception">
        /// Thrown when <see cref="BaseConvertTypeToIntermediateRepresentation(Type, IProtoGenerationOptions)"/>
        /// returns a metadata with at least one method that have more than one parameter.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the given <paramref name="type"/> can not be handled by this custom converter.
        /// </exception>
        public IContractTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!CanHandle(type, generationOptions))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be handled by this custom converter.");

            var metadata = BaseConvertTypeToIntermediateRepresentation(type, generationOptions);
            foreach (var method in metadata.Methods)
            {
                if (method.Parameters.Count() > 1)
                    throw new Exception($"The metadata you created contains a method named {method.MethodInfo.Name} that have more than one parameter.");
            }
            return metadata;
        }

        /// <inheritdoc/>
        /// <inheritdoc cref="ConvertTypeToIntermediateRepresentation(Type, IProtoGenerationOptions)" path="/exception"/>
        public virtual IEnumerable<Type> ExtractUsedTypes(Type type, IProtoGenerationOptions generationOptions)
        {
            var metadata = ConvertTypeToIntermediateRepresentation(type, generationOptions);
            var usedTypes = new HashSet<Type>();
            foreach (var method in metadata.Methods)
            {
                usedTypes.Add(method.ReturnType);
                if (method.Parameters.Count() == 0)
                    usedTypes.Add(typeof(void));
                else
                    // If there are more than on parameter
                    // the ConvertTypeToIntermediateRepresentation
                    // would thrown an exception.
                    usedTypes.Add(method.Parameters.Single().Type);
            }
            return usedTypes;
        }

        /// <inheritdoc/>
        public abstract bool CanHandle(Type type, IProtoGenerationOptions generationOptions);

        /// <inheritdoc cref="ICSharpToIntermediateConverter{TIntermediate}.ConvertTypeToIntermediateRepresentation(Type, IProtoGenerationOptions)"/>
        /// <remarks><b>Note: All the methods in the returned metadata MUST have one parameter or less.</b></remarks>
        protected abstract IContractTypeMetadata BaseConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions);
    }
}
