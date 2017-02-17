// Ref: https://github.com/jam40jeff/ITask

namespace System.Threading.Tasks
{
    /// <summary>
    /// An interface representing a Task which returns a value of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the result of the task.
    /// </typeparam>
    public interface ITask<out TResult> : ITask, IAwaitable<TResult>
    {
        /// <summary>
        /// Gets the result value of this <see cref="ITask{TResult}"/>.
        /// </summary>
        /// <remarks>
        /// The get accessor for this property ensures that the asynchronous operation is complete before
        /// returning. Once the result of the computation is available, it is stored and will be returned
        /// immediately on later calls to <see cref="Result"/>.
        /// </remarks>
        TResult Result { get; }

        /// <summary>Configures an awaiter used to await this <see cref="ITask"/>.</summary>
        /// <param name="continueOnCapturedContext">
        /// true to attempt to marshal the continuation back to the original context captured; otherwise, false.
        /// </param>
        /// <returns>An object used to await this task.</returns>
        new IAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext);
    }
}
