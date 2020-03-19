using MReader.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MReader.Core.Services
{
    public interface ISettingsService
    {
        public void SetSplittersWidth (int splittersWidth);
        public void SetReaderMode (ReaderMode readerMode);
        public void SetSplittersUnlocked (bool splittersLocked);
        public Settings GetSettings();
        public Settings LoadSettings();
    }
}
