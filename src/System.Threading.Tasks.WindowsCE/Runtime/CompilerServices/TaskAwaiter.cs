using System.Threading;
using System.Threading.Tasks;

#if NET35_CF
using System.Runtime.ExceptionServices;
#else
using Mock.System.Runtime.ExceptionServices;
#endif

namespace System.Runtime.CompilerServices
{
    public struct TaskAwaiter : IAwaiter, INotifyCompletion
    {
        readonly Task _task;

        public bool IsCompleted
            => _task.IsCompleted;

        internal TaskAwaiter(Task task)
        {
            _task = task;
        }

        public void OnCompleted(Action continuation)
        {
            OnCompletedInternal(_task, continuation, true);
        }

        public void GetResult()
        {
            ValidateEnd(_task);
        }

        internal static void ValidateEnd(Task task)
        {
            if (task.Status == TaskStatus.RanToCompletion)
                return;

            IAsyncResult asyncResult = task;
            if (!asyncResult.AsyncWaitHandle.WaitOne())
                throw new InvalidOperationException("Error waiting for wait handle signal");

            if (task.Status == TaskStatus.RanToCompletion)
                return;

            // TODO: Handle cancellation
            var aggEx = task.Exception;
            if (aggEx.InnerExceptions.Count > 0)
            {
                ExceptionDispatchInfo.Capture(aggEx.InnerExceptions[0]).Throw();
            }
            else
            {
                throw aggEx;
            }
        }

        internal static void OnCompletedInternal(Task task, Action continuation, bool continueOnCapturedContext)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            var syncContext = continueOnCapturedContext ? SynchronizationContext.Current : null;
            if (syncContext != null && syncContext.GetType() != typeof(SynchronizationContext))
            {
                task.ContinueWith(t =>
                {
                    syncContext.Post(state => ((Action)state)(), continuation);
                });
            }
            // TODO: TaskScheduler?
            else
            {
                task.ContinueWith((t, state) => ((Action)state)(), continuation);
            }
        }
    }
}
