using CommunityToolkit.WinUI.UI;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using System.Windows.Input;
using System.Runtime.InteropServices;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Loaf
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
            var m_appWindow = GetAppWindowForCurrentWindow();
            if (m_appWindow != null)
            {
                // You now have an AppWindow object and can call its methods to manipulate the window.
                // Just to do something here, let's change the title of the window...
                m_appWindow.Title = "WinUI ❤️ AppWindow";
            }
            //m_appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
            //  FrameworkElementExtensions.SetCursor(sender as FrameworkElement, Microsoft.UI.Input.InputSystemCursorShape.AppStarting);

            while (ShowCursor(true) < 0)

            {

                ShowCursor(true); //显示光标

            }
            while (ShowCursor(false) >= 0)

            {

                ShowCursor(false); //隐藏光标

            }
            //var view = ApplicationView.GetForCurrentView();
            //view.TryEnterFullScreenMode();
        }


        [DllImport("user32", EntryPoint = "ShowCursor")]
        public extern static int ShowCursor(bool show);

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(myWndId);
        }
    }
}
