using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RemoteAppTool.Core
{
    public class Rdp2Msi
    {
        private string _rdpFilePath;
        public string RdpPath
        {
            get => _rdpFilePath;
            set
            {
                _rdpFilePath = value;
                ReadRdpFile();
            }
        }

        private string ProductName;
        public string FlatFileTypes = "";
        public bool PerUser = false;
        private string ProductBaseFileName;
        public string ProductPublisher;
        public string ProductVersion = "1.0.0.0";
        public bool ProductUpgradeRandom = false;
        public string ProductRemoteTag = "remote";
        public bool ShortcutInStart = true;
        public bool ShortcutSubfolderInStart = true;
        public bool ShortcutOnDesktop = true;

        private string _rdpFileContents;
        private bool _hasIcon;
        private bool _rdpInTemp;

        public void CreateMsi(string destinationPath = "")
        {
            if (!WixInstalled()) return;

            int lastSlashIndex = _rdpFilePath.LastIndexOf('\\');
            string rdpParentFolder = _rdpFilePath.Substring(0, lastSlashIndex);

            string rdpFileName = Path.GetFileName(_rdpFilePath);
            ProductBaseFileName = Path.GetFileNameWithoutExtension(rdpFileName);

            if (string.IsNullOrEmpty(destinationPath))
            {
                destinationPath = Path.Combine(rdpParentFolder, ProductBaseFileName + ".msi");
            }

            string iconFilePath = Path.Combine(rdpParentFolder, ProductBaseFileName + ".ico");
            _hasIcon = File.Exists(iconFilePath);

            string tempPath = Path.GetTempPath();
            string wxsPath = Path.Combine(tempPath, ProductBaseFileName + ".wxs");
            string wixobjPath = Path.Combine(tempPath, ProductBaseFileName + ".wixobj");
            string wixpdbPath = Path.Combine(tempPath, ProductBaseFileName + ".wixpdb");
            string msiPath = Path.Combine(tempPath, ProductBaseFileName + ".msi");

            string rdpTempPath = Path.Combine(tempPath, ProductBaseFileName + ".rdp");
            string icoTempPath = Path.Combine(tempPath, ProductBaseFileName + ".ico");

            var filesToDelete = new List<string> { wxsPath, wixobjPath, wixpdbPath };

            if (rdpParentFolder.TrimEnd('\\') == tempPath.TrimEnd('\\'))
                _rdpInTemp = true;

            if (!_rdpInTemp)
            {
                CheckLockAndCopy(rdpTempPath, _rdpFilePath);
                filesToDelete.Add(rdpTempPath);

                if (_hasIcon)
                {
                    CheckLockAndCopy(icoTempPath, iconFilePath);
                    filesToDelete.Add(icoTempPath);
                }
            }

            File.WriteAllText(wxsPath, GenerateWxsString());

            string candlePath = Path.Combine(WixPath(), "candle.exe");
            string lightPath = Path.Combine(WixPath(), "light.exe");

            RunWait(candlePath, $"-out \"{wixobjPath}\" \"{wxsPath}\"");
            RunWait(lightPath, $"-out \"{msiPath}\" \"{wixobjPath}\"");

            string lockRes = LockChecker.CheckLock(destinationPath);
            if (lockRes != "No locks")
            {
                throw new IOException($"Destination MSI file is locked: {lockRes}");
            }

            if (File.Exists(msiPath))
            {
                if (File.Exists(destinationPath)) File.Delete(destinationPath);
                File.Move(msiPath, destinationPath);
            }

            DeleteFiles(filesToDelete);
        }

        private void CheckLockAndCopy(string dest, string src)
        {
            string lockRes = LockChecker.CheckLock(dest);
            if (lockRes != "No locks")
            {
                throw new IOException($"File is locked: {lockRes}");
            }
            File.Copy(src, dest, true);
        }

        public bool WixInstalled()
        {
            return !string.IsNullOrEmpty(WixPath());
        }

        private string WixPath()
        {
            string searchExe = "candle.exe";
            string envWix = Environment.GetEnvironmentVariable("WIX");

            if (!string.IsNullOrEmpty(envWix))
            {
                return Path.Combine(envWix, "bin");
            }
            
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(Path.Combine(baseDir, "wix", searchExe)))
                return Path.Combine(baseDir, "wix");
            
            if (File.Exists(Path.Combine(baseDir, "wix", "bin", searchExe)))
                return Path.Combine(baseDir, "wix", "bin");

            return string.Empty;
        }

        private void DeleteFiles(List<string> filesArray)
        {
            foreach (var dFile in filesArray)
            {
                if (LockChecker.CheckLock(dFile) == "No locks")
                {
                    if (File.Exists(dFile)) File.Delete(dFile);
                }
            }
        }

        private void ReadRdpFile()
        {
            if (!File.Exists(_rdpFilePath) || !_rdpFilePath.ToLower().EndsWith(".rdp")) return;

            _rdpFileContents = File.ReadAllText(_rdpFilePath);

            if (string.IsNullOrEmpty(ReadRdpProperty("full address"))) return;

            ProductName = ReadRdpProperty("remoteapplicationname");

            if (string.IsNullOrEmpty(ProductName))
                ProductName = Path.GetFileNameWithoutExtension(_rdpFilePath);

            if (string.IsNullOrEmpty(ProductPublisher))
                ProductPublisher = ProductName;
        }

        public string ProductUpgradeCode()
        {
            if (!ProductUpgradeRandom)
                return GenerateGuidFromString(ProductName);

            var rnd = new Random();
            return GenerateGuidFromString(rnd.Next().ToString());
        }

        public string MakeProgId(string appName)
        {
            var rx = new Regex("[^a-zA-Z0-9_]");
            return rx.Replace(appName, "_");
        }

        private string GenerateWxsString()
        {
            if (string.IsNullOrEmpty(ProductPublisher)) ProductPublisher = ProductName;

            string regRoot = PerUser ? "HKCU" : "HKLM";

            string appFilesGuid = GenerateGuidFromString("AppFiles" + ProductUpgradeCode());
            string appStartShortcutsGuid = GenerateGuidFromString("AppStartShortcuts" + ProductUpgradeCode());
            string appDeskShortcutsGuid = GenerateGuidFromString("AppDeskShortcuts" + ProductUpgradeCode());

            string progId = MakeProgId(ProductBaseFileName);

            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine($"<?define ProductVersion = \"{ProductVersion}\"?>");
            sb.AppendLine($"<?define ProductUpgradeCode = \"{ProductUpgradeCode()}\"?>");
            sb.AppendLine($"<?define AppFilesGuid = \"{appFilesGuid}\"?>");
            sb.AppendLine($"<?define AppStartShortcutsGuid = \"{appStartShortcutsGuid}\"?>");
            sb.AppendLine($"<?define AppDeskShortcutsGuid = \"{appDeskShortcutsGuid}\"?>");
            sb.AppendLine($"<?define ProductName = \"{ProductName}\"?>");
            sb.AppendLine($"<?define ProductPublisher = \"{ProductPublisher}\"?>");
            sb.AppendLine($"<?define ProductBaseFileName = \"{ProductBaseFileName}\"?>");
            sb.AppendLine($"<?define RegRoot = \"{regRoot}\"?>");
            
            if (!string.IsNullOrEmpty(FlatFileTypes)) sb.AppendLine($"<?define ProductProgID = \"{progId}\"?>");
            
            string productRemoteTagStr = !string.IsNullOrEmpty(ProductRemoteTag) ? $" ({ProductRemoteTag})" : "";
            sb.AppendLine($"<?define ProductRemoteTag = \"{productRemoteTagStr}\"?>");
            sb.AppendLine("<Wix xmlns=\"http://schemas.microsoft.com/wix/2006/wi\">");

            sb.AppendLine("   <Product Id=\"*\" UpgradeCode=\"$(var.ProductUpgradeCode)\" ");
            sb.AppendLine("            Name=\"$(var.ProductName)$(var.ProductRemoteTag)\" Version=\"$(var.ProductVersion)\" Manufacturer=\"$(var.ProductPublisher)\" Language=\"1033\">");
            sb.AppendLine("      <Package InstallerVersion=\"200\" Compressed=\"yes\" Comments=\"Windows Installer Package\"");
            
            if (!PerUser) sb.AppendLine(" InstallScope=\"perMachine\"/>");
            else sb.AppendLine(" InstallPrivileges=\"limited\"/>");

            sb.AppendLine("      <Media Id=\"1\" Cabinet=\"rdp2msi.cab\" EmbedCab=\"yes\"/>");
            if (!PerUser) sb.AppendLine("      <Property Id=\"AllUSERS\" Value=\"1\"/>");
            
            sb.AppendLine("      <Upgrade Id=\"$(var.ProductUpgradeCode)\">");
            sb.AppendLine("         <UpgradeVersion Minimum=\"$(var.ProductVersion)\" OnlyDetect=\"yes\" Property=\"NEWERVERSIONDETECTED\"/>");
            sb.AppendLine("         <UpgradeVersion Minimum=\"0.0.0\" Maximum=\"$(var.ProductVersion)\" IncludeMinimum=\"yes\" IncludeMaximum=\"no\" Property=\"OLDERVERSIONBEINGUPGRADED\"/>");
            sb.AppendLine("      </Upgrade>");
            sb.AppendLine("      <Condition Message=\"A newer version of this software is already installed.\">NOT NEWERVERSIONDETECTED</Condition>");
            sb.AppendLine("      <Property Id=\"MstscProperty\" Value=\"mstsc.exe\"/>");
            sb.AppendLine("      <Directory Id=\"TARGETDIR\" Name=\"SourceDir\">");
            
            if (!PerUser) sb.AppendLine("         <Directory Id=\"ProgramFilesFolder\">");
            else sb.AppendLine("         <Directory Id=\"LocalAppDataFolder\">");

            sb.AppendLine("            <Directory Id=\"INSTALLDIR\" Name=\"RemotePackages\">");
            sb.AppendLine("               <Component Id=\"ApplicationFiles\" Guid=\"$(var.AppFilesGuid)\">");
            sb.AppendLine("                  <File Id=\"rdpFile1\" Source=\"$(var.ProductBaseFileName).rdp\"/>");
            
            if (_hasIcon) sb.AppendLine("                  <File Id=\"rdpIcon1\" Source=\"$(var.ProductBaseFileName).ico\"/>");

            if (!string.IsNullOrEmpty(FlatFileTypes))
            {
                foreach (var fileType in FlatFileTypes.Replace(".", "").Split(','))
                {
                    sb.AppendLine($"                  <File Id=\"$(var.ProductProgID).{fileType}.ico\" Source=\"$(var.ProductBaseFileName).{fileType}.ico\" />");
                    sb.AppendLine($"                  <ProgId Id=\"remote.$(var.ProductProgID).{fileType}file\" Description=\"$(var.ProductName) {fileType} file\" Icon=\"$(var.ProductProgID).{fileType}.ico\">");
                    sb.AppendLine($"                  <Extension Id=\"{fileType}\" ContentType=\"application/{fileType}\">");
                    sb.AppendLine("                  <Verb Id=\"open\" Command=\"Open\" TargetProperty='MstscProperty' Argument='/REMOTEFILE:\"%1\" \"[INSTALLDIR]$(var.ProductBaseFileName).rdp\"' />");
                    sb.AppendLine("                  </Extension>");
                    sb.AppendLine("                  </ProgId>");
                }
            }

            sb.AppendLine("               </Component>");
            sb.AppendLine("            </Directory>");
            sb.AppendLine("         </Directory>");

            if (ShortcutInStart)
            {
                sb.AppendLine("         <Directory Id=\"ProgramMenuFolder\">");
                if (ShortcutSubfolderInStart) sb.AppendLine("            <Directory Id=\"ProgramMenuSubfolder\" Name=\"$(var.ProductName)$(var.ProductRemoteTag)\">");
                sb.AppendLine("               <Component Id=\"ApplicationStartShortcuts\" Guid=\"$(var.AppStartShortcutsGuid)\">");
                sb.AppendLine("                  <Shortcut Id=\"rdpStartShortcut1\" Name=\"$(var.ProductName)$(var.ProductRemoteTag)\" Description=\"$(var.ProductName)$(var.ProductRemoteTag)\" Target=\"[INSTALLDIR]$(var.ProductBaseFileName).rdp\" WorkingDirectory=\"INSTALLDIR\"");

                if (_hasIcon)
                {
                    sb.AppendLine(" Icon=\"rdpStartIcon.rdp\" IconIndex=\"0\">");
                    sb.AppendLine("                            <Icon Id=\"rdpStartIcon.rdp\" SourceFile=\"$(var.ProductBaseFileName).ico\" />");
                    sb.AppendLine("                  </Shortcut>");
                }
                else
                {
                    sb.AppendLine("/>");
                }

                sb.AppendLine("                  <RegistryValue Root=\"$(var.RegRoot)\" Key=\"Software\\RDP2MSI\\$(var.ProductName)\" Name=\"installed\" Type=\"integer\" Value=\"1\" KeyPath=\"yes\"/>");
                sb.AppendLine("                  <RemoveFolder Id=\"ProgramMenuSubfolder\" On=\"uninstall\"/>");
                sb.AppendLine("               </Component>");
                if (ShortcutSubfolderInStart) sb.AppendLine("            </Directory>");
                sb.AppendLine("         </Directory>");
            }

            if (ShortcutOnDesktop)
            {
                sb.AppendLine("         <Directory Id=\"DesktopFolder\">");
                sb.AppendLine("           <Component Id=\"ApplicationDesktopShortcuts\" Guid=\"$(var.AppDeskShortcutsGuid)\">");
                sb.AppendLine("              <Shortcut Id=\"rdpDesktopShortcut1\" Name=\"$(var.ProductName)$(var.ProductRemoteTag)\" Description=\"$(var.ProductName)$(var.ProductRemoteTag)\" Target=\"[INSTALLDIR]$(var.ProductBaseFileName).rdp\" WorkingDirectory=\"INSTALLDIR\"");

                if (_hasIcon)
                {
                    sb.AppendLine(" Icon=\"rdpDeskIcon.rdp\" IconIndex=\"0\">");
                    sb.AppendLine("                        <Icon Id=\"rdpDeskIcon.rdp\" SourceFile=\"$(var.ProductBaseFileName).ico\" />");
                    sb.AppendLine("              </Shortcut>");
                }
                else
                {
                    sb.AppendLine("/>");
                }

                sb.AppendLine("              <RegistryValue Root=\"$(var.RegRoot)\" Key=\"Software\\RDP2MSI\\$(var.ProductName)\" Name=\"installed\" Type=\"integer\" Value=\"1\" KeyPath=\"yes\"/>");
                sb.AppendLine("           </Component>");
                sb.AppendLine("         </Directory>");
            }
            sb.AppendLine("      </Directory>");

            sb.AppendLine("      <InstallExecuteSequence>");
            sb.AppendLine("         <RemoveExistingProducts After=\"InstallValidate\"/>");
            sb.AppendLine("      </InstallExecuteSequence>");
            sb.AppendLine("      <Feature Id=\"DefaultFeature\" Level=\"1\">");
            sb.AppendLine("         <ComponentRef Id=\"ApplicationFiles\"/>");
            if (ShortcutInStart) sb.AppendLine("         <ComponentRef Id=\"ApplicationStartShortcuts\"/>");
            if (ShortcutOnDesktop) sb.AppendLine("         <ComponentRef Id=\"ApplicationDesktopShortcuts\"/>");
            sb.AppendLine("      </Feature>");

            if (_hasIcon)
            {
                sb.AppendLine("<Icon Id=\"rdpARPIcon.rdp\" SourceFile=\"$(var.ProductBaseFileName).ico\" />");
                sb.AppendLine("<Property Id=\"ARPPRODUCTICON\" Value=\"rdpARPIcon.rdp\" />");
            }
            sb.AppendLine("   </Product>");
            sb.AppendLine("</Wix>");

            return sb.ToString();
        }

        public string ReadRdpProperty(string rdpProperty)
        {
            if (string.IsNullOrEmpty(_rdpFileContents)) return "";

            var lines = _rdpFileContents.Split('\n');
            foreach (var line in lines)
            {
                var cleanLine = line.Replace("\r", "").Replace("|", "");
                var split = cleanLine.Split(new[] { ':' }, 3);
                if (split.Length == 3 && split[0] == rdpProperty)
                {
                    return split[2];
                }
            }
            return "";
        }

        private string GenerateGuidFromString(string input)
        {
            string hash = GetMd5Hash(input);
            var guid = new Guid(hash);
            return guid.ToString();
        }

        private string GetMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(bytes);
                var sb = new StringBuilder();
                foreach (var b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private int RunWait(string app, string parameters)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = app,
                    Arguments = parameters
                }
            };
            proc.Start();
            proc.WaitForExit();
            return proc.ExitCode;
        }
    }
}
