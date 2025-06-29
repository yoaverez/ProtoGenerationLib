using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.Linq;
using static ProtoGenerationLib.Converters.Internals.CSharpToIntermediate.CSharpToIntermediateUtils;

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

                var documentationExtractionStrategy = componentsProvider.GetDocumentationExtractionStrategy(generationOptions.AnalysisOptions.DocumentationExtractionStrategy);
                if (TryGetTypeDocumentation(type, generationOptions.AnalysisOptions.DocumentationProvider, documentationExtractionStrategy, out var documentation))
                    metadata.Documentation = documentation;

                var values = Enum.GetValues(type).Cast<int>();
                var names = Enum.GetNames(type);
                metadata.Values = names.Zip(values, (name, value) =>
                                            {
                                                var enumValueMetadata = new EnumValueMetadata(name, value);

                                                if (TryGetEnumValueDocumentation(type, value, generationOptions.AnalysisOptions.DocumentationProvider, documentationExtractionStrategy, out var documentation))
                                                    enumValueMetadata.Documentation = documentation;

                                                return enumValueMetadata;
                                            })
                                           .Cast<IEnumValueMetadata>()
                                           .ToList();
                enumMetadata = metadata;
            }

            return enumMetadata;
        }

        /// <summary>
        /// Try get enum value documentation.
        /// </summary>
        /// <param name="enumType">The type of the enum.</param>
        /// <param name="enumValue">The enum value whose documentation is requested.</param>
        /// <param name="documentationProvider">A provider for user defined documentation.</param>
        /// <param name="documentationExtractionStrategy">An extractor for csharp entities documentation.</param>
        /// <param name="documentation">The documentation if found.</param>
        /// <returns>
        /// <see langword="true"/> if the documentation of the given <paramref name="enumValue"/>
        /// was found otherwise <see langword="false"/>.
        /// </returns>
        private bool TryGetEnumValueDocumentation(Type enumType,
                                                  int enumValue,
                                                  IDocumentationProvider documentationProvider,
                                                  IDocumentationExtractionStrategy documentationExtractionStrategy,
                                                  out string documentation)
        {
            if (!documentationProvider.TryGetEnumValueDocumentation(enumType, enumValue, out documentation))
            {
                if (!documentationExtractionStrategy.TryGetEnumValueDocumentation(enumType, enumValue, out documentation))
                {
                    documentation = string.Empty;
                    return false;
                }
            }

            return true;
        }
    }
}
