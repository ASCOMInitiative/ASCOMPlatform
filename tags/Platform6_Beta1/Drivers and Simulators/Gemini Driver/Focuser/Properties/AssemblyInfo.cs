﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
// TODO - Add your authorship information here
[assembly: AssemblyTitle("Gemini Focuser Driver")]
[assembly: AssemblyDescription("ASCOM Focuser Driver for Gemini")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("ASCOM Initiative")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c5c6b4bf-4cba-447d-810a-b0013b0a77a5")]

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
// TODO - Set your driver's version here
[assembly: AssemblyVersion("1.0.3.0")]
[assembly: AssemblyFileVersion("1.0.3.0")]

[assembly: ASCOM.ServedClassNameAttribute("Gemini Focuser .NET")]
[assembly: ASCOM.DeviceIdAttribute("ASCOM.GeminiTelescope.Focuser")]