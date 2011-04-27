#include "StdAfx.h"

const char *_szAlertTitle = "ASCOM Standard Telescope";

//
// Mount characteristics
//
char *_szScopeName = NULL;                      // Name for crosshair labeling (delete[])
bool _bScopeCanSlew = false;                    // Capabilites of this scope
bool _bScopeCanSlewAsync = false;
bool _bScopeCanSlewAltAz = false;				
bool _bScopeCanSync = false;
bool _bScopeIsGEM = false;
bool _bScopeCanSetTracking = false;
bool _bScopeCanSetTrackRates = false;
bool _bScopeCanPark = false;
bool _bScopeCanUnpark = false;
bool _bScopeCanSetPark = false;
bool _bScopeDoesRefraction = false;
bool _bScopeCanSideOfPier = false;

//
// State variables
//
bool _bScopeActive = false;                     // This is true if mount is active


extern "C" PlugInExport int sbPlugInName2(BasicStringInterface& str)
{
	str = _szAlertTitle;

	return 0;
}

extern "C" PlugInExport int sbPlugInFactory2(	const char* pszDisplayName, 
												const int& nInstanceIndex,
												SerXInterface					* pSerXIn, 
												TheSkyXFacadeForDriversInterface	* pTheSkyXIn, 
												SleeperInterface					* pSleeperIn,
												BasicIniUtilInterface			* pIniUtilIn,
												LoggerInterface					* pLoggerIn,
												MutexInterface					* pIOMutexIn,
												TickCountInterface				* pTickCountIn,
												void** ppObjectOut)
{
	*ppObjectOut = NULL;
	X2Mount* gpMyImpl = NULL;

	if (NULL == gpMyImpl)
		gpMyImpl = new X2Mount(	pszDisplayName, 
									nInstanceIndex,
									pSerXIn, 
									pTheSkyXIn, 
									pSleeperIn,
									pIniUtilIn,
									pLoggerIn,
									pIOMutexIn,
									pTickCountIn);


	*ppObjectOut = gpMyImpl;

	return 0;
}

