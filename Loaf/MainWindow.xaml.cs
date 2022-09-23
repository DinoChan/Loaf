using Loaf.Utils;
using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;
using static PInvoke.User32;
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
        private WinProc _newWndProc = null;
        private IntPtr _oldWndProc = IntPtr.Zero;
        private MicaController _micaController;
        private SystemBackdropConfiguration _configurationSource;
        private const int MIN_WINDOW_WIDTH = 612;
        private const int MIN_WINDOW_HEIGHT = 740;

        public MainWindow()
        {
            this.InitializeComponent();
            new DispatcherUtils().EnsureWindowsSystemDispatcherQueueController();
            _appWindow = GetAppWindowForCurrentWindow();
            _appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
            _appWindow.SetPresenter(AppWindowPresenterKind.Default);
            Instance = this;
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

        public static MainWindow Instance { get; private set; }

        private void Root_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (_isLoafing && e.Key == Windows.System.VirtualKey.Escape)
            {
                Frame.GoBack();
                Unloaf();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomeView));
            _appWindow = GetAppWindowForCurrentWindow();

            _appWindow.Title = "WinUI ❤️ " + ResourceExtensions.GetLocalized("Loaf");
            SubClassing();
            TrySetMicaBackdrop();
        }

        private void OnThemeChanged(FrameworkElement sender, object args)
        {
            if (_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
        }

        private AppWindow _appWindow;
        private delegate IntPtr WinProc(IntPtr hWnd, PInvoke.User32.WindowMessage msg, IntPtr wParam, IntPtr lParam);

        public void Loaf()
        {
            RestoreWindow();
            AppTitleBar.Visibility = Visibility.Collapsed;
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
            // 阻止系统睡眠，阻止屏幕关闭。
            SystemSleep.PreventForCurrentThread();
            _isLoafing = true;
        }

        public void Unloaf()
        {
            _isLoafing = false;
            AppTitleBar.Visibility = Visibility.Visible;
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
            // 恢复此线程曾经阻止的系统休眠和屏幕关闭。
            SystemSleep.RestoreForCurrentThread();

            HideAllWindows();
        }

        [DllImport("user32", EntryPoint = "ShowCursor")]
        public extern static int ShowCursor(bool show);

        [DllImport("user32")]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, PInvoke.User32.WindowLongIndexFlags nIndex, WinProc newProc);

        [DllImport("user32")]
        private static extern IntPtr SetWindowLongW(IntPtr hWnd, PInvoke.User32.WindowLongIndexFlags nIndex, WinProc newProc);

        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        internal static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, PInvoke.User32.WindowMessage msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(myWndId);
        }

        private void RestoreWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            ShowWindow(hWnd, 9);
        }

        private void SubClassing()
        {
            var windowHandle = WindowNative.GetWindowHandle(this);
            _newWndProc = new WinProc(NewWindowProc);
            if (Environment.Is64BitProcess)
                _oldWndProc = SetWindowLongPtr(windowHandle, PInvoke.User32.WindowLongIndexFlags.GWL_WNDPROC, _newWndProc);
            else
                _oldWndProc = SetWindowLongW(windowHandle, PInvoke.User32.WindowLongIndexFlags.GWL_WNDPROC, _newWndProc);
        }

        private IntPtr NewWindowProc(IntPtr hWnd, PInvoke.User32.WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case PInvoke.User32.WindowMessage.WM_GETMINMAXINFO:
                    {
                        var minMaxInfo = Marshal.PtrToStructure<PInvoke.User32.MINMAXINFO>(lParam);
                        minMaxInfo.ptMinTrackSize.x = AppUtils.GetScalePixel(MIN_WINDOW_WIDTH, hWnd);
                        minMaxInfo.ptMinTrackSize.y = AppUtils.GetScalePixel(MIN_WINDOW_HEIGHT, hWnd);
                        Marshal.StructureToPtr(minMaxInfo, lParam, true);
                        break;
                    }
            }

            return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }

        private void OnBackButtonClick(object sender, EventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        /// <summary>
        ///     发送 Win + D，最小化所有窗口
        /// </summary>
        private void HideAllWindows()
        {
            INPUT[] inDow = new INPUT[4];
            int i = 0;

            while (i < inDow.Length)
            {
                inDow[i] = new INPUT();
                inDow[i++].type = InputType.INPUT_KEYBOARD;
            }

            INPUT[] inDown = new INPUT[4];

            inDown[0] = new INPUT();
            inDown[1] = new INPUT();
            inDown[2] = new INPUT();
            inDown[3] = new INPUT();

            inDown[0].type = inDown[1].type = inDown[2].type = inDown[3].type = InputType.INPUT_KEYBOARD;
            inDown[0].Inputs.ki.wVk = inDown[2].Inputs.ki.wVk = VirtualKey.VK_LWIN;
            inDown[1].Inputs.ki.wVk = inDown[3].Inputs.ki.wVk = VirtualKey.VK_D;

            inDown[2].Inputs.ki.dwFlags = inDown[3].Inputs.ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP;

            SendInput(4, inDown, Marshal.SizeOf(inDown[0]));
        }

        private bool TrySetMicaBackdrop()
        {
            if (MicaController.IsSupported())
            {
                // Hooking up the policy object
                _configurationSource = new SystemBackdropConfiguration();
                Activated += OnActivated;
                Closed += OnClosed;
                ((FrameworkElement)this.Content).ActualThemeChanged += OnThemeChanged;

                // Initial configuration state.
                _configurationSource.IsInputActive = true;
                SetConfigurationSourceTheme();

                _micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                _micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                _micaController.SetSystemBackdropConfiguration(_configurationSource);
                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }

        private void OnActivated(object sender, WindowActivatedEventArgs args)
            => _configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;

        private void OnClosed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
            // use this closed window.
            if (_micaController != null)
            {
                _micaController.Dispose();
                _micaController = null;
            }

            Activated -= OnActivated;
            _configurationSource = null;
        }

        private void SetConfigurationSourceTheme()
        {
            switch (((FrameworkElement)Content).ActualTheme)
            {
                case ElementTheme.Dark:
                    _configurationSource.Theme = SystemBackdropTheme.Dark;
                    break;
                case ElementTheme.Light:
                    _configurationSource.Theme = SystemBackdropTheme.Light;
                    break;
                case ElementTheme.Default:
                    _configurationSource.Theme = SystemBackdropTheme.Default;
                    break;
            }
        }
    }
}
