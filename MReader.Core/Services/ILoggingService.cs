using MReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Services
{
    public interface ILoggingService
    {
        public void AddNewMessage(string message, LoggingMessageType type = LoggingMessageType.Normal);
        public void AddNewMessage(LoggingMessage message);
        public List<LoggingMessage> GetMessages();

    }
}
