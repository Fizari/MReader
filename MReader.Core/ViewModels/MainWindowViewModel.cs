using Microsoft.Win32;
using MReader.Core.Extensions;
using MReader.Core.Models;
using MReader.Core.Exceptions;
using MReader.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MReader.Core.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

#if DEBUG
        private string _title = "MReader - Dev";
#else
        private string _title = "MReader";
#endif

        private IFileService _fileService;
        private ISettingsService _settingsService;
        private ILoggingService _loggingService;
        private BitmapImage _imageSource;
        private int _splittersWidth = 10;
        private int _imagePanelMinWidth = 100;
        private DelegateCommand _openFileDialogCommand;
        private DelegateCommand _lockOrUnlockSplittersCommand;
        private DelegateCommand _windowLoadedCommand;
        private DelegateCommand _toggleLoggingWindowCommand;
        private DelegateCommand _pressMe;//TODO REMOVE
        private bool _areSplittersUnlocked;
        private bool _isLoggingWindowVisible;
        private ObservableCollection<LoggingMessage> _logMessageList;
        
        #region properties

        public DelegateCommand OpenFileDialogCommand =>
            _openFileDialogCommand ?? (_openFileDialogCommand = new DelegateCommand(ChooseFile));
        public DelegateCommand LockSplittersCommand =>
            _lockOrUnlockSplittersCommand ?? (_lockOrUnlockSplittersCommand = new DelegateCommand(LockOrUnlockSplitters));
        public DelegateCommand WindowLoadedCommand =>
            _windowLoadedCommand ?? (_windowLoadedCommand = new DelegateCommand(WindowLoaded));
        public DelegateCommand ToggleLoggingWindowCommand =>
            _toggleLoggingWindowCommand ?? (_toggleLoggingWindowCommand = new DelegateCommand(ToggleLoggingWindow));
        public DelegateCommand PressMeCommand =>
            _pressMe ?? (_pressMe = new DelegateCommand(PressMe));

        public DelegateCommand<KeyEventArgs> WindowKeyDownCommand { get; private set; }
        public DelegateCommand<KeyEventArgs> GridKeyDownCommand { get; private set; }

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

        public bool AreSplittersUnlocked
        {
            get { return _areSplittersUnlocked; }
            set { SetProperty(ref _areSplittersUnlocked, value); }
        }

        public ObservableCollection<LoggingMessage> LogMessageList
        {
            get { return _logMessageList; }
            set {SetProperty(ref _logMessageList, value);}
        }

        public bool IsLoggingWindowVisible
        {
            get { return _isLoggingWindowVisible; }
            set { SetProperty(ref _isLoggingWindowVisible, value); }
        }

#endregion

        public MainWindowViewModel(IFileService fileService, ISettingsService settingsService, ILoggingService loggingService)
        {
            this.PrintDebug();
            _loggingService = loggingService;
            LogMessageList = new ObservableCollection<LoggingMessage>();

            //TEST
            DisplayLogMessage(new LoggingMessage("This is a first message for testing"));
            DisplayLogMessage(new LoggingMessage("This is a warning message for testing",LoggingMessageType.Warning));
            DisplayLogMessage(new LoggingMessage("This is an error message for testing", LoggingMessageType.Error));
            //ENDTEST

            _fileService = fileService;
            _fileService.CurrentImageLoaded += OnCurrentImageJustLoaded;

            //Load the settings
            _settingsService = settingsService;

            var settings = new Settings();
            try
            {
                settings = _settingsService.LoadSettings();
            }
            catch (FailedToLoadSettingsException f)
            {
                DisplayLogMessage(_loggingService.AddSettingsNotFoundWarningMessage());
            }
            SplittersWidth = settings.SplittersWidth;
            AreSplittersUnlocked = settings.SplittersUnlocked;

            WindowKeyDownCommand = new DelegateCommand<KeyEventArgs>(OnWindowInputKeyDown);
            GridKeyDownCommand = new DelegateCommand<KeyEventArgs>(OnGridInputKeyDown);
        }

        public void WindowLoaded()
        {
            this.PrintDebug();
        }

        private void PressMe()
        {
            LogMessageList.Add(new LoggingMessage("This was just added", LoggingMessageType.Error));
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

        private void LockOrUnlockSplitters()
        {
            AreSplittersUnlocked = !AreSplittersUnlocked;
            _settingsService.SetSplittersUnlocked(AreSplittersUnlocked);
        }

        private void ToggleLoggingWindow()
        {
            IsLoggingWindowVisible = !IsLoggingWindowVisible;
        }

        // Display the Image in the panel when receiving the event 
        private void OnCurrentImageJustLoaded(object sender, EventArgs e)
        {
            DisplayImage((ImageData)sender);
        }

        private void DisplayImage(ImageData imageToDisplay)
        {
            ImageData imageData = imageToDisplay;
            this.PrintDebug(imageData.File.FullName);
            ImageSource = imageData.BitmapImg;
        }

        private void DisplayLogMessage(LoggingMessage logMessage)
        {
            //Handles displaying of message
            LogMessageList.Add(logMessage);
        }

#region key bindings
        private void OnWindowInputKeyDown(KeyEventArgs e)
        {
            //CTRL + O
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O)
            {
                ChooseFile();
                e.Handled = true;
            }
            //CTRL + ENTER
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                //LOGIC
                e.Handled = true;
            }
        }

        private void OnGridInputKeyDown(KeyEventArgs e)
        {
            //LEFT
            if (e.Key == Key.Left)
            {
                ImageData newImageToDisplay = _fileService.LoadPreviousImage();
                DisplayImage(newImageToDisplay);
                e.Handled = true;
            }
            //RIGHT
            if (e.Key == Key.Right)
            {
                ImageData newImageToDisplay = _fileService.LoadNextImage();
                DisplayImage(newImageToDisplay);
                e.Handled = true;
            }
            //UP
            if (e.Key == Key.Up)
            {
                //LOGIC
                e.Handled = true;
            }
            //DOWN
            if (e.Key == Key.Down)
            {
                //LOGIC
                e.Handled = true;
            }
        }

#endregion
    }
}
