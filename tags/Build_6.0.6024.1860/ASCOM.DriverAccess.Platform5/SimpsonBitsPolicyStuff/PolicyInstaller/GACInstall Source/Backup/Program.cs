using System;
using System.Collections.Generic;
using System.Text;
using System.GAC;
//// Artinsoft
//// Author: Mauricio Rojas orellabac@gmail.com mrojas@artinsoft.com
//// This program uses the undocumented GAC API to perform a simple install of an assembly
namespace AddAssemblyToGAC
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Create an FUSION_INSTALL_REFERENCE struct and fill it with data
            FUSION_INSTALL_REFERENCE[] installReference = new FUSION_INSTALL_REFERENCE[1];
            installReference[0].dwFlags = 0;
            
            // We use opaque scheme here
            installReference[0].guidScheme = System.GAC.AssemblyCache.FUSION_REFCOUNT_OPAQUE_STRING_GUID;

            installReference[0].szIdentifier = "My Pretty Aplication Identifier";
            installReference[0].szNonCannonicalData= "My other info";

            // Get an IAssemblyCache interface

            IAssemblyCache pCache = AssemblyCache.CreateAssemblyCache();
            String AssemblyFilePath = args[0];

            if (!System.IO.File.Exists(AssemblyFilePath))
            {
                Console.WriteLine("Hey! Please use a valid path to an assembly, assembly was not found!");
            }
            int result = pCache.InstallAssembly(0, AssemblyFilePath,installReference);

            Console.WriteLine("Process returned " + result);
            Console.WriteLine("Done!");

        }

    }
}
