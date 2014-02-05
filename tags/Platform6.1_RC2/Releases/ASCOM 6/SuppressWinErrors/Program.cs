using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ASCOM.Internal.SuppressWinErrors
{
    class Program
    {
        static int Main(string[] args)
        {
            const string SuppressKey = @"SOFTWARE\Microsoft\Windows\Windows Error Reporting";
            const string SuppressKeyValue = @"DontShowUI";

            // Process arguments

            string action = "";
            bool help = false;
            bool versions = false;

            if (args.Length == 0) help = true; // No arguments will print a help message
            if (args.Length == 1) // One argument could be /H /V or a filename
            {
                if (string.Compare(args[0], "/H", true) == 0) help = true; // Test for help switch
                if (string.Compare(args[0], "-H", true) == 0) help = true;
                if (string.Compare(args[0], "/V", true) == 0) versions = true; // Test for version switch
                if (string.Compare(args[0], "-V", true) == 0) versions = true;
                action = args[0].ToUpper();
            }

            if (args.Length == 2)
            {
                if (string.Compare(args[1], "/H", true) == 0) help = true; // Test for help switch
                if (string.Compare(args[1], "-H", true) == 0) help = true;
                if (string.Compare(args[1], "/V", true) == 0) versions = true; // Test for version switch
                if (string.Compare(args[1], "-V", true) == 0) versions = true;

            }

            // help, versions, action now contain the supplied parameters
            if (help)
            {
                // Help requested so print message
                Console.WriteLine("SuppressWinErrors sets or clears a flag that suppresses the default \"An application has stopped working\" message dialogue");
                Console.WriteLine("");
                Console.WriteLine("Usage: SuppressWinErrors True - Sets the supporess flag");
                Console.WriteLine("       SuppressWinErrors False - Clears the suppress flag");
                Console.WriteLine("       SuppressWinErrors Remove - Completely removes the flag");
            }

            if (versions)
            {
                // versions requested so list the required information
                Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().FullName);
            }

            if (versions || help) return 0; // Exit if either help or version was requested

            //Process the supplied action
            RegistryKey suppressKey = Registry.LocalMachine.CreateSubKey(SuppressKey); // Create the registry key we need

            switch (action)
            {
                case "TRUE":
                case "1":
                    suppressKey.SetValue(SuppressKeyValue, 1); // Write a value to suppress the dialogue
                    break;
                case "FALSE":
                case "0":
                    suppressKey.SetValue(SuppressKeyValue, 0); // Write a value to enable the dialogue
                    break;
                case "REMOVE":
                    suppressKey.DeleteValue(SuppressKeyValue, false); // Remove the value altogether
                    break;
                default: Console.WriteLine("Unknown parameter: " + action);
                    break;
            }

            return 0;

        }
    }
}
