#if !defined(_MountDriverInterface_H_)
#define _MountDriverInterface_H_

#ifdef THESKYX_FOLDER_TREE
#include "imagingsystem/hardware/interfaces/licensed/driverrootinterface.h"
#include "imagingsystem/hardware/interfaces/licensed/linkinterface.h"
#include "imagingsystem/hardware/interfaces/licensed/deviceinfointerface.h"
#include "imagingsystem/hardware/interfaces/licensed/driverinfointerface.h"
#include "imagingsystem/hardware/interfaces/licensed/mount/needsrefractioninterface.h"
#else//TheSkyX Plug In Build
#include "../../licensedinterfaces/driverrootinterface.h"
#include "../../licensedinterfaces/linkinterface.h"
#include "../../licensedinterfaces/deviceinfointerface.h"
#include "../../licensedinterfaces/driverinfointerface.h"
#include "../../licensedinterfaces/mount/needsrefractioninterface.h"
#endif

/*!
\brief The MountDriverInterface allows an X2 implementor to a write X2 mount driver.

\ingroup Driver

See the X2Mount for an example.
*/
class MountDriverInterface : public DriverRootInterface, public LinkInterface, public HardwareInfoInterface, public DriverInfoInterface, public NeedsRefractionInterface 
{
public:
	/*! MotorState, Paramount only.*/
	enum MotorState
	{
		MKS_MOTOR_HOMING     								=(0x0100),
		MKS_MOTOR_SERVO       								=(0x0200),
		MKS_MOTOR_INDEXING    								=(0x0400),
		MKS_MOTOR_SLEWING     								=(0x0800),
		MKS_MOTOR_HOMED       								=(0x1000),
		MKS_MOTOR_JOYSTICKING 								=(0x2000),
		MKS_MOTOR_OFF         								=(0x4000),
		MKS_MOTOR_MOVING									= (MKS_MOTOR_HOMING | MKS_MOTOR_SLEWING | MKS_MOTOR_JOYSTICKING),

	};

	/*!Axis, Parmamount only.*/
	enum Axis
	{
		AXIS_RA		= 0,
		AXIS_DEC	= 1,
	};

	/*!Move direction.*/
	enum MoveDir
	{
		MD_NORTH	= 0,
		MD_SOUTH	= 1,
		MD_EAST		= 2,
		MD_WEST		= 3,
	};

	/*!MoveRate, Parmamount only.*/
	enum MoveRate
	{
		MR_FLASH	=	-2,
		MR_BASE		=	-1,
		MR_0R5X		=	 0,
		MR_1X		=	 1,
		MR_2X		=	 2,
		MR_4X		=	 3,
		MR_8X		=	 4,
		MR_16X		=	 5,
		MR_32X		=	 6,
		MR_64X		=	 7,
		MR_128X		=	 8,
		MR_256X		=	 9,
		MR_SLEW		=	 10,
	};

public:
	virtual ~MountDriverInterface(){} 

public:

// Operations
public:

	/*!\name DriverRootInterface Implementation
	See DriverRootInterface.*/
	//@{ 
	virtual DeviceType							deviceType(void)							  {return DriverRootInterface::DT_MOUNT;}
	virtual int									queryAbstraction(const char* pszName, void** ppVal) = 0;
	//@} 

	/*!\name LinkInterface Implementation
	See LinkInterface.*/
	//@{ 
	virtual int									establishLink(void)						= 0;
	virtual int									terminateLink(void)						= 0;
	virtual bool								isLinked(void) const					= 0;
	virtual bool								isEstablishLinkAbortable(void) const	{return false;}
	//@} 

	/*!\name DriverInfoInterface Implementation
	See DriverInfoInterface.*/
	//@{ 
	virtual void								driverInfoDetailedInfo(BasicStringInterface& str) const	=0;
	virtual double								driverInfoVersion(void) const				= 0;
	//@} 

	//
	/*!\name HardwareInfoInterface Implementation
	See HardwareInfoInterface.*/
	//@{ 
	virtual void deviceInfoNameShort(BasicStringInterface& str) const				= 0;
	virtual void deviceInfoNameLong(BasicStringInterface& str) const				= 0;
	virtual void deviceInfoDetailedDescription(BasicStringInterface& str) const	= 0;
	virtual void deviceInfoFirmwareVersion(BasicStringInterface& str)				= 0;
	virtual void deviceInfoModel(BasicStringInterface& str)						= 0;
	//@} 

	//NeedsRefractionInterface
	//Don't forget, each mount implementation must return type cast in queryAbstraction
	/*! Return true if the mount wants TheSkyX to handle refraction if and only if the mount itself does not internally handle refraction.*/
	virtual bool							needsRefactionAdjustments(void)	{return true;}

	/*! Return the mechanical or raw RA and declination of the mount.*/
	virtual int									raDec(double& dRawRA, double& dRawDec, const bool& bCached = false)					= 0;
	/*! Abort any operation currently in progress.*/
	virtual int									abort(void)																	= 0;

// Implementation
private:	

	
	
};

#endif
