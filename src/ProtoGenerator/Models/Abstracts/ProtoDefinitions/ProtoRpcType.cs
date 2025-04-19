namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Proto RPC method types.
    /// </summary>
    public enum ProtoRpcType
    {
        /// <summary>
        /// The RPC method gets a single request and return a single response.
        /// </summary>
        Unary,

        /// <summary>
        /// The RPC method gets a single request and return a stream of responses.
        /// The service write responses to the stream and
        /// the client can read those responses from the return stream.
        /// </summary>
        ServerStreaming,

        /// <summary>
        /// The RPC method gets a stream of requests and return a single response.
        /// The client write requests to the stream and
        /// the service read those requests from the stream and return a single response.
        /// </summary>
        ClientStreaming,

        /// <summary>
        /// The RPC method gets a stream of requests and return a stream for responses.
        /// The client write to request to the stream and
        /// the service write the responses for each request to the stream.
        /// </summary>
        BidirectionalStreaming,
    }
}
