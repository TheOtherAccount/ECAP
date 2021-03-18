using ClientBCL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ClientBCLTest
{
    /// <summary>
    /// Provides methods for testing the ConnectionInfo class
    /// </summary>
    [TestClass]
    public class ConnectionInfoTest
    {
        /// <summary>
        /// A bad host name for testing.
        /// </summary>
        private const string TestHostName = "theHostName";
        /// <summary>
        /// A port number for testing.
        /// </summary>
        private const int TestPortNumber = 1234;
        /// <summary>
        /// Tests the defualt constructor.
        /// </summary>
        [TestMethod]
        public void TestNormalConstructor()
        {
            ConnectionInfo info = new ConnectionInfo(TestHostName, TestPortNumber);

            Assert.IsTrue(info.HostName == TestHostName && info.PortNumber == TestPortNumber);
        }
        /// <summary>
        /// Tests the object instantiation based on command line arguments.
        /// </summary>
        [TestMethod]
        public void TestFromArgs()
        {
            ConnectionInfo info = ConnectionInfo.FromArgs(new string[] {
                $"{nameof(ConnectionInfo.HostName)}={TestHostName}",
                $"{nameof(ConnectionInfo.PortNumber)}={TestPortNumber}",
            });

            Assert.IsTrue(info.HostName == TestHostName && info.PortNumber == TestPortNumber);
        }
        /// <summary>
        /// Tests the object instantiation based on a dictionary.
        /// </summary>
        [TestMethod]
        public void TestDictionaryConstructor()
        {
            Dictionary<string, string> theDictionary = new Dictionary<string, string>();

            theDictionary.Add(nameof(ConnectionInfo.HostName), TestHostName);
            theDictionary.Add(nameof(ConnectionInfo.PortNumber), TestPortNumber.ToString());

            ConnectionInfo info = new ConnectionInfo(theDictionary);

            Assert.IsTrue(info.HostName == TestHostName && info.PortNumber == TestPortNumber);
        }
        /// <summary>
        /// Tests default property values when there is no values specified in the instantiation.
        /// </summary>
        [TestMethod]
        public void TestDefaultValues()
        {
            ConnectionInfo info = new ConnectionInfo();

            Assert.IsTrue(info.HostName == info.DefaultDebugHostName
                && info.PortNumber == info.DefaultPortNumber);
        }
    }
}