TheSky Telescope API Plugin (TeleAPI) V5.0.3
--------------------------------------------

THIS FILE (TeleAPI-ReadMe.txt) IS LOCATED IN PROGRAM FILES\COMMON FILES\ASCOM (on 64-bit systems, look in "Program Files (x86)")

The TeleAPI plugin provides TheSky X and TheSky 6 with the ability to control telescopes via their ASCOM standard drivers. With this plugin, you can use telescopes not supported internally by TheSky but which have ASCOM drivers. Also, some telescopes have ASCOM drivers that provide enhanced functionality as compared to that provided with the internal support in TheSky. You can use such a telescope via its ASCOM driver with this plugin. 5.0.3 adds support for a special registry value used by TPOINT to determine if the connected mount is a German Equatorial type. This has been tested with TheSky 6 and its TPOINT version, but not with TheSky X and its (newer) TPOINT. The registry value for TheSky X is set via a best guess at whatit should be. 

Using with TheSky X
-------------------

One-Time Setup:

  1. Start TheSky X
  2. In the Telescope menu, select Setup...
  3. In the Imaging System Setup window that appears, select Mount in the left pane/list.
  4. Select Mount Setup at the top of the right pane, and choose Choose...
  5. In the Choose Mount window that appears, scroll down to Software Bisque, expand the tree, and select TeleAPI. Click OK.
  6. Again select Mount Setup and this time choose Settings... the ASCOM Telescope Chooser will appear.
  7. Select the type of mount you have. This tells TheSky to use the ASCOM driver for that mount type.
  8. Now click Properties... The driver's setup window will appear. Verify that it is set up (the correct COM port, etc.).
  9. Click OK to close the setup dialog.
 10. Click OK to close the Chooser.
 11. Click Close to close the Imaging System Setup window of TheSky X.

Routine Use:

There are two ways to connect the telescope: from the Telescope menu and from the Telescope tab on the left edge of TheSky's main window. The Connect item on the Telescope menu will disconnect the scope if it is connected. There is a separete disconnect control on the Telescope tab. Refer to TheSky X help info for details. 

NOTE: Due to a complexity in TheSky X that is not present in TheSky 6, disconnecting from a telescope that has an executable driver will not automatically release and allow the driver executable to exit. If you wait long enough (up to 10 minutes), the executable driver WILL automatically release and exit. It's best to just let it exit on its own; if you want to have it end sooner you'll have to kill it with the Task Manager. 


Using with TheSky 6
-------------------

One-time setup:

  1. In the Telescope menu, select Setup.
  2. In the Telescope Setup window, at the top, select Telescope API
  3. Just below that selector, click Settings...  the ASCOM Telescope Chooser will appear.
  4. Select the type of mount you have. This tells TheSky to use the ASCOM driver for that mount type.
  5. Now click Properties... The driver's setup window will appear. Verify that it is set up (the correct COM port, etc.).
  6. Click OK to close the setup dialog.
  7. Click OK to close the Chooser.
  8. Back in the Telescope Setup window, at the bottom, set the Cross hair update frequency to 1000 (ms) or more. Some telescopes can't handle faster requests for updates.
  9. Click Close to close the Telescope Setup window.

Routine use:

To connect, in the Telescope menu, select Link, then in the submenu, Establish. To disconnect, in the Telescope menu, select Link, then in the submenu, Terminate.
