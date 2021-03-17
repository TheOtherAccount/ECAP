using ClientBCL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ClientBCLTest
{
    [TestClass]
    public class ConnectionInfoTest
    {
        private const string TestHostName = "theHostName";
        private const int TestPortNumber = 1234;

        [TestMethod]
        public void TestNormalConstructor()
        {
            string testHostName = "theHostName";
            int testPortNumber = 1234;

            ConnectionInfo info = new ConnectionInfo(testHostName, testPortNumber);

            Assert.IsTrue(info.HostName == testHostName && info.PortNumber == testPortNumber);
        }

        [TestMethod]
        public void TestFromArgs()
        {
            ConnectionInfo info = ConnectionInfo.FromArgs(new string[] {
                $"hostName={TestHostName}",
                $"portNumber={TestPortNumber}",
            });

            Assert.IsTrue(info.HostName == TestHostName && info.PortNumber == TestPortNumber);
        }

        [TestMethod]
        public void TestDictionaryConstructor()
        {
            Dictionary<string, string> theDictionary = new Dictionary<string, string>();

            theDictionary.Add("hostName", TestHostName);
            theDictionary.Add("portNumber", TestPortNumber.ToString());

            ConnectionInfo info = new ConnectionInfo(theDictionary);

            Assert.IsTrue(info.HostName == TestHostName && info.PortNumber == TestPortNumber);
        }

        [TestMethod]
        public void TestDefaultValues()
        {
            ConnectionInfo info = new ConnectionInfo();

            Assert.IsTrue(info.HostName == info.DefaultDebugHostName && info.PortNumber == info.DefaultPortNumber);
        }
    }
}