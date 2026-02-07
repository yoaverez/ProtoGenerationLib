using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ProtoGenerationLib.Strategies.Internals.MethodSignatureExtractionStrategies
{
    /// <summary>
    /// Extract method signature so it will resemble proto client method.
    /// i.e. if and rpc has a request type then the grpc client will have
    /// a method with the following parameters: request type, headers, deadline, cancellationToken.
    /// </summary>
    /// <remarks>
    /// This means that the type that defines the rpcs in c sharp
    /// can also have the headers, deadline, cancellationToken parameters
    /// and the proto rpc will ignore those (since rpc is defined without them).
    /// </remarks>
    public class ResembleProtoClientMethodsStrategy : IMethodSignatureExtractionStrategy
    {
        /// <summary>
        /// The default method signature extraction strategy.
        /// </summary>
        private DefaultMethodSignatureExtractionStrategy defaultMethodSignatureExtractionStrategy;

        /// <summary>
        /// A mapping between the names of the additional parameters that are created
        /// for a proto rpc (i.e. the headers of the rpc,
        /// the deadline for the rpc or the cancellation of the rpc)
        /// to a method that checks that the type of the parameter also matches.
        /// </summary>
        private Dictionary<string, Func<Type, bool>> protoRpcAdditionalParametersNames;

        /// <summary>
        /// Create new instance of the <see cref="ResembleProtoClientMethodsStrategy"/> class.
        /// </summary>
        /// <param name="protoRpcAdditionalParametersNames"><inheritdoc cref="protoRpcAdditionalParametersNames" path="/node()"/></param>
        public ResembleProtoClientMethodsStrategy(IReadOnlyDictionary<string, Func<Type, bool>>? protoRpcAdditionalParametersNames = null)
        {
            defaultMethodSignatureExtractionStrategy = new DefaultMethodSignatureExtractionStrategy();
            this.protoRpcAdditionalParametersNames = protoRpcAdditionalParametersNames?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                new Dictionary<string, Func<Type, bool>> {
                    ["headers"] = (type) => type.Name == "Metadata",
                    ["deadline"] = (type) => type == typeof(DateTime) || type == typeof(DateTime?),
                    ["cancellationToken"] = (type) => type == typeof(CancellationToken) || type == typeof(CancellationToken?),
                };
        }

        /// <inheritdoc/>
        public (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) ExtractMethodSignature(MethodInfo method, Type parameterIgnoreAttribute)
        {
            var (returnType, parameters) = defaultMethodSignatureExtractionStrategy.ExtractMethodSignature(method, parameterIgnoreAttribute);

            var parametersWithoutAdditionals = parameters.Where(p =>
            {
                if(protoRpcAdditionalParametersNames.TryGetValue(p.Name, out var isTypeCorrectMethod))
                {
                    // If type matches the additional parameter, remove it.
                    if(isTypeCorrectMethod(p.Type))
                        return false;
                }

                return true;
            }).ToArray();

            return (returnType, parametersWithoutAdditionals);
        }
    }
}
