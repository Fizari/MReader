using System;

namespace MReader.Core.Models
{
    public class Settings
    {
        public int SplittersWidth { get; set; }
        public bool SplittersUnlocked { get; set; }
        public ReaderMode ReaderMode { get; set; }
        public double ReaderPanelWidth { get; set; }
        public WindowSize AppWindowSize { get; set; }//(Width, Height)

        public Settings ()
        {
            SplittersWidth = 10;
            SplittersUnlocked = true;
            ReaderMode = ReaderMode.Splitters;
            ReaderPanelWidth = double.NaN;
            AppWindowSize = new WindowSize(525.0, 350.0);
        }

        public Settings(int splittersWidth, bool splittersUnlocked, ReaderMode readerMode, double readerPlanelWidth, WindowSize appWindowSize)
        {
            SplittersWidth = splittersWidth;
            SplittersUnlocked = splittersUnlocked;
            ReaderMode = readerMode;
            ReaderPanelWidth = readerPlanelWidth;
            AppWindowSize = appWindowSize;
        }
    }
}
