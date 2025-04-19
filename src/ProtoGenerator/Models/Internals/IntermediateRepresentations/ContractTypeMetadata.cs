using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IContractTypeMetadata"/>
    public class ContractTypeMetadata : IContractTypeMetadata
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
        /// Create new instance of the <see cref="ContractTypeMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public ContractTypeMetadata(IContractTypeMetadata other)
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
                   && Type.Equals(other.Type)
                   && Methods.SequenceEqual(other.Methods);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Type,
                    Methods.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
