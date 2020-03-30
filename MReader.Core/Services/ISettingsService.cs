using MReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MReader.Core.Services
{
    public interface ISettingsService
    {
        public event EventHandler SettingsMessageRaised; 
        public Settings Settings { get; }
        public ReaderState ReaderState { get; }
        public void SetSplittersWidth (int splittersWidth);
        public void SetReaderMode (ReaderMode readerMode);
        public void SetSplittersUnlocked (bool splittersLocked);
        public void SaveReaderState(ControlSize windowSize, double readerPanelWidth);
        public Settings LoadSettingsFromFile();
        public ReaderState LoadReaderStateFromFile();
        public ReaderMode SwitchMode();
    }
}
