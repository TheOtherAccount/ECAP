using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ECAPClient
{
    public ConnectionInfo ConnectionInfo { get; set; }

    private static TcpClient tcpClient;

    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    public event EventHandler Connecting;
    public event EventHandler ConnectionRefused;
    public event EventHandler ConnectionEstablished;
    public event MessageEventHandler MessageReceived;
    public event EventHandler ConnectionLost;

    public ECAPClient(ConnectionInfo connectionInfo)
    {
        ConnectionInfo = connectionInfo;
    }

    public async Task Start()
    {
        if (tcpClient == null)
        {
            tcpClient = new TcpClient();
        }

        await Connect();
    }

    private async Task Connect()
    {
        try
        {
            OnConnecting(EventArgs.Empty);

            await tcpClient.ConnectAsync(ConnectionInfo.HostName, ConnectionInfo.PortNumber);

            await OnConnectionEstablished(EventArgs.Empty);
        }
        catch
        {
            await OnConnectionRefused(EventArgs.Empty);
        }
    }

    private async Task WaitMessages()
    {
        NetworkStream ns = tcpClient.GetStream();

        while (true)
        {
            var message = new byte[1];

            var receivedByteCount = await ns.ReadAsync(message, 0, message.Length);

            if (receivedByteCount > 0)
            {
                string theMessage = Encoding.ASCII.GetString(message, 0, receivedByteCount);

                OnMessageReceived(new MessageEventArgs(theMessage));
            }
            else
            {
                tcpClient.Close();
                tcpClient = new TcpClient();

                await OnConnectionLost(EventArgs.Empty);
            }
        }
    }

    protected virtual async Task OnConnectionLost(EventArgs args)
    {
        if (ConnectionLost != null)
        {
            ConnectionLost(this, args);
        }

        await Start();
    }

    protected virtual async Task OnConnectionEstablished(EventArgs args)
    {
        if (ConnectionEstablished != null)
        {
            ConnectionEstablished(this, args);
        }

        await WaitMessages();
    }

    protected virtual void OnMessageReceived(MessageEventArgs args)
    {
        if (MessageReceived != null)
        {
            MessageReceived(this, args);
        }
    }

    protected virtual void OnConnecting(EventArgs args)
    {
        if (Connecting != null)
        {
            Connecting(this, args);
        }
    }

    protected virtual async Task OnConnectionRefused(EventArgs args)
    {
        if (ConnectionRefused != null)
        {
            ConnectionRefused(this, args);
        }

        await Task.Delay(ConnectionInfo.RetryDuration);

        await Connect();
    }
}