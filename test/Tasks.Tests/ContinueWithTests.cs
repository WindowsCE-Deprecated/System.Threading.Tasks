using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class ContinueWithTests
    {
        [TestMethod]
        public void ContinueWithInvalidArguments()
        {
            var task = new Task(() => { });
            try
            {
                task.ContinueWith(null);
                Assert.Fail("#1");
            }
            catch (ArgumentNullException) { }

            //try
            //{
            //    task.ContinueWith(delegate { }, null);
            //    Assert.Fail("#2");
            //}
            //catch (ArgumentNullException) { }
        }
    }
}
