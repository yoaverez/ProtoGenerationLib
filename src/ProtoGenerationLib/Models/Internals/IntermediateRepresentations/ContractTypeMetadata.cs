using ProtoGenerationLib.Models.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IContractTypeMetadata"/>
    public class ContractTypeMetadata : DocumentableObject, IContractTypeMetadata
    {
        /// <inheritdoc/>
        public Type Type { get; set; }

        /// <inheritdoc cref="IContractTypeMetadata.Methods"/>
        public List<IMethodMetadata> Methods { get; set; }
        IEnumerable<IMethodMetadata> IContractTypeMetadata.Methods => Methods;

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="ContractTypeMetadata"/> class.
        /// </summary>
        public ContractTypeMetadata()
        {
            Methods = new List<IMethodMetadata>();
        }

        /// <summary>
        /// Create new instance of the <see cref="ContractTypeMetadata"/> class.
        /// </summary>
        /// <param name="type"><inheritdoc cref="Type" path="/node()"/></param>
        /// <param name="methods"><inheritdoc cref="Methods" path="/node()"/></param>
        public ContractTypeMetadata(Type type, IEnumerable<IMethodMetadata> methods)
        {
            Type = type;
            Methods = methods.ToList();
        }

        /// <inheritdoc cref="ContractTypeMetadata(Type, IEnumerable{IMethodMetadata})"/>
        /// <inheritdoc cref="DocumentableObject(string)" path="/param"/>
        public ContractTypeMetadata(Type type, IEnumerable<IMethodMetadata> methods, string documentation) : base(documentation)
        {
            Type = type;
            Methods = methods.ToList();
        }

        /// <summary>
        /// Create new instance of the <see cref="ContractTypeMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public ContractTypeMetadata(IContractTypeMetadata other) : base(other)
        {
            Type = other.Type;
            Methods = other.Methods.Select(method => new MethodMetadata(method)).Cast<IMethodMetadata>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as ContractTypeMetadata;
            return other != null
                   && base.Equals(other)
                   && Type.Equals(other.Type)
                   && Methods.SequenceEqual(other.Methods);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (base.GetHashCode(),
                    Type,
                    Methods.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
