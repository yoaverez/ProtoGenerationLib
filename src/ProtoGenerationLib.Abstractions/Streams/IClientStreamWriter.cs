using System.Threading.Tasks;

namespace ProtoGenerationLib.Abstractions.Streams
{
    /// <summary>
    /// Contract for clients that writes to a server using a stream.
    /// </summary>
    /// <typeparam name="T">The type of the objects that passes in the stream.</typeparam>
    public interface IClientStreamWriter<in T> : IStreamWriter<T>
    {
        /// <summary>
        /// Signals that the client finished writing to the stream.
        /// </summary>
        /// <returns>A task that will be completed when the stream finished closing.</returns>
        Task CompleteAsync();
    }
}
