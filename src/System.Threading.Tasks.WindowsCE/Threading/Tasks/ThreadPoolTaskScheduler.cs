namespace System.Threading.Tasks
{
    public class ThreadPoolTaskScheduler : TaskScheduler
    {
        private static readonly WaitCallback _executeCallback = TaskExecuteCallback;

        private static readonly ThreadStart _longRunningThreadWork = LongRunningThreadWork;

        protected override void QueueTask(Task task)
        {
            ThreadPool.QueueUserWorkItem(_executeCallback, task);
        }

        private static void TaskExecuteCallback(object state)
        {
            var task = state as Task;
            if (task != null)
                task.ExecuteEntry(true);
        }
    }
}