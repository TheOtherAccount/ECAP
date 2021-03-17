using System;
using System.Collections.Generic;

/// <summary>
/// Provides the properties required for connecting to a machine via TCP/IP.
/// </summary>
public class ConnectionInfo
{
    /// <summary>
    /// The DNS Name, IP Address or the Computer Name/Alias.
    /// </summary>
    public string HostName { get; set; }
    /// <summary>
    /// The Port Number that you want to establish the connection on.
    /// </summary>
    public int PortNumber { get; set; }
    /// <summary>
    /// Represents this instance in a friendly string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{HostName} on {PortNumber}";
    }
    /// <summary>
    /// Instantiates an object with the specified host and port number.
    /// </summary>
    /// <param name="hostName">The DNS Name, IP Address or the Computer Name/Alias.</param>
    /// <param name="portNumber">The Port Number that you want to establish the connection on.</param>
    public ConnectionInfo(string hostName, int portNumber)
    {
        HostName = hostName;
        PortNumber = portNumber;
    }
    /// <summary>
    /// Creates an instance based on command line arguemtns
    /// </summary>
    /// <param name="args">The command line arguments. Each one should use the format option=value</param>
    /// <returns></returns>
    public static ConnectionInfo FromArgs(string[] args)
    {
        return new ConnectionInfo(Utils.ParseCommandLineArguments(args));
    }
    /// <summary>
    /// Instanitates an object with property values based on the specified dictionary.
    /// </summary>
    /// <param name="args">An optional dictionary that holds the property values.. e.g. hostName
    /// Typically you get it by parsing the command line arguments.
    /// The default hostName while debugging is: localhost; but on release it is : ecap_server.
    /// The default portNumber is 6060</param>
    public ConnectionInfo(Dictionary<string, string> args = null)
    {
        string hostName = null;
        string portNumber = null;

        if (args != null)
        {
            args.TryGetValue("hostName", out hostName);
            args.TryGetValue("portNumber", out portNumber);
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