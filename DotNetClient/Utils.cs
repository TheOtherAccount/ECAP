using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Utils
{
    public static Dictionary<string, string> ParseCommandLineArguments(string[] args)
    {
        Dictionary<string, string> theArgs = new Dictionary<string, string>();

        if (args != null)
        {
            foreach (var arg in args)
            {
                string[] splittedArg = arg.Split('=');

                if (splittedArg.Length > 1)
                {
                    if (theArgs.ContainsKey(splittedArg[0]))
                    {
                        theArgs[splittedArg[0]] = splittedArg[1];
                    }
                    else
                    {
                        theArgs.Add(splittedArg[0], splittedArg[1]);
                    }
                }
            }
        }

        return theArgs;
    }
}