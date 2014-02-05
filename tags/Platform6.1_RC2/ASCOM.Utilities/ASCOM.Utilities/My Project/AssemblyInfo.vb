Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices

' General Information about an assembly is controlled through the following
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

<Assembly: AssemblyTitle("ASCOM Utilities")> 
<Assembly: AssemblyDescription("ASCOM Utilities")> 
<Assembly: AssemblyCompany("ASCOM Initiative")>
<Assembly: AssemblyProduct("ASCOM Utilities")> 
<Assembly: AssemblyCopyright("Copyright © ASCOM Initiative 2009, 2014")> 
<Assembly: AssemblyTrademark("")>
<Assembly: AssemblyCulture("")>

' Version information for an assembly consists of the following four values:

'	Major version
'	Minor Version
'	Build Number
'	Revision

' You can specify all the values or you can default the Build and Revision Numbers
' by using the '*' as shown below:

<Assembly: AssemblyVersion("6.0.0.0")> 

<Assembly: ComVisibleAttribute(False)> 
<Assembly: GuidAttribute("EC785106-0F00-4e7f-90BA-7CCC9E9740E1")> 

'Permissions for particular assemblies to access internal "Friend" variables and methods of the ASCOM.Utilities assembly
<Assembly: InternalsVisibleToAttribute("ASCOM.Utilities.Video, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")> 
<Assembly: InternalsVisibleToAttribute("ProfileExplorer, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")> 
<Assembly: InternalsVisibleToAttribute("MigrateProfile, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")> 
<Assembly: InternalsVisibleToAttribute("ASCOM Diagnostics, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")> 
<Assembly: InternalsVisibleToAttribute("ASCOM.Astrometry, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")>
<Assembly: InternalsVisibleToAttribute("ASCOM.Simulator.Focuser, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")> 
<Assembly: InternalsVisibleToAttribute("ASCOM.Simulator.Telescope, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")> 
<Assembly: InternalsVisibleToAttribute("ASCOM.TelescopeSimulator, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")> 
<Assembly: InternalsVisibleToAttribute("Conform, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")>
<Assembly: InternalsVisibleToAttribute("ASCOM.DriverAccess, PublicKey= " + _
"0024000004800000940000000602000000240000525341310004000001000100afa2def19e73b7" + _
"2a7cf149888c93ec828db75e4d481b5c652aa43417a77689ce9151853ce10278c44bff4aa559b9" + _
"0a4f534cef9eaa6cb8b0f6a5f0719f9ad18795416367e9f485712075c54aea9b4e9aa990326e2f" + _
"929a990a5c61b679564343ff21547e096db1a90e50419c12c390641686270105620f51fd7d94ef" + _
"54beeee8")>