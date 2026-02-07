using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Replacers.Internals;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProtoGenerationLib.Strategies.Internals.MethodSignatureExtractionStrategies
{
    /// <summary>
    /// The default strategy for extracting method signature.
    /// </summary>
    public class DefaultMethodSignatureExtractionStrategy : IMethodSignatureExtractionStrategy
    {
        /// <summary>
        /// Replacers that replaces specific types from methods signature.
        /// </summary>
        private readonly IEnumerable<IMethodSignatureTypeReplacer> typeReplacers;

        /// <summary>
        /// Create new instance of the <see cref="DefaultMethodSignatureExtractionStrategy"/> class.
        /// </summary>
        /// <param name="typeReplacers"><inheritdoc cref="typeReplacers" path="/node()"/></param>
        public DefaultMethodSignatureExtractionStrategy(IEnumerable<IMethodSignatureTypeReplacer>? typeReplacers = null)
        {
            this.typeReplacers = typeReplacers ?? DefaultMethodSignatureTypeReplacersProvider.GetDefaultMethodSignatureTypeReplacers();
        }

        /// <inheritdoc/>
        public (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) ExtractMethodSignature(MethodInfo method, Type parameterIgnoreAttribute)
        {
            var methodReturnType = GetEffectiveType(method.ReturnType, isReturnType: true);
            var methodParameters = method.GetParameters()
                                         // Take all the parameters excepts for the ones to ignore.
                                         .Where(p => !p.IsDefined(parameterIgnoreAttribute, parameterIgnoreAttribute.IsAttributeInherited()))
                                         .Select(p => new MethodParameterMetadata(GetEffectiveType(p.ParameterType, isReturnType: false), p.Name))
                                         .ToArray();

            return (methodReturnType, methodParameters);
        }

        /// <summary>
        /// Get the effective signature type of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose effective signature type is requested.</param>
        /// <param name="isReturnType">Whether or not the given type is a method return type.</param>
        /// <returns>The effective signature type of the given <paramref name="type"/>.</returns>
        private Type GetEffectiveType(Type type, bool isReturnType)
        {
            foreach(var typeReplacer in typeReplacers)
            {
                if(typeReplacer.CanReplace(type, isReturnType))
                    return typeReplacer.ReplaceType(type, isReturnType);
            }

            // There was no need in replacing the type.
            return type;
        }
    }
}
