// Ref: https://github.com/jam40jeff/ITask

using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    /// <summary>
    /// An interface representing a Task which does not return a value.
    /// </summary>
    public interface ITask
    {
        /// <summary>Creates an awaiter used to await this <see cref="ITask"/>.</summary>
        /// <returns>An awaiter instance.</returns>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        IAwaiter GetAwaiter();

        /// <summary>Configures an awaiter used to await this <see cref="ITask"/>.</summary>
        /// <param name="continueOnCapturedContext">
        /// true to attempt to marshal the continuation back to the original context captured; otherwise, false.
        /// </param>
        /// <returns>An object used to await this task.</returns>
        IConfiguredTask ConfigureAwait(bool continueOnCapturedContext);
    }
}
