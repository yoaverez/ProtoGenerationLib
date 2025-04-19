using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Utilities.CollectionUtilities;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IServiceDefinition"/>
    public class ServiceDefinition : ProtoObject, IServiceDefinition
    {
        /// <inheritdoc cref="IServiceDefinition.RpcMethods"/>
        public List<IRpcDefinition> RpcMethods { get; set; }
        IEnumerable<IRpcDefinition> IServiceDefinition.RpcMethods => RpcMethods.AsReadOnly();

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="ServiceDefinition"/> class.
        /// </summary>
        public ServiceDefinition() : base()
        {
            RpcMethods = new List<IRpcDefinition>();
        }

        /// <summary>
        /// Create new instance of the <see cref="ServiceDefinition"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public ServiceDefinition(IServiceDefinition other) : base(other)
        {
            RpcMethods = other.RpcMethods.Select(x => new RpcDefinition(x)).Cast<IRpcDefinition>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as ServiceDefinition;
            return other != null
                   && base.Equals(other)
                   && RpcMethods.SequenceEqual(other.RpcMethods);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (base.GetHashCode(),
                    RpcMethods.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
