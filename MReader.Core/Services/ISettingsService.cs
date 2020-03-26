using MReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Services
{
    public interface ISettingsService
    {
        public event EventHandler SettingsMessageRaised;
        public void SetSplittersWidth (int splittersWidth);
        public void SetReaderMode (ReaderMode readerMode);
        public void SetSplittersUnlocked (bool splittersLocked);
        public void SetApplicationWindowSize(double width, double height);
        public void SetReaderPanelWidth(double width);
        public Settings GetSettings();
        public Settings LoadSettings();
        public ReaderMode SwitchMode();
    }
}
