using Prism.Commands;
using Prism.Mvvm;

namespace MReader.Core.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "MReader - Dev";

        private int _splittersWidth = 10;
        private int _imagePanelMinWidth = 100;

        private DelegateCommand _pressme;
        public DelegateCommand PressMe =>
            _pressme ?? (_pressme = new DelegateCommand(ExecuteCommandName));

        void ExecuteCommandName()
        {
            //SplittersWidth == 10 ? SplittersWidth = 50 : SplittersWidth = 10;
            if (SplittersWidth == 10)
            {
                SplittersWidth = 50;
            }
            else
            {
                SplittersWidth = 10;
            }
        }
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public int SplittersWidth 
        {
            get { return _splittersWidth; }
            set { SetProperty(ref _splittersWidth, value); }
        }

        public int ImagePanelMinWidth
        {
            get { return _imagePanelMinWidth; }
            set { SetProperty(ref _imagePanelMinWidth, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
