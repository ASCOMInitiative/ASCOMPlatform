#ifndef _HardwareInfoInterface_H
#define _HardwareInfoInterface_H

#define HardwareInfoInterface_Name "com.bisque.TheSkyX.HardwareInfoInterface/1.0"

class BasicStringInterface;

/*!
\brief The HardwareInfoInterface provides X2 implementors a standarized way to provide hardware specific information.

\ingroup Interface

*/
class HardwareInfoInterface
{
public:

	virtual ~HardwareInfoInterface(){}

public:
	//HardwareInfoInterface
	/*! Return a short device name.*/
	virtual void deviceInfoNameShort			(BasicStringInterface& str) const=0;
	/*! Return a detailed device name.*/
	virtual void deviceInfoNameLong				(BasicStringInterface& str) const=0;
	/*! Return a detailed device description.*/
	virtual void deviceInfoDetailedDescription	(BasicStringInterface& str) const=0;
	/*! Return the firmware version, if available.*/
	virtual void deviceInfoFirmwareVersion		(BasicStringInterface& str)		 =0;
	/*! Return the device model name.*/
	virtual void deviceInfoModel				(BasicStringInterface& str)		 =0;
};

#endif