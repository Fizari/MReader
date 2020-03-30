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
        // Be extra careful : the name fo the fucntion below have to match a certain format "AddSettings" + SettingsMessageType + "Message"
        // in order to be called with a reflexive method in MainWindowViewModel
        //**
        public LoggingMessage AddSettingsLoadingFailedMessage(Type target)
        {
            string msgString = "";
            if (target == typeof(ReaderState))
                msgString = "The state file is corrupted and couldn't be loaded, loading default state instead.";
            if (target == typeof(Settings))
                msgString = "The settings file is corrupted and couldn't be loaded, loading default settings instead.";
            var msg = new LoggingMessage(msgString, LoggingMessageType.Error);
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddSettingsFileNotFoundMessage(Type target)
        {
            string msgString = "";
            if (target == typeof(ReaderState))
                msgString = "The state file wasn't found, a new one has been generated with default values.";
            if (target == typeof(Settings))
                msgString = "The settings file wasn't found, a new one has been generated with default values.";
            var msg = new LoggingMessage(msgString, LoggingMessageType.Warning);
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddSettingsSavingFailedMessage(Type target)
        {
            string msgString = "";
            if (target == typeof(ReaderState))
                msgString = "The state file couldn't be saved under the usual name.";
            if (target == typeof(Settings))
                msgString = "The settings file couldn't be saved under the usual name.";
            var msg = new LoggingMessage(msgString, LoggingMessageType.Warning);
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddSettingsLoadingSuccessfulMessage(Type target)
        {
            string msgString = "";
            if (target == typeof(ReaderState))
                msgString = "Saved state was loaded succesfully.";
            if (target == typeof(Settings))
                msgString = "Saved settings were loaded succesfully.";
            var msg = new LoggingMessage(msgString);
            _messages.Add(msg);
            return msg;
        }
        public LoggingMessage AddSettingsSavingSuccessfulMessage(Type target)
        {
            string msgString = "";
            if (target == typeof(ReaderState))
                msgString = "The reader state was saved succesfully.";
            if (target == typeof(Settings))
                msgString = "Settings were saved succesfully.";
            var msg = new LoggingMessage(msgString);
            _messages.Add(msg);
            return msg;
        }
    }
}
