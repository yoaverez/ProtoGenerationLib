using ProtoGenerationLib.Abstractions.Streams;
using System.Threading.Tasks;

namespace ProtoGenerationLib.Abstractions
{
    /// <summary>
    /// Contracts representing the response that a client gets from a server when
    /// the client calls a "ClientStreaming" method.
    /// </summary>
    /// <typeparam name="TRequest">The type of the requests that the client sends.</typeparam>
    /// <typeparam name="TResponse">The type of the response that the server returns.</typeparam>
    public interface IClientStreamingResponse<TRequest, TResponse>
    {
        /// <summary>
        /// The stream to which the client writes its requests.
        /// </summary>
        IClientStreamWriter<TRequest> RequestsStream { get; }

        /// <summary>
        /// The response that the client gets from the server.
        /// </summary>
        /// <remarks>The task will be completed when the server response reached the client.</remarks>
        Task<TResponse> Response { get; }
    }
}
