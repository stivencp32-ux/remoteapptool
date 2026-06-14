using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace RemoteAppTool.Core
{
    public class LockChecker
    {
        private const int CCH_RM_MAX_APP_NAME = 255;
        private const int CCH_RM_MAX_SVC_NAME = 63;
        private const int RmRebootReasonNone = 0;

        [StructLayout(LayoutKind.Sequential)]
        private struct RM_UNIQUE_PROCESS
        {
            public int dwProcessId;
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        private enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public string strAppName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            public string strServiceShortName;

            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        private static extern int RmRegisterResources(uint pSessionHandle, uint nFiles, string[] rgsFilenames,
            uint nApplications, [In] RM_UNIQUE_PROCESS[] rgApplications, uint nServices, string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        private static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        private static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        private static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo,
            [In, Out] RM_PROCESS_INFO[] rgAffectedApps, out uint lpdwRebootReasons);

        public static string CheckLock(string filename)
        {
            uint handle;
            string sessionkey = Guid.NewGuid().ToString();
            string result = "";

            int res = RmStartSession(out handle, 0, sessionkey);
            if (res != 0)
            {
                return "Could not begin restart session.";
            }

            try
            {
                uint pnProcInfoNeeded = 0;
                uint pnProcInfo = 100;
                uint lpdwRebootReasons = RmRebootReasonNone;

                string[] resources = { filename };
                RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[pnProcInfo];

                res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);
                if (res != 0)
                {
                    return "Could not register resource.";
                }

                res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, out lpdwRebootReasons);
                if (res == 0)
                {
                    if (pnProcInfo != 0)
                    {
                        for (int i = 0; i < pnProcInfo; i++)
                        {
                            result += $"File Name: {resources[0]}\r\n";
                            result += $"Application locking the file: {processInfo[i].strAppName}\r\n";
                            result += $"PID of process locking the file: {processInfo[i].Process.dwProcessId}\r\n";
                        }
                    }
                    else
                    {
                        result = "No locks";
                    }
                }
                else
                {
                    return "Could not list processes locking resource.";
                }
            }
            catch (Exception exception)
            {
                result = exception.Message;
            }
            finally
            {
                RmEndSession(handle);
            }

            return result;
        }
    }
}
