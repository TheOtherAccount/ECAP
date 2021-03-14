using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class TcpTimeClient
{
    public static async Task Main(string[] args)
    {
        var client = new TcpClient();

        try
        {
            Console.WriteLine($"Connecting to the server..");

            await client.ConnectAsync("localhost", 6060);
        }
        catch
        {
            int retryPeriod = 9;

            Console.Write($"Couldn't connect to the server. Trying again in {retryPeriod} second(s)");

            for (var i = retryPeriod; i > 0; i--)
            {
                Thread.Sleep(1000);

                Console.Write("\b \b");
                Console.Write(i);
            }
        }

        //NetworkStream ns = client.GetStream();

        //while (true)
        //{
        //    if (ns.DataAvailable)
        //    {
        //        byte[] bytes = new byte[1024];
        //        int bytesRead = ns.Read(bytes, 0, bytes.Length);

        //        Console.Write(Encoding.ASCII.GetString(bytes, 0, bytesRead));
        //    }
        //}
    }
}