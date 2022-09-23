using Loaf.Utils;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Graphics;

namespace Loaf.Controls
{
    public sealed partial class AppTitleBar : UserControl
    {
        public event EventHandler BackButtonClick;
        private AppWindowTitleBar _appWindowTitleBar;

        public AppTitleBar()
        {
            this.InitializeComponent();
            ActualThemeChanged += OnActualThemeChanged;
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        public bool IsShowBackButton
        {
            get { return (bool)GetValue(IsShowBackButtonProperty); }
            set { SetValue(IsShowBackButtonProperty, value); }
        }

        public static readonly DependencyProperty IsShowBackButtonProperty =
            DependencyProperty.Register(nameof(IsShowBackButton), typeof(bool), typeof(AppTitleBar), new PropertyMetadata(false));

        private void OnActualThemeChanged(FrameworkElement sender, object args)
            => AppUtils.InitializeTitleBar(AppUtils.AppWindow.TitleBar);

        private void OnLoaded(object sender, RoutedEventArgs e)
            => SetDragArea();

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
            => SetDragArea();

        private void SetDragArea()
        {
            var appWindow = AppUtils.AppWindow;
            _appWindowTitleBar = appWindow.TitleBar;
            if (_appWindowTitleBar == null)
            {
                return;
            }

            var dragRect = default(RectInt32);
            dragRect.X = AppUtils.GetScalePixel(40, AppUtils.MainWindowHandle);
            dragRect.Y = 0;
            dragRect.Height = AppUtils.GetScalePixel(ActualHeight, AppUtils.MainWindowHandle);
            dragRect.Width = AppUtils.GetScalePixel(ActualWidth, AppUtils.MainWindowHandle) - dragRect.X;

            _appWindowTitleBar.SetDragRectangles(new[] { dragRect });
        }

        private void OnBackButtonClickAsync(object sender, RoutedEventArgs e)
        {
            BackButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
