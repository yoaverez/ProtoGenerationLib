using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProtoGenerationLib.Abstractions.Streams
{
    /// <summary>
    /// Contract for reading from a stream of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the objects that passes in the stream.</typeparam>
    public interface IStreamReader<out T>
    {
        /// <summary>
        /// The first object in the stream.
        /// </summary>
        /// <remarks>
        /// This should be used only after calling to <see cref="MoveNextAsync(CancellationToken)"/> at least once
        /// otherwise the data will be garbage.
        /// </remarks>
        T Current { get; }

        /// <summary>
        /// Read the next object from the stream asynchronously and write it to <see cref="Current"/>.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token for canceling the operation.</param>
        /// <returns>
        /// A task that will be finished when one of the following occurs:
        /// <list type="bullet">
        /// <item>Finished reading the next object from the stream. In this case the result of the task
        /// will be <see langword="true"/>.</item>
        /// <item>The stream was completed on the writer side and there is noting more to read. Therefore
        /// the result of the task will be <see langword="false"/>.</item>
        /// </list>
        /// </returns>
        /// <exception cref="OperationCanceledException">Thrown if the given <paramref name="cancellationToken"/> was canceled.</exception>
        Task<bool> MoveNextAsync(CancellationToken cancellationToken);
    }
}
