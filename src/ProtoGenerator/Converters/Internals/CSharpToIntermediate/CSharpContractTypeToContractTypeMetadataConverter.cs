using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Linq;
using static ProtoGenerator.Converters.Internals.CSharpToIntermediate.CSharpToIntermediateUtils;

namespace ProtoGenerator.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp contract types to <see cref="IContractTypeMetadata"/>.
    /// </summary>
    public class CSharpContractTypeToContractTypeMetadataConverter : ICSharpToIntermediateConverter<IContractTypeMetadata>
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
            var serviceAttribute = generationOptions.AnalysisOptions.ProtoServiceAttribute;
            var rpcAttribute = generationOptions.AnalysisOptions.ProtoRpcAttribute;
            if (!type.IsDefined(serviceAttribute, serviceAttribute.IsAttributeInherited()))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not a contract type.", nameof(type));

            var customConverters = componentsProvider.GetContractTypeCustomConverters();

            IContractTypeMetadata contractMetadata;
            if (!TryConvertWithCustomConverters(type, customConverters, generationOptions, out contractMetadata))
            {
                var contractTypeMetadata = new ContractTypeMetadata();
                contractTypeMetadata.Type = type;

                var methods = type.ExtractMethods(rpcAttribute);
                contractTypeMetadata.Methods.AddRange(methods.Select(method => new MethodMetadata(method)));
                contractMetadata = contractTypeMetadata;
            }

            return contractMetadata;
        }
    }
}
