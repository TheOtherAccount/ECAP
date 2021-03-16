using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ECAPClient
{
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);
    public event MessageEventHandler MessageReceived;

    public void x()
    {
        if (MessageReceived != null)
        {
            MessageReceived(this, new MessageEventArgs(""));
        }
    }
}