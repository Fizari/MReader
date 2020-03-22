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

        //**
        // Be extra careful : the name fo the fucntion below have to match a certain format "Add + SettingsMessageType + Message"
        // in order to be called with a reflexive method in MainWindowViewModel
        //**
        public LoggingMessage AddLoadingFailedMessage()
        {
            var msg = new LoggingMessage("The sttings file is corrupted and couldn't be loaded, loading default settings instead.", LoggingMessageType.Error);
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddFileNotFoundMessage()
        {
            var msg = new LoggingMessage("The settings file wasn't found, a new one has been generated with default values.", LoggingMessageType.Warning);
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddSavingFailedMessage()
        {
            var msg = new LoggingMessage("The settings file couldn't be saved under the usual name.", LoggingMessageType.Warning);
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddLoadingSuccessfulMessage()
        {
            var msg = new LoggingMessage("Settings were loaded succesfully.");
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddSavingSuccessfulMessage()
        {
            var msg = new LoggingMessage("Settings were saved succesfully.");
            _messages.Add(msg);
            return msg;
        }
    }
}
