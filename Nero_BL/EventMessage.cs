using System;

namespace Nero_Line_WPF
{
    public class EventMessage : EventArgs
    {
        public string Message { get; }

        public EventMessage(string message)
        {
            Message = message;
        }

    }
}