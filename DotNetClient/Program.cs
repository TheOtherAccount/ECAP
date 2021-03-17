using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        ECAPClient ecapClient = new ECAPClient(new ConnectionInfo(Utils.ParseCommandLineArguments(args)));

        ecapClient.Connecting += (sender, e) =>
        {
            Console.WriteLine();
            Console.WriteLine($"Connecting to the {ecapClient.ConnectionInfo.HostName} on {ecapClient.ConnectionInfo.PortNumber}..");
        };

        ecapClient.ConnectionEstablished += (sender, e) =>
        {
            Console.WriteLine("Connection Established. Waiting new messages..");
        };

        ecapClient.MessageReceived += (sender, e) =>
        {
            Console.Write(e.Message);
        };

        ecapClient.ConnectionLost += (sender, e) =>
        {
            Console.WriteLine();
            Console.WriteLine("Connection Lost.");
        };

        ecapClient.ConnectionRefused += async (sender, e) =>
        {
            Console.Write($"Couldn't connect to the server. Trying again in {ecapClient.ConnectionInfo.RetryDuration}");

            await CountDown(ecapClient.ConnectionInfo.RetryDuration);
        };

        await ecapClient.Start();
    }

    private async static Task CountDown(int retryDuration)
    {
        for (var i = retryDuration; i > 0; i--)
        {
            Console.Write("\b \b");

            Console.Write(i);

            await Task.Delay(1000);
        }
    }
}