using System.Collections.Generic;

/// <summary>
/// A utility class for extension and helper methods.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Converts a command line arguments array to a dictionary.
    /// </summary>
    /// <param name="args">The arguments array that you typically get from the Main method.
    /// e.g. hostName=something.</param>
    /// <returns>A dictionary that holds the argument keys and values.</returns>
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
                    theArgs.AddOrUpdate(splittedArg[0], splittedArg[1]);
                }
            }
        }

        return theArgs;
    }

    /// <summary>
    /// Add a new key and a value to a dictionary; and if it the key exists the value gets updates.
    /// </summary>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key you want to add or update.</param>
    /// <param name="value">The value.</param>
    public static void AddOrUpdate(this Dictionary<string, string> dictionary, string key, string value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }
}