﻿using System;
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
        await Connect();
    }

    private async Task Connect()
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

    private async Task WaitMessages()
    {
        NetworkStream ns = tcpClient.GetStream();

        while (true)
        {
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

                OnMessageReceived(new MessageEventArgs(theMessage));
            }
            else
            {
                await OnConnectionLost(EventArgs.Empty);
            }
        }
    }

    protected virtual async Task OnConnectionLost(EventArgs args)
    {
        tcpClient.Close();

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

        await Task.Delay(500);

        await Connect();
    }
}