#ifndef _RotatorDriverInterface_H
#define _RotatorDriverInterface_H

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

class BasicStringInterface;

/*!
\brief The RotatorDriverInterface allows an X2 implementor to a write X2 rotator driver.

\ingroup Driver

See the X2Rotator for an example.
*/
class RotatorDriverInterface  : public DriverRootInterface, public LinkInterface, public HardwareInfoInterface, public DriverInfoInterface 
{
public:
	virtual ~RotatorDriverInterface(){}

	/*!\name DriverRootInterface Implementation
	See DriverRootInterface.*/
	//@{ 
	virtual DeviceType							deviceType(void)							  {return DriverRootInterface::DT_ROTATOR;}
	virtual int									queryAbstraction(const char* pszName, void** ppVal) = 0;
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

	/*!\name LinkInterface Implementation
	See LinkInterface.*/
	//@{ 
	virtual int									establishLink(void)						= 0;
	virtual int									terminateLink(void)						= 0;
	virtual bool								isLinked(void) const					= 0;
	virtual bool								isEstablishLinkAbortable(void) const	{return false;}
	//@} 

	/*!Return the position of the rotator.*/
	virtual int									position(double& dPosition)			= 0;
	/*!Abort any operation in progress.*/
	virtual int									abort(void)							= 0;

	/*!Initiate the rotator goto.*/
	virtual int									startRotatorGoto(const double& dTargetPosition)	= 0;
	/*!Called to moitor the goto process.  \param bComplete Set to true if the goto is complete, otherwise set to false.*/
	virtual int									isCompleteRotatorGoto(bool& bComplete) const	= 0;
	/*!Called after the goto process is complete. This is called once for every corresponding startRotatorGoto() allowing software implementations of gotos.*/
	virtual int									endRotatorGoto(void)							= 0;


};


#endif