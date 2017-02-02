using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    public struct TaskAwaiter<TResult> : INotifyCompletion
    {
        readonly Task<TResult> _task;

        public bool IsCompleted
            => _task.IsCompleted;

        internal TaskAwaiter(Task<TResult> task)
        {
            _task = task;
        }

        public void OnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            // TODO: TaskScheduler?
            _task.ContinueWith(t => continuation());
        }

        public TResult GetResult()
        {
            IAsyncResult task = _task;
            if (!task.AsyncWaitHandle.WaitOne())
                throw new InvalidOperationException("Error waiting for wait handle signal");

            if (_task.Status == TaskStatus.RanToCompletion)
                return _task.Result;

            // TODO: Handle cancellation
            ExceptionDispatchInfo.Capture(_task.Exception).Throw();
            return default(TResult);
        }
    }
}
