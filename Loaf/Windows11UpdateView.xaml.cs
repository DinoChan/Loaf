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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Loaf
{
    public sealed partial class Windows11UpdateView 
    {
        private bool _disposed = false;

        public Windows11UpdateView()
        {
            this.InitializeComponent();
            Loaded += Windows11UpdateView_Loaded;
            Unloaded += Windows11UpdateView_Unloaded;
            this.KeyDown += Windows11UpdateView_KeyDown;
            this.PointerReleased += Windows11UpdateView_PointerReleased;
            
        }

        private void Windows11UpdateView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.Focus(FocusState.Keyboard);
        }

        private void Windows11UpdateView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
           
        }

        private void Windows11UpdateView_KeyDown(object sender, KeyRoutedEventArgs e)
        {
           
        }

        private void Windows11UpdateView_Unloaded(object sender, RoutedEventArgs e)
        {
            _disposed = true;
        }

        private async void Windows11UpdateView_Loaded(object sender, RoutedEventArgs e)
        {
            int index = 0;
            while (_disposed == false)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                Ru.Text = (index++ % 100) + "%";
            }
        }
    }
}
