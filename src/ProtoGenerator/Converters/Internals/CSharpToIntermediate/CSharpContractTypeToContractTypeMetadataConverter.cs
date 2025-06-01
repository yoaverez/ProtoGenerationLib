using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Linq;

namespace ProtoGenerator.Converters.Internals.CSharpToIntermediate
{
    /// <summary>
    /// A converter for converting csharp contract types to <see cref="IContractTypeMetadata"/>.
    /// </summary>
    public class CSharpContractTypeToContractTypeMetadataConverter : ICSharpToIntermediateConverter<IContractTypeMetadata>
    {
        /// <inheritdoc/>
        public IContractTypeMetadata ConvertTypeToIntermediateRepresentation(Type type, IProtoGeneratorConfiguration generationOptions)
        {
            var serviceAttribute = generationOptions.AnalysisOptions.ProtoServiceAttribute;
            var rpcAttribute = generationOptions.AnalysisOptions.ProtoRpcAttribute;
            if (!type.IsDefined(serviceAttribute, serviceAttribute.IsAttributeInherited()))
                throw new ArgumentException($"Given {nameof(type)}: {type.Name} is not a contract type.", nameof(type));

            var contractMetadata = new ContractTypeMetadata();
            contractMetadata.Type = type;

            var methods = type.ExtractMethods(rpcAttribute);
            contractMetadata.Methods.AddRange(methods.Select(method => new MethodMetadata(method)));

            return contractMetadata;
        }
    }
}
