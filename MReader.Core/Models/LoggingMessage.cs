using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Models
{
    public class LoggingMessage
    {
        public DateTime Time { get; }
        public string Message { get; }
        public LoggingMessageType Type { get; }

        public LoggingMessage (string message)
        {
            Message = message;
            Type = LoggingMessageType.Normal;
            Time = DateTime.Now;
        }

        public LoggingMessage (string message, LoggingMessageType type)
        {
            Message = message;
            Type = type;
            Time = DateTime.Now;
        }
    }
}
