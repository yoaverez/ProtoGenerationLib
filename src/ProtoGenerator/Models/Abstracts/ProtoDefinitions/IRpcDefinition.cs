namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents RPC in a Protocol Buffer service.
    /// </summary>
    public interface IRpcDefinition
    {
        /// <summary>
        /// The name of the RPC method.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The type of the RPC method response.
        /// </summary>
        string ResponseType { get; }

        /// <summary>
        /// The type of the RPC method request.
        /// </summary>
        string RequestType { get; }

        /// <summary>
        /// The type of the RPC method.
        /// </summary>
        ProtoRpcType RpcType { get; }
    }
}
