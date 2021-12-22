using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
