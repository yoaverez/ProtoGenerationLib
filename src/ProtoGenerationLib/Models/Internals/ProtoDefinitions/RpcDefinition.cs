using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;

namespace ProtoGenerationLib.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IRpcDefinition"/>
    public class RpcDefinition : IRpcDefinition
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string ResponseType { get; set; }

        /// <inheritdoc/>
        public string RequestType { get; set; }

        /// <inheritdoc/>
        public ProtoRpcType RpcType { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instances of the <see cref="RpcDefinition"/> class.
        /// </summary>
        public RpcDefinition()
        {
            Name = string.Empty;
            ResponseType = string.Empty;
            RequestType = string.Empty;
            RpcType = ProtoRpcType.Unary;
        }

        /// <summary>
        /// Create new instances of the <see cref="RpcDefinition"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name" path="/node()"/></param>
        /// <param name="responseType"><inheritdoc cref="ResponseType" path="/node()"/></param>
        /// <param name="requestType"><inheritdoc cref="RequestType" path="/node()"/></param>
        /// <param name="rpcType"><inheritdoc cref="RpcType" path="/node()"/></param>
        public RpcDefinition(string name, string responseType, string requestType, ProtoRpcType rpcType)
        {
            Name = name;
            ResponseType = responseType;
            RequestType = requestType;
            RpcType = rpcType;
        }

        /// <summary>
        /// Create new instance of the <see cref="RpcDefinition"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public RpcDefinition(IRpcDefinition other)
        {
            Name = other.Name;
            ResponseType = other.ResponseType;
            RequestType = other.RequestType;
            RpcType = other.RpcType;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as RpcDefinition;
            return other != null
                   && Name.Equals(other.Name)
                   && ResponseType.Equals(other.ResponseType)
                   && RequestType.Equals(other.RequestType)
                   && RpcType.Equals(other.RpcType);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Name,
                    ResponseType,
                    RequestType,
                    RpcType).GetHashCode();
        }

        #endregion Object Overrides
    }
}
