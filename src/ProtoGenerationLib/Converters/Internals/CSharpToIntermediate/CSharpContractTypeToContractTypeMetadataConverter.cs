using ProtoGenerationLib.CommonUtilities;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.Linq;
using System.Reflection;
using static ProtoGenerationLib.Converters.Internals.CSharpToIntermediate.CSharpToIntermediateUtils;

namespace ProtoGenerationLib.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp contract types to <see cref="IContractTypeMetadata"/>.
    /// </summary>
    internal class CSharpContractTypeToContractTypeMetadataConverter : ICSharpToIntermediateConverter<IContractTypeMetadata>
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// Create new instance of the <see cref="CSharpContractTypeToContractTypeMetadataConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        public CSharpContractTypeToContractTypeMetadataConverter(IProvider componentsProvider)
        {
            this.componentsProvider = componentsProvider;
        }

        /// <inheritdoc/>
        public IContractTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions)
        {
            if (!type.IsProtoService(generationOptions.AnalysisOptions))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not a contract type.", nameof(type));

            var customConverters = generationOptions.GetContractTypeCustomConverters();

            IContractTypeMetadata contractMetadata;
            if (!TryConvertWithCustomConverters(type, customConverters, out contractMetadata))
            {
                var contractTypeMetadata = new ContractTypeMetadata();
                contractTypeMetadata.Type = type;

                var documentationExtractionStrategy = componentsProvider.GetDocumentationExtractionStrategy(generationOptions.AnalysisOptions.DocumentationExtractionStrategy);
                if (TryGetTypeDocumentation(type, generationOptions.AnalysisOptions.DocumentationProvider, documentationExtractionStrategy, out var documentation))
                {
                    contractTypeMetadata.Documentation = documentation;
                }

                var methods = type.ExtractRpcMethods(generationOptions.AnalysisOptions);
                contractTypeMetadata.Methods.AddRange(methods.Select(method =>
                {
                    if (TryGetMethodDocumentation(type, method, generationOptions.AnalysisOptions.DocumentationProvider, documentationExtractionStrategy, out var documentation))
                        return new MethodMetadata(method, documentation);
                    return new MethodMetadata(method);
                }));
                contractMetadata = contractTypeMetadata;
            }

            return contractMetadata;
        }

        /// <summary>
        /// Try get method documentation.
        /// </summary>
        /// <param name="contractType">The type that declares the method.</param>
        /// <param name="methodInfo">The method whose documentation is requested.</param>
        /// <param name="documentationProvider">A provider for user defined documentation.</param>
        /// <param name="documentationExtractionStrategy">An extractor for csharp entities documentation.</param>
        /// <param name="documentation">The documentation if found.</param>
        /// <returns>
        /// <see langword="true"/> if the documentation of the given <paramref name="methodInfo"/>
        /// was found otherwise <see langword="false"/>.
        /// </returns>
        private bool TryGetMethodDocumentation(Type contractType,
                                               MethodInfo methodInfo,
                                               IDocumentationProvider documentationProvider,
                                               IDocumentationExtractionStrategy documentationExtractionStrategy,
                                               out string documentation)
        {
            if (!documentationProvider.TryGetMethodDocumentation(contractType, methodInfo.Name, methodInfo.GetParameters().Length, out documentation))
            {
                if (!documentationExtractionStrategy.TryGetMethodDocumentation(methodInfo, out documentation))
                {
                    documentation = string.Empty;
                    return false;
                }
            }

            return true;
        }
    }
}
