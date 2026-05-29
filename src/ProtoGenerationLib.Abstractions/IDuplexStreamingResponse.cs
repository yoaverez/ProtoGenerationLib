using ProtoGenerationLib.Abstractions.Streams;

namespace ProtoGenerationLib.Abstractions
{
    /// <summary>
    /// Contracts representing the response that a client gets from a server when
    /// the client calls a "Duplex/Bidirectional Streaming" method.
    /// </summary>
    /// <typeparam name="TRequest">The type of the requests that the client sends.</typeparam>
    /// <typeparam name="TResponse">The type of the response that the server returns.</typeparam>
    public interface IDuplexStreamingResponse<TRequest, TResponse>
    {
        /// <summary>
        /// The stream to which the client writes its requests.
        /// </summary>
        IClientStreamWriter<TRequest> RequestsStream { get; }

        /// <summary>
        /// The stream from which to read the server responses.
        /// </summary>
        IStreamReader<TResponse> ResponseStream { get; }
    }
}
