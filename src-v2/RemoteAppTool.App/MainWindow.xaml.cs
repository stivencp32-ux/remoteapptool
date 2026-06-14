using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RemoteAppTool.Core;

namespace RemoteAppTool.App
{
    public partial class MainWindow : Window
    {
        private SystemRemoteApps _systemRemoteApps;
        public ObservableCollection<RemoteApp> RemoteAppsList { get; set; }
        private bool _isEditing = false;
        private string _originalEditName = "";

        public MainWindow()
        {
            InitializeComponent();
            _systemRemoteApps = new SystemRemoteApps();
            RemoteAppsList = new ObservableCollection<RemoteApp>();
            AppsDataGrid.ItemsSource = RemoteAppsList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
            TxtName.IsEnabled = true; // Can edit alias on create
            
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
                    TxtName.IsEnabled = false; // Cannot edit alias name after creation usually, prevents orphan keys easily

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
                    VPath = TxtPath.Text.Trim(), // Usually VPath is same as Path unless overriden
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
            MessageBox.Show("Select a RemoteApp first and click this to generate an .rdp file. Implementation in progress.", "Coming Soon");
        }

        private void BtnCreateMsi_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Select a RemoteApp first and click this to generate an MSI installer. Implementation in progress.", "Coming Soon");
        }
    }
}