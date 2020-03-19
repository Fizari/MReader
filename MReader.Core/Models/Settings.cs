namespace MReader.Core.Models
{
    public class Settings
    {
        public int SplittersWidth { get; set; }
        public bool SplittersUnlocked { get; set; }
        public ReaderMode ReaderMode { get; set; }

        public Settings ()
        {
            SplittersWidth = 10;
            SplittersUnlocked = true;
            ReaderMode = ReaderMode.Splitters;
        }

        public Settings (int splittersWidth, bool splittersUnlocked, ReaderMode readerMode)
        {
            SplittersWidth = splittersWidth;
            SplittersUnlocked = splittersUnlocked;
            ReaderMode = readerMode;
        }
    }
}
