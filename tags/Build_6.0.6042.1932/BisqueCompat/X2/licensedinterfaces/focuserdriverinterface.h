#ifndef _FocuserDriverInterface_H
#define _FocuserDriverInterface_H

#include "../../licensedinterfaces/driverrootinterface.h"
#include "../../licensedinterfaces/linkinterface.h"
#include "../../licensedinterfaces/deviceinfointerface.h"
#include "../../licensedinterfaces/driverinfointerface.h"

//
// Made up by Bob Denny for the Focuser plugin.
//
class FocuserDriverInterface : public DriverRootInterface, 
							public LinkInterface, 
							public HardwareInfoInterface, 
							public DriverInfoInterface 
{
public:
	virtual ~FocuserDriverInterface(){}

	/*!\name DriverRootInterface Implementation
		See DriverRootInterface.*/
	//@{ 
	virtual DeviceType							deviceType(void) {return DriverRootInterface::DT_FOCUSER;}
	virtual int									queryAbstraction(const char* pszName, void** ppVal) = 0;
	//@} 

	/*!\name LinkInterface Implementation
		See LinkInterface.*/
	//@{ 
	virtual int									establishLink(void)						= 0;
	virtual int									terminateLink(void)						= 0;
	virtual bool								isLinked(void) const					= 0;
	//@} 

	/*!\name HardwareInfoInterface Implementation
		See HardwareInfoInterface.*/
	//@{ 
	virtual void deviceInfoNameShort(BasicStringInterface& str) const					{};
	virtual void deviceInfoNameLong(BasicStringInterface& str) const					{};
	virtual void deviceInfoDetailedDescription(BasicStringInterface& str) const			{};
	virtual void deviceInfoFirmwareVersion(BasicStringInterface& str)					{};
	virtual void deviceInfoModel(BasicStringInterface& str)								{};
	//@} 

	/*!\name DriverInfoInterface Implementation
		See DriverInfoInterface.*/
	//@{ 
	virtual void								driverInfoDetailedInfo(BasicStringInterface& str) const	= 0;
	virtual double								driverInfoVersion(void) const							= 0;
	//@} 

};



#endif