namespace MReader.Core.Models
{
    public class Settings
    {
        public int SplittersWidth { get; set; }
        public bool SplittersUnlocked { get; set; }
        public Mode ReaderMode { get; set; }

        public Settings ()
        {
            SplittersWidth = 10;
            SplittersUnlocked = true;
            ReaderMode = Mode.Splitters;
        }

        public Settings (int splittersWidth, bool splittersUnlocked, Mode readerMode)
        {
            SplittersWidth = splittersWidth;
            SplittersUnlocked = splittersUnlocked;
            ReaderMode = readerMode;
        }
    }
}
