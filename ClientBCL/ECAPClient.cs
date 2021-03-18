using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientBCL
{
    /// <summary>
    /// Connects to a remote server via TCP/IP and receives messages.
    /// </summary>
    public class ECAPClient
    {
        /// <summary>
        /// When set to true (the default value).. it tries connecting again when the connection is refused or lost.
        /// </summary>
        public bool RetryWhenFails { get; set; } = true;
        /// <summary>
        /// The main TcpClient object used for the communication.
        /// </summary>
        private static TcpClient tcpClient;
        /// <summary>
        /// An object holding the connection information of the server that you want to connect to.
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; set; }
        /// <summary>
        /// Message event handler used mainly for the MessageReceived event.
        /// </summary>
        /// <param name="sender">The publisher of the event, typically the ecpClient object.</param>
        /// <param name="e">An EventArgs object contains the message that has been received.</param>
        public delegate void MessageEventHandler(object sender, MessageEventArgs e);
        /// <summary>
        /// An event that gets raised while connecting to the server.
        /// </summary>
        public event EventHandler Connecting;
        /// <summary>
        /// An event that gets raised when the client is unable to connect to the server.
        /// </summary>
        public event EventHandler ConnectionRefused;
        /// <summary>
        /// An event that gets raised when the connection is established with the server.
        /// </summary>
        public event EventHandler ConnectionEstablished;
        /// <summary>
        /// An event that gets raised when a message has been received.
        /// </summary>
        public event MessageEventHandler MessageReceived;
        /// <summary>
        /// An event that gets raised when an open connection gets lost.
        /// </summary>
        public event EventHandler ConnectionLost;
        /// <summary>
        /// Instaniates an object with the specified connectionInfo object.
        /// </summary>
        /// <param name="connectionInfo">An object holds the connection information required to connect to the server.</param>
        public ECAPClient(ConnectionInfo connectionInfo)
        {
            ConnectionInfo = connectionInfo;
        }
        /// <summary>
        /// Starts connecting again after a short delay (500 ms).
        /// </summary>
        private async Task StartOver()
        {
            await Task.Delay(500);

            await Start();
        }
        /// <summary>
        /// Starts connecting and waiting for the first message.
        /// </summary>
        public async Task Start()
        {
            try
            {
                OnConnecting(EventArgs.Empty);

                tcpClient = new TcpClient();

                await tcpClient.ConnectAsync(ConnectionInfo.HostName, ConnectionInfo.PortNumber);

                await OnConnectionEstablished(EventArgs.Empty);
            }
            catch
            {
                await OnConnectionRefused(EventArgs.Empty);
            }
        }
        /// <summary>
        /// Keep receiving messages until the connection is lost.
        /// </summary>
        public async Task WaitMessage()
        {
            NetworkStream ns = tcpClient.GetStream();

            var message = new byte[1];
            int receivedByteCount = 0;

            try
            {
                receivedByteCount = await ns.ReadAsync(message, 0, message.Length);
            }
            catch
            {
                await OnConnectionLost(EventArgs.Empty);
            }

            if (receivedByteCount > 0)
            {
                string theMessage = Encoding.ASCII.GetString(message, 0, receivedByteCount);

                await OnMessageReceived(new MessageEventArgs(theMessage));
            }
            else
            {
                await OnConnectionLost(EventArgs.Empty);
            }
        }
        /// <summary>
        /// Closes the underlying tcpClient, raises the appropriate event and starts over the process.
        /// </summary>
        protected virtual async Task OnConnectionLost(EventArgs args)
        {
            tcpClient.Close();

            if (ConnectionLost != null)
            {
                ConnectionLost(this, args);
            }

            if (RetryWhenFails)
            {
                await StartOver();
            }
        }
        /// <summary>
        /// Raises the event and starts receiving the messages.
        /// </summary>
        protected virtual async Task OnConnectionEstablished(EventArgs args)
        {
            if (ConnectionEstablished != null)
            {
                ConnectionEstablished(this, args);
            }

            await WaitMessage();
        }
        /// <summary>
        /// It just raises the event.
        /// </summary>
        /// <param name="args">An object contains the message that has been received.</param>
        protected virtual async Task OnMessageReceived(MessageEventArgs args)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, args);
            }

            if (args.WaitAnotherMessage)
            {
                await WaitMessage();
            }
        }
        /// <summary>
        /// It just raises the event.
        /// </summary>
        protected virtual void OnConnecting(EventArgs args)
        {
            if (Connecting != null)
            {
                Connecting(this, args);
            }
        }
        /// <summary>
        /// Raises the event and trying to connect again.
        /// </summary>
        protected virtual async Task OnConnectionRefused(EventArgs args)
        {
            if (ConnectionRefused != null)
            {
                ConnectionRefused(this, args);
            }

            if (RetryWhenFails)
            {
                await StartOver();
            }
        }
    }
}