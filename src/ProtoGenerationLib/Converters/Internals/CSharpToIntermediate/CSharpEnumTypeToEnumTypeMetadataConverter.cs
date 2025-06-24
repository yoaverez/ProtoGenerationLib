using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using System;
using System.Linq;
using static ProtoGenerationLib.Converters.Internals.CSharpToIntermediate.CSharpToIntermediateUtils;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Converters.Abstracts;

namespace ProtoGenerationLib.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp enum types to <see cref="IEnumTypeMetadata"/>.
    /// </summary>
    internal class CSharpEnumTypeToEnumTypeMetadataConverter : ICSharpToIntermediateConverter<IEnumTypeMetadata>
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// Create new instance of the <see cref="CSharpEnumTypeToEnumTypeMetadataConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        public CSharpEnumTypeToEnumTypeMetadataConverter(IProvider componentsProvider)
        {
            this.componentsProvider = componentsProvider;
        }

        /// <inheritdoc/>
        public IEnumTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!type.IsEnum)
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not an enum.", nameof(type));

            var customConverters = generationOptions.GetEnumTypeCustomConverters();

            IEnumTypeMetadata enumMetadata;
            if (!TryConvertWithCustomConverters(type, customConverters, out enumMetadata))
            {
                var metadata = new EnumTypeMetadata();
                metadata.Type = type;

                var values = Enum.GetValues(type).Cast<int>();
                var names = Enum.GetNames(type);
                metadata.Values = names.Zip(values, (name, value) => new EnumValueMetadata(name, value))
                                           .Cast<IEnumValueMetadata>()
                                           .ToList();
                enumMetadata = metadata;
            }

            return enumMetadata;
        }
    }
}
