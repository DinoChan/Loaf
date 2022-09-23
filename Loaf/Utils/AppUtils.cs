using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.UI;

namespace Loaf.Utils
{
    internal static class AppUtils
    {
        public static AppWindow AppWindow;
        public static IntPtr MainWindowHandle;

        public static void InitializeTitleBar(AppWindowTitleBar titleBar)
        {
            if (titleBar == null)
            {
                return;
            }

            titleBar.ExtendsContentIntoTitleBar = true;
            if (Application.Current.RequestedTheme == ApplicationTheme.Light)
            {
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.InactiveBackgroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.DarkGray;
                titleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                titleBar.ButtonHoverForegroundColor = Colors.DarkGray;
                titleBar.ButtonPressedBackgroundColor = Colors.Gray;
                titleBar.ButtonPressedForegroundColor = Colors.DarkGray;
                titleBar.ButtonInactiveBackgroundColor = Color.FromArgb(255, 240, 243, 249);
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;
            }
            else
            {
                titleBar.BackgroundColor = Colors.Transparent;
                titleBar.InactiveBackgroundColor = Colors.Black;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 20, 20, 20);
                titleBar.ButtonHoverForegroundColor = Colors.White;
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 40, 40, 40);
                titleBar.ButtonPressedForegroundColor = Colors.White;
                titleBar.ButtonInactiveBackgroundColor = Color.FromArgb(255, 32, 32, 32);
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;
            }
        }

        public static int GetScalePixel(double pixel, IntPtr windowHandle)
        {
            var dpi = PInvoke.User32.GetDpiForWindow(windowHandle);
            return Convert.ToInt32(pixel * (dpi / 96.0));
        }
    }
}
