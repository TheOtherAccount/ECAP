using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class ConnectionInfo
{
    public int RetryDuration { get; set; } = 3;
    public string HostName { get; set; }
    public int PortNumber { get; set; }

    public ConnectionInfo(string hostName, int portNumber)
    {
        HostName = hostName;
        PortNumber = portNumber;
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