This code is a copy of the Newtonsoft JSON library version 12.0.3, current at 28th February 2020

This high maintenance approach been necessitated because several ASCOM components use the Newtonsoft JSON library but are also stored in the GAC. If an assembly is stored in the GAC it means that all
of its dependencies MUST also be stored in the GAC. I don't want to store an original Newtonsoft executable there because it could be picked up ahead of future minor version upgrades from Newtonsoft 
that clients and drivers may include in their executable's directory.

I investigated using reflection and this did work, but still required a Newtonsoft library DLL to be stored somewhere in the file system so I went for the simpler approach of adapting the Newtonsoft code 
and integrating it into the ASCOM code base in order to create an ASCOM version of the JSON.NET library that can be installed in the GAC and that will not conflict with future releases by Newtonsoft.

This approach allows for easy future use of all Newtonsoft.JSON features if required.

Integration notes:

	The project file has been modified just to build a .NET 3.5 assembly
	The assembly is signed with the ASCOM code signing key and the SIGNED compiler variable has been set
	Package building has been turned off
	Text updated to indicate the approach taken and that this is not a genuine Newtonsoft DLL but an ASCOM adaptation.
	No change whatsoever has been made to the functional code

Keeping up to date
With luck, it should be possible just to copy over source code from future versions. The ASCOM.Newtonsoft.Json project file should continue to work because it is named differently to Newtonsoft's version and so won't be over-written. 
However, a manual inspection of the project files after copying would be a beneficial precaution!

Peter Simpson 28th February 2020