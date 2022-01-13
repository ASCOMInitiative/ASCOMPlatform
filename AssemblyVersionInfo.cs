using System.Reflection;
using System.Runtime.CompilerServices;

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
 * Version information is isolated in a separate file to facilitate integration with the build server.
 * The build server will replace this entire file with a machine-generated one. Therefore,
 * this file should contain ONLY the global version number and no other settings.
 * 
 * For local builds, the major and minor versions are fixed at 6.x.99999.99999 indicating an uncontrolled item.
 * ***/

// Revision and Build both set to 9999 indicates a private build on a developer's private workstation.
[assembly: AssemblyFileVersion("6.6.9999.9999")]	    // Win32 File Version (not used by .NET)

[assembly: InternalsVisibleToAttribute("Conform, PublicKey= " +
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" +
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" +
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" +
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" +
"54beeee8")]