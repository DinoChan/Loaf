using Loaf.Utils;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.Graphics;
using WinRT.Interop;
using static PInvoke.User32;

namespace Loaf
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("c71080c8-ac9c-4246-8651-ccf786533b21",
                   typeof(Analytics), typeof(Crashes));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            InitializeMainWindow();
            _window.Activate();
        }

        private void InitializeMainWindow()
        {
            _windowHandle = WindowNative.GetWindowHandle(_window);
            var windowId = Win32Interop.GetWindowIdFromWindow(_windowHandle);
            _appWindow = AppWindow.GetFromWindowId(windowId);
            _appWindow.Title = ResourceExtensions.GetLocalized("Loaf");

            AppUtils.AppWindow = _appWindow;
            AppUtils.MainWindowHandle = _windowHandle;

            // 在 Win10 中, AppWindow.TitleBar 始终为 null.
            if (_appWindow.TitleBar != null)
            {
                AppUtils.InitializeTitleBar(_appWindow.TitleBar);
            }

            var preferWidth = AppUtils.GetScalePixel(612, _windowHandle);
            var preferHeight = AppUtils.GetScalePixel(740, _windowHandle);
            _appWindow.Resize(new SizeInt32(preferWidth, preferHeight));
            CenterOnScreen(preferWidth, preferHeight);
        }

        private void CenterOnScreen(int width, int height)
        {
            var hwndDesktop = MonitorFromWindow(_windowHandle, MonitorOptions.MONITOR_DEFAULTTONEAREST);
            var info = new MONITORINFO();
            info.cbSize = 40;
            GetMonitorInfo(hwndDesktop, ref info);
            
            var cx = (info.rcMonitor.left + info.rcMonitor.right) / 2;
            var cy = (info.rcMonitor.bottom + info.rcMonitor.top) / 2;
            var left = cx - (width / 2);
            var top = cy - (height / 2);
            SetWindowPos(_windowHandle, SpecialWindowHandles.HWND_NOTOPMOST, left, top, width, height, SetWindowPosFlags.SWP_SHOWWINDOW);
        }

        private Window _window;
        private IntPtr _windowHandle;
        private AppWindow _appWindow;
    }
}
