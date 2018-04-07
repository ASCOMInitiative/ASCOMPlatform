#ifndef _DomeDriverInterface_H
#define _DomeDriverInterface_H

#ifdef THESKYX_FOLDER_TREE
#include "imagingsystem/hardware/interfaces/licensed/driverrootinterface.h"
#include "imagingsystem/hardware/interfaces/licensed/linkinterface.h"
#include "imagingsystem/hardware/interfaces/licensed/deviceinfointerface.h"
#include "imagingsystem/hardware/interfaces/licensed/driverinfointerface.h"
#else
#include "../../licensedinterfaces/driverrootinterface.h"
#include "../../licensedinterfaces/linkinterface.h"
#include "../../licensedinterfaces/deviceinfointerface.h"
#include "../../licensedinterfaces/driverinfointerface.h"
#endif


/*!
\brief The DomeDriverInterface allows an X2 implementor to a write X2 dome driver.

\ingroup Driver

*/
class DomeDriverInterface : public DriverRootInterface, public LinkInterface, public HardwareInfoInterface, public DriverInfoInterface 
{
public:
	virtual ~DomeDriverInterface(){}

	/*!\name DriverRootInterface Implementation
		See DriverRootInterface.*/
	//@{ 
	virtual DeviceType							deviceType(void)							  {return DriverRootInterface::DT_DOME;}
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

	/*! Return the dome azimuth (and elevation, if necessary).*/
	virtual int dapiGetAzEl(double* pdAz, double* pdEl)=0;
	/*! Inititate a dome goto. \sa dapiIsGotoComplete()*/
	virtual int dapiGotoAzEl(double dAz, double dEl)=0;
	/*! Abort any operation in progress.*/
	virtual int dapiAbort(void)=0;
	/*! Inititate opening the dome slit. \sa dapiIsOpenComplete()*/
	virtual int dapiOpen(void)=0;
	/*! Inititate closing the dome slit. \sa dapiIsCloseComplete()*/
	virtual int dapiClose(void)=0;
	/*! Inititate parking the dome. \sa dapiIsParkComplete()*/
	virtual int dapiPark(void)=0;
	/*! Inititate unparking the dome. \sa dapiIsUnparkComplete()*/
	virtual int dapiUnpark(void)=0;
	/*! Inititate finding home. \sa dapiIsFindHomeComplete()*/
	virtual int dapiFindHome(void)=0;

	/*!\name Is Complete Members*/
	//@{ 
	/*! Return if the goto is complete.*/
	virtual int dapiIsGotoComplete(bool* pbComplete)=0;
	/*! Return if the open is complete.*/
	virtual int dapiIsOpenComplete(bool* pbComplete)=0;
	/*! Return if the open is complete.*/
	virtual int	dapiIsCloseComplete(bool* pbComplete)=0;
	/*! Return if the park is complete.*/
	virtual int dapiIsParkComplete(bool* pbComplete)=0;
	/*! Return if the unpark is complete.*/
	virtual int dapiIsUnparkComplete(bool* pbComplete)=0;
	/*! Return if find home is complete.*/
	virtual int dapiIsFindHomeComplete(bool* pbComplete)=0;
	//@} 

	/*! Initialize the dome coordinate to dAz (and dEl if necessary)*/
	virtual int dapiSync(double dAz, double dEl)=0;

};



#endif