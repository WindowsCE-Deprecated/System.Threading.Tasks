using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class TaskCancellation
    {
        [TestMethod]
        public void TaskCancellation_Simple()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var task = Task.Factory.StartNew(() =>
            {
                // Were we already canceled?
                ct.ThrowIfCancellationRequested();

                bool moreToDo = true;
                while (moreToDo)
                {
                    // Poll on this property if you have to do
                    // other cleanup before throwing.
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }

                }
            }, tokenSource2.Token); // Pass same token to StartNew.

            tokenSource2.Cancel();

            // Just continue on this thread, or Wait/WaitAll with try-catch:
            try
            {
                task.Wait();
                Assert.Fail("Should throw an Exception");
            }
            catch (AggregateException) { }
            finally
            {
                tokenSource2.Dispose();
            }
        }

        [TestMethod]
        public void TaskCancellation_AsyncStateMachine()
            => Internal_TaskCancellation_AsyncStateMachine().Wait();

        private async Task Internal_TaskCancellation_AsyncStateMachine()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var task = Task.Factory.StartNew(() =>
            {
                // Were we already canceled?
                ct.ThrowIfCancellationRequested();

                bool moreToDo = true;
                while (moreToDo)
                {
                    // Poll on this property if you have to do
                    // other cleanup before throwing.
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }

                }
            }, tokenSource2.Token); // Pass same token to StartNew.

            tokenSource2.Cancel();

            try
            {
                await task.ConfigureAwait(false);
                Assert.Fail("Should be canceled");
            }
            catch (Mock.System.OperationCanceledException ex)
            {
                Assert.AreEqual(ct, ex.CancellationToken);
            }
            finally
            {
                tokenSource2.Dispose();
            }
        }

        [TestMethod]
        public void TaskCancellation_CancelBeforeStart()
        {
            using (var src = new CancellationTokenSource())
            {
                using (var t = new Task(() => { }, src.Token))
                {
                    src.Cancel();
                    Assert.AreEqual(TaskStatus.Canceled, t.Status, "#1");

                    try
                    {
                        t.Start();
                        Assert.Fail("#2");
                    }
                    catch (InvalidOperationException ex)
                    {
                        GC.KeepAlive(ex);
                    }
                }
            }
        }

        // TODO: Create cancelable continuation Task
        //[TestMethod]
        //public void TaskCancellation_CanceledContinuationExecuteSynchronouslyTest()
        //{
        //    using (var source = new CancellationTokenSource())
        //    {
        //        using (var evt = new ManualResetEvent(false))
        //        {
        //            var token = source.Token;
        //            var result = false;
        //            var thrown = false;

        //            var task = Task.Factory.StartNew(() => evt.WaitOne(100));
        //            var cont = task.ContinueWith(t => result = true, token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

        //            source.Cancel();
        //            evt.Set();
        //            task.Wait(100);
        //            try
        //            {
        //                cont.Wait(100);
        //            }
        //            catch (Exception ex)
        //            {
        //                GC.KeepAlive(ex);
        //                thrown = true;
        //            }

        //            Assert.IsTrue(task.IsCompleted);
        //            Assert.IsTrue(cont.IsCanceled);
        //            Assert.IsFalse(result);
        //            Assert.IsTrue(thrown);
        //        }
        //    }
        //}
    }
}
