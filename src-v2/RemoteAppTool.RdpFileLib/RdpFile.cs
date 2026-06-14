using System;
using System.IO;
using System.Text;

namespace RemoteAppTool.RdpFileLib
{
    public class RdpFile
    {
        public int AdministrativeSession { get; set; } = 0;
        public int AllowDesktopComposition { get; set; } = 0;
        public int AllowFontSmoothing { get; set; } = 0;
        public string AlternateFullAddress { get; set; } = "";
        public string AlternateShell { get; set; } = "";
        public int AudioCaptureMode { get; set; } = 0;
        public int AudioMode { get; set; } = 0;
        public int AudioQualityMode { get; set; } = 0;
        public int AuthenticationLevel { get; set; } = 2;
        public int AutoReconnectMaxRetries { get; set; } = 20;
        public int AutoReconnectionEnabled { get; set; } = 1;
        public int BandwidthAutoDetect { get; set; } = 1;
        public int BitmapCachePersistEnable { get; set; } = 1;
        public int BitmapCacheSize { get; set; } = 1500;
        public string CameraStoreDirect { get; set; } = "";
        public int Compression { get; set; } = 1;
        public int ConnectToConsole { get; set; } = 0;
        public int ConnectionType { get; set; } = 2;
        public int DesktopSizeId { get; set; } = 0;
        public int DesktopHeight { get; set; } = 600;
        public int DesktopWidth { get; set; } = 800;
        public string DeviceStoreDirect { get; set; } = "";
        public int DisableCtrlAltDel { get; set; } = 1;
        public int DisableFullWindowDrag { get; set; } = 1;
        public int DisableMenuAnims { get; set; } = 1;
        public int DisableThemes { get; set; } = 0;
        public int DisableWallpaper { get; set; } = 1;
        public int DisableConnectionSharing { get; set; } = 0;
        public int DisableRemoteAppCapsCheck { get; set; } = 0;
        public int DisplayConnectionBar { get; set; } = 1;
        public string Domain { get; set; } = "";
        public string DriveStoreDirect { get; set; } = "";
        public int EnableCredSspSupport { get; set; } = 1;
        public int EnableSuperPan { get; set; } = 0;
        public int EncodeRedirectedVideoCapture { get; set; } = 1;
        public string FullAddress { get; set; } = "";
        public int GatewayCredentialsSource { get; set; } = 4;
        public string GatewayHostname { get; set; } = "";
        public int GatewayProfileUsageMethod { get; set; } = 0;
        public int GatewayUsageMethod { get; set; } = 4;
        public int KeyboardHook { get; set; } = 2;
        public int NegotiateSecurityLayer { get; set; } = 1;
        public int NetworkAutoDetect { get; set; } = 1;
        public int PinConnectionBar { get; set; } = 1;
        public int PromptForCredentials { get; set; } = 0;
        public int PromptForCredentialsOnClient { get; set; } = 0;
        public int PromptCredentialOnce { get; set; } = 1;
        public int PublicMode { get; set; } = 0;
        public int RedirectClipboard { get; set; } = 1;
        public int RedirectComPorts { get; set; } = 0;
        public int RedirectDirectX { get; set; } = 1;
        public int RedirectDrives { get; set; } = 0;
        public int RedirectedVideoCaptureEncodingQuality { get; set; } = 0;
        public int RedirectLocation { get; set; } = 0;
        public int RedirectPosDevices { get; set; } = 0;
        public int RedirectPrinters { get; set; } = 1;
        public int RedirectSmartCards { get; set; } = 1;
        public int RedirectWebAuthn { get; set; } = 1;
        public string RemoteApplicationCmdLine { get; set; } = "";
        public int RemoteApplicationExpandCmdLine { get; set; } = 1;
        public int RemoteApplicationExpandWorkingDir { get; set; } = 0;
        public string RemoteApplicationFile { get; set; } = "";
        public string RemoteApplicationFileExtensions { get; set; } = "";
        public string RemoteApplicationIcon { get; set; } = "";
        public int RemoteApplicationMode { get; set; } = 0;
        public string RemoteApplicationName { get; set; } = "";
        public string RemoteApplicationProgram { get; set; } = "";
        public int ScreenModeId { get; set; } = 2;
        public string SelectedMonitors { get; set; } = "";
        public int ServerPort { get; set; } = 3389;
        public int SessionBpp { get; set; } = 32;
        public string ShellWorkingDirectory { get; set; } = "";
        public string SignScope { get; set; } = "";
        public int SmartSizing { get; set; } = 0;
        public int SpanMonitors { get; set; } = 0;
        public int SuperPanAccelerationFactor { get; set; } = 1;
        public string UsbDeviceStoreDirect { get; set; } = "";
        public int UseMultimon { get; set; } = 0;
        public string Username { get; set; } = "";
        public int VideoPlaybackMode { get; set; } = 1;
        public string WinPosStr { get; set; } = "0,3,0,0,800,600";
        public string WorkspaceId { get; set; } = "";

