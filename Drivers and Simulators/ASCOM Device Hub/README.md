This repository contains the source code for the ASCOM Device Hub application. Device Hub provides the ability to use the ASCOM Platform and conforming drivers to control a telescope, a dome, and a focuser. This is similar to the venerable POTH Hub tool. The Device Hub also provides itself as a local server for a telescope, a dome, and a focuser.

The local server provides several advantages such as eliminating 32-bit to 64-bit conflicts and allowing multiple client programs to be concurrently connected to, say, a focuser, or a telescope, or a dome. 

Device Hub can also provide dome slaving transparently from the viewpoint of a connected telescope client.

The source code is written in C# and XAML with Visual Studio solution and project files for Visual Studio 2019. A PDF-based user and technical document is included with the installation. This document describes the architecture of the application and may be useful to a developer who desires to make changes to the code. There are extensive unit test and conformance test projects that are included in the solution. These test projects can be used from within Visual Studio to test each layer of the software without the need for the user interface. While the conformance test project is not as robust as the ASCOM Conform tool, it can be conveniently run at any time without leaving Visual Studio to do so.

I hope that this application is useful to the astronomical community and can eventually replace POTH before Microsoft drops support for the Visual Basic 6 runtime libraries.
