// TeleAPI.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"

//
// Bisque error codes
//
#define OK					0
#define TS_E_STARTFAIL		1400
#define TS_E_NOSCOPE		1401
#define TS_E_NOTIMPL		1402

//Notes
//All functions should return 0 (zero) to indicate success.
//Othersize return a "custom" error code between 1400-1499 (inclusive)
//All functions block TheSky until they return.
//Any and all errors returned are fatal and cause TheSky to display an error message,
// The one exception is that two successive errors returned from tapiGetRaDec 
// must be encountered to cause TheSky to automatically terminate a successful 
// link to the telescope.

const char *_szAlertTitle = "TheSky TeleAPI ASCOM Plugin";

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
HWND _hWndMain = NULL;							// For bringing to front after slew

//
//Do not alter this function,  
//201 Added tapiPulseFocuser and tapiSettings
//202 Explicitly added TELEAPIEXPORT and CALLBACK to all functions
//As of January 19, 1999 the version is 2.02
//In June 2010 Matt Bisque gave me (rbd) "the latest" and it is 
//all the same 2.02...
//
#define nTeleAPIVersion 202
#define sTeleAPIVersion "2.02"
#define sTeleAPIDate "Jan 1999"
TELEAPIEXPORT int CALLBACK tapiGetDLLVersion(void)
{
	return nTeleAPIVersion;
}

//Called when Telescope, Link, Establish is selected
//Do any initialization here
TELEAPIEXPORT int CALLBACK tapiEstablishLink(void)
{
	int iRes = OK;

	if (_bScopeActive) {		// TheSky X allows connect to connected!
		return iRes;
	}

	__try {
		InitScope();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = TS_E_STARTFAIL;
	}
	return iRes;
}

//Called when Telescope, Link, Terminate is selected
//Do any clean up here
TELEAPIEXPORT int CALLBACK tapiTerminateLink(void)
{
	if (_bScopeActive) {		// TheSky X allows disconnect to disconnected!
		TermScope(false);
	}

	return 0;
}

//Called when TheSky needs to know the telescope position.
//Return as quickly as possible as this is called very frequently.
//ra 0.0 to 24.0
//dec -90.0 to 90.0
TELEAPIEXPORT int CALLBACK tapiGetRaDec(double* ra, double* dec)
{
	int iRes = OK;

	if(!_bScopeActive)	{								// No scope hookup?
		*ra = *dec = 0.0;			
		return(TS_E_NOSCOPE);							// Forget this
	}

	__try {
		*ra = GetRightAscension();
		*dec = GetDeclination();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = TS_E_NOSCOPE;
	}
	return iRes;
}

//Called when the "Sync" button is selected in TheSky
//(to set the telescope's current coordinates)
//ra 0.0 to 24.0
//dec -90.0 to 90.0
TELEAPIEXPORT int CALLBACK tapiSetRaDec(const double ra, const double dec)
{
	int iRes = OK;

	if(!_bScopeActive)									// No scope hookup?
		return(TS_E_NOSCOPE);							// Forget this

	__try {
		SyncScope(ra, dec);
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = TS_E_NOSCOPE;
	}
	return iRes;
}

//Called to instruct the telescope to go to these coordinates
//TheSky will display the "Telescope Slewing" dialog box
//until the coordinates returned from tapiGetRaDec are no longer changing
//This of course assumes that an ansychronous process has been started and you
//have returned from this function and given control back to TheSky.
//ra 0.0 to 24.0
//dec -90.0 to 90.0
TELEAPIEXPORT int CALLBACK tapiGotoRaDec(const double ra, const double dec)
{
	int iRes = OK;

	if(!_bScopeActive)									// No scope hookup?
		return(TS_E_NOSCOPE);							// Forget this

	_hWndMain = GetActiveWindow();						// For later restore
	__try {
		SlewScope(ra, dec);
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = TS_E_NOSCOPE;
	}
	return iRes;
}

//TheSky calls this function at a given interval (every 1000 ms by default) 
//to see if the Goto is complete.  If necessary, return an error code
//if the goto is not successfully completed.
TELEAPIEXPORT int CALLBACK tapiIsGotoComplete(BOOL* pbComplete)
{
	int iRes = OK;									// Assume success

	if(!_bScopeActive)								// No scope hookup?
		return(TS_E_NOSCOPE);						// Forget this

	__try {
		*pbComplete = !IsSlewing();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = TS_E_NOSCOPE;
	}
	return iRes;
}

//Called when the user hits the abort button on the "Telescope Slewing" dialog box
//to stop any Goto currently in progress
TELEAPIEXPORT int CALLBACK tapiAbortGoto(void)
{
	int iRes = OK;									// Assume success

	if(!_bScopeActive)								// No scope hookup?
		return(TS_E_NOSCOPE);						// Forget this

	__try {
		AbortSlew();
		SetForegroundWindow(_hWndMain);				// Bring TheSky to front
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = TS_E_NOSCOPE;
	}
	return iRes;
}

//Called when the user presses the Settings button
//under Telescope, Setup
//The return value is ignored.
TELEAPIEXPORT int CALLBACK tapiSettings(void)
{
	(void) InitDrivers();
	if (ConfigScope() == 0)
		return OK;
	return TS_E_STARTFAIL;
}

//Called to move the focuser a distinct amount
//bFastSpeed = 1 or 0 to move the focus fast (or larger distance) or slow (or smaller distance)
//bIn = 1 or 0 to move the focuser in or out
TELEAPIEXPORT int CALLBACK tapiPulseFocuser(const BOOL bFastSpeed, const BOOL bIn)
{
	return TS_E_NOTIMPL;
}

//Called when TELEAPI.DLL is loaded
BOOL WINAPI DllMain (HANDLE hDLL, DWORD dwReason, LPVOID lpReserved)
{
	
    if (dwReason == DLL_PROCESS_ATTACH)
	{			
		DisableThreadLibraryCalls((HINSTANCE)hDLL);	// No thread att/det calls
		InitDrivers();
		return TRUE;
	}
    else if (dwReason == DLL_PROCESS_DETACH)
    {
	    TermScope(false);
	    return TRUE;
    }
    return TRUE;   // ok
}

