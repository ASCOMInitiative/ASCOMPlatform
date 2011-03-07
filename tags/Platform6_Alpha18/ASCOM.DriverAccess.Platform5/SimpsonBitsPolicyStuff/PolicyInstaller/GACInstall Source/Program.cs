using System;
using System.Collections.Generic;
using System.Text;
using System.GAC;
//// Artinsoft
//// Author: Mauricio Rojas orellabac@gmail.com mrojas@artinsoft.com
//// This program uses the undocumented GAC API to perform a simple install of an assembly

//// Revised by Peter Simpson to add help
////                          to uninstall as well as install
////                          to provide textual description of uninstall outcome

namespace GACInstall
{
    class Program
    {
        static int Main(string[] args)
        {
            // Process arguments

            bool install = true; // Initialise variables
            string filename = "";
            bool help = false;
            if (args.Length == 0) help = true; // No arguments will print a help message
            if (args.Length == 1) // One argument could be /H or a filename
            {
                if (string.Compare(args[0],"/H",true) == 0) help = true; // Test for help switch
                if (string.Compare(args[0],"-H",true) == 0) help = true;
                filename = args[0];
            }
            if (args.Length == 2)
            {
                if (string.Compare(args[0],"/H",true) == 0) help = true; // Test for help switch
                if (string.Compare(args[0],"-H",true) == 0) help = true; 
                if (string.Compare(args[1],"/H",true) == 0) help = true; 
                if (string.Compare(args[1],"-H",true) == 0) help = true; 
            
                if ((string.Compare(args[0],"/U",true) == 0) || (string.Compare(args[0],"-U",true) == 0))
                { // Found uninstall switch in first argument
                    install = false; 
                    filename=args[1]; // Filename must be second argument
                }
                else
                {
                    filename=args[0]; // Filename must be first argument
                    if ((string.Compare(args[1],"/U",true) == 0) || (string.Compare(args[1],"-U",true) == 0))
                    {
                        install = false; // Uninstall switch found as second argument
                    }
                }
            }
             
            // help, install and filename now contain the supplied parameters
            if (help)
            {
                // Help requested so print message
                Console.WriteLine("GACInstall adds and removes assemblies from the cache");
                Console.WriteLine(""); 
                Console.WriteLine("Usage: GACInstall AssemblyFileName");
                Console.WriteLine("       GACInstall /U AssemblyName");
                return 0;
            }

            // Create an FUSION_INSTALL_REFERENCE struct and fill it with data
            // We use opaque scheme here
            FUSION_INSTALL_REFERENCE[] installReference = new FUSION_INSTALL_REFERENCE[1];
            installReference[0].dwFlags = 0;
            installReference[0].guidScheme = System.GAC.AssemblyCache.FUSION_REFCOUNT_OPAQUE_STRING_GUID;
            installReference[0].szIdentifier = "GACInstall";
            installReference[0].szNonCannonicalData= "Installed by Peter Simpson's GACInstall program";
            installReference[0].cbSize = 40;

            // Get an IAssemblyCache interface
            IAssemblyCache pCache = AssemblyCache.CreateAssemblyCache();

            if (install)
            { // We are going to install an assembly
                if (System.IO.File.Exists(filename))
                {
                    int result = pCache.InstallAssembly(0, filename, installReference);
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
            else
            { // We are going to uninstall an assembly
                IASSEMBLYCACHE_UNINSTALL_DISPOSITION puldisposition = IASSEMBLYCACHE_UNINSTALL_DISPOSITION.IASSEMBLYCACHE_UNINSTALL_DISPOSITION_UNKNOWN; // Initialise variable
                int result = pCache.UninstallAssembly(0, filename, installReference, out puldisposition);
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
                return result;
            }
           
        }
    }
}
