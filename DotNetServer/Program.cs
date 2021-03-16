using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotNetServer
{
    class Program
    {
        private static List<TcpClient> ConnectedClients = new List<TcpClient>();

        private static Queue<char> Messages = new Queue<char>();

        private static async void Listen()
        {
            listener = new TcpListener(IPAddress.Any, 6060);
            listener.Start();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                ConnectedClients.Add(client);
            }
        }

        private static TcpListener listener;

        private static bool IsSending { get; set; }

        static char? GetNextMessage()
        {
            return Messages.Count > 0 ? Messages.Dequeue() : null;
        }

        static async Task Send()
        {
            IsSending = true;

            char? message = GetNextMessage();

            while (message != null)
            {
                foreach (var client in ConnectedClients)
                {
                    NetworkStream ns = client.GetStream();

                    byte[] messageToSend = Encoding.Unicode.GetBytes(message.ToString());

                    try
                    {
                        await ns.WriteAsync(messageToSend, 0, messageToSend.Length);
                    }
                    catch
                    {
                        
                    }
                }

                message = GetNextMessage();
            }

            IsSending = false;
        }

        static async Task Main(string[] args)
        {
            await Task.Run(Listen);

            Console.WriteLine("Please start typing..");

            while (true)
            {
                var pressedKey = Console.ReadKey(true);

                if (!char.IsControl(pressedKey.KeyChar))
                {
                    Console.Write(pressedKey.KeyChar);

                    Messages.Enqueue(pressedKey.KeyChar);

                    if (!IsSending)
                    {
                        await Send();
                    }
                }
            }
        }
    }
}