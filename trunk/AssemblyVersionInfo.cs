

/***
 * This file sets the assembly version for all generated assemblies.
 * All projects in the build should reference this file by including the following
 * in their project file:
 *
 *   <Compile Include="..\AssemblyVersionInfo.cs">
 *     <Link>Properties\AssemblyVersionInfo.cs</Link>
 *   </Compile>
 *   <Compile Include="..\SolutionInfo.cs">
 *     <Link>Properties\SolutionInfo.cs</Link>
 *   </Compile>
 * 
 * Version information is isolated in a seperate file to facilitate integration with the build server.
 * The build server will replace this entire file with a machine-generated one. Therefore,
 * this file should contain ONLY the global version number and no other settings.
 * 
 * For local builds, the major and minor versions are fixed at 6.x.99999.99999 indicating an uncontrolled item.
 * ***/

using System.Reflection;
// Revision and Build both set to 9999 indicates a private build on a developer's private workstation.
[assembly: AssemblyFileVersion("6.2.9999.9999")]	    // Win32 File Version (not used by .NET)

