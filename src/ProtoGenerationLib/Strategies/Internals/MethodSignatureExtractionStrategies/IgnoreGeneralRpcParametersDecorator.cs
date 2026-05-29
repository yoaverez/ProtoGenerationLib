using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ProtoGenerationLib.Strategies.Internals.MethodSignatureExtractionStrategies
{
    /// <summary>
    /// Decorator for a given method signature extraction strategy.
    /// This decorator filter method parameters
    /// that are added by default after the proto services generate the c# service.
    /// i.e. the following request parameters: headers, deadline, cancellationToken.
    /// </summary>
    /// <remarks>
    /// This means that the type that defines the rpcs in c sharp
    /// can also have the headers, deadline, cancellationToken parameters
    /// and the proto rpc will ignore those (since rpc is defined without them).
    /// </remarks>
    public class IgnoreGeneralRpcParametersDecorator : MethodSignatureExtractionStrategyDecorator
    {
        /// <summary>
        /// A mapping between the names of the additional parameters that are created
        /// for a proto rpc (i.e. the headers of the rpc,
        /// the deadline for the rpc or the cancellation of the rpc)
        /// to a method that checks that the type of the parameter also matches.
        /// </summary>
        private Dictionary<string, Func<Type, bool>> protoRpcGeneralParametersNames;

        /// <summary>
        /// Create new instance of the <see cref="IgnoreGeneralRpcParametersDecorator"/> class.
        /// </summary>
        /// <param name="underlineStrategy"><inheritdoc cref="MethodSignatureExtractionStrategyDecorator(IMethodSignatureExtractionStrategy)" path="/param[@name='underlineStrategy']"/></param>
        /// <param name="protoRpcGeneralParametersNames"><inheritdoc cref="protoRpcGeneralParametersNames" path="/node()"/></param>
        public IgnoreGeneralRpcParametersDecorator(IMethodSignatureExtractionStrategy underlineStrategy, IReadOnlyDictionary<string, Func<Type, bool>>? protoRpcGeneralParametersNames = null) : base(underlineStrategy)
        {
            this.protoRpcGeneralParametersNames = protoRpcGeneralParametersNames?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ??
                new Dictionary<string, Func<Type, bool>> {
                    ["headers"] = (type) => type.Name == "Metadata",
                    ["deadline"] = (type) => type == typeof(DateTime) || type == typeof(DateTime?),
                    ["cancellationToken"] = (type) => type == typeof(CancellationToken) || type == typeof(CancellationToken?),
                };
        }

        /// <inheritdoc/>
        protected override (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) DecorateStrategy(MethodInfo method, Type returnType, IEnumerable<IMethodParameterMetadata> parameters, IAnalysisOptions analysisOptions)
        {
            var parametersWithoutAdditionals = parameters.Where(p =>
            {
                if (protoRpcGeneralParametersNames.TryGetValue(p.Name, out var isTypeCorrectMethod))
                {
                    // If type matches the additional parameter, remove it.
                    if (isTypeCorrectMethod(p.Type))
                        return false;
                }

                return true;
            }).ToArray();

            return (returnType, parametersWithoutAdditionals);
        }
    }
}
