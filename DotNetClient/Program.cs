﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class TcpTimeClient
{
    private static async Task Connect()
    {
        Console.WriteLine($"Connecting to the server..");

        await TcpClient.ConnectAsync("localhost", 6060);

        Console.WriteLine($"Connected successfully.");
    }
    private static TcpClient TcpClient { get; set; } = new TcpClient();
    private static int RetryDuration { get; set; } = 3;
    private static void CountDown()
    {
        for (var i = RetryDuration; i > 0; i--)
        {
            Console.Write("\b \b");

            Console.Write(i);

            Thread.Sleep(1000);
        }
    }

    private static async Task GetMessages()
    {
        Console.WriteLine($"Started receiving messages..");

        NetworkStream ns = TcpClient.GetStream();

        while (true)
        {
            byte[] message = new byte[1024];

            int receivedByteCount = await ns.ReadAsync(message, 0, message.Length);

            if (receivedByteCount > 0)
            {
                Console.Write(Encoding.ASCII.GetString(message, 0, receivedByteCount));
            }
        }
    }

    public static async Task Main(string[] args)
    {
        await Start();
    }

    private static async Task Start()
    {
        await TryConnect();

        try
        {
            await GetMessages();
        }
        catch
        {
            TcpClient.Close();

            TcpClient = new TcpClient();

            Console.WriteLine();

            await Start();
        }
    }

    public static async Task TryConnect()
    {
        bool connected = false;

        while (!connected)
        {
            try
            {
                await Connect();

                connected = true;
            }
            catch
            {
                Console.Write($"Couldn't connect to the server. Trying again in {RetryDuration}");

                CountDown();

                Console.WriteLine();
            }
        }
    }
}