// Ref: https://github.com/jam40jeff/ITask

using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Provides an awaitable object that allows for configured awaits.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the task.</typeparam>
    public interface IAwaitable<out TResult>
    {
        /// <summary>
        /// Creates an awaiter used to await this <see cref="IAwaitable{TResult}"/>.
        /// </summary>
        /// <returns>An awaiter instance.</returns>
        IAwaiter<TResult> GetAwaiter();
    }
}
