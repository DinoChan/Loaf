using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.Graphics;
using WinRT.Interop;

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

            // 在 Win10 中, AppWindow.TitleBar 始终为 null.
            if (_appWindow.TitleBar != null)
            {
                AppUtils.InitializeTitleBar(_appWindow.TitleBar);
                InitializeDragArea();
            }

            var preferWidth = AppUtils.GetScalePixel(612, _windowHandle);
            var preferHeight = AppUtils.GetScalePixel(740, _windowHandle);
            _appWindow.Resize(new SizeInt32(preferWidth, preferHeight));
        }

        private void InitializeDragArea()
        {
            var rect = new RectInt32(40, 0, AppUtils.GetScalePixel(_appWindow.Size.Width, _windowHandle), AppUtils.GetScalePixel(36, _windowHandle));
            _appWindow.TitleBar.SetDragRectangles(new RectInt32[] { rect });
        }

        private Window _window;
        private IntPtr _windowHandle;
        private AppWindow _appWindow;
    }
}
