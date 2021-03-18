using ClientBCL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientBCLTest
{
    [TestClass]
    public class ECAPClientTest
    {
        [TestMethod]
        public async Task TestNoServer()
        {
            ECAPClient client = new ECAPClient(new ConnectionInfo() { }) { RetryWhenFails = false };

            bool failed = false;

            client.ConnectionRefused += (sender, e) => { failed = true; };

            await client.Connect();

            Assert.IsTrue(failed);
        }
    }
}