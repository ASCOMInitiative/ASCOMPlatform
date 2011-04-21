#include <math.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <time.h>
#include "ASCOM.Rotator.h"
#include "../../licensedinterfaces/sberrorx.h"

#include "../../licensedinterfaces/basiciniutilinterface.h"
#include "../../licensedinterfaces/basicstringinterface.h"


X2Rotator::X2Rotator(const char* pszDriverSelection,
						const int&							nInstanceIndex,
						SerXInterface						* pSerX, 
						TheSkyXFacadeForDriversInterface	* pTheSkyX, 
						SleeperInterface					* pSleeper,
						BasicIniUtilInterface				* pIniUtil,
						LoggerInterface						* pLogger,
						MutexInterface						* pIOMutex,
						TickCountInterface					* pTickCount)
{
	m_nInstanceIndex				= nInstanceIndex;
	m_pSerX							= pSerX;		
	m_pTheSkyXForMounts				= pTheSkyX;
	m_pSleeper						= pSleeper;
	m_pIniUtil						= pIniUtil;
	m_pLogger						= pLogger;	
	m_pIOMutex						= pIOMutex;
	m_pTickCount					= pTickCount;

	char m_pszDriverSelection[DRIVER_MAX_STRING];
	sprintf(m_pszDriverSelection, pszDriverSelection);
	
}

X2Rotator::~X2Rotator()
{
	if (GetSimpleIniUtil())
		delete GetSimpleIniUtil();
}

int	X2Rotator::queryAbstraction(const char* pszName, void** ppVal) 
{
	*ppVal = NULL;

	if (!strcmp(pszName, ModalSettingsDialogInterface_Name))
		*ppVal = dynamic_cast<ModalSettingsDialogInterface*>(this);

	return SB_OK;
}


int	X2Rotator::establishLink(void)						
{
	return SB_OK;
}
int	X2Rotator::terminateLink(void)						
{
	return SB_OK;
}
bool X2Rotator::isLinked(void) const					
{
	return false;
}

void X2Rotator::deviceInfoNameShort(BasicStringInterface& str) const				
{
	str = "Rotator";
}
void X2Rotator::deviceInfoNameLong(BasicStringInterface& str) const				
{
	str = PLUGIN_DISPLAY_NAME;
}
void X2Rotator::deviceInfoDetailedDescription(BasicStringInterface& str) const		
{
	str = "Connects to asn ASCOM Rotator driver";
}
void X2Rotator::deviceInfoFirmwareVersion(BasicStringInterface& str)				
{
	str = "n/a";
}
void X2Rotator::deviceInfoModel(BasicStringInterface& str)							
{
	str = "n/a";	// TODO last chosen rotator type
}

void X2Rotator::driverInfoDetailedInfo(BasicStringInterface& str) const
{
	str = "<runtime from chosen driver>";	// TODO from chosen driver
}
double X2Rotator::driverInfoVersion(void) const				
{
	return 0.0;		// TODO from chosen driver
}

int	X2Rotator::position(double& dPosition)			
{
	dPosition = 25000.0;
	return SB_OK;
}
int	X2Rotator::abort(void)							
{
	return SB_OK;
}

int	X2Rotator::startRotatorGoto(const double& dTargetPosition)	
{
	return SB_OK;
}
int	X2Rotator::isCompleteRotatorGoto(bool& bComplete) const	
{
	return SB_OK;
}
int	X2Rotator::endRotatorGoto(void)							
{
	return SB_OK;
}


int X2Rotator::execModalSettingsDialog()
{
	return SB_OK;
}