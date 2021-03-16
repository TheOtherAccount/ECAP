using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MessageEventArgs : EventArgs
{
    public MessageEventArgs(string message)
    {
        Message = message;
    }
    public string Message { get; set; }
}