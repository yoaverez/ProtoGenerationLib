using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using System;
using System.Linq;
using static ProtoGenerator.Converters.Internals.CSharpToIntermediate.CSharpToIntermediateUtils;

namespace ProtoGenerator.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp enum types to <see cref="IEnumTypeMetadata"/>.
    /// </summary>
    public class CSharpEnumTypeToEnumTypeMetadataConverter : ICSharpToIntermediateConverter<IEnumTypeMetadata>
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

            var customConverters = componentsProvider.GetEnumTypeCustomConverters();

            IEnumTypeMetadata enumMetadata;
            if (!TryConvertWithCustomConverters(type, customConverters, generationOptions, out enumMetadata))
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
