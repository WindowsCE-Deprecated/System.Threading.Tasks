// Ref: https://github.com/jam40jeff/ITask

namespace System.Threading.Tasks
{
    /// <summary>
    /// Class providing task extension methods.
    /// </summary>
    public static class TaskExtensionMethods
    {
        /// <summary>
        /// Converts the <see cref="Task"/> instance into an <see cref="ITask"/>.
        /// </summary>
        /// <param name="task">
        /// The task to convert.
        /// </param>
        /// <returns>
        /// The <see cref="ITask"/>.
        /// </returns>
        public static ITask AsITask(this Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            return task;
        }

        /// <summary>
        /// Converts the <see cref="Task{TResult}"/> instance into an <see cref="ITask{TResult}"/>.
        /// </summary>
        /// <param name="task">
        /// The task to convert.
        /// </param>
        /// <typeparam name="TResult">
        /// The type of the result of the task.
        /// </typeparam>
        /// <returns>
        /// The <see cref="ITask{TResult}"/>.
        /// </returns>
        public static ITask<TResult> AsITask<TResult>(this Task<TResult> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            return task;
        }

        /// <summary>
        /// Converts the <see cref="ITask"/> instance into a <see cref="Task"/>.
        /// </summary>
        /// <param name="task">
        /// The task to convert.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task AsTask(this ITask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Converts the <see cref="ITask{TResult}"/> instance into a <see cref="Task{TResult}"/>.
        /// </summary>
        /// <param name="task">
        /// The task to convert.
        /// </param>
        /// <typeparam name="TResult">
        /// The type of the result of the task.
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task{TResult}"/>.
        /// </returns>
        public static async Task<TResult> AsTask<TResult>(this ITask<TResult> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            return await task.ConfigureAwait(false);
        }
    }
}
