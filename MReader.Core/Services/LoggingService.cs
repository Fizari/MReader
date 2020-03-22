using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MReader.Core.Models;

namespace MReader.Core.Services
{
    public class LoggingService : ILoggingService
    {
        private ObservableCollection<LoggingMessage> _messages;

        public LoggingService ()
        {
            _messages = new ObservableCollection<LoggingMessage>();
        }

        public void AddNewMessage(LoggingMessage message)
        {
            _messages.Add(message);
        }
        public void AddNewMessage(string message, LoggingMessageType type = LoggingMessageType.Normal)
        {
            _messages.Add(new LoggingMessage(message, type));
        }

        public ObservableCollection<LoggingMessage> GetMessages()
        {
            return _messages;
        }

        public LoggingMessage AddFileCorruptedErrorMessage(string fileName)
        {
            var msg = new LoggingMessage("The file "+ fileName +" is corrupted and couldn't be loaded.", LoggingMessageType.Error);
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddSettingsNotFoundWarningMessage()
        {
            var msg = new LoggingMessage("The settings file wasn't found or was corrupted, a new one has been generated with default values.", LoggingMessageType.Warning);
            _messages.Add(msg);
            return msg;
        }
    }
}
