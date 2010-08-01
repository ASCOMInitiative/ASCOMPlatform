using System.Reflection;

/***
 * This file sets the assembly version for all generated assemblies.
 * All projects in the build should reference this file by including the following
 * in their project file:

    <Compile Include="..\AssemblyVersionInfo.cs">
      <Link>Properties\AssemblyVersionInfo.cs</Link>
    </Compile>
    <Compile Include="..\SolutionInfo.cs">
      <Link>Properties\SolutionInfo.cs</Link>
    </Compile>
 * 
 * This information is isolated in a seperate file to facilitate a future integration with the build server,
 * where the buils server will generate this file updated with the correct build number. Therefore,
 * this file should contain ONLY the global version number and no other settings.
 * ***/


// Version information for an assembly consists of the following four values:
////
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
////
// For local builds, the major and minor versions are fixed and the
// revision and build numbers are set automatically by Visual Studio.
// For TeamCity builds, this version string may be substitued
// by the TeamCity build number.

[assembly: AssemblyVersion("6.0.*")]		// .Net Assembly Version
//[assembly: AssemblyFileVersion("5.5.0")]	// Win32 File Version

