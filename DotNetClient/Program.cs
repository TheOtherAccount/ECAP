using System;
using System.Threading.Tasks;
/// <summary>
/// The main entry point of the application.
/// </summary>
public class Program
{
    /// <summary>
    /// The entry-point method.
    /// </summary>
    /// <param name="args">An optional command-line arguments in the following format: name=value</param>
    /// <returns></returns>
    public static async Task Main(string[] args)
    {
        Console.WriteLine("ECAP .NET Client Started.\r\n");

        // Instantiates a TCP Client based on the parsed command line arguments (or the default values)
        ECAPClient ecapClient = new ECAPClient(ConnectionInfo.FromArgs(args));

        #region Displaying connection status when changed
        ecapClient.Connecting += (sender, e) => Console.WriteLine($"Connecting to {ecapClient.ConnectionInfo}..");
        ecapClient.ConnectionEstablished += (sender, e) => Console.WriteLine("Connection Established. Waiting new messages..");
        ecapClient.ConnectionLost += (sender, e) => Console.WriteLine($"\r\nConnection to {ecapClient.ConnectionInfo} Lost.");
        ecapClient.ConnectionRefused += (sender, e) => Console.Write($"Couldn't connect to {ecapClient.ConnectionInfo}.");
        #endregion

        ecapClient.MessageReceived += (sender, e) => Console.Write(e.Message);

        // Starting the TCP client
        await ecapClient.Start();
    }
}