        public string AdditionalOptions { get; set; } = "";

        public string GetRdpString(bool includeDefaultSettings = false)
        {
            var sb = new StringBuilder();
            var defaultRdp = new RdpFile();

            Action<string, object, object> appendIfDifferent = (key, currentVal, defaultVal) =>
            {
                if (includeDefaultSettings || !currentVal.Equals(defaultVal))
                {
                    string typeCode = currentVal is int ? "i" : "s";
                    sb.AppendLine($"{key}:{typeCode}:{currentVal}");
                }
            };

            appendIfDifferent("administrative session", AdministrativeSession, defaultRdp.AdministrativeSession);
            appendIfDifferent("allow desktop composition", AllowDesktopComposition, defaultRdp.AllowDesktopComposition);
            appendIfDifferent("allow font smoothing", AllowFontSmoothing, defaultRdp.AllowFontSmoothing);
            appendIfDifferent("alternate full address", AlternateFullAddress, defaultRdp.AlternateFullAddress);
            appendIfDifferent("alternate shell", AlternateShell, defaultRdp.AlternateShell);
            appendIfDifferent("audiocapturemode", AudioCaptureMode, defaultRdp.AudioCaptureMode);
            appendIfDifferent("audiomode", AudioMode, defaultRdp.AudioMode);
            appendIfDifferent("audioqualitymode", AudioQualityMode, defaultRdp.AudioQualityMode);
            appendIfDifferent("authentication level", AuthenticationLevel, defaultRdp.AuthenticationLevel);
            appendIfDifferent("autoreconnect max retries", AutoReconnectMaxRetries, defaultRdp.AutoReconnectMaxRetries);
            appendIfDifferent("autoreconnection enabled", AutoReconnectionEnabled, defaultRdp.AutoReconnectionEnabled);
            appendIfDifferent("bandwidthautodetect", BandwidthAutoDetect, defaultRdp.BandwidthAutoDetect);
            appendIfDifferent("bitmapcachepersistenable", BitmapCachePersistEnable, defaultRdp.BitmapCachePersistEnable);
            appendIfDifferent("bitmapcachesize", BitmapCacheSize, defaultRdp.BitmapCacheSize);
            appendIfDifferent("camerastoredirect", CameraStoreDirect, defaultRdp.CameraStoreDirect);
            appendIfDifferent("compression", Compression, defaultRdp.Compression);
            appendIfDifferent("connect to console", ConnectToConsole, defaultRdp.ConnectToConsole);
            appendIfDifferent("connection type", ConnectionType, defaultRdp.ConnectionType);
            appendIfDifferent("desktop size id", DesktopSizeId, defaultRdp.DesktopSizeId);
            appendIfDifferent("desktopheight", DesktopHeight, defaultRdp.DesktopHeight);
            appendIfDifferent("desktopwidth", DesktopWidth, defaultRdp.DesktopWidth);
            appendIfDifferent("devicestoredirect", DeviceStoreDirect, defaultRdp.DeviceStoreDirect);
            appendIfDifferent("disable ctrl+alt+del", DisableCtrlAltDel, defaultRdp.DisableCtrlAltDel);
            appendIfDifferent("disable full window drag", DisableFullWindowDrag, defaultRdp.DisableFullWindowDrag);
            appendIfDifferent("disable menu anims", DisableMenuAnims, defaultRdp.DisableMenuAnims);
            appendIfDifferent("disable themes", DisableThemes, defaultRdp.DisableThemes);
            appendIfDifferent("disable wallpaper", DisableWallpaper, defaultRdp.DisableWallpaper);
            appendIfDifferent("disableconnectionsharing", DisableConnectionSharing, defaultRdp.DisableConnectionSharing);
            appendIfDifferent("disableremoteappcapscheck", DisableRemoteAppCapsCheck, defaultRdp.DisableRemoteAppCapsCheck);
            appendIfDifferent("displayconnectionbar", DisplayConnectionBar, defaultRdp.DisplayConnectionBar);
            appendIfDifferent("domain", Domain, defaultRdp.Domain);
            appendIfDifferent("drivestoredirect", DriveStoreDirect, defaultRdp.DriveStoreDirect);
            appendIfDifferent("enablecredsspsupport", EnableCredSspSupport, defaultRdp.EnableCredSspSupport);
            appendIfDifferent("enablesuperpan", EnableSuperPan, defaultRdp.EnableSuperPan);
            appendIfDifferent("encode redirected video capture", EncodeRedirectedVideoCapture, defaultRdp.EncodeRedirectedVideoCapture);
            appendIfDifferent("full address", FullAddress, defaultRdp.FullAddress);
            appendIfDifferent("gatewaycredentialssource", GatewayCredentialsSource, defaultRdp.GatewayCredentialsSource);
            appendIfDifferent("gatewayhostname", GatewayHostname, defaultRdp.GatewayHostname);
            appendIfDifferent("gatewayprofileusagemethod", GatewayProfileUsageMethod, defaultRdp.GatewayProfileUsageMethod);
            appendIfDifferent("gatewayusagemethod", GatewayUsageMethod, defaultRdp.GatewayUsageMethod);
            appendIfDifferent("keyboardhook", KeyboardHook, defaultRdp.KeyboardHook);
            appendIfDifferent("negotiate security layer", NegotiateSecurityLayer, defaultRdp.NegotiateSecurityLayer);
            appendIfDifferent("networkautodetect", NetworkAutoDetect, defaultRdp.NetworkAutoDetect);
            appendIfDifferent("pinconnectionbar", PinConnectionBar, defaultRdp.PinConnectionBar);
            appendIfDifferent("prompt for credentials", PromptForCredentials, defaultRdp.PromptForCredentials);
            appendIfDifferent("prompt for credentials on client", PromptForCredentialsOnClient, defaultRdp.PromptForCredentialsOnClient);
            appendIfDifferent("promptcredentialonce", PromptCredentialOnce, defaultRdp.PromptCredentialOnce);
            appendIfDifferent("public mode", PublicMode, defaultRdp.PublicMode);
            appendIfDifferent("redirectclipboard", RedirectClipboard, defaultRdp.RedirectClipboard);
            appendIfDifferent("redirectcomports", RedirectComPorts, defaultRdp.RedirectComPorts);
            appendIfDifferent("redirectdirectx", RedirectDirectX, defaultRdp.RedirectDirectX);
            appendIfDifferent("redirectdrives", RedirectDrives, defaultRdp.RedirectDrives);
            appendIfDifferent("redirected video capture encoding quality", RedirectedVideoCaptureEncodingQuality, defaultRdp.RedirectedVideoCaptureEncodingQuality);
            appendIfDifferent("redirectlocation", RedirectLocation, defaultRdp.RedirectLocation);
            appendIfDifferent("redirectposdevices", RedirectPosDevices, defaultRdp.RedirectPosDevices);
            appendIfDifferent("redirectprinters", RedirectPrinters, defaultRdp.RedirectPrinters);
            appendIfDifferent("redirectsmartcards", RedirectSmartCards, defaultRdp.RedirectSmartCards);
            appendIfDifferent("redirectwebauthn", RedirectWebAuthn, defaultRdp.RedirectWebAuthn);
            appendIfDifferent("remoteapplicationcmdline", RemoteApplicationCmdLine, defaultRdp.RemoteApplicationCmdLine);
            appendIfDifferent("remoteapplicationexpandcmdline", RemoteApplicationExpandCmdLine, defaultRdp.RemoteApplicationExpandCmdLine);
            appendIfDifferent("remoteapplicationexpandworkingdir", RemoteApplicationExpandWorkingDir, defaultRdp.RemoteApplicationExpandWorkingDir);
            appendIfDifferent("remoteapplicationfile", RemoteApplicationFile, defaultRdp.RemoteApplicationFile);
            appendIfDifferent("remoteapplicationfileextensions", RemoteApplicationFileExtensions, defaultRdp.RemoteApplicationFileExtensions);
            appendIfDifferent("remoteapplicationicon", RemoteApplicationIcon, defaultRdp.RemoteApplicationIcon);
            appendIfDifferent("remoteapplicationmode", RemoteApplicationMode, defaultRdp.RemoteApplicationMode);
            appendIfDifferent("remoteapplicationname", RemoteApplicationName, defaultRdp.RemoteApplicationName);
            appendIfDifferent("remoteapplicationprogram", RemoteApplicationProgram, defaultRdp.RemoteApplicationProgram);
            appendIfDifferent("screen mode id", ScreenModeId, defaultRdp.ScreenModeId);
            appendIfDifferent("selectedmonitors", SelectedMonitors, defaultRdp.SelectedMonitors);
            appendIfDifferent("server port", ServerPort, defaultRdp.ServerPort);
            appendIfDifferent("session bpp", SessionBpp, defaultRdp.SessionBpp);
            appendIfDifferent("shell working directory", ShellWorkingDirectory, defaultRdp.ShellWorkingDirectory);
            appendIfDifferent("signscope", SignScope, defaultRdp.SignScope);
            appendIfDifferent("smart sizing", SmartSizing, defaultRdp.SmartSizing);
            appendIfDifferent("span monitors", SpanMonitors, defaultRdp.SpanMonitors);
            appendIfDifferent("superpanaccelerationfactor", SuperPanAccelerationFactor, defaultRdp.SuperPanAccelerationFactor);
            appendIfDifferent("usbdevicestoredirect", UsbDeviceStoreDirect, defaultRdp.UsbDeviceStoreDirect);
            appendIfDifferent("use multimon", UseMultimon, defaultRdp.UseMultimon);
            appendIfDifferent("username", Username, defaultRdp.Username);
            appendIfDifferent("videoplaybackmode", VideoPlaybackMode, defaultRdp.VideoPlaybackMode);
            appendIfDifferent("winposstr", WinPosStr, defaultRdp.WinPosStr);
            appendIfDifferent("workspaceid", WorkspaceId, defaultRdp.WorkspaceId);

            if (!string.IsNullOrEmpty(AdditionalOptions))
            {
                sb.AppendLine(AdditionalOptions);
            }

            return sb.ToString();
        }

