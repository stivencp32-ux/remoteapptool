using System.IO;
using System.Text.Json;
using System;

namespace RemoteAppTool.Core
{
    public class ClientSettings
    {
        public string ServerAddress { get; set; } = Environment.MachineName;
        public string ServerPort { get; set; } = "3389";
        public string Gateway { get; set; } = "";
        public bool ConnectAsAdmin { get; set; } = false;
        public bool RedirectClipboard { get; set; } = true;
        public bool RedirectPrinters { get; set; } = true;
        public bool RedirectDrives { get; set; } = false;
        public string AdvancedOptions { get; set; } = "";

        private static string SettingsFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RemoteAppTool", "ClientSettings.json");

        public static ClientSettings Load()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    var settings = JsonSerializer.Deserialize<ClientSettings>(json);
                    return settings ?? new ClientSettings();
                }
            }
            catch { }
            return new ClientSettings();
        }

        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(SettingsFilePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFilePath, json);
            }
            catch { }
        }
    }
}
