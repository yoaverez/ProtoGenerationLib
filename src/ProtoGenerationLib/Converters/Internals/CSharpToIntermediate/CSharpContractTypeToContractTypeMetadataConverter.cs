using ProtoGenerationLib.CommonUtilities;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using System;
using System.Linq;
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

            var customConverters = componentsProvider.GetContractTypeCustomConverters();

            IContractTypeMetadata contractMetadata;
            if (!TryConvertWithCustomConverters(type, customConverters, generationOptions, out contractMetadata))
            {
                var contractTypeMetadata = new ContractTypeMetadata();
                contractTypeMetadata.Type = type;

                var methods = type.ExtractRpcMethods(generationOptions.AnalysisOptions);
                contractTypeMetadata.Methods.AddRange(methods.Select(method => new MethodMetadata(method)));
                contractMetadata = contractTypeMetadata;
            }

            return contractMetadata;
        }
    }
}
