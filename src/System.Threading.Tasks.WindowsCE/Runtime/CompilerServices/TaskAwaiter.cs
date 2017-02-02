using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    public struct TaskAwaiter : INotifyCompletion
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
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            // TODO: TaskScheduler?
            _task.ContinueWith(t => continuation());
        }

        public void GetResult()
        {
            IAsyncResult task = _task;
            if (!task.AsyncWaitHandle.WaitOne())
                throw new InvalidOperationException("Error waiting for wait handle signal");

            if (_task.Status == TaskStatus.RanToCompletion)
                return;

            // TODO: Handle cancellation
            ExceptionDispatchInfo.Capture(_task.Exception).Throw();
        }
    }
}
