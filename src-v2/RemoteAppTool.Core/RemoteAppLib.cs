using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace RemoteAppTool.Core
{
    public class FileTypeAssociation
    {
        public string Extension { get; set; }
        public string IconPath { get; set; }
        public int IconIndex { get; set; }
    }

    public class RemoteApp
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Path { get; set; }
        public string VPath { get; set; }
        public string IconPath { get; set; }
        public int IconIndex { get; set; } = 0;
        public string CommandLine { get; set; } = "";
        public int CommandLineOption { get; set; } = 1;
        public bool TSWA { get; set; } = false;
        public List<FileTypeAssociation> FileTypeAssociations { get; set; } = new List<FileTypeAssociation>();
    }

    public class SystemRemoteApps
    {
        private bool _legacy32bit = false;
        private string _registryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\TSAppAllowList\Applications";
        private RegistryKey _baseKey;
        private RegistryKey _baseKeyWrite;

        public bool WoW6432Node
        {
            get => _legacy32bit;
            set
            {
                _legacy32bit = value;
                string pathStart = _legacy32bit ? @"SOFTWARE\Wow6432Node" : "SOFTWARE";
                _registryPath = $@"{pathStart}\Microsoft\Windows NT\CurrentVersion\Terminal Server\TSAppAllowList\Applications";
            }
        }

        public void Init()
        {
            string registryPathCV = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            using (var cvKey = Registry.LocalMachine.OpenSubKey(registryPathCV, true))
            {
                if (cvKey != null)
                {
                    using (var tsKey = cvKey.CreateSubKey("Terminal Server"))
                    using (var tsaaKey = tsKey.CreateSubKey("TSAppAllowList"))
                    {
                        tsaaKey.CreateSubKey("Applications");
                    }
                }
            }
        }

        private void EnsureKeysOpen()
        {
            _baseKey = Registry.LocalMachine.OpenSubKey(_registryPath);
            _baseKeyWrite = Registry.LocalMachine.OpenSubKey(_registryPath, true);
        }

        public List<RemoteApp> GetAll()
        {
            EnsureKeysOpen();
            var systemAppCollection = new List<RemoteApp>();

            if (_baseKey == null) return systemAppCollection;

            foreach (string appName in _baseKey.GetSubKeyNames())
            {
                var remoteApp = GetApp(appName);
                if (remoteApp != null)
                {
                    systemAppCollection.Add(remoteApp);
                }
            }

            _baseKey.Close();
            return systemAppCollection;
        }

        public RemoteApp GetApp(string name)
        {
            EnsureKeysOpen();
            if (_baseKey == null) return null;

            using (var appKey = _baseKey.OpenSubKey(name))
            {
                if (appKey == null) return null;

                var app = new RemoteApp
                {
                    Name = name,
                    FullName = appKey.GetValue("Name", "")?.ToString(),
                    Path = appKey.GetValue("Path", "")?.ToString(),
                    VPath = appKey.GetValue("VPath", "")?.ToString(),
                    CommandLine = appKey.GetValue("RequiredCommandLine", "")?.ToString(),
                    CommandLineOption = Convert.ToInt32(appKey.GetValue("CommandLineSetting", 1)),
                    IconPath = appKey.GetValue("IconPath", "")?.ToString(),
                    IconIndex = Convert.ToInt32(appKey.GetValue("IconIndex", 0)),
                    TSWA = Convert.ToInt32(appKey.GetValue("ShowInTSWA", 0)) != 0
                };

                using (var ftaKey = appKey.OpenSubKey("Filetypes"))
                {
                    if (ftaKey != null)
                    {
                        foreach (string ftaValueName in ftaKey.GetValueNames())
                        {
                            var ftaValueStr = ftaKey.GetValue(ftaValueName)?.ToString();
                            if (!string.IsNullOrEmpty(ftaValueStr))
                            {
                                var parts = ftaValueStr.Split(',');
                                if (parts.Length >= 2)
                                {
                                    app.FileTypeAssociations.Add(new FileTypeAssociation
                                    {
                                        Extension = ftaValueName,
                                        IconPath = parts[0],
                                        IconIndex = int.TryParse(parts[1], out int idx) ? idx : 0
                                    });
                                }
                            }
                        }
                    }
                }

                return app;
            }
        }

        public void SaveApp(RemoteApp remoteApp)
        {
            EnsureKeysOpen();
            if (_baseKeyWrite == null) return;

            _baseKeyWrite.CreateSubKey(remoteApp.Name);
            using (var appKey = _baseKeyWrite.OpenSubKey(remoteApp.Name, true))
            {
                if (appKey == null) return;

                appKey.SetValue("Name", remoteApp.FullName ?? "", RegistryValueKind.String);
                appKey.SetValue("Path", remoteApp.Path ?? "", RegistryValueKind.String);
                appKey.SetValue("VPath", remoteApp.VPath ?? "", RegistryValueKind.String);
                appKey.SetValue("RequiredCommandLine", remoteApp.CommandLine ?? "", RegistryValueKind.String);
                appKey.SetValue("CommandLineSetting", remoteApp.CommandLineOption, RegistryValueKind.DWord);
                appKey.SetValue("IconPath", remoteApp.IconPath ?? "", RegistryValueKind.String);
                appKey.SetValue("IconIndex", remoteApp.IconIndex, RegistryValueKind.DWord);
                appKey.SetValue("ShowInTSWA", remoteApp.TSWA ? 1 : 0, RegistryValueKind.DWord);

                if (remoteApp.FileTypeAssociations != null && remoteApp.FileTypeAssociations.Count > 0)
                {
                    appKey.DeleteSubKeyTree("Filetypes", throwOnMissingSubKey: false);
                    using (var ftaKey = appKey.CreateSubKey("Filetypes"))
                    {
                        foreach (var fta in remoteApp.FileTypeAssociations)
                        {
                            ftaKey.SetValue(fta.Extension, $"{fta.IconPath},{fta.IconIndex}", RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        public void DuplicateApp(string name)
        {
            var newApp = GetApp(name);
            if (newApp == null) return;

            string newName = newApp.Name;
            while (GetApp(newName) != null)
            {
                newName += " copy";
            }

            newApp.Name = newName;
            SaveApp(newApp);
        }

        public void RenameApp(string remoteAppOldName, string remoteAppNewName)
        {
            var app = GetApp(remoteAppOldName);
            if (app != null)
            {
                DeleteApp(remoteAppOldName);
                app.Name = remoteAppNewName;
                SaveApp(app);
            }
        }

        public void DeleteApp(string name)
        {
            EnsureKeysOpen();
            _baseKeyWrite?.DeleteSubKeyTree(name, throwOnMissingSubKey: false);
        }
    }
}
