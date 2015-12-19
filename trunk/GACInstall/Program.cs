using System;
using System.Collections.Generic;
using System.Reflection;

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
            bool list = false;

            if (args.Length == 0) help = true; // No arguments will print a help message
            if (args.Length == 1) // One argument could be /H /V or /L a filename
            {
                if (string.Compare(args[0], "/H", true) == 0) help = true; // Test for help switch
                if (string.Compare(args[0], "-H", true) == 0) help = true;
                if (string.Compare(args[0], "/V", true) == 0) versions = true; // Test for help switch
                if (string.Compare(args[0], "-V", true) == 0) versions = true;
                if (string.Compare(args[0], "/L", true) == 0) list = true; // Test for list switch
                if (string.Compare(args[0], "-L", true) == 0) list = true;
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

            if (list)
            {
                // List ASCOM assemblies
                SortedList<String, String> assemblyNames = new SortedList<String, String>();
                IAssemblyEnum assemblyEnumerator = AssemblyCache.CreateGACEnum(); // Get an enumerator for the GAC assemblies
                IAssemblyName iAssemblyName = null;

                while ((AssemblyCache.GetNextAssembly(assemblyEnumerator, out iAssemblyName) == 0))
                {
                    try
                    {
                        AssemblyName assemblyName = new AssemblyName();
                        try
                        {
                            assemblyName.Name = AssemblyCache.GetName(iAssemblyName);
                            assemblyName.Version = AssemblyCache.GetVersion(iAssemblyName);
                            assemblyName.CultureInfo = AssemblyCache.GetCulture(iAssemblyName);
                            assemblyName.SetPublicKeyToken(AssemblyCache.GetPublicKeyToken(iAssemblyName));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("GetAssemblyName Exception: " + ex.ToString());
                        }

                        assemblyNames.Add(assemblyName.FullName, assemblyName.Name);
                    }
                    catch
                    {
                        // Ignore an exceptions here due to duplicate names, these are all MS assemblies
                    }

                }

                Console.WriteLine("\r\nInstalled ASCOM assemblies: ");

                foreach (KeyValuePair<string, string> assemblyName in assemblyNames)
                {
                    //  Process each assembly in turn
                    try
                    {
                        if (((assemblyName.Key.IndexOf("ASCOM") + 1) > 0) || (assemblyName.Key.Contains("565de7938946fba7")))
                        {
                            Console.WriteLine(assemblyName.Key);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("AssemblyName.Key Exception: " + ex.ToString());
                    }
                }
                return 0;
            }

            // Must be install or uninstall
            int result = 0; // Initialise return code to OK

            // Create an FUSION_INSTALL_REFERENCE struct and fill it with data
            // We use opaque scheme here
            FUSION_INSTALL_REFERENCE[] installReference = new FUSION_INSTALL_REFERENCE[1];
            installReference[0].dwFlags = 0;
            installReference[0].guidScheme = AssemblyCache.FUSION_REFCOUNT_OPAQUE_STRING_GUID;
            installReference[0].szIdentifier = "GACInstall";
            installReference[0].szNonCannonicalData = "Installed by Peter Simpson's GACInstall program";
            installReference[0].cbSize = 40;

            // Get an IAssemblyCache interface
            IAssemblyCache pCache = AssemblyCache.CreateAssemblyCache();

            if (install)
            { // We are going to install an assembly
                try
                {
                    if (System.IO.File.Exists(filename))
                    {
                        result = pCache.InstallAssembly(0, filename, null);
                        if (result == 0) Console.WriteLine("Installed OK!");
                        else Console.WriteLine("Install returned error code: " + result);
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
                    { // We are going to uninstall an assembly
                        IASSEMBLYCACHE_UNINSTALL_DISPOSITION puldisposition = IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNKNOWN; // Initialise variable
                        result = pCache.UninstallAssembly(0, filename, null, out puldisposition);
                        if (result == 0) Console.WriteLine("Uninstalled with no error!");
                        else Console.WriteLine("Uninstall returned error code: " + result);

                        switch (puldisposition)
                        {
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_ALREADY_UNINSTALLED:
                                Console.WriteLine("Outcome: Assembly already uninstalled"); break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_DELETE_PENDING:
                                Console.WriteLine("Outcome: Delete currently pending"); break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_HAS_INSTALL_REFERENCES:
                                Console.WriteLine("Outcome: Assembly has remaining install references"); break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_REFERENCE_NOT_FOUND:
                                Console.WriteLine("Outcome: Unable to find assembly - " + filename); break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_STILL_IN_USE:
                                Console.WriteLine("Outcome: Assembly still in use"); break;
                            case IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNINSTALLED:
                                Console.WriteLine("Outcome: Assembly uninstalled"); break;
                            default:
                                Console.WriteLine("Unknown uninstall outcome code: " + puldisposition); break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = 2;
                    Console.WriteLine("Exception: " + ex.ToString());
                }

                return result;
            }

        }
    }
}
