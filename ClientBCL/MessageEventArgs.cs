using System;

namespace ClientBCL
{
    /// <summary>
    /// Holding the arguemtns of the the MessageReceived event.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        public bool WaitAnotherMessage { get; set; } = true;
        /// <summary>
        /// The message that has been received.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Instaniates an object with the specified message.
        /// </summary>
        /// <param name="message"></param>
        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }
}