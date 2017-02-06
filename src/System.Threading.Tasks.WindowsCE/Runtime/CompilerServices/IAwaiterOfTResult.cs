// Ref: https://github.com/jam40jeff/ITask

using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    /// <summary>Awaiter interface for awaiting an <see cref="ITask{TResult}"/>.</summary>
    /// <typeparam name="TResult">
    /// The type of the result of the task.
    /// </typeparam>
    /// <remarks>This type is intended for compiler use only.</remarks>
    public interface IAwaiter<out TResult> : INotifyCompletion
    {
        /// <summary>Gets a value indicating whether the task being awaited is completed.</summary>
        /// <remarks>This property is intended for compiler user rather than use directly in code.</remarks>
        /// <exception cref="System.NullReferenceException">The awaiter was not properly initialized.</exception>
        bool IsCompleted { get; }

        /// <summary>Ends the await on the completed <see cref="ITask{TResult}"/>.</summary>
        /// <returns>The result of the completed <see cref="ITask{TResult}"/>.</returns>
        /// <exception cref="NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <exception cref="Exception">The task completed in a Faulted state.</exception>
        // <exception cref="System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
        TResult GetResult();
    }
}
