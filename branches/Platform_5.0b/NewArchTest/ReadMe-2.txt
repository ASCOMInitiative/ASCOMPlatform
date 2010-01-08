Test Harness for ASCOM Early Binding & Interface Migration
==========================================================

January 2009 (Bob Denny) - Updating this from the 2007 package and using it to test interface migration. Please see the original ReadMe.txt for background. I'm going to make this brief, if you have problems feel free to contact me!

The question that needed to be answered is "Can we simply add members to ("extend") an interface, without deleting or changing existing ones, and maintain client/driver compatibility?" This boils down to two important cases:

1. Client early bound to V1 interface and driver that implements V2, and

2. client early bound to V2 interface and driver that implements V1.

Late binding has no problem with interface extensions. SUMMARY: Case 1 succeeds and case 2 fails in .NET only. ANd it fails in a way that can't be controlled within the client code. The failure occurs at the initial call to CreateObject(), at which time it seels that COM Interop reflects on the driver's interface and immediately finds the missing members.

As of Jan 2009 (SVN Rev 835, which does not include this document) the components are

EarlyBindDemo:    .NET client
InProcServerCOM:  Three servers, C#, VB.NET, and VB6
LocalServerCOM:   Serves objects implemented in VB.NET and C#.net
SampleInterface:  Sources and build/install/uninstall scripts for the V1 interface
SampelInterface2: Sources and build/install/uninstall scripts for the V1 interface

All of these have conditional code for building (and for the servers) inplementing against either the V1 or V2 interface. You'll just have to figure out how to do all of this - it's too much for me to type up. But I'll be happy to help via phone +1 480 396 9700.

The process for testing all of this is:

1. Build and register/install the V1 interface
2. Set conditional ForV2=False in EarlyBindDemo and build it. Now it is a V1 client.
3. Set ForV2=False in all 4 COM servers (3 inproc and 1 Local) and build them. Now you have V1 servers. Each of the objects served by the LocalServer has its own ForV2 conditional!
-- At this point you have V1 client and V1 servers --
4. Run EarlyBindTest and confirm that all of the test cases work. This will validate with V1 client and V1 servers.
5. Clean and unregister all 4 COM servers(!!). Unregister LocalServer from the command line.
6. Unregister/uninstall the V1 interface
7. Build and register/install the V2 interface
8. Set ForV2=True in all of the COM servers and rebuild them from scratch. Register the LocalServer from the command line. Now you have four V2 servers.
-- At this point you have a V1 client and V2 servers --
9. Run EarlyBindTest (V1) and confirm that it still works with V2 interface and servers. This verifies that Case #1 above works.
10. Change ForV2=True in EarlyBindTest and build it.
-- At this point you have a V2 client and V2 servers --
11. Run EarlyBindTest just to confirm that its V2 code runs and that the servers are really V2.
12. Clean and unregister all 4 COM servers. Unregister LocalServer from the command line.
13. Unregister/uninstall the V2 interface
14. Register/install the V1 interface
15. Set ForV2=False in all 4 COM servers and rebuild from scratch. Register the LocalServer from the command line. Now you have four V1 servers.
-- At this point you have a V2 client and V1 servers --
16. Try to run EarlyBindTest, you'll see it fails immediately. You won't be able to run it in the debugger because it won't even build! That's because the V1 interface is currently installed. You can get it going in the debugger by uninstalling the V1 interface and installing the V2 interface, but then the COM servers will see the wrong world and not run. 

I may have overlooked something here, and it would be nice for someone else to repro all of this. 
