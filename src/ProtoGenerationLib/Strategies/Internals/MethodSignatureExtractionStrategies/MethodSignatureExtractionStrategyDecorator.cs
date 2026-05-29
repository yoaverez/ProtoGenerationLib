using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ProtoGenerationLib.Strategies.Internals.MethodSignatureExtractionStrategies
{
    /// <summary>
    /// Wrapper for method signature extraction strategies.
    /// It apply the wrappee strategy on the method and then alter its result.
    /// </summary>
    public abstract class MethodSignatureExtractionStrategyDecorator : IMethodSignatureExtractionStrategy
    {
        /// <summary>
        /// The strategy to wrap.
        /// </summary>
        private readonly IMethodSignatureExtractionStrategy underlineStrategy;

        /// <summary>
        /// Initialize a new instance of the <see cref="MethodSignatureExtractionStrategyDecorator"/> class.
        /// </summary>
        /// <param name="underlineStrategy"><inheritdoc cref="underlineStrategy" path="/node()"/></param>
        public MethodSignatureExtractionStrategyDecorator(IMethodSignatureExtractionStrategy underlineStrategy)
        {
            this.underlineStrategy = underlineStrategy;
        }

        /// <inheritdoc/>
        public (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) ExtractMethodSignature(MethodInfo method, IAnalysisOptions analysisOptions)
        {
            var underlineStrategyResult = underlineStrategy.ExtractMethodSignature(method, analysisOptions);
            return DecorateStrategy(method, underlineStrategyResult.ReturnType, underlineStrategyResult.Parameters, analysisOptions);
        }

        /// <summary>
        /// Modify the given <paramref name="returnType"/> and <paramref name="parameters"/> that was
        /// supplied by the <see cref="underlineStrategy"/>.
        /// </summary>
        /// <param name="method">The method whose signature is requested.</param>
        /// <param name="returnType">The return type of the <see cref="underlineStrategy"/> result.</param>
        /// <param name="parameters">The parameters of the <see cref="underlineStrategy"/> result.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <returns>The modified return type and parameters.</returns>
        protected abstract (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) DecorateStrategy(MethodInfo method, Type returnType, IEnumerable<IMethodParameterMetadata> parameters, IAnalysisOptions analysisOptions);
    }
}
