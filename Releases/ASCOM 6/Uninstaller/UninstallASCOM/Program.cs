using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.IO;

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
           Console.WriteLine("Removing previous versions of ASCOM....");
           bool is64BitProcess = (IntPtr.Size == 8);
           bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

            string platform564KeyValue = null;
            string platform5564KeyValue = null;
            bool found = false;

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
                Console.WriteLine("64 Removing ASCOM 5.5...");
                Console.WriteLine(platform5564KeyValue);
                found = true;
                RunProcess(platform5564KeyValue, " /SILENT");
            }
            else
            {
                if (platform5532KeyValue != null)
                {
                    Console.WriteLine("32 Removing ASCOM 5.5...");
                    Console.WriteLine(platform5532KeyValue);
                    found = true;
                    RunProcess(platform5532KeyValue, " /SILENT");
                }
            }

            //remove 5.0
            if (platform564KeyValue != null)
            {
                Console.WriteLine("64 Removing ASCOM 5...");
                Console.WriteLine(platform564KeyValue);
                found = true;
                RunProcess("MsiExec.exe", SplitKey(platform564KeyValue));
            }
            else
            {
                if (platform532KeyValue != null)
                {
                    Console.WriteLine("32 Removing ASCOM 5...");
                    Console.WriteLine(platform532KeyValue);
                    found = true;
                    RunProcess("MsiExec.exe", SplitKey(platform532KeyValue));
                }
            }
            if (found == false)
            {
                CleanUp55();
                CleanUp5();
                Console.WriteLine("Nothing Found");
            }
            Pic();
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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

        //clean up any left over files from 5.0
        protected static void CleanUp5()
        {
            //start menu
            var startMenuDir = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            var shortcut = Path.Combine(startMenuDir, @"Programs\ASCOM Platform");
            if (Directory.Exists(shortcut))
                DeleteDirectory(shortcut);

            //clea up prog files
            const string ascomDir = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\ASCOM Platform";
            if (Directory.Exists(ascomDir))
                DeleteDirectory(ascomDir);

            //clean up files
            const string pathToAscom = @"C:\Program Files (x86)\Common Files\ASCOM";
            if (Directory.Exists(pathToAscom))
                DeleteDirectory(pathToAscom);
        }

        //clean up any left over files from 5.5
        public static void CleanUp55()
        {
                //start menu
                var startMenuDir = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                var shortcut = Path.Combine(startMenuDir, @"Programs\ASCOM Platform");
                if (Directory.Exists(shortcut))
                    DeleteDirectory(shortcut);

                //clean up prog files
                const string ascomDir = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\ASCOM Platform";
                if (Directory.Exists(ascomDir))
                    DeleteDirectory(ascomDir);

                //clean up files
                const string pathToAscom = @"C:\Program Files (x86)\Common Files\ASCOM";
                if (Directory.Exists(pathToAscom))
                    DeleteDirectory(pathToAscom);
                
            //clean up files
                const string pathToAscom1 = @"C:\Program Files (x86)\ASCOM";
                if (Directory.Exists(pathToAscom1))
                    DeleteDirectory(pathToAscom1);

                //clean up files
                const string pathToAscom2 = @"C:\Program Files\ASCOM";
                if (Directory.Exists(pathToAscom2))
                    DeleteDirectory(pathToAscom2);
        }

        //reset the file attributes and then deletes the file
        protected static bool DeleteDirectory(string targetDir)
        {
            try
            {
                string[] files = Directory.GetFiles(targetDir);
                string[] dirs = Directory.GetDirectories(targetDir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(targetDir, true);

                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                   return false;
            }

        } 

        public static void Pic()
        {
            Console.WriteLine(" ");
            Console.WriteLine(@" @  *  .  . *       *    .        .        .   *    ..");
            Console.WriteLine(@" @. /\ *     ###     .      .        .            *");
            Console.WriteLine(@" @ /  \  *  #####   .     *      *        *    .");
            Console.WriteLine(@" ]/ [] \  ######### *    .  *       .  //    .  *   .");
            Console.WriteLine(@" / [][] \###\#|#/###   ..    *     .  //  *  .  ..  *");
            Console.WriteLine(@" |  __  | ###\|/###  *    *  ___o |==// .      *   *");
            Console.WriteLine(@" |  |!  |  # }|{  #         /\  \/  //|\");
            Console.WriteLine(@" |  ||  |    }|{    ejm97  / /        | \");
            Console.WriteLine(@"                           ` `        '  '");
            Console.WriteLine(" ");
            Console.WriteLine("Clear Skies:   Press any key to continue... ");
            Console.Read();
        }
    }
}
