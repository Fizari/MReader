using Microsoft.Win32;
using MReader.Core.Extensions;
using MReader.Core.Models;
using MReader.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Media.Imaging;

namespace MReader.Core.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IFileService _fileService;

        private string _title = "MReader - Dev";
        private BitmapImage _imageSource;
        private int _splittersWidth = 10;
        private int _imagePanelMinWidth = 100;

        private DelegateCommand _openFileDialog;
        public DelegateCommand OpenFileDialog =>
            _openFileDialog ?? (_openFileDialog = new DelegateCommand(ChooseFile));

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
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

        private void ChooseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileToLoad = openFileDialog.FileName;
                _fileService.LoadFile(fileToLoad);
            }
        }

        public MainWindowViewModel(IFileService fileService)
        {
            _fileService = fileService;
            _fileService.CurrentImageLoaded += OnCurrentImageJustLoaded;
        }

        // Display the Image in the panel when receiving the event 
        private void OnCurrentImageJustLoaded(object sender, EventArgs e)
        {
            ImageData imageData = (ImageData)sender;
            this.PrintDebug(imageData.File.FullName);
            ImageSource = imageData.BitmapImg;
        }
    }
}
