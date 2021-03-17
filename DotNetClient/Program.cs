using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("ECAP .NET Client Started.\r\n");

        ECAPClient ecapClient = new ECAPClient(ConnectionInfo.FromArgs(args));

        ecapClient.Connecting += (sender, e) => Console.WriteLine($"Connecting to {ecapClient.ConnectionInfo}..");
        ecapClient.ConnectionEstablished += (sender, e) => Console.WriteLine("Connection Established. Waiting new messages..");
        ecapClient.MessageReceived += (sender, e) => Console.Write(e.Message);
        ecapClient.ConnectionLost += (sender, e) => Console.WriteLine($"\r\nConnection to {ecapClient.ConnectionInfo} Lost.");
        ecapClient.ConnectionRefused += (sender, e) => Console.Write($"Couldn't connect to {ecapClient.ConnectionInfo}.");

        await ecapClient.Start();
    }
}