#include "StdAfx.h"
#include "../../licensedinterfaces/sberrorx.h"
#include "../../licensedinterfaces/basicstringinterface.h"

X2Mount::X2Mount(const char* pszDriverSelection,
				const int& nInstanceIndex,
				SerXInterface						* pSerX, 
				TheSkyXFacadeForDriversInterface	* pTheSkyX, 
				SleeperInterface					* pSleeper,
				BasicIniUtilInterface				* pIniUtil,
				LoggerInterface						* pLogger,
				MutexInterface						* pIOMutex,
				TickCountInterface					* pTickCount)
{
	m_nPrivateMultiInstanceIndex	= nInstanceIndex;
	m_pSerX							= pSerX;		
	m_pTheSkyXForMounts				= pTheSkyX;
	m_pSleeper						= pSleeper;
	m_pIniUtil						= pIniUtil;
	m_pLogger						= pLogger;	
	m_pIOMutex						= pIOMutex;
	m_pTickCount					= pTickCount;

}
X2Mount::~X2Mount()
{
}


int	X2Mount::queryAbstraction(const char* pszName, void** ppVal)
{
	*ppVal = NULL;

	if (!strcmp(pszName, SyncMountInterface_Name))
		*ppVal = dynamic_cast<SyncMountInterface*>(this);
	else if (!strcmp(pszName, SlewToInterface_Name))
		*ppVal = dynamic_cast<SlewToInterface*>(this);
	else if (!strcmp(pszName, TrackingRatesInterface_Name))
		*ppVal = dynamic_cast<TrackingRatesInterface*>(this);

	return SB_OK;
}

//LinkInterface
int	X2Mount::establishLink(void)					
{
	return ERR_NOT_IMPL;
}

int	X2Mount::terminateLink(void)						
{
	return ERR_NOT_IMPL;
}

bool X2Mount::isLinked(void) const					
{
	return false;
}
bool X2Mount::isEstablishLinkAbortable(void) const	{return false;}

//AbstractDriverInfo

#define DISPLAY_NAME "Mount Plug In"
void	X2Mount::driverInfoDetailedInfo(BasicStringInterface& str) const
{
	str = DISPLAY_NAME;
}

double	X2Mount::driverInfoVersion(void) const				
{
	return 0.0;
}

//AbstractDeviceInfo
void X2Mount::deviceInfoNameShort(BasicStringInterface& str) const				
{
	str = DISPLAY_NAME;
}

void X2Mount::deviceInfoNameLong(BasicStringInterface& str) const				
{
	str = DISPLAY_NAME;

}

void X2Mount::deviceInfoDetailedDescription(BasicStringInterface& str) const	
{
	str = DISPLAY_NAME;
	
}

void X2Mount::deviceInfoFirmwareVersion(BasicStringInterface& str)		
{
	str = DISPLAY_NAME;
}

void X2Mount::deviceInfoModel(BasicStringInterface& str)				
{
	str = DISPLAY_NAME;
}

//Common Mount specifics
int	X2Mount::raDec(double& ra, double& dec, const bool& bCached)
{
	return ERR_NOT_IMPL;
}

int	X2Mount::abort(void)												
{
	return ERR_NOT_IMPL;
}

//SyncMountInterface
int X2Mount::syncMount(const double& ra, const double& dec)
{
	return SB_OK;
}

bool X2Mount::isSynced()
{
	return true;
}

//SlewToInterface
/*!Initiate the slew.*/
int X2Mount::startSlewTo(const double& dRa, const double& dDec)
{
	return SB_OK;
}

/*!Called to monitor the slew process. \param bComplete Set to true if the slew is complete, otherwise return false.*/
int X2Mount::isCompleteSlewTo(bool& bComplete) const
{
	return SB_OK;
}

/*!Called once the slew is complete. This is called once for every corresponding startSlewTo() allowing software implementations of gotos.*/
int X2Mount::endSlewTo(void)
{
	return SB_OK;
}

//TrackingRatesInterface
/*!Set the tracking rates.*/
int X2Mount::setTrackingRates( const bool& bTrackingOn, const bool& bIgnoreRates, const double& dRaRateArcSecPerSec, const double& dDecRateArcSecPerSec)
{
	return SB_OK;
}

/*!Return the current tracking rates.  A special case for mounts that can set rates, but not read them...
   So the TheSkyX's user interface can know this, set bTrackSidereal=false and both rates to -1000.0*/
int X2Mount::trackingRates( bool& bTrackingOn, double& dRaRateArcSecPerSec, double& dDecRateArcSecPerSec)
{
	return SB_OK;
}
