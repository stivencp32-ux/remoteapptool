using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using RemoteAppTool.Core;

namespace RemoteAppTool.SignLib
{
    public class RdpSign
    {
        public int ErrorNumber { get; set; } = 0;
        public string ErrorString { get; set; } = "";

        public string[] GetCertificateFriendlyName()
        {
            var certStoreLM = GetCertificateStoreLM();
            var certStoreCU = GetCertificateStoreCU();

            var friendlyNames = new System.Collections.Generic.List<string>();

            foreach (var cert in certStoreLM.Certificates)
            {
                if (!string.IsNullOrEmpty(cert.FriendlyName))
                    friendlyNames.Add(cert.FriendlyName);
            }

            foreach (var cert in certStoreCU.Certificates)
            {
                if (!string.IsNullOrEmpty(cert.FriendlyName))
                    friendlyNames.Add(cert.FriendlyName);
            }

            return friendlyNames.ToArray();
        }

        private X509Store GetCertificateStoreLM()
        {
            var certStore = new X509Store(StoreLocation.LocalMachine);
            certStore.Open(OpenFlags.ReadOnly);
            return certStore;
        }

        private X509Store GetCertificateStoreCU()
        {
            var certStore = new X509Store(StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            return certStore;
        }

        public string GetThumbprint(string friendlyName)
        {
            var certStoreLM = GetCertificateStoreLM();
            foreach (var cert in certStoreLM.Certificates)
            {
                if (cert.FriendlyName == friendlyName)
                {
                    certStoreLM.Close();
                    return cert.Thumbprint;
                }
            }
            certStoreLM.Close();

            var certStoreCU = GetCertificateStoreCU();
            foreach (var cert in certStoreCU.Certificates)
            {
                if (cert.FriendlyName == friendlyName)
                {
                    certStoreCU.Close();
                    return cert.Thumbprint;
                }
            }
            certStoreCU.Close();

            return "0000";
        }

        public void SignRdp(string thumbprint, string rdpFileLocation, bool createBackup)
        {
            if (thumbprint == "0000") return;

            if (createBackup)
            {
                string backupFile = Path.Combine(Path.GetDirectoryName(rdpFileLocation), Path.GetFileNameWithoutExtension(rdpFileLocation) + "-Unsigned.rdp");
                
                string fileLocked = LockChecker.CheckLock(backupFile);
                if (fileLocked != "No locks")
                {
                    // Logic to prompt the user or handle the lock should be implemented at the UI layer.
                    // For now, if it's locked, we might skip or throw an exception.
                    // Throwing exception to notify caller.
                    throw new IOException($"Backup file is locked: {fileLocked}");
                }

                File.Copy(rdpFileLocation, backupFile, true);
            }

            string command = GetRdpsignExeLocation();

            if (File.Exists(command))
            {
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(command);
                string arguments = (fileVersionInfo.ProductMajorPart >= 10) 
                    ? $" /sha256 {thumbprint} \"{rdpFileLocation}\"" 
                    : $" /sha1 {thumbprint} \"{rdpFileLocation}\"";

                var startInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process.Start(startInfo);
            }
            else
            {
                throw new FileNotFoundException("RDPSign executable not found", command);
            }
        }

        public string GetRdpsignExeLocation()
        {
            string[] possibleRdpsignPaths = {
                Environment.SystemDirectory,
                AppDomain.CurrentDomain.BaseDirectory
            };

            foreach (var path in possibleRdpsignPaths)
            {
                string fullPath = Path.Combine(path, "rdpsign.exe");
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            return string.Empty;
        }
    }
}
