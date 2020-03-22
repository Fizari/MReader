using MReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MReader.Core.Services
{
    public interface ILoggingService
    {
        public void AddNewMessage(string message, LoggingMessageType type = LoggingMessageType.Normal);
        public void AddNewMessage(LoggingMessage message);
        public ObservableCollection<LoggingMessage> GetMessages();
        public LoggingMessage AddLoadingFailedMessage();
        public LoggingMessage AddFileNotFoundMessage();
        public LoggingMessage AddSavingFailedMessage();
        public LoggingMessage AddLoadingSuccessfulMessage();
        public LoggingMessage AddSavingSuccessfulMessage();
    }
}
