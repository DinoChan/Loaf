using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.UI.Xaml;

namespace Loaf
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            AppCenter.Start("c71080c8-ac9c-4246-8651-ccf786533b21", typeof(Analytics), typeof(Crashes));
        }
        private Window m_window;
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }
    }
}