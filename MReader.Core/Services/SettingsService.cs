using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MReader.Core.Models;
using MReader.Core.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization.NamingConventions;
using System.Windows;

namespace MReader.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private static string _settingsFileName = "settings.yaml";
        private static string _settingsFullPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, _settingsFileName);
        private static string _readerStateFileName = "state.yaml";
        private static string _readerStateFullPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, _readerStateFileName);
        private Settings _settings;
        private ReaderState _readerState;

        public event EventHandler SettingsMessageRaised;

        public Settings Settings { get => _settings; }
        public ReaderState ReaderState { get => _readerState; }

        public SettingsService ()
        {
            _settings = new Settings();
            _readerState = new ReaderState();
        }

        //Save the object objectToBeSaved to a Yaml file with path filePath
        private void SaveYamlFile(object objectToBeSaved, string filePath)
        {
            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(objectToBeSaved);
            this.PrintDebug("Saving at " + _settingsFullPath);
            this.PrintDebug(yaml);

            //TODO manage error in case file exist and cannot be written over
            //by incrementing a counter in the name of the file
            using (StreamWriter outputFile = new StreamWriter(filePath))
            {
                outputFile.WriteLine(yaml);
            }
            FireSettingsMessage(SettingsMessageType.SavingSuccessful, objectToBeSaved.GetType());
        }

        //Load the object of type T from a Yaml file with path filePath. 
        //Returns defaultObject if the file couldn't be deserialized (corrupted or doesn't exist)
        private object LoadYamlFile<T>(string filePath, T defaultObject)
        {
            if (!File.Exists(filePath))
            {
                FireSettingsMessage(SettingsMessageType.FileNotFound, typeof(T));
                SaveYamlFile(defaultObject, filePath);
                return defaultObject;
            }

            string textString = File.ReadAllText(filePath);
            var input = new StringReader(textString);
            var deserializer = new DeserializerBuilder().Build();
            T resultObject;
            
            try
            {
                resultObject = deserializer.Deserialize<T>(input);
            }
            catch
            {
                this.PrintDebug("Exception during deserialization of "+typeof(T).ToString()+" file");
                FireSettingsMessage(SettingsMessageType.LoadingFailed, typeof(T));
                SaveYamlFile(defaultObject, filePath);
                return defaultObject;
            }

            FireSettingsMessage(SettingsMessageType.LoadingSuccessful, typeof(T));
            return resultObject;
        }

        private void SaveSettingsToFile ()
        {
            if (_settings == null)
            {
                LoadYamlFile(_settingsFullPath, new Settings());
                return;
            }
            SaveYamlFile(_settings, _settingsFullPath);
        }

        public Settings LoadSettingsFromFile()
        {
            return (Settings)LoadYamlFile(_settingsFullPath, new Settings());
        }

        private void SaveReaderStateToFile()
        {
            if (_readerState == null)
            {
                LoadYamlFile(_readerStateFullPath, new ReaderState());
                return;
            }
            SaveYamlFile(_readerState, _readerStateFullPath);
        }

        public ReaderState LoadReaderStateFromFile()
        {
            return (ReaderState)LoadYamlFile(_readerStateFullPath, new ReaderState());
        }

        public void SaveReaderState(ControlSize windowSize, double readerPanelWidth)
        {
            _readerState.AppWindowSize = windowSize;
            _readerState.ReaderPanelWidth = readerPanelWidth;

            SaveReaderStateToFile();
        }

        public void SetReaderMode(ReaderMode readerMode)
        {
            _settings.ReaderMode = readerMode;
            SaveSettingsToFile();
        }

        public void SetSplittersUnlocked(bool splittersUnlocked)
        {
            _settings.SplittersUnlocked = splittersUnlocked;
            SaveSettingsToFile();
        }

        public void SetSplittersWidth(int splittersWidth)
        {
            _settings.SplittersWidth = splittersWidth;
            SaveSettingsToFile();
        }

        public ReaderMode SwitchMode()
        {
            if (_settings.ReaderMode == ReaderMode.MainPanel)
                _settings.ReaderMode = ReaderMode.Splitters;
            else
                _settings.ReaderMode = ReaderMode.MainPanel;
            SaveSettingsToFile();
            return _settings.ReaderMode;
        }

        public void FireSettingsMessage(SettingsMessageType type, Type target)
        {
            this.PrintDebug("type : " + type + ", target : "+target.ToString());
            var args = new SettingsEventArgs()
            {
                Target = target
            };
            SettingsMessageRaised?.Invoke(type, args);
        }
    }

    public class SettingsEventArgs : EventArgs
    {
        public Type Target { get; set; }
    }
}
