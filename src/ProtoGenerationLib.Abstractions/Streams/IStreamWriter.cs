using System.Threading.Tasks;

namespace ProtoGenerationLib.Abstractions.Streams
{
    /// <summary>
    /// Contract for writing objects of type <typeparamref name="T"/> to a stream.
    /// </summary>
    /// <typeparam name="T">The type of the objects that passes in the stream.</typeparam>
    public interface IStreamWriter<in T>
    {
        /// <summary>
        /// Write the given <paramref name="value"/> to the stream.
        /// </summary>
        /// <param name="value">The object to write to the stream.</param>
        /// <returns>
        /// A task that is completed when the given <paramref name="value"/> was written to the stream.
        /// </returns>
        Task WriteAsync(T value);
    }
}
