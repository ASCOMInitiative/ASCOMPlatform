#ifndef _DriverRootInterface_H
#define _DriverRootInterface_H

#define DriverRootInterface_Name "com.bisque.TheSkyX.DriverRootInterface/1.0"

#define DRIVER_MAX_STRING 1000

/*!
\brief The DriverRootInterface is the foundation for all X2 device drivers.

\ingroup Tool

Each specific DeviceType implementation inherits this interface and
adds methods/proproties common to all devices in kind, if any.  TheSkyX
leverages queryAbstraction() as a runtime means of obtaining, optional
well-defined interfaces.
Tested and works on Windows, Mac, Ubuntu Linux.
*/

class DriverRootInterface
{
public:

	/*! DeviceType. */
	enum DeviceType
	{
		DT_UNKNOWN		= 0,/**<  Unknown device type.*/
		DT_MOUNT		= 1,/**<  Mount.*/
		DT_FOCUSER		= 2,/**<  Focuser.*/
		DT_CAMERA		= 3,/**<  Camera.*/
		DT_FILTERWHEEL	= 4,/**<  Filter wheel.*/
		DT_DOME			= 5,/**<  Dome.*/
		DT_ROTATOR		= 6,/**<  Rotator.*/
		DT_WEATHER		= 7,/**<  Weather station.*/
		DT_GPSTFP		= 8,/**<  Accurate timing.*/
	};

	virtual ~DriverRootInterface(){}

public:
	/*!Returns the type of device.*/
	virtual DeviceType							deviceType(void)										= 0;
	/*!Return a pointer to well defined interface.*/
	virtual int									queryAbstraction(const char* pszName, void** ppVal)		= 0;

};

#endif