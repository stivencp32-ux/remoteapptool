using System;
using System.Windows;
using System.Windows.Controls;
using RemoteAppTool.RdpFileLib;
using System.IO;

namespace RemoteAppTool.App.Views
{
    public partial class ClientConnectionView : UserControl
    {
        public ClientConnectionView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                var settings = RemoteAppTool.Core.ClientSettings.Load();
                TxtServerAddress.Text = settings.ServerAddress;
                TxtServerPort.Text = settings.ServerPort;
                TxtGateway.Text = settings.Gateway;
                ChkAdmin.IsChecked = settings.ConnectAsAdmin;
                ChkClipboard.IsChecked = settings.RedirectClipboard;
                ChkPrinters.IsChecked = settings.RedirectPrinters;
                ChkDrives.IsChecked = settings.RedirectDrives;
                TxtAdvancedOptions.Text = settings.AdvancedOptions;
            }
            catch
            {
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settings = new RemoteAppTool.Core.ClientSettings
                {
                    ServerAddress = TxtServerAddress.Text.Trim(),
                    ServerPort = TxtServerPort.Text.Trim(),
                    Gateway = TxtGateway.Text.Trim(),
                    ConnectAsAdmin = ChkAdmin.IsChecked ?? false,
                    RedirectClipboard = ChkClipboard.IsChecked ?? false,
                    RedirectPrinters = ChkPrinters.IsChecked ?? false,
                    RedirectDrives = ChkDrives.IsChecked ?? false,
                    AdvancedOptions = TxtAdvancedOptions.Text.Trim()
                };
                settings.Save();
                MessageBox.Show("Client Connection Defaults saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
