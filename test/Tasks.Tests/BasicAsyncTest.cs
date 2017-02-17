using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class BasicAsyncTest
    {
        private static async Task<string> GetData()
        {
            var data = await SlowOperation();
            return data.ToString(CultureInfo.InvariantCulture);
        }

        private static Task<int> SlowOperation()
        {
            Thread.Sleep(1);
            return Task.FromResult(7);
        }

        [TestMethod]
        public void SimpleTest()
        {
            Assert.AreEqual("7", GetData().Result);
        }
    }
}
