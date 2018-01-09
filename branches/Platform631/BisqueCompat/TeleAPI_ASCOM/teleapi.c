#include "AscomScope.h"
#include <windows.h>
#include "stdio.h"
#include "teleapi.h"

//Notes
//All functions should return 0 (zero) to indicate success.
//Othersize return a "custom" error code between 1400-1499 (inclusive)
//All functions block TheSky until they return.
//Any and all errors returned are fatal and cause TheSky to display an error message,
// The one exception is that two successive errors returned from tapiGetRaDec 
// must be encountered to cause TheSky to automatically terminate a successful 
// link to the telescope.

const char *_szAlertTitle = "TheSky TeleAPI ASCOM Plugin";

bool _bScopeCanSlew = false;                    // This is true if scope can slew at all
bool _bScopeCanSlewAsync = false;               // This is true if scope can slew asynchronously
bool _bScopeCanSync = false;                    // This is true if scope can be sync'ed
bool _bScopeHasEqu = false;                     // This is true if scope provides RA/Dec
bool _bScopeActive = false;                     // This is true if scope is active
char *_szScopeName = NULL;                      // Name for crosshair labeling (delete[])

//Do not alter this function
#define nTeleAPIVersion 202
//201 Added tapiPulseFocuser and tapiSettings
//202 Explicitly added TELEAPIEXPORT and CALLBACK to all fuctions
TELEAPIEXPORT int CALLBACK tapiGetDLLVersion(void)
{
	return nTeleAPIVersion;
}

//Called when Telescope, Link, Establish is selected
//Do any initialization here
TELEAPIEXPORT int CALLBACK tapiEstablishLink(void)
{
	(void) InitScope();
	return 0;
}

//Called when Telescope, Link, Terminate is selected
//Do any clean up here
TELEAPIEXPORT int CALLBACK tapiTerminateLink(void)
{
	TermScope();

	return 0;
}

//Called when TheSky needs to know the telescope position.
//Return as quickly as possible as this is called very frequently.
//ra 0.0 to 24.0
//dec -90.0 to 90.0
TELEAPIEXPORT int CALLBACK tapiGetRaDec(double* ra, double* dec)
{
	// probably should cache these, poll with timer

	*ra = GetRightAscension();
	*dec = GetDeclination();
	return 0;
}

//Called when the "Sync" button is selected in TheSky
//(to set the telescope's current coordinates)
//ra 0.0 to 24.0
//dec -90.0 to 90.0
TELEAPIEXPORT int CALLBACK tapiSetRaDec(const double ra, const double dec)
{
	if (SyncScope(ra, dec) == 0)
		return 0;
	return 1;
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
	SlewScope(ra, dec);
	return 0;
}

//TheSky calls this function at a given interval (every 1000 ms by default) 
//to see if the Goto is complete.  If necessary, return an error code
//if the goto is not successfully completed.
TELEAPIEXPORT int CALLBACK tapiIsGotoComplete(BOOL* pbComplete)
{

	*pbComplete = IsSlewing();
	return 0;
}

//Called when the user hits the abort button on the "Telescope Slewing" dialog box
//to stop any Goto currently in progress
TELEAPIEXPORT int CALLBACK tapiAbortGoto(void)
{
	AbortSlew();
	return 0;
}

//Called when the user presses the Settings button
//under Telescope, Setup
//The return value is ignored.
TELEAPIEXPORT int CALLBACK tapiSettings(void)
{
	char buf[255];
	sprintf(buf,"[LOUIE TEST] Software Bisque Telescope API Version %3.2f",
		nTeleAPIVersion/100.0);
	MessageBox(NULL, buf, "TeleAPI", MB_OK);

	if (ConfigScope() == 0)
		return 0;
	return -1;
}

//Called to move the focuser a distinct amount
//bFastSpeed = 1 or 0 to move the focus fast (or larger distance) or slow (or smaller distance)
//bIn = 1 or 0 to move the focuser in or out
TELEAPIEXPORT int CALLBACK tapiPulseFocuser(const BOOL bFastSpeed, const BOOL bIn)
{
	return 0;
}

//Called when TELEAPI.DLL is loaded
BOOL WINAPI DllMain (HANDLE hDLL, DWORD dwReason, LPVOID lpReserved)
{
	
    if (dwReason == DLL_PROCESS_ATTACH)
	{			
		InitDrivers();
		return TRUE;
	}
    else if (dwReason == DLL_PROCESS_DETACH)
    {
	    TermScope();
	    return TRUE;
    }
    return TRUE;   // ok
}
