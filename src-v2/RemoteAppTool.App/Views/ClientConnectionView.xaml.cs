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
                // In a real scenario, this would load from a default .rdp file or app settings
                // For now, load machine defaults
                TxtServerAddress.Text = Environment.MachineName;
                TxtServerPort.Text = "3389";
            }
            catch
            {
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Client Connection Defaults saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
