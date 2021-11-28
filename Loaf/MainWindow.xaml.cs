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
using System.Diagnostics;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Loaf
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private bool _isLoafing;

        public MainWindow()
        {
            this.InitializeComponent();
            Current = this;
            Root.Loaded += OnLoaded;
            Root.KeyDown += Root_KeyDown;
            Activated += MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            var control = Frame.Content as Page;
            if (control != null && _isLoafing)
                control.Focus(FocusState.Keyboard);
        }

        public static MainWindow Current { get; private set; }

        private void Root_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (_isLoafing && e.Key == Windows.System.VirtualKey.Escape)
            {
                Frame.GoBack();
                _isLoafing = false;
                _appWindow.SetPresenter(AppWindowPresenterKind.Default);
                var parent = VisualTreeHelper.GetParent(Root);
                while (parent != null)
                {
                    if (parent is FrameworkElement element)
                    {
                        element.IsHitTestVisible = true;
                    }

                    parent = VisualTreeHelper.GetParent(parent);
                }
                while (ShowCursor(true) < 0)
                {
                    ShowCursor(true); //显示光标
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainView));
            _appWindow = GetAppWindowForCurrentWindow();
            _appWindow.Title = "WinUI ❤️ Loaf";
        }

        private AppWindow _appWindow;

        public void Loaf()
        {
            var parent = VisualTreeHelper.GetParent(Root);
            while (parent != null)
            {
                if (parent is FrameworkElement element)
                {
                    element.IsHitTestVisible = false;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }
            Frame.Navigate(typeof(Windows11UpdateView));
            _appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
            while (ShowCursor(true) < 0)
            {
                ShowCursor(true); //显示光标
            }
            while (ShowCursor(false) >= 0)
            {
                ShowCursor(false); //隐藏光标
            }
            _isLoafing = true;
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
