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
        public LoggingMessage AddSettingsLoadingFailedMessage(Type target);
        public LoggingMessage AddSettingsFileNotFoundMessage(Type target);
        public LoggingMessage AddSettingsSavingFailedMessage(Type target);
        public LoggingMessage AddSettingsLoadingSuccessfulMessage(Type target);
        public LoggingMessage AddSettingsSavingSuccessfulMessage(Type target);
    }
}
