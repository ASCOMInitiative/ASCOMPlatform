Design and Test for New ASCOM Technology
========================================

Bob Denny 06-May-2007 (second release)

IF YOU INSTALLED THE PREVIOUS VERSION, RUN THE OLD REMOVE.BAT SCRIPT NOW! ONLY THEN SHOULD YOU DELETE THE OLD ONE.

This kit contains a set of projects which demonstrate a way to move ASCOM forward without limiting developers to .NET or impacting existing drivers and clients. It all starts with defining an interface with IDL at the COM level, then compiling and registering the resulting type library. Then a PIA is built from that and installed in the GAC, providing the metadata needed to use the interface in .NET. Meanwhile the registered type library can be used by VB6, C++/ATL, etc., to implement the interface. How the PIA came into being is irrelevant for .NET, it still contains metadata defining the interface. I have figured out how to implement a standard interface in VB6, something I couldn't figure out back in 2001. This means that VB6 and C++/ATLdrivers can also be converted to be early-bound! 

The kit consists of 3 parts:

* The Sample Interface folder which contains an IDL definition of a simple interface IAscomSample and shell scripts to compile into a type library, register it, make a PIA, register it, and install it into the GAC. It includes RegTlb.exe, a little type library registration/unregistration tool I wrote. Is there already something like this? I couldn't find it.

* A set of .NET assemblies that implement compatible in-proc servers for IAscomSample, one in C# and the other in VB.NET.

* A VB6 sample showing how to implement an interface and serve a public class. This one has three interfaces in its type library, with one of them derived from the master interface and the other (the default) simply a wrapper. It can serve the master interface through its public class, so early binding is supported!

* **THIS HAS BEEN UPDATED!!** A LocalServer implementation of IAscomSample. This little goodie acts just like a VB6 LocalServer. I did it by using COM as an OS service, and implementing my own ClassFactory. It registers itself as a LocalServer. As you'll see, it starts and exits, and if you ask for multiple instances of the IAscomSample interface, it will serve them out of the single EXE. This is a BIG BREAKTHROUGH in my opinion. It is completely compatible with all things present, including Windows Scripts. 

The new version of this is the "universal local server" I finally got working. Another BIG BREAKTHROUGH! It is capable of serving multiple instances of multiple objects with a single EXE host. And the served classes can be written in C#, VB or a mixture (demo has one in C# and the other in VB). Rather than needing to have the served classes complied dogether with the supporting logic, it will serve classes from assemblies dropped into the Supported Subclasses folder. The CSharpSample and VBSample projects in the solution have post-build events that copy their project output assemblies into that folder. The LocalServerCOM project has no references to either CSharpSample or VBSample projects. This could be morphed into an object broker or something. 

Anyway, it will make developing LocalServer served components MUCH easier for moderately skilled developers. THE CLASSES ARE IDENTICAL TO THOSE FOR INPROC SERVING EXCEPT FOR AN "Inhereits ReferenceCountedObject" (VB)statement at the top. That's all. Of course, of the served classes need to share some data (shared serial port or ???) then that stuff will need to be added to LocalServerCOM itself or better yet, a separate SharedResources class!

* A VB.NET console application that demonstrates early binding and the automatic use of either native .NET or COM to access the components. This should convince you that this approach is solid, and permits seamless use of native .NET or COM both with IntelliSense and early binding.

I just don't have  the time to write up anything more fancy, so please feel free to call and talk about it. It took me 6 full days as it is!

Prerequisites:
-------------

  .NET Framework SDK 2.0 or later  (the SDK! not just the framework!)
  Visual Studio.NET 2005  (includes the above)
  .NET Command line tools accessible from a shell ("DOS box")
  (recommended) OLEView
  DO NOT TRY TO BUILD PROJECTS WITHOUT FIRST INSTALLING (see below)

To install: 
----------

  Open a shell to the root and run Install.bat.
  In InprocServerCOM, load InprocServerCOM.sln solution and build. 

I don't know why, but the inprocServerCOM C#.NET and VB.NET assemblies need to be rebuilt after installing the master interface to the GAC. The LocalServerCOM does NOT have to be rebuilt. Any ideas?

To uninstall:
------------ 

  Open a shell to the root and run Remove.bat.

Tests:
------

In each implementation (Inproc C#/VB.NET/VB6) there are Tests folders with test scripts. Use a shell to run the tests as cscript xxx.vbs.

The EarlyBindDemo shows the whole scheme at work. Double click the exe and watch. Then look at the sources.

What next?
---------

Look over all of the sources including shell scripts ("bat files"), and of course try to spot the important project settings scattered through out the VS.NET properties tabs, as well as the references (or lack of same) in .NET and VB6. Check out the Intellisense in .NET and VB6. Look at the type library IAscomSample.tlb with OLEView, View TypeLib. Same with the typelib of InprocServerVB6.dll. Use GACUTL to see the master interface PIA. Use ILDASM to look at the PIA, etc. etc. Get familiar with how this all works.

Write additional test programs, in VB/C#.net or C++. Add a reference to the master interface type library IAscomSample 1.0 Interface. Then create one of the Inproc or the Local server by ProgID and set the return to an IAscomSample. You'll have early binding. Try making a .NET program that lets you choose which of the three Inproc servers or the local server object to create, then use the same code early bound to any of them.

