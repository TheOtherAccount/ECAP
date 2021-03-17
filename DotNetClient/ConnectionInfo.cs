using System;
using System.Collections.Generic;

public class ConnectionInfo
{
    public string HostName { get; set; }
    public int PortNumber { get; set; }

    public override string ToString()
    {
        return $"{HostName} on {PortNumber}";
    }

    public ConnectionInfo(string hostName, int portNumber)
    {
        HostName = hostName;
        PortNumber = portNumber;
    }

    public static ConnectionInfo FromArgs(string[] args)
    {
        return new ConnectionInfo(Utils.ParseCommandLineArguments(args));
    }

    public ConnectionInfo(Dictionary<string, string> args = null)
    {
        string hostName = null;
        string portNumber = null;

        if (args != null)
        {
            args.TryGetValue("host", out hostName);
            args.TryGetValue("port", out portNumber);
        }

        if (string.IsNullOrWhiteSpace(hostName))
        {
            hostName = "ecap_server";
#if DEBUG
            hostName = "localhost";
#endif
        }

        if (string.IsNullOrWhiteSpace(portNumber))
        {
            portNumber = "6060";
        }

        if (!int.TryParse(portNumber, out int parsedPortNumber))
        {
            throw new ArgumentException("Port number is not a valid integer");
        }

        HostName = hostName;
        PortNumber = parsedPortNumber;
    }
}