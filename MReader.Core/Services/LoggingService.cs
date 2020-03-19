using System;
using System.Collections.Generic;
using System.Text;
using MReader.Core.Models;

namespace MReader.Core.Services
{
    public class LoggingService : ILoggingService
    {
        private List<LoggingMessage> _messages;

        public LoggingService ()
        {
            _messages = new List<LoggingMessage>();
        }

        public void AddNewMessage(LoggingMessage message)
        {
            _messages.Add(message);
        }
        public void AddNewMessage(string message, LoggingMessageType type = LoggingMessageType.Normal)
        {
            _messages.Add(new LoggingMessage(message, type));
        }

        public List<LoggingMessage> GetMessages()
        {
            return _messages;
        }
    }
}
