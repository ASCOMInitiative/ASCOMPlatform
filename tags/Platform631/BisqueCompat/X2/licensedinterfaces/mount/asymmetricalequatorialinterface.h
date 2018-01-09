#ifndef _AsymmetricalEquatorialInterface_H
#define _AsymmetricalEquatorialInterface_H

#include "mounttypeinterface.h"

#define AsymmetricalEquatorialInterface_Name "com.bisque.TheSkyX.AsymmetricalEquatorialInterface/1.0"

/*!\defgroup Tool Device Driver Tool Kit*/
/*!\defgroup Driver Driver Interfaces */
/*!\defgroup Interface Optional Interfaces*/
/*!\defgroup Example Example Drivers*/
/*!\defgroup GUI Graphical User Interface (GUI)*/

/*!
\mainpage
<CENTER><H1>The X2 Standard</H1>

TheSkyX's Cross-Platform, Plug In, Device Driver Architecture</CENTER>

<B>X2 Features</B>


- Cross-platform (Windows/Mac/Linux)
- Based upon C++ standard.  Minimum dependencies for easy portability/maintainability across operating systems.  
- Qt is NOT required.  Allows third parties to develop drivers independent of any cross-platform library.   
- Suitable architecture for hardware manufacturers to rely upon for their native, device driver development.  
- Consumable from most every programming language (thin operating system specific callable layering required).
- Architected upon the basic principles of object oriented programming namely encapsulation, polymorphism, and inheritance.  
	- Inheritance is used carefully where an interface (a C++ class with one or more pure virtual functions) purposefully hides implementation from caller (TheSkyX) minimizing dependencies while focusing the interface on one task.
	- Keeps client (TheSkyX) code more managable.
- Modular in terms of levels of support. 
	- Can handle devices of a given kind with a broad range of capabilities without being plagued with a "CanXXX" for every property/method.
	- Basic or "essence" support isn't plagued/complicated by "no op" stubs that must be re-implemented for devices that don't have such a capability.
	- Flexible for adding a new capability not found in any other device in kind.  Only the device with the new feature needs recompiled not all devices in kind. 
		- All other devices don't have to be brought up to the same level of support.
		- Clients (TheSkyX) wanting to leverage new capability must of course be updated (recompiled) to take advantage of new capability.
- Easy to implement features in devices not up to superset.  Clients (TheSkyX) compatible with superset automatically leverage new capability.
- Shows/provides a way (means) to evolve.
- Not a native "discovery" methodology (but discovery standard could be accommodated).
- Supports control of an open-ended number of devices (coming to TheSkyX) along with their persistence .

<B>Introduction</B>

One of the main goals of the X2 standard is to make it possible and easy for third parties to write and maintain their own hardware drivers compatible with TheSkyX on all operating systems it supports.  In addition, the X2 standard is by nature extensible, that is, it can grow at the optional, interface level without all clients needing to be simultaneously brought up to the same level or even recompiled for that matter.  
 
<B>How to Write a TheSkyX Driver</B>

For illustration purposes, the following steps are for creation of a camera driver, but the same steps are involved in making any X2 driver.

The X2Camera example source code provides a pattern to follow to create a TheSkyX camera driver.  

-# Obtain TheSkyX version 10.1.9 or later.
-# Tell TheSkyX A) how to display your camera to the user for selection and B) the name of the file that is your plug in binary by making your own company specific list of camera hardware.
	-# Make a copy of "cameralist.txt" distributed by TheSkyX and name it "cameralist X2.txt" (it goes in the same folder as cameralist.txt).
	-#	Edit "cameralist X2.txt" remove all lines except one and following the existing format, enter your specific camera information and the name of the file is your plug in binary in the PlugInDLLName field, for example:
		-# "1|X2|X2Camera|The X2Camera is great!| |X2Camera"
		-#	See the header of the file "hardwarelist.txt" distributed by TheSkyX for more details on the file format.
-# Compile the X2Camera sample unmodified, and place the binary into the TheSkyX/Resources/Common/PlugIns/CameraPlugIns folder. Start TheSkyX, go to Telescope, Setup and in the Imaging System Setup tree select Cameras, select the X2Camera and choose Connect.  The X2 plug in dll CCEstablishLink will be called.
-# Implement the X2Camera with device dependent io calls.  See the X2Camera source for more details on function calls.  Use conditional compilation for OS specific io calls or branch in main.cpp with two entirely different X2Camera implementations depending upon OS.

<B>Distributing Your X2 Driver</B>

You'll need to distribute at least two files, your own "hardwarelist <Company Name>.txt" with your list of hardware and the corresponding plug in binary (not named X2Camera, of course).  Any other libraries your binary requires also needs distributed, strive to minimize other dependencies and static link libraries when possible.





<B>Change Log</B>
- 1.01 - Changed x2mount example to use TheSkyXFacadeForDriversInterface instead of deprecated TheSkyXFacadeForMountsInterface (interface identical).
- 1.02 - Added Mac make files for X2 examples.
- 1.03 - The ModalSettingsDialogInterface and SerialPortParams2Interface are no longer mutually exclusive.
- 1.04 - SerXInterface::open allows setting data bits.
- 1.05 - Filter wheel support, see FilterWheelDriverInterface.
- 1.06 - Documented \ref embeddeddevices.
*/

/*!
\brief The AsymmetricalEquatorialInterface for equtorial mounts.

\ingroup Interface

If a X2 mount driver implements this interface, the mount is an asymmetrical equtorial mount (e.g. GEM or cross-axis).
\sa SymmetricalEquatorialInterface
*/

class AsymmetricalEquatorialInterface 
{
public:

	virtual ~AsymmetricalEquatorialInterface(){}

	/*!
	The default implemtnation returns the appropriate type of mount.
	*/
	MountTypeInterface::Type mountType(){return MountTypeInterface::Asymmetrical_Equatorial;}

	/*!
	If knowsBeyondThePole() returns false, the mount
	cannot distinguish unambiguosly if the OTA end of the declination axis 
	is either east or west of the pier. This somewhat restricts use of the 
	mount with TPoint - the mount must always have the OTA end of the declination 
	axis higher than the counterweights. In other words, the mount should not slew past the meridian.
	*/
	virtual bool knowsBeyondThePole() {return false;}

	/*!
	If knowsBeyondThePole() returns true,
	then beyondThePole() tells TheSkyX unambiguously 
	if the OTA end of the declination axis 
	is either east (0) or west of the pier (1).
	Note, the return value must be correct even
	for cases where the OTA end of the Dec axis 
	is lower than the counterweights.
	*/
	virtual int beyondThePole(bool& bYes){bYes=false; return 0;}
	
	/*!
	Return the hour angle at which the mount automatically flips.
	*/
	virtual double flipHourAngle() {return 0;}

	/*!
	Return the east and west hour angle limits.
	*/
	virtual int gemLimits(double& dHoursEast, double& dHoursWest){dHoursEast=dHoursWest=0;return 0;}

};

#endif