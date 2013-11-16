using System;
using System.Collections.Generic;
using System.Text;
using System.EnterpriseServices.Internal;
//// Artinsoft
//// Author: Mauricio Rojas orellabac@gmail.com mrojas@artinsoft.com
//// This program uses the undocumented GAC API to perform a simple install of an assembly

//// Revised by Peter Simpson to add help
////                          to uninstall as well as install
////                          to provide textual description of uninstall outcome

//// Updated to use the mangaged API System.EnterpriseServices.Internal.Publish() by Peter Simpson 16th October 2013
//// This gets rid of the unpublished FusionLib and GAC access assembly of Maurico Rojas and brings us back to 
//// managed APIs at last! Hooray!

//// Return code: 0 OK
////              1 Unable to find file
////              2 An exception happened

namespace ASCOM.Internal.GACInstall
{
    class Program
    {
        static int Main(string[] args)
        {
            // Process arguments

            bool install = true; // Initialise variables
            string filename = "";
            bool help = false;
            bool versions = false;

            if (args.Length == 0) help = true; // No arguments will print a help message
            if (args.Length == 1) // One argument could be /H /V or a filename
            {
                if (string.Compare(args[0], "/H", true) == 0) help = true; // Test for help switch
                if (string.Compare(args[0], "-H", true) == 0) help = true;
                if (string.Compare(args[0], "/V", true) == 0) versions = true; // Test for help switch
                if (string.Compare(args[0], "-V", true) == 0) versions = true;
                filename = args[0];
            }

            if (args.Length == 2)
            {
                if (string.Compare(args[0], "/H", true) == 0) help = true; // Test for help switch
                if (string.Compare(args[0], "-H", true) == 0) help = true;
                if (string.Compare(args[1], "/H", true) == 0) help = true;
                if (string.Compare(args[1], "-H", true) == 0) help = true;

                if ((string.Compare(args[0], "/U", true) == 0) || (string.Compare(args[0], "-U", true) == 0))
                { // Found uninstall switch in first argument
                    install = false;
                    filename = args[1]; // Filename must be second argument
                }
                else
                {
                    filename = args[0]; // Filename must be first argument
                    if ((string.Compare(args[1], "/U", true) == 0) || (string.Compare(args[1], "-U", true) == 0))
                    {
                        install = false; // Uninstall switch found as second argument
                    }
                }
            }

            // help, versions, install and filename now contain the supplied parameters
            if (help)
            {
                // Help requested so print message
                Console.WriteLine("GACInstall adds and removes assemblies from the cache");
                Console.WriteLine("");
                Console.WriteLine("Usage: GACInstall AssemblyFileName");
                Console.WriteLine("       GACInstall /U AssemblyName");
                return 0;
            }

            if (versions)
            {
                // versions requested so list the required information
                Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().FullName);
                Console.WriteLine(System.IO.Directory.Exists("fusion.dll"));
                Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                Console.WriteLine("Full path to fusion.dll: " + System.IO.Path.GetFullPath("fusion.dll") + "#");
                Console.WriteLine("Path: " + System.Environment.GetEnvironmentVariable("PATH"));
                return 0;
            }

            int result = 0; // Initialise return code to OK

            if (install)
            { // We are going to install an assembly
                try
                {
                    if (System.IO.File.Exists(filename))
                    {
                        new System.EnterpriseServices.Internal.Publish().GacInstall(filename);
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("Unable to find file: " + filename);
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    result = 2;
                    Console.WriteLine("Exception: " + ex.ToString());
                    return result;
                }
            }
            else // We are going to uninstall an assembly
            {
                try
                {
                    if (System.IO.File.Exists(filename))
                    {
                        Publish pub = new Publish();
                        Console.WriteLine("ToString(): " + filename + " " + pub.ToString());
                        pub.GacRemove(filename);
                    }
                    else
                    {
                        Console.WriteLine("Unable to find file: " + filename);
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    result = 2;
                    Console.WriteLine("Exception: " + ex.ToString());
                    return result;
                }

                return result;
            }

        }
    }
}
