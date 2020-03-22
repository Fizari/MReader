using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MReader.Core.Models;
using MReader.Core.Exceptions;
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

        public SettingsService ()
        {
            _settings = new Settings();
        }

        private void SaveSettings ()
        {
            if (_settings == null)
            {
                LoadSettings();
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
        }
        public Settings GetSettings ()
        {
            return _settings;
        }

        public Settings LoadSettings()
        {
            if (!File.Exists(_settingsFullPath))
            {
                throw new FailedToLoadSettingsException();
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
                throw new FailedToLoadSettingsException();
            }
            
            _settings = settings;

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
    }
}
