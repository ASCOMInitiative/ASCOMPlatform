ASCOM Mount/Telescope Plugin for TheSky X
=========================================

This X2 driver/plugin allows you to control any mount/telescope that has an ASCOM driver. The plugin conforms to the Software Bisque X2 standard for device control. It provides a pathway from TheSky X to any ASCOM Telescope driver and its mount.


One-time Setup
--------------

1. Select ASCOM/Telescope Driver for the mount type (Mount Setup, Choose...)

2. Mount Setup, Settings... will open the usual ASCOM Chooser

3. Select your telescope type. Click Properties in the Chooser to configure the telescope.

4. Close the Chooser, and from there it's normal TheSky X operation.


Variable Capabilities
---------------------

Depending on the capabilities of the selected ASCOM mount/telescope, controls may or may not appear in TheSky X. For example, if the ASCOM mount does not support control of sidereal tracking (on/off), TheSky X's Telescope panel Tools popup menu will not contain the "Turn Sidereal Tracking On" or "Turn Tracking Off" items.


Operational Issues
------------------

* After changing ASCOM telescope drivers, exit and restart TheSKy X to refresh the descriptive info and available controls (see Variable Capabilities above).

* If your mount supports RA/Dec offset tracking rates (for tracking asteroids, etc.), you can use the Set Track Rates button to enable offsets after selecting the asteroid or whatever. To return the rates to RA=Sidereal/Dec=Zero you need to select a star as a target then use the Set Track Rates button, then click Yes to explicitly set the rate offsets to 0 and 0. The No button in the COnfirm Set Track Rate window does not work (as of TheSky X 10.1.4749).

* If your mount does not support RA/Dec offset tracking rates, but it does support simple on-off control of Sidereal Tracking, TheSky X will still show the Set Track Rates button. This is due to a limitation in TheSky X where support for the two features is tied to the same enabling/disabling logic. If you try to use the Set Track Rates button, you'll get an Unsupported Function popup, but telescope control will continue uninterrupted.


Reporting Problems
------------------

This plugin is not part of TheSky X. It was developed for the ASCOM Initiative by Bob Denny. The place to report problems and ask questions is in the ASCOM Talk group on Yahoo:

http://tech.groups.yahoo.com/group/ASCOM-Talk/

