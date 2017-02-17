// Ref: https://github.com/jam40jeff/ITask

using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Provides an awaitable object that allows awaits.
    /// </summary>
    public interface IAwaitable
    {
        /// <summary>Creates an awaiter used to await this <see cref="IAwaitable"/>.</summary>
        /// <returns>An awaiter instance.</returns>
        IAwaiter GetAwaiter();
    }
}
