using ClientBCL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientBCLTest
{
    [TestClass]
    public class ECAPClientTest
    {
        [TestMethod]
        public async Task TestWithNoServer()
        {
            ECAPClient client = new ECAPClient(new ConnectionInfo() { HostName = Guid.NewGuid().ToString() });
            client.RetryWhenFails = false;

            bool failed = false;

            client.ConnectionRefused += (sender, e) => failed = true;

            await client.Start();

            Assert.IsTrue(failed);
        }

        [TestMethod]
        public async Task TestReceivingAMessage()
        {
            string testMessage = "E";

            var serverTask = Task.Run(async () =>
            {
                await TCPSend(testMessage);
            });

            ECAPClient client = new ECAPClient(new ConnectionInfo()) { RetryWhenFails = false };

            string messageReceived = null;

            client.MessageReceived += (sender, e) => { messageReceived = e.Message; e.WaitAnotherMessage = false; };

            await client.Start();

            Assert.IsTrue(messageReceived == testMessage);
        }

        private async Task TCPSend(string message)
        {
            var listener = new TcpListener(IPAddress.Any, 6060);
            listener.Start();

            var client = await listener.AcceptTcpClientAsync();

            NetworkStream ns = client.GetStream();

            byte[] messageToSend = Encoding.ASCII.GetBytes(message);

            await ns.WriteAsync(messageToSend, 0, messageToSend.Length);

            ns.Close();

            listener.Stop();
        }
    }
}