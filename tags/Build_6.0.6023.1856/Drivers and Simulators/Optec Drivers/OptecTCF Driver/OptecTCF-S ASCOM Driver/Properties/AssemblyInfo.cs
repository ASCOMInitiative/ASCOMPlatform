using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Revision History:
// Version 5.1.0.1          -Increased timout delay for move command.
//                          -Added Lock statements around serial communication code
//                          -Added exception to be thrown if temp comp is set true while
//                           the temp probe is disabled. (To achieve conformance)
//                          -
//                          
// Version 5.1.0.0          Initial Release Version

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("OptecTCF-S ASCOM Driver")]
[assembly: AssemblyDescription("ASCOM Driver for Optec TCF-S Focuser")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Optec Inc.")]
[assembly: AssemblyProduct("Optec TCF-S Focuser")]
[assembly: AssemblyCopyright("Copyright © Optec Inc. 2010 ")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("54fe2110-a152-4234-8123-66b6c8e2b922")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
//
//[assembly: AssemblyVersion("5.1.1.0")]
//[assembly: AssemblyFileVersion("5.1.1.0")]
