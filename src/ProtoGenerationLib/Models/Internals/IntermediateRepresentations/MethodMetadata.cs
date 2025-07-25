﻿using ProtoGenerationLib.Models.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProtoGenerationLib.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IMethodMetadata"/>
    public class MethodMetadata : DocumentableObject, IMethodMetadata
    {
        /// <inheritdoc/>
        public MethodInfo MethodInfo { get; set; }

        /// <inheritdoc/>
        public Type ReturnType { get; set; }

        /// <inheritdoc cref="IMethodMetadata.Parameters"/>
        public List<IMethodParameterMetadata> Parameters { get; set; }
        IEnumerable<IMethodParameterMetadata> IMethodMetadata.Parameters => Parameters;

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="MethodMetadata"/> class.
        /// </summary>
        public MethodMetadata()
        {
            Parameters = new List<IMethodParameterMetadata>();
        }

        /// <summary>
        /// Create new instance of the <see cref="MethodMetadata"/> class.
        /// </summary>
        /// <param name="methodInfo"><inheritdoc cref="MethodInfo" path="/node()"/></param>
        /// <param name="returnType"><inheritdoc cref="ReturnType" path="/node()"/></param>
        /// <param name="parameters"><inheritdoc cref="Parameters" path="/node()"/></param>
        public MethodMetadata(MethodInfo methodInfo, Type returnType, IEnumerable<IMethodParameterMetadata> parameters)
        {
            MethodInfo = methodInfo;
            ReturnType = returnType;
            Parameters = parameters.ToList();
        }

        /// <inheritdoc cref="MethodMetadata(MethodInfo, Type, IEnumerable{IMethodParameterMetadata})"/>
        /// <inheritdoc cref="DocumentableObject(string)" path="/param"/>
        public MethodMetadata(MethodInfo methodInfo, Type returnType, IEnumerable<IMethodParameterMetadata> parameters, string documentation) : base(documentation)
        {
            MethodInfo = methodInfo;
            ReturnType = returnType;
            Parameters = parameters.ToList();
        }

        /// <summary>
        /// Create new instance of the <see cref="MethodMetadata"/> class.
        /// </summary>
        /// <param name="methodInfo">The method info to create the <see cref="MethodMetadata"/> from.</param>
        public MethodMetadata(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
            ReturnType = methodInfo.ReturnType;
            Parameters = methodInfo.GetParameters().Select(parameterInfo => new MethodParameterMetadata(parameterInfo)).Cast<IMethodParameterMetadata>().ToList();
        }

        /// <inheritdoc cref="MethodMetadata(MethodInfo)"/>
        /// <inheritdoc cref="DocumentableObject(string)" path="/param"/>
        public MethodMetadata(MethodInfo methodInfo, string documentation) : base(documentation)
        {
            MethodInfo = methodInfo;
            ReturnType = methodInfo.ReturnType;
            Parameters = methodInfo.GetParameters().Select(parameterInfo => new MethodParameterMetadata(parameterInfo)).Cast<IMethodParameterMetadata>().ToList();
        }

        /// <summary>
        /// Create new instance of the <see cref="MethodMetadata"/> class.
        /// </summary>
        /// <param name="other"></param>
        public MethodMetadata(IMethodMetadata other) : base(other)
        {
            MethodInfo = other.MethodInfo;
            ReturnType = other.ReturnType;
            Parameters = other.Parameters.Select(parameter => new MethodParameterMetadata(parameter)).Cast<IMethodParameterMetadata>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as MethodMetadata;
            return other != null
                   && base.Equals(other)
                   && MethodInfo.Equals(other.MethodInfo)
                   && ReturnType.Equals(other.ReturnType)
                   && Parameters.SequenceEqual(other.Parameters);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (base.GetHashCode(),
                    MethodInfo,
                    ReturnType,
                    Parameters.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
