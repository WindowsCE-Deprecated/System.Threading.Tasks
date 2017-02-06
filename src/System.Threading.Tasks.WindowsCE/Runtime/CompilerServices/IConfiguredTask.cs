// Ref: https://github.com/jam40jeff/ITask

using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    /// <summary>Provides an awaitable object that allows for configured awaits on <see cref="ITask"/>.</summary>
    /// <remarks>This type is intended for compiler use only.</remarks>
    public interface IConfiguredTask
    {
        /// <summary>Creates an awaiter used to await this <see cref="IConfiguredTask"/>.</summary>
        /// <returns>An awaiter instance.</returns>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        IAwaiter GetAwaiter();
    }
}
