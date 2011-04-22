#include "StdAfx.h"
#include "main.h"
#include "../../licensedinterfaces/basicstringinterface.h"

const char *_szAlertTitle = "ASCOM Standard Telescope";

//
// Mount characteristics
//
bool _bScopeCanSlew = false;                    // This is true if mount can slew at all
bool _bScopeCanSlewAsync = false;               // This is true if mount can slew asynchronously
bool _bScopeCanSync = false;                    // This is true if mount can be sync'ed
bool _bScopeHasEqu = false;                     // This is true if mount provides RA/Dec
bool _bScopeIsGEM = false;						// True if driver reports that mount is GEM
char *_szScopeName = NULL;                      // Name for crosshair labeling (delete[])

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

