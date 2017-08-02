// Ref: https://github.com/dotnet/coreclr

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Represents an abstract scheduler for tasks.
    /// </summary>
    [DebuggerDisplay("Id={Id}")]
    public abstract class TaskScheduler
    {
        #region User Provided Methods and Properties

        /// <summary>
        /// Queues a <see cref="Task"/> to the scheduler.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to be queued.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="task"/> argument is null.</exception>
        protected abstract void QueueTask(Task task);

        /// <summary>
        /// Determines whether the provided <see cref="Task"/> can be executed
        /// synchronously in this call, and if it can, executes it.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> to be executed.</param>
        /// <param name="taskWasPreviouslyQueued">A Boolean denoting whether or not task has previously been
        /// queued. If this parameter is True, then the task may have been previously queued (scheduled); if
        /// False, then the task is known not to have been queued, and this call is being made in order to
        /// execute the task inline without queueing it.</param>
        /// <returns>A Boolean value indicating whether the task was executed inline.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="task"/> argument is null.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="task"/> was already executed.</exception>
        protected abstract bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued);

        /// <summary>
        /// Generates an enumerable of <see cref="Task"/> instances
        /// currently queued to the scheduler waiting to be executed.
        /// </summary>
        /// <returns>An enumerable that allows traversal of tasks currently queued to this scheduler.</returns>
        /// <exception cref="NotSupportedException">
        /// This scheduler is unable to generate a list of queued tasks at this time.
        /// </exception>
        protected abstract IEnumerable<Task> GetScheduledTasks();

        /// <summary>
        /// Indicates the maximum concurrency level this
        /// <see cref="TaskScheduler"/>  is able to support.
        /// </summary>
        public virtual int MaximumConcurrencyLevel
            => int.MaxValue;

        #endregion

        #region Member variables

#if DEBUG
        // The global container that keeps track of TaskScheduler instances for debugging purposes.
        private static List<TaskScheduler> s_activeTaskSchedulers;
#endif

        // An AppDomain-wide default manager.
        private static readonly TaskScheduler s_defaultTaskScheduler = new ThreadPoolTaskScheduler();

        //static counter used to generate unique TaskScheduler IDs
        internal static int s_taskSchedulerIdCounter;

        // this TaskScheduler's unique ID
        private volatile int m_taskSchedulerId;

        #endregion

        #region Constructors and public properties

        /// <summary>
        /// Initializes the <see cref="TaskScheduler"/>.
        /// </summary>
        protected TaskScheduler()
        {
#if DEBUG
            // Register the scheduler in the active scheduler list.  This is only relevant when debugging, 
            // so we only pay the cost if the debugger is attached when the scheduler is created.  This
            // means that the internal TaskScheduler.GetTaskSchedulersForDebugger() will only include
            // schedulers created while the debugger is attached.
            if (Debugger.IsAttached)
            {
                AddToActiveTaskSchedulers();
            }
#endif
        }

#if DEBUG
        /// <summary>
        /// Adds this scheduler ot the active schedulers tracking collection for debugging purposes.
        /// </summary>
        private void AddToActiveTaskSchedulers()
        {
            List<TaskScheduler> activeTaskSchedulers = s_activeTaskSchedulers;
            if (activeTaskSchedulers == null)
            {
                Interlocked.CompareExchange(ref s_activeTaskSchedulers, new List<TaskScheduler>(), null);
                activeTaskSchedulers = s_activeTaskSchedulers;
            }
            activeTaskSchedulers.Add(this);
        }
#endif

        /// <summary>
        /// Gets the default <see cref="TaskScheduler"/> instance.
        /// </summary>
        public static TaskScheduler Default
            => s_defaultTaskScheduler;

        ///// <summary>
        ///// Gets the <see cref="TaskScheduler"/> associated with the currently
        ///// executing task.
        ///// </summary>
        ///// <remarks>
        ///// When not called from within a task, <see cref="Current"/> will return the <see cref="Default"/> scheduler.
        ///// </remarks>
        //public static TaskScheduler Current
        //{
        //    get
        //    {
        //        TaskScheduler current = InternalCurrent;
        //        return current ?? Default;
        //    }
        //}

        ///// <summary>
        ///// Gets the <see cref="TaskScheduler"/> associated with the currently
        ///// executing task.
        ///// </summary>
        ///// <remarks>
        ///// When not called from within a task, <see cref="InternalCurrent"/> will return null.
        ///// </remarks>
        //internal static TaskScheduler InternalCurrent
        //{
        //    get
        //    {
        //        Task currentTask = Task.InternalCurrent;
        //        return ((currentTask != null)
        //            && ((currentTask.CreationOptions & TaskCreationOptions.HideScheduler) == 0)
        //            ) ? currentTask.ExecutingTaskScheduler : null;
        //    }
        //}

        /// <summary>
        /// Creates a <see cref="TaskScheduler"/> associated with the current
        /// <see cref="SynchronizationContext"/>.
        /// </summary>
        public static TaskScheduler FromCurrentSynchronizationContext()
            => new SynchronizationContextTaskScheduler();

        /// <summary>
        /// Gets the unique ID for this <see cref="TaskScheduler"/>.
        /// </summary>
        public int Id
        {
            get
            {
                if (m_taskSchedulerId == 0)
                {
                    int newId = 0;

                    // We need to repeat if Interlocked.Increment wraps around and returns 0.
                    // Otherwise next time this scheduler's Id is queried it will get a new value
                    do
                    {
                        newId = Interlocked.Increment(ref s_taskSchedulerIdCounter);
                    } while (newId == 0);

                    Interlocked.CompareExchange(ref m_taskSchedulerId, newId, 0);
                }

                return m_taskSchedulerId;
            }
        }

        protected bool TryExecuteTask(Task task)
        {
            if (task.ExecutingTaskScheduler != this)
                throw new InvalidOperationException("The specified Task is not linked to current TaskScheduler");

            return task.ExecuteEntry();
        }

        #endregion
    }
}
