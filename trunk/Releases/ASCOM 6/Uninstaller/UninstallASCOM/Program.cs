using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UninstallAscom
{
    class Program
    {
        const string platform532 ="SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{14C10725-0018-4534-AE5E-547C08B737B7}";
        const string platform564 = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{14C10725-0018-4534-AE5E-547C08B737B7}";
        const string platform5532 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ASCOM.Platform.NET.Components_is1";
        const string platform5564 = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ASCOM.Platform.NET.Components_is1";
        const string uninstallString = "UninstallString";

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        ); 

        static void Main()
        {
           bool is64BitProcess = (IntPtr.Size == 8);
           bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

            string platform564KeyValue = null;
            string platform5564KeyValue = null;

            if (is64BitOperatingSystem)
            {
                platform5564KeyValue = Read(uninstallString, platform5564);
                platform564KeyValue =  Read(uninstallString, platform564);
            }

            var platform5532KeyValue = Read(uninstallString, platform5532);
            var platform532KeyValue = Read(uninstallString, platform532);

            //remove 5.5
            if (platform5564KeyValue != null)
            {
                Console.WriteLine(platform5564KeyValue);
                RunProcess(platform5564KeyValue, " /SILENT");
            }
            else
            {
                if (platform5532KeyValue != null)
                {
                    Console.WriteLine(platform5532KeyValue);
                    RunProcess(platform5532KeyValue, " /SILENT");
                }
            }

            //remove 5.0
            if (platform564KeyValue != null)
            {
                Console.WriteLine(platform564KeyValue);
                RunProcess("MsiExec.exe", SplitKey(platform564KeyValue));
            }
            else
            {
                if (platform532KeyValue != null)
                {
                    Console.WriteLine(platform532KeyValue);
                    RunProcess("MsiExec.exe", SplitKey(platform532KeyValue));
                }
            }
        }

        //split the installer string
        public static string SplitKey(string keyToSplit)
        {

            string[] s = keyToSplit.Split(new[] {' '});
            s[1] = s[1].Replace("/I", "/q /x ");
            return s[1];
        }

        //run the uninstaller
        public static bool RunProcess(string processToRun, string args)
        {
            try
            {
                var startInfo = new ProcessStartInfo(processToRun) {Arguments = args};
                var myProcess = Process.Start(startInfo);
                myProcess.WaitForExit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        //Read a key
        public static string Read(string keyName, string subKey)
        {
            // Opening the registry key
            var rk = Registry.LocalMachine;
            // Open a subKey as read-only
            var sk1 = rk.OpenSubKey(subKey,false);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            try
            {
                // If the RegistryKey exists I get its value
                // or null is returned.
                return (string)sk1.GetValue(keyName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //check the OS
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            return false;
        }
    }
}
