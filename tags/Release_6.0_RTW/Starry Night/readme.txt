Starry Night Plug-in SDK for Windows and Visual C++ .NET


Included in this package you will find Starry Night Plug-in API, version
10, as well as two sample plug-in projects. The first sample plug-in is the latest 
working version of Starry Night Telescope plug-in for Starry Night Pro and Pro
Plus version 3 and greater (using ASCOM). However, many of the features will 
only be available in Starry Night Pro/Pro Plus 4 and above. The second plug-in is
the ASCOM Focuser plug-in and it will work in Starry Night Pro/Pro Plus version
5.7 and above. 

The callback API header is StarryNightPlugins.h (which is located beside
this readme file as well as in the project files). In the header you will find all
methods that your plug-in can call to interact with Starry Night. This
file needs to be included in all your plug-in projects. Similarly, starrynight.lib
needs to be a part of your projects.

We hope that this expanded v10 API will better serve your needs when
developing new plug-ins or enhancing your old ones. Feel free to contact
us if you have any questions or would like to see any new additions to
the next version of this API. The ingenuity of our users does not cease to amaze
us and we are striving to enable plug-in developers to take full advantage of
the functionality of Starry Night.


Starry Night Development Team
November, 2005

for further assistance with Starry Night SDK please contact
Marko Kudjerski
mkudjerski@starrynight.com




Sample Plug-ins Visual Studio Project notes

The sample plug-ins included contain their own project files. The plug-ins
should compile out of the box. If that is not the case please make sure that your
Visual Studio is properly configured.

The only setting that needs to be modified for proper plug-in testing is
Project->Project properties->Linker->General->Output file
in your Visual Studio Project. Please set this path to the location of your
Starry Night Pro/Pro Plus.

NOTE: 	Your plug-in must have the .plug extension and reside in
	"Starry Night Path\Sky Data\Plug-ins\" directory in order for Starry Night
	to use it.
