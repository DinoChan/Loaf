using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel;

namespace Loaf
{
    public sealed partial class MainView
    {
        public MainView()
        {
            this.InitializeComponent();
            VersionElement.Text = GetVersion();
        }
        private string GetVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
        private void OnLoaf(object sender, RoutedEventArgs e)
        {
            LoafTeachingTip.IsOpen = true;
        }
        private void OnStartLoaf(TeachingTip sender, object args)
        {
            MainWindow.Instance.Loaf();
        }
        private async void OnRate(object sender, RoutedEventArgs e)
        {
            await SystemInformation.LaunchStoreForReviewAsync();
        }
    }
}