using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RemoteAppTool.Core;

namespace RemoteAppTool.App.Views
{
    public partial class RemoteAppsView : UserControl
    {
        private SystemRemoteApps _systemRemoteApps;
        public ObservableCollection<RemoteApp> RemoteAppsList { get; set; }
        private bool _isEditing = false;
        private string _originalEditName = "";

        public RemoteAppsView()
        {
            InitializeComponent();
            _systemRemoteApps = new SystemRemoteApps();
            RemoteAppsList = new ObservableCollection<RemoteApp>();
            AppsDataGrid.ItemsSource = RemoteAppsList;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRemoteApps();
        }

        private void LoadRemoteApps()
        {
            try
            {
                _systemRemoteApps.Init();
                var apps = _systemRemoteApps.GetAll();
                RemoteAppsList.Clear();
                foreach (var app in apps)
                {
                    RemoteAppsList.Add(app);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading RemoteApps: {ex.Message}\nMake sure you are running as Administrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRemoteApps();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            _isEditing = false;
            DialogTitle.Text = "Create New RemoteApp";
            TxtName.Text = "";
            TxtFullName.Text = "";
            TxtPath.Text = "";
            TxtIconPath.Text = "";
            ChkTSWA.IsChecked = false;
            TxtName.IsEnabled = true;
            
            MaterialDesignThemes.Wpf.DialogHost.Show(RootDialog.DialogContent, "RootDialog");
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string appName)
            {
                var app = RemoteAppsList.FirstOrDefault(a => a.Name == appName);
                if (app != null)
                {
                    _isEditing = true;
                    _originalEditName = app.Name;
                    DialogTitle.Text = $"Edit {app.Name}";
                    TxtName.Text = app.Name;
                    TxtFullName.Text = app.FullName;
                    TxtPath.Text = app.Path;
                    TxtIconPath.Text = app.IconPath;
                    ChkTSWA.IsChecked = app.TSWA;
                    TxtName.IsEnabled = false;

                    MaterialDesignThemes.Wpf.DialogHost.Show(RootDialog.DialogContent, "RootDialog");
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string appName)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the RemoteApp '{appName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _systemRemoteApps.DeleteApp(appName);
                        LoadRemoteApps();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "Executables (*.exe)|*.exe|All files (*.*)|*.*" };
            if (ofd.ShowDialog() == true)
            {
                TxtPath.Text = ofd.FileName;
                if (string.IsNullOrEmpty(TxtName.Text))
                {
                    TxtName.Text = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
                    TxtFullName.Text = TxtName.Text;
                }
            }
        }

        private void BtnBrowseIcon_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = "Icons/Executables (*.ico;*.exe)|*.ico;*.exe|All files (*.*)|*.*" };
            if (ofd.ShowDialog() == true)
            {
                TxtIconPath.Text = ofd.FileName;
            }
        }

        private void BtnSaveDialog_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text) || string.IsNullOrWhiteSpace(TxtPath.Text))
            {
                MessageBox.Show("Alias and Path are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newApp = new RemoteApp
                {
                    Name = TxtName.Text.Trim(),
                    FullName = TxtFullName.Text.Trim(),
                    Path = TxtPath.Text.Trim(),
                    VPath = TxtPath.Text.Trim(),
                    IconPath = TxtIconPath.Text.Trim(),
                    TSWA = ChkTSWA.IsChecked ?? false
                };

                _systemRemoteApps.SaveApp(newApp);
                LoadRemoteApps();
                MaterialDesignThemes.Wpf.DialogHost.Close("RootDialog");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCreateRdp_Click(object sender, RoutedEventArgs e)
        {
            if (AppsDataGrid.SelectedItem is RemoteApp app)
            {
                var sfd = new SaveFileDialog { Filter = "RDP Files (*.rdp)|*.rdp", FileName = app.Name + ".rdp" };
                if (sfd.ShowDialog() == true)
                {
                    try
                    {
                        var rdp = new RemoteAppTool.RdpFileLib.RdpFile
                        {
                            RemoteApplicationName = app.FullName,
                            RemoteApplicationProgram = app.Name,
                            RemoteApplicationMode = 1,
                            RemoteApplicationCmdLine = app.CommandLine,
                            FullAddress = Environment.MachineName
                        };

                        rdp.SaveRdpFile(sfd.FileName, true);
                        MessageBox.Show($"RDP file saved successfully to:\n{sfd.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to create RDP file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a RemoteApp from the list first.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCreateMsi_Click(object sender, RoutedEventArgs e)
        {
            if (AppsDataGrid.SelectedItem is RemoteApp app)
            {
                var ofd = new OpenFileDialog { Filter = "RDP Files (*.rdp)|*.rdp", Title = "Select the RDP file to package" };
                if (ofd.ShowDialog() == true)
                {
                    var sfd = new SaveFileDialog { Filter = "MSI Installers (*.msi)|*.msi", FileName = app.Name + ".msi" };
                    if (sfd.ShowDialog() == true)
                    {
                        try
                        {
                            var rdp2msi = new Rdp2Msi
                            {
                                RdpPath = ofd.FileName
                            };

                            if (!rdp2msi.WixInstalled())
                            {
                                MessageBox.Show("WiX Toolset (candle.exe/light.exe) is not installed or found in the 'wix' folder.", "Missing Dependency", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            rdp2msi.CreateMsi(sfd.FileName);
                            MessageBox.Show($"MSI installer created successfully at:\n{sfd.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to create MSI: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a RemoteApp from the list first.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
