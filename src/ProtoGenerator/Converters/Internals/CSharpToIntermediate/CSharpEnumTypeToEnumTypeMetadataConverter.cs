using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using System;
using System.Linq;

namespace ProtoGenerator.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp enum types to <see cref="IEnumTypeMetadata"/>.
    /// </summary>
    public class CSharpEnumTypeToEnumTypeMetadataConverter : ICSharpToIntermediateConverter<IEnumTypeMetadata>
    {
        /// <inheritdoc/>
        public IEnumTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGeneratorConfiguration generationOptions)
        {
            if (!type.IsEnum)
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not an enum.", nameof(type));

            var enumMetadata = new EnumTypeMetadata();
            enumMetadata.Type = type;

            var values = Enum.GetValues(type).Cast<int>();
            var names = Enum.GetNames(type);
            enumMetadata.Values = names.Zip(values, (name, value) => new EnumValueMetadata(name, value))
                                       .Cast<IEnumValueMetadata>()
                                       .ToList();
            return enumMetadata;
        }
    }
}
