using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace MReader.CustomControlLibrary
{
    public class ActionWindow : Window
    {
        public Button MinimizeButton { get; private set; }
        public Button MaximizeButton { get; private set; }
        public Button RestoreButton { get; private set; }
        public Button CloseButton { get; private set; }
        public Button ExpendControlsButton { get; private set; }
        public Button CollapseControlsButton { get; private set; }
        public StackPanel ActionButtonsPanel { get; private set; }
        public StackPanel CollapseAndExpandPanel { get; private set; }
        public TextBlock TitleTextBlock { get; private set; }
        public Border ActionButtonsBorder { get; private set; }
        public Grid TitleBar { get; private set; }


        public T GetRequiredTemplateChild<T>(string childName) where T : DependencyObject
        {
            return (T)base.GetTemplateChild(childName);
        }

        #region magical stuff taken from https://pastebin.com/WDZpDKRN
        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;
            /// <summary>y coordinate of point.</summary>
            public int y;
            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();
            public int Width { get { return Math.Abs(right - left); } }
            public int Height { get { return bottom - top; } }
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty { get { return left >= right || top >= bottom; } }
            public override string ToString()
            {
                if (this == Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }
            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode() => left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2) { return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom); }
            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2) { return !(rect1 == rect2); }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion

        public static readonly DependencyProperty ActionButtonsProperty =
        DependencyProperty.Register("ActionButtons", typeof(ObservableCollection<Control>), typeof(Control));

        public static readonly DependencyProperty DisplayTitleProperty =
        DependencyProperty.Register("ShowTitle", typeof(bool), typeof(ActionWindow), new PropertyMetadata(true));

        public ObservableCollection<Control> ActionButtons
        {
            get => (ObservableCollection<Control>)GetValue(ActionButtonsProperty);
            set => SetValue(ActionButtonsProperty, value);
        }

        public bool ShowTitle
        {
            get => (bool)GetValue(DisplayTitleProperty);
            set => SetValue(DisplayTitleProperty, value);
        }

        public ActionWindow()
        {
            ActionButtons = new ObservableCollection<Control>();
        }

        public override void OnApplyTemplate()
        {
            MinimizeButton = GetRequiredTemplateChild<Button>("windowMinimizeButton");
            MaximizeButton = GetRequiredTemplateChild<Button>("windowMaximizeButton");
            RestoreButton = GetRequiredTemplateChild<Button>("windowRestoreButton");
            CloseButton = GetRequiredTemplateChild<Button>("windowCloseButton");
            ExpendControlsButton = GetRequiredTemplateChild<Button>("expendControlsButton");
            CollapseControlsButton = GetRequiredTemplateChild<Button>("collapseControlsButton");
            ActionButtonsPanel = GetRequiredTemplateChild<StackPanel>("controlsButtonsPanel");
            ActionButtonsBorder = GetRequiredTemplateChild<Border>("controlsButtonsBorder");
            TitleTextBlock = GetRequiredTemplateChild<TextBlock>("titleTextBlock");
            TitleBar = GetRequiredTemplateChild<Grid>("titleBar");

            if (CloseButton != null)
            {
                CloseButton.Click += CloseButton_Click;
            }

            if (MinimizeButton != null)
            {
                MinimizeButton.Click += MinimizeButton_Click;
            }

            if (RestoreButton != null)
            {
                RestoreButton.Click += RestoreButton_Click;
            }

            if (MaximizeButton != null)
            {
                MaximizeButton.Click += MaximizeButton_Click;
            }

            if (ExpendControlsButton != null)
            {
                ExpendControlsButton.Click += (s, e) => {
                    ActionButtonsPanel.Visibility = Visibility.Visible;
                    CollapseControlsButton.Visibility = Visibility.Visible;
                    ExpendControlsButton.Visibility = Visibility.Collapsed;
                };
            }

            if (CollapseControlsButton != null)
            {
                CollapseControlsButton.Click += (s, e) => {
                    ActionButtonsPanel.Visibility = Visibility.Collapsed;
                    ExpendControlsButton.Visibility = Visibility.Visible;
                    CollapseControlsButton.Visibility = Visibility.Collapsed;
                };
            }

            //Apply Magic
            SourceInitialized += (s, e) =>
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };
            /////////////

            if (!ShowTitle)
                TitleTextBlock.Visibility = Visibility.Collapsed;

            SetupActionButtons();

            base.OnApplyTemplate();
        }

        private void SetupActionButtons()
        {
            if (ActionButtons == null || ActionButtons.Count == 0)
            {
                ActionButtonsBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                foreach (Control c in ActionButtons)
                {
                    ActionButtonsPanel.Children.Add(c);
                    if (c is Button)
                        c.Style = (Style)FindResource("ActionButtonStyle");
                }
            }
        }

        protected void ToggleWindowState()
        {
            if (base.WindowState != WindowState.Maximized)
            {
                base.WindowState = WindowState.Maximized;
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                base.WindowState = WindowState.Normal;
                MaximizeButton.Visibility = Visibility.Visible;
                RestoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleWindowState();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleWindowState();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
