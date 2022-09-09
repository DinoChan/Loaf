using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Loaf.Controls
{
    public sealed partial class AppTitleBar : UserControl
    {
        public event EventHandler BackButtonClick;

        public AppTitleBar()
        {
            this.InitializeComponent();
        }

        public bool IsShowBackButton
        {
            get { return (bool)GetValue(IsShowBackButtonProperty); }
            set { SetValue(IsShowBackButtonProperty, value); }
        }

        public static readonly DependencyProperty IsShowBackButtonProperty =
            DependencyProperty.Register(nameof(IsShowBackButton), typeof(bool), typeof(AppTitleBar), new PropertyMetadata(false));

        private void OnBackButtonClickAsync(object sender, RoutedEventArgs e)
        {
            BackButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
