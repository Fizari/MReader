using Microsoft.Win32;
using MReader.Core.Extensions;
using MReader.Core.Models;
using MReader.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Windows;

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
        private int _splittersWidth;
        private double _mainScrollViewerWidth;
        private double _mainScrollViewerWidthChanged;
        private double _topToolbarHeight = 23;
        private double _footerBarHeight = 20;
        private double _savedTopToolbarHeight;
        private double _savedFooterBarHeight;
        private double _appWindowWidth;
        private double _appWindowHeight;
        private string _fullScreenButtonText = "FullScreen";
        private WindowState _appWindowState = WindowState.Normal;
        private WindowStyle _appWindowStyle = WindowStyle.SingleBorderWindow;
        private ReaderMode _readerMode;
        private DelegateCommand _openFileDialogCommand;
        private DelegateCommand _lockOrUnlockSplittersCommand;
        private DelegateCommand _windowLoadedCommand;
        private DelegateCommand _toggleLoggingWindowCommand;
        private DelegateCommand _pressMe;//TODO REMOVE
        private DelegateCommand _switchModeCommand;
        private DelegateCommand _enableFullScreenCommand;
        private DelegateCommand _windowClosingCommand;
        private bool _areSplittersUnlocked;
        private bool _isAppFullScreen = false;
        private bool _areToolbarsVisible = true;
        private bool _isLoggingWindowVisible;
        private ObservableCollection<LoggingMessage> _logMessageList;
        private LoggingMessage _logMessageCurrent;

        #region properties
        public DelegateCommand WindowClosingCommand =>
            _windowClosingCommand ?? (_windowClosingCommand = new DelegateCommand(ApplicationClosing));
        public DelegateCommand SwitchModeCommand =>
            _switchModeCommand ?? (_switchModeCommand = new DelegateCommand(SwitchMode));
        public DelegateCommand OpenFileDialogCommand =>
            _openFileDialogCommand ?? (_openFileDialogCommand = new DelegateCommand(ChooseFile));
        public DelegateCommand LockSplittersCommand =>
            _lockOrUnlockSplittersCommand ?? (_lockOrUnlockSplittersCommand = new DelegateCommand(LockOrUnlockSplitters));
        public DelegateCommand WindowLoadedCommand =>
            _windowLoadedCommand ?? (_windowLoadedCommand = new DelegateCommand(WindowLoaded));
        public DelegateCommand ToggleLoggingWindowCommand =>
            _toggleLoggingWindowCommand ?? (_toggleLoggingWindowCommand = new DelegateCommand(ToggleLoggingWindow));
        public DelegateCommand EnableFullScreenCommand =>
            _enableFullScreenCommand ?? (_enableFullScreenCommand = new DelegateCommand(EnableFullScreen));
        public DelegateCommand PressMeCommand =>
            _pressMe ?? (_pressMe = new DelegateCommand(PressMe));

        public DelegateCommand<KeyEventArgs> WindowKeyDownCommand { get; private set; }
        public DelegateCommand<KeyEventArgs> GridKeyDownCommand { get; private set; }
        public DelegateCommand<object> ScrollViewerSizeChangedCommand { get; private set; }

        public string Title
        {
            get => _title; 
            set => SetProperty(ref _title, value); 
        }

        public BitmapImage ImageSource
        {
            get =>  _imageSource; 
            set => SetProperty(ref _imageSource, value); 
        }

        public ReaderMode ReaderMode
        {
            get => _readerMode; 
            set => SetProperty(ref _readerMode, value); 
        }

        public int SplittersWidth 
        {
            get => _splittersWidth;
            set => SetProperty(ref _splittersWidth, value);
        }

        public double MainScrollViewerWidth
        {
            get => _mainScrollViewerWidth;
            set => SetProperty(ref _mainScrollViewerWidth, value);
        }

        public double TopToolBarHeight
        {
            get => _topToolbarHeight;
            set => SetProperty(ref _topToolbarHeight, value);
        }

        public double FooterBarHeight
        {
            get => _footerBarHeight;
            set => SetProperty(ref _footerBarHeight, value);
        }

        public double AppWindowWidth
        {
            get => _appWindowWidth;
            set => SetProperty(ref _appWindowWidth, value);
        }

        public double AppWindowHeight
        {
            get => _appWindowHeight;
            set => SetProperty(ref _appWindowHeight, value);
        }

        public WindowStyle AppWindowStyle
        {
            get => _appWindowStyle;
            set => SetProperty(ref _appWindowStyle, value);
        }

        public WindowState AppWindowState
        {
            get => _appWindowState;
            set => SetProperty(ref _appWindowState, value);
        }

        public string FullScreenButtonText
        {
            get => _fullScreenButtonText;
            set => SetProperty(ref _fullScreenButtonText, value);
        }

        public bool AreSplittersUnlocked
        {
            get => _areSplittersUnlocked;
            set => SetProperty(ref _areSplittersUnlocked, value);
        }

        public ObservableCollection<LoggingMessage> LogMessageList
        {
            get => _logMessageList; 
            set => SetProperty(ref _logMessageList, value);
        }

        public LoggingMessage LogMessageCurrent
        {
            get => _logMessageCurrent ?? new LoggingMessage(""); 
            set => SetProperty(ref _logMessageCurrent, value);
        }

        public bool IsLoggingWindowVisible
        {
            get =>  _isLoggingWindowVisible;
            set => SetProperty(ref _isLoggingWindowVisible, value); 
        }

