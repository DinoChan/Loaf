using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;

namespace Loaf
{
    public sealed partial class Windows11UpdateView
    {
        private bool _disposed = false;
        private readonly HookManager hookManager;
        public Windows11UpdateView()
        {
            this.InitializeComponent();
            hookManager = new HookManager();
            Loaded += Windows11UpdateView_Loaded;
            Unloaded += Windows11UpdateView_Unloaded;
            this.KeyDown += Windows11UpdateView_KeyDown;
            this.PointerReleased += Windows11UpdateView_PointerReleased;
            if (Environment.OSVersion.Version.Build < 22000)
                Root.Background = new SolidColorBrush(Color.FromArgb(255, 0, 109, 174));
        }

        private void Windows11UpdateView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.Focus(FocusState.Keyboard);
        }
        private void Windows11UpdateView_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }
        private void Windows11UpdateView_Unloaded(object sender, RoutedEventArgs e)
        {
            SetCursor(true);
            _disposed = true;
        }
        private CoreCursor Cursor;
        private void SetCursor(bool show)
        {
            if (!show)
            {
                hookManager.HookStart();
                Cursor = Window.Current.CoreWindow.PointerCursor;
                Window.Current.CoreWindow.PointerCursor = null;
            }
            else
            {
                hookManager.HookStop();
                Window.Current.CoreWindow.PointerCursor = Cursor;
            }
        }
        private async void Windows11UpdateView_Loaded(object sender, RoutedEventArgs e)
        {
            SetCursor(false);
            int index = 0;
            while (_disposed == false)
            {
                UpdatingElement.Text = string.Format(ResourceExtensions.GetLocalized("UpdatingText"), Math.Min(98, index++));
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}