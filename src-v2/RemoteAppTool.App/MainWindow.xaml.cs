using System.Windows;
using RemoteAppTool.App.Views;

namespace RemoteAppTool.App
{
    public partial class MainWindow : Window
    {
        private RemoteAppsView _remoteAppsView;
        private HostOptionsView _hostOptionsView;
        private ClientConnectionView _clientConnectionView;
        private AboutView _aboutView;

        public MainWindow()
        {
            InitializeComponent();
            _remoteAppsView = new RemoteAppsView();
            _hostOptionsView = new HostOptionsView();
            _clientConnectionView = new ClientConnectionView();
            _aboutView = new AboutView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Set default view
            MainContent.Content = _remoteAppsView;
        }

        private void BtnNavRemoteApps_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _remoteAppsView;
        }

        private void BtnNavHostOptions_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _hostOptionsView;
        }

        private void BtnNavClientConnection_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _clientConnectionView;
        }

        private void BtnNavAbout_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _aboutView;
        }
    }
}