#endregion

        public MainWindowViewModel(IFileService fileService, ISettingsService settingsService, ILoggingService loggingService)
        {
            this.PrintDebug();

            //Initiate logging service
            _loggingService = loggingService;
            LogMessageList = new ObservableCollection<LoggingMessage>();

            //TEST
            DisplayLogMessage(new LoggingMessage("This is a first message for testing"));
            DisplayLogMessage(new LoggingMessage("This is a warning message for testing",LoggingMessageType.Warning));
            DisplayLogMessage(new LoggingMessage("This is an error message for testing", LoggingMessageType.Error));
            //ENDTEST

            //Initiate file service
            _fileService = fileService;
            _fileService.CurrentImageLoaded += OnCurrentImageJustLoaded;

            //Load the settings
            _settingsService = settingsService;
            _settingsService.SettingsMessageRaised += OnSettingsEvent;
            var settings = _settingsService.LoadSettingsFromFile();
            var state = _settingsService.LoadReaderStateFromFile();
            _mainScrollViewerWidthChanged = state.ReaderPanelWidth;
            SplittersWidth = settings.SplittersWidth;
            AreSplittersUnlocked = settings.SplittersUnlocked;
            MainScrollViewerWidth = state.ReaderPanelWidth;
            AppWindowWidth = state.AppWindowSize.Width;
            AppWindowHeight = state.AppWindowSize.Height;

            //key binding events
            WindowKeyDownCommand = new DelegateCommand<KeyEventArgs>(OnWindowInputKeyDown);
            GridKeyDownCommand = new DelegateCommand<KeyEventArgs>(OnGridInputKeyDown);
            ScrollViewerSizeChangedCommand = new DelegateCommand<object>(ScrollViewerSizeChanged);
        }

        public void WindowLoaded()
        {
            this.PrintDebug();
            ReaderMode = _settingsService.Settings.ReaderMode; //should be set after MainScrollViewerWidth has been set 
        }

        private void PressMe()
        {
            DisplayLogMessage(new LoggingMessage("This was just added", LoggingMessageType.Error));
        }

        private void SwitchMode()
        {
            ReaderMode = _settingsService.SwitchMode();
        }

        private void EnableFullScreen()
        {
            if (_isAppFullScreen)
            {
                FullScreenButtonText = "FullScreen";
                AppWindowStyle = WindowStyle.SingleBorderWindow;
                AppWindowState = WindowState.Normal;
            } 
            else
            {
                FullScreenButtonText = "[Exit FullScreen]";
                AppWindowStyle = WindowStyle.None;
                AppWindowState = WindowState.Maximized;
            }
            _isAppFullScreen = !_isAppFullScreen;
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

        //show or hide the log journal
        private void ToggleLoggingWindow()
        {
            IsLoggingWindowVisible = !IsLoggingWindowVisible;
        }

        //show or hide all the toolbars
        private void ToggleToolBars()
        {
            if (_areToolbarsVisible)
            {
                _savedFooterBarHeight = FooterBarHeight;
                _savedTopToolbarHeight = TopToolBarHeight;
                FooterBarHeight = 0;
                TopToolBarHeight = 0;
            }
            else
            {
                TopToolBarHeight = _savedTopToolbarHeight;
                FooterBarHeight = _savedFooterBarHeight;
            }
            _areToolbarsVisible = !_areToolbarsVisible;
        }

        //Triggered everytime the size of the scrollviewer is changed
        private void ScrollViewerSizeChanged(object e)
        {
            Size s = (Size)e;
            _mainScrollViewerWidthChanged = s.Width;
        }

        // Display the Image in the panel when receiving the event 
        private void OnCurrentImageJustLoaded(object sender, EventArgs e)
        {
            DisplayImage((ImageData)sender);
        }

        //Reflective way to trigger logging function based on event received (see LoggingService class)
        private void OnSettingsEvent(object type, EventArgs e)
        {
            SettingsEventArgs args = (SettingsEventArgs)e;
            MethodInfo addMethod = _loggingService.GetType().GetMethod("AddSettings"+type.ToString()+"Message");
            this.PrintDebug("Method to call : "+addMethod.Name);
            DisplayLogMessage((LoggingMessage)addMethod.Invoke(_loggingService, new object[] { args.Target }));
        }

        //Handles displaying of image
        private void DisplayImage(ImageData imageToDisplay)
        {
            this.PrintDebug(imageToDisplay.File.FullName);
            ImageSource = imageToDisplay.BitmapImg;
        }

        //Handles displaying of message
        private void DisplayLogMessage(LoggingMessage logMessage)
        {
            LogMessageList.Add(logMessage);
            LogMessageCurrent = logMessage;
        }

        private void ApplicationClosing()
        {
            _settingsService.SaveReaderState(new ControlSize(AppWindowWidth, AppWindowHeight), _mainScrollViewerWidthChanged);
        }

        #region key bindings
        private void OnWindowInputKeyDown(KeyEventArgs e)
        {
            //CTRL + O (open file)
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O)
            {
                ChooseFile();
                e.Handled = true;
            }
            //CTRL + ENTER (Fullscreen)
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
            {
                EnableFullScreen();
                e.Handled = true;
            }
            //CTRL + L (Display log window)
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.L)
            {
                ToggleLoggingWindow();
                e.Handled = true;
            }
            //CTRL + B (Display/Hide toolbars)
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.B)
            {
                ToggleToolBars();
                e.Handled = true;
            }
        }

        private void OnGridInputKeyDown(KeyEventArgs e)
        {
            //LEFT (previous image)
            if (e.Key == Key.Left)
            {
                ImageData newImageToDisplay = _fileService.LoadPreviousImage();
                DisplayImage(newImageToDisplay);
                e.Handled = true;
            }
            //RIGHT  (next image)
            if (e.Key == Key.Right)
            {
                ImageData newImageToDisplay = _fileService.LoadNextImage();
                DisplayImage(newImageToDisplay);
                e.Handled = true;
            }
            //UP (scroll up)
            if (e.Key == Key.Up)
            {
                //LOGIC
                e.Handled = true;
            }
            //DOWN (scroll down)
            if (e.Key == Key.Down)
            {
                //LOGIC
                e.Handled = true;
            }
        }

#endregion
    }
}
