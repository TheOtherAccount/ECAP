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
        public async Task TestNormalConstructor()
        {
            ECAPClient client = new ECAPClient(new ConnectionInfo());

            bool failed = false;

            client.ConnectionRefused += (sender, e) => { failed = false; };

            await client.Connect();

            Assert.IsTrue(failed);
        }
    }
}