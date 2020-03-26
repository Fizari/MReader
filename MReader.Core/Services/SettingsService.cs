using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MReader.Core.Models;
using MReader.Core.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization.NamingConventions;

namespace MReader.Core.Services
{
    public class SettingsService : ISettingsService
    {
        private static string _settingsFileName = "settings.yaml";
        private static string _settingsFullPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, _settingsFileName);
        private Settings _settings;

        public event EventHandler SettingsMessageRaised;

        public SettingsService ()
        {
            _settings = new Settings();
        }

        private void SaveSettings ()
        {
            if (_settings == null)
            {
                LoadSettings();
                return;
            }
            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(_settings);
            this.PrintDebug("Saving at "+ _settingsFullPath);
            this.PrintDebug(yaml);

            //TODO manage error in case file exist and cannot be written over
            //by incrementing a counter in the name of the file 
            using (StreamWriter outputFile = new StreamWriter(_settingsFullPath))
            {
                outputFile.WriteLine(yaml);
            }
            FireSettingsMessage(SettingsMessageType.SavingSuccessful);
        }
        public Settings GetSettings ()
        {
            return _settings;
        }

        public Settings LoadSettings()
        {
            if (!File.Exists(_settingsFullPath))
            {
                FireSettingsMessage(SettingsMessageType.FileNotFound);
                _settings = new Settings();
                SaveSettings();
                return _settings;
            }

            string settingsString = File.ReadAllText(_settingsFullPath);
            var input = new StringReader(settingsString);
            var deserializer = new DeserializerBuilder().Build();
            Settings settings;
            try
            {
                settings = deserializer.Deserialize<Settings>(input);
            }
            catch
            {
                this.PrintDebug("Exception during deserialization of settings file");
                FireSettingsMessage(SettingsMessageType.LoadingFailed);
                _settings = new Settings();
                SaveSettings();
                return _settings;
            }
            
            _settings = settings;
            FireSettingsMessage(SettingsMessageType.LoadingSuccessful);
            return settings;
        }

        public void SetReaderMode(ReaderMode readerMode)
        {
            _settings.ReaderMode = readerMode;
            SaveSettings();
        }

        public void SetSplittersUnlocked(bool splittersUnlocked)
        {
            _settings.SplittersUnlocked = splittersUnlocked;
            SaveSettings();
        }

        public void SetSplittersWidth(int splittersWidth)
        {
            _settings.SplittersWidth = splittersWidth;
            SaveSettings();
        }

        public void SetApplicationWindowSize(double width, double height)
        {
            _settings.AppWindowSize.Width = width;
            _settings.AppWindowSize.Height = height;
            SaveSettings();
        }

        public void SetReaderPanelWidth(double width)
        {
            _settings.ReaderPanelWidth = width;
            SaveSettings();
        }

        public ReaderMode SwitchMode()
        {
            if (_settings.ReaderMode == ReaderMode.MainPanel)
                _settings.ReaderMode = ReaderMode.Splitters;
            else
                _settings.ReaderMode = ReaderMode.MainPanel;
            SaveSettings();
            return _settings.ReaderMode;
        }

        public void FireSettingsMessage(SettingsMessageType type)
        {
            this.PrintDebug("type : " + type);
            SettingsMessageRaised?.Invoke(type, new EventArgs());
        }
    }
}