        public void LoadRdpFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var split = line.Split(':');
                if (split.Length < 3) continue;

                string key = split[0];
                string type = split[1];
                string val = string.Join(":", split, 2, split.Length - 2);

                if (string.IsNullOrEmpty(val)) continue;

                Action<Action<int>> setInt = (setter) => { if (int.TryParse(val, out int i)) setter(i); };

                switch (key)
                {
                    case "administrative session": setInt(v => AdministrativeSession = v); break;
                    case "allow desktop composition": setInt(v => AllowDesktopComposition = v); break;
                    case "allow font smoothing": setInt(v => AllowFontSmoothing = v); break;
                    case "alternate full address": AlternateFullAddress = val; break;
                    case "alternate shell": AlternateShell = val; break;
                    case "audiocapturemode": setInt(v => AudioCaptureMode = v); break;
                    case "audiomode": setInt(v => AudioMode = v); break;
                    case "audioqualitymode": setInt(v => AudioQualityMode = v); break;
                    case "authentication level": setInt(v => AuthenticationLevel = v); break;
                    case "autoreconnect max retries": setInt(v => AutoReconnectMaxRetries = v); break;
                    case "autoreconnection enabled": setInt(v => AutoReconnectionEnabled = v); break;
                    case "bandwidthautodetect": setInt(v => BandwidthAutoDetect = v); break;
                    case "bitmapcachepersistenable": setInt(v => BitmapCachePersistEnable = v); break;
                    case "bitmapcachesize": setInt(v => BitmapCacheSize = v); break;
                    case "camerastoredirect": CameraStoreDirect = val; break;
                    case "compression": setInt(v => Compression = v); break;
                    case "connect to console": setInt(v => ConnectToConsole = v); break;
                    case "connection type": setInt(v => ConnectionType = v); break;
                    case "desktop size id": setInt(v => DesktopSizeId = v); break;
                    case "desktopheight": setInt(v => DesktopHeight = v); break;
                    case "desktopwidth": setInt(v => DesktopWidth = v); break;
                    case "devicestoredirect": DeviceStoreDirect = val; break;
                    case "disable ctrl+alt+del": setInt(v => DisableCtrlAltDel = v); break;
                    case "disable full window drag": setInt(v => DisableFullWindowDrag = v); break;
                    case "disable menu anims": setInt(v => DisableMenuAnims = v); break;
                    case "disable themes": setInt(v => DisableThemes = v); break;
                    case "disable wallpaper": setInt(v => DisableWallpaper = v); break;
                    case "disableconnectionsharing": setInt(v => DisableConnectionSharing = v); break;
                    case "disableremoteappcapscheck": setInt(v => DisableRemoteAppCapsCheck = v); break;
                    case "displayconnectionbar": setInt(v => DisplayConnectionBar = v); break;
                    case "domain": Domain = val; break;
                    case "drivestoredirect": DriveStoreDirect = val; break;
                    case "enablecredsspsupport": setInt(v => EnableCredSspSupport = v); break;
                    case "enablesuperpan": setInt(v => EnableSuperPan = v); break;
                    case "encode redirected video capture": setInt(v => EncodeRedirectedVideoCapture = v); break;
                    case "full address": FullAddress = val; break;
                    case "gatewaycredentialssource": setInt(v => GatewayCredentialsSource = v); break;
                    case "gatewayhostname": GatewayHostname = val; break;
                    case "gatewayprofileusagemethod": setInt(v => GatewayProfileUsageMethod = v); break;
                    case "gatewayusagemethod": setInt(v => GatewayUsageMethod = v); break;
                    case "keyboardhook": setInt(v => KeyboardHook = v); break;
                    case "negotiate security layer": setInt(v => NegotiateSecurityLayer = v); break;
                    case "networkautodetect": setInt(v => NetworkAutoDetect = v); break;
                    case "pinconnectionbar": setInt(v => PinConnectionBar = v); break;
                    case "prompt for credentials": setInt(v => PromptForCredentials = v); break;
                    case "prompt for credentials on client": setInt(v => PromptForCredentialsOnClient = v); break;
                    case "promptcredentialonce": setInt(v => PromptCredentialOnce = v); break;
                    case "public mode": setInt(v => PublicMode = v); break;
                    case "redirectclipboard": setInt(v => RedirectClipboard = v); break;
                    case "redirectcomports": setInt(v => RedirectComPorts = v); break;
                    case "redirectdirectx": setInt(v => RedirectDirectX = v); break;
                    case "redirectdrives": setInt(v => RedirectDrives = v); break;
                    case "redirected video capture encoding quality": setInt(v => RedirectedVideoCaptureEncodingQuality = v); break;
                    case "redirectlocation": setInt(v => RedirectLocation = v); break;
                    case "redirectposdevices": setInt(v => RedirectPosDevices = v); break;
                    case "redirectprinters": setInt(v => RedirectPrinters = v); break;
                    case "redirectsmartcards": setInt(v => RedirectSmartCards = v); break;
                    case "redirectwebauthn": setInt(v => RedirectWebAuthn = v); break;
                    case "remoteapplicationcmdline": RemoteApplicationCmdLine = val; break;
                    case "remoteapplicationexpandcmdline": setInt(v => RemoteApplicationExpandCmdLine = v); break;
                    case "remoteapplicationexpandworkingdir": setInt(v => RemoteApplicationExpandWorkingDir = v); break;
                    case "remoteapplicationfile": RemoteApplicationFile = val; break;
                    case "remoteapplicationfileextensions": RemoteApplicationFileExtensions = val; break;
                    case "remoteapplicationicon": RemoteApplicationIcon = val; break;
                    case "remoteapplicationmode": setInt(v => RemoteApplicationMode = v); break;
                    case "remoteapplicationname": RemoteApplicationName = val; break;
                    case "remoteapplicationprogram": RemoteApplicationProgram = val; break;
                    case "screen mode id": setInt(v => ScreenModeId = v); break;
                    case "selectedmonitors": SelectedMonitors = val; break;
                    case "server port": setInt(v => ServerPort = v); break;
                    case "session bpp": setInt(v => SessionBpp = v); break;
                    case "shell working directory": ShellWorkingDirectory = val; break;
                    case "signscope": SignScope = val; break;
                    case "smart sizing": setInt(v => SmartSizing = v); break;
                    case "span monitors": setInt(v => SpanMonitors = v); break;
                    case "superpanaccelerationfactor": setInt(v => SuperPanAccelerationFactor = v); break;
                    case "usbdevicestoredirect": UsbDeviceStoreDirect = val; break;
                    case "use multimon": setInt(v => UseMultimon = v); break;
                    case "username": Username = val; break;
                    case "videoplaybackmode": setInt(v => VideoPlaybackMode = v); break;
                    case "winposstr": WinPosStr = val; break;
                    case "workspaceid": WorkspaceId = val; break;
                }
            }
        }
        
        public void SaveRdpFile(string filePath, bool includeDefaultSettings = false)
        {
            // Simple lock checking logic could go here
            File.WriteAllText(filePath, GetRdpString(includeDefaultSettings));
        }
    }
}
