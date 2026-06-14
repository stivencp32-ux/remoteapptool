using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace RemoteAppTool.App.Views
{
    public partial class HostOptionsView : UserControl
    {
        public HostOptionsView()
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
                string policyKeyString = @"SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
                using (var policyKey = Registry.LocalMachine.OpenSubKey(policyKeyString, false))
                {
                    TxtDisconnectTime.Text = "0";
                    TxtIdleTime.Text = "0";

                    var fDisabledAllowList = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\TSAppAllowList", "fDisabledAllowList", null);
                    if (fDisabledAllowList != null && Convert.ToInt32(fDisabledAllowList) == 1)
                        ChkDisableAllowList.IsChecked = true;
                    else
                        ChkDisableAllowList.IsChecked = false;

                    if (policyKey != null)
                    {
                        var maxDisconnectionTime = policyKey.GetValue("MaxDisconnectionTime", -1);
                        if (Convert.ToInt32(maxDisconnectionTime) != -1)
                        {
                            ChkTimeoutDisconnected.IsChecked = true;
                            TxtDisconnectTime.Text = (Convert.ToInt32(maxDisconnectionTime) / 1000).ToString();
                        }
                        else
                        {
                            ChkTimeoutDisconnected.IsChecked = false;
                        }

                        var maxIdleTime = policyKey.GetValue("MaxIdleTime", -1);
                        if (Convert.ToInt32(maxIdleTime) != -1)
                        {
                            ChkTimeoutIdle.IsChecked = true;
                            TxtIdleTime.Text = (Convert.ToInt32(maxIdleTime) / 1000).ToString();
                        }
                        else
                        {
                            ChkTimeoutIdle.IsChecked = false;
                        }

                        var fResetBroken = policyKey.GetValue("fResetBroken");
                        ChkLogoffWhenTimeout.IsChecked = fResetBroken != null;

                        var fAllowUnlisted = policyKey.GetValue("fAllowUnlistedRemotePrograms");
                        ChkAllowUnlisted.IsChecked = fAllowUnlisted != null;
                    }
                }

                UpdateTextBoxStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read Host Options. Run as Administrator.\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string policyKeyStringMS = @"SOFTWARE\Policies\Microsoft";
                string policyKeyString = @"SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

                using (var msKey = Registry.LocalMachine.OpenSubKey(policyKeyStringMS, true))
                {
                    if (msKey != null)
                    {
                        using (var wntKey = msKey.CreateSubKey("Windows NT"))
                        using (var tsKey = wntKey.CreateSubKey("Terminal Services"))
                        {
                            // Ensure key is created
                        }
                    }
                }

                using (var policyKey = Registry.LocalMachine.OpenSubKey(policyKeyString, true))
                {
                    if (ChkDisableAllowList.IsChecked == true)
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\TSAppAllowList", "fDisabledAllowList", 1, RegistryValueKind.DWord);
                    else
                        Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\TSAppAllowList", "fDisabledAllowList", 0, RegistryValueKind.DWord);

                    if (policyKey != null)
                    {
                        if (ChkAllowUnlisted.IsChecked == true)
                            policyKey.SetValue("fAllowUnlistedRemotePrograms", 1, RegistryValueKind.DWord);
                        else
                            policyKey.DeleteValue("fAllowUnlistedRemotePrograms", false);

                        if (ChkTimeoutDisconnected.IsChecked == true && int.TryParse(TxtDisconnectTime.Text, out int discTime))
                            policyKey.SetValue("MaxDisconnectionTime", discTime * 1000, RegistryValueKind.DWord);
                        else
                            policyKey.DeleteValue("MaxDisconnectionTime", false);

                        if (ChkTimeoutIdle.IsChecked == true && int.TryParse(TxtIdleTime.Text, out int idleTime))
                            policyKey.SetValue("MaxIdleTime", idleTime * 1000, RegistryValueKind.DWord);
                        else
                            policyKey.DeleteValue("MaxIdleTime", false);

                        if (ChkLogoffWhenTimeout.IsChecked == true)
                            policyKey.SetValue("fResetBroken", 1, RegistryValueKind.DWord);
                        else
                            policyKey.DeleteValue("fResetBroken", false);
                    }
                }

                MessageBox.Show("Host Options saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save Host Options. Are you running as Administrator?\n\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChkTimeoutDisconnected_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxStates();
        }

        private void ChkTimeoutIdle_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxStates();
        }

        private void UpdateTextBoxStates()
        {
            if (TxtDisconnectTime != null)
                TxtDisconnectTime.IsEnabled = ChkTimeoutDisconnected.IsChecked == true;
            if (TxtIdleTime != null)
                TxtIdleTime.IsEnabled = ChkTimeoutIdle.IsChecked == true;
        }
    }
}
