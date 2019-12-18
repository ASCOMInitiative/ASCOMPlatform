This is a copy of the Newtonsoft JSON library version 12.0.3, current at 18th December 2019

This high maintenance approach been taken because the ASCOM Chooser and Alpaca Support components use the Newtonsoft JSON library, but these ASCOM assemblies are also stored in the GAC. This means that all 
of their dependencies MUST also be stored in the GAC and I don't want to store an original Newtonsoft executable there because it could be picked up ahead of future minor version upgrades 
that clients and drivers may include in their executable's directory.

I investigated using reflection and this did work but still required a Newtonsoft library DLL to be stored somewhere in the file system so I went for a simpler approach that allows easy future use of all 
Newtonsoft.JSON features if required.

Integration notes:

	The project file has been modified jJust to build a .NET 3.5 assembly
	The assembly is signed with the ASCOM code signing key and the SIGNED compiler variable has been set
	Package building has been turned off
	Text updated to indicate the approach taken and that this is not a genuine Newtonsoft DLL but an ASCOM adaptation.
	No change whatsoever has been made to the v12.0.3 functional code

Keeping up to date
With luck it should be possible just to copy over source code from future versions. The ASCOM.Newtonsoft.Json project file should just work because it is named differently to Newtonsoft's version. 
However, a manual inspection of the project files would be a beneficial precaution!

Peter Simpson 18th December 2019