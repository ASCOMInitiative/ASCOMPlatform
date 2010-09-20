//========================================================================
//
// TITLE:		DriverInterface.cpp
//
// FACILITY:	Telescope API for TheSky 5/6/X
//
// ABSTRACT:	Provides COM calls to the Telescope driver, and various COM
//				related functions. This is "ATL-free" for lightness of
//				weight, so you see "bare-metal COM" here. This was a 
//				conscious design choice.
//
// USING:		Global Interface Table to marshal driver interface across 
//				threads. See http://msdn.microsoft.com/en-us/library/ms678517
//
// ENVIRONMENT:	Microsoft Windows Windows 95/98/NT/2000
//				Developed under Microsoft Visual C++ Version 6.0
//
// AUTHOR:		Robert B. Denny
//
// Edit Log:
//
// When			Who		What
//----------	---		--------------------------------------------------
// 26-Jul-00	rbd		Initial edit
// 24-Aug-00	rbd		Scope Chooser, new InitDrivers() and ConfigScope()
// 28-Aug-00	rbd		Refined Chooser, now takes current driver sel
//						and better handles modality via pass-in of our
//						hWnd.
// 20-Sep-00	rbd		Initialize buffers for RegQueryKeyEx!
// 12-Nov-00	rbd		Support both sync and async slewing. Allow 
//						Lat, Long, and Date to be optional and not
//						alert if not supported.
// 13-Nov-00	rbd		Major overhaul of exception handling, provision
//						for unimplemented functions. Much cleaner now!
//						Allow driver to provide either equatorial or 
//						alt/az coordinates. Add settable Lat and Long.
// 11-Dec-00	rbd		Add IsSlewing(), logic to test whether scope 
//						is slewing, whether it supports sync or async
//						slewing.
// 26-Jan-01	rbd		Add calls to new capability (CanXXX) properties,
//						simplify "can slew async" logic using capability.
//						Some variable name changes for consistency.
// 28-Jan-01	rbd		Add AbortSlew() method call.
// 21-Aug-03	rbd		Disconnect before releasing the driver.
// 25-Nov-04	rbd		4.0.1 Unpark and turn on tracking (if supported) 
//						when connecting to the scope.
// 04-Jun-10	rbd		5.0.1 - For TheSky X, need to remarshal the 
//						interface to the scope between threads. When 
//						disconnecting the scope, the call to AbortSlew()
//						comes in on the main thread, while other calls 
//						come in on a worker thread. 
// 10-Jun-10	rbd		5.0.2 - Can't get the driver to release! Switch
//						to using the Global Interface Table as a cleaner
//						way to marshal across threads. Prevent recursive
//						errors in TermScope() which is called by drvFail().
// 23-Aug-10	rbd		5.0.3 - Add GetAlignmentMode and integer COM support.
//						Add support for registry value that tells TheSky
//						whether the mount is a GEM or not (used by TPOINT).
//========================================================================

/* #include "AscomScope.h" */

#include "StdAfx.h"

#define OUR_REGISTRY_BASE HKEY_LOCAL_MACHINE
#define OUR_REGISTRY_AREA "Software\\SPACE.com\\TheSky TeleAPI-ASCOM Plugin"
#define OUR_DRIVER_SEL "ASCOM Driver ID"

#define TELEAPI_REGISTRY_AREA_6 "Software\\Software Bisque\\TheSky6\\TELEAPI"
#define TELEAPI_REGISTRY_AREA_X "Software\\Software Bisque\\TheSkyX\\TELEAPI"

static int get_integer(OLECHAR *name);
static double get_double(OLECHAR *name);
static void set_double(OLECHAR *name, double val);
static bool get_bool(OLECHAR *name);
static void set_bool(OLECHAR *name, bool val);
static void switchThreadIf();

static IDispatch *_p_DrvDisp = NULL;							// [sentinel] Pointer to driver interface
static IGlobalInterfaceTable *_p_GIT;							// Pointer to Global Interface Table interface
static DWORD dwIntfcCookie;										// Driver interface cookie for GIT
static DWORD dCurrIntfcThreadId;								// ID of thread on which the interface is currently marshalled
static bool bSyncSlewing = false;								// True if scope is doing a sync slew

// -------------
// InitDrivers()
// -------------
//
// Really just starts OLE
//
bool InitDrivers(void)
{
	DWORD dwVer;
	bool bResult = true;
	static bool bInitDone = false;								// Exec this only once

	if(!bInitDone)
	{
		__try
		{
			SetMessageQueue(96);
			dwVer = CoBuildVersion();
			if(rmm != HIWORD(dwVer))
				drvFail("Wrong version of OLE", NULL, true);
			if(FAILED(CoInitializeEx(NULL, COINIT_APARTMENTTHREADED)))
				drvFail("Failed to start OLE", NULL, true);
			if(FAILED(CoCreateInstance(CLSID_StdGlobalInterfaceTable,
						NULL,
						CLSCTX_INPROC_SERVER,
						IID_IGlobalInterfaceTable,
						(void **)&_p_GIT)))
				drvFail("Failed to connect to Global Interface Table", NULL, true);
			bInitDone = true;
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			bResult = false;	// Failed to initialize
		}
	}

	return bResult;
}

// ---------
// InitScope
// ---------
//
// Here is where we fire up COM and create an instance of the scope driver.
//
short InitScope(void)
{
	CLSID CLSID_driver;
	OLECHAR *name = L"Connected";
	DISPID didPut = DISPID_PROPERTYPUT;
	short iRes = 0;				// Assume success (our retval)
	HKEY hKey;
	DWORD dwSize;
	char szProgID[256];
	OLECHAR *ocProgID = NULL;
	DWORD dwType;
	DWORD dwGemVal;

	__try {
		_bScopeActive = false;									// Assume failure

		//
		// Retrieve the ProgID of the driver we're to use. Get it into 
		// OLESTR format.
		//
		if(RegOpenKeyEx(OUR_REGISTRY_BASE, 
				OUR_REGISTRY_AREA,
				0,
				KEY_READ,
				&hKey) != ERROR_SUCCESS)
			drvFail(
				"You have not yet configured your telescope type and settings.",
				NULL, true);

		dwSize = 256;
		if(RegQueryValueEx(hKey, 
				   OUR_DRIVER_SEL,
				   NULL,
				   &dwType,
				   (BYTE *)szProgID,
				   &dwSize) != ERROR_SUCCESS)
		{
			RegCloseKey(hKey);
			drvFail(
				"Failed to read the driver ID from the registry.",
				NULL, true);
		}
		RegCloseKey(hKey);

		ocProgID = ansi_to_uni(szProgID);

		//
		// Create an instance of our ASCOM driver.
		//
		if(FAILED(CLSIDFromProgID(ocProgID, &CLSID_driver)))
		{
			char buf[256];

			wsprintf(buf, "Failed to find scope driver %s.", szProgID);
			free(ocProgID);
			drvFail(buf, NULL, true);
		}
		free(ocProgID);

		if(FAILED(CoCreateInstance(
			CLSID_driver,
			NULL,
			CLSCTX_SERVER,
			IID_IDispatch,
			(LPVOID *)&_p_DrvDisp)))
		{
			char buf[256];

			wsprintf(buf, "Failed to create an instance of the scope driver %s.", szProgID);
			drvFail(buf, NULL, true);
		}

		//
		// Get the marshalled interface pointer for later possible thread switching
		//
		dCurrIntfcThreadId = GetCurrentThreadId();
		if(FAILED(_p_GIT->RegisterInterfaceInGlobal(
				_p_DrvDisp,
				IID_IDispatch,
				&dwIntfcCookie)))
			drvFail("Failed to register driver interface in GIT", NULL, true);
		//
		// We now need to connect the scope. To do this, we set the 
		// Connected property to TRUE.
		//
		set_bool(L"Connected", true);
// THIS WORKS
//dCurrIntfcThreadId = 0;
//TermScope(false);
		//
		// At this point we should be able to call into the ASCOM scope
		// driver through its IDIspatch. Check to see if things are OK. 
		// We call GetName, because the driver MUST support that, then
		// grab the capabilities we need (these also MUST be supported).
		//
		_szScopeName = GetName();								// Indicator label/name
		_bScopeCanSync = GetCanSync();							// Can it sync?
		_bScopeCanSlew = GetCanSlew();							// Can it slew at all?	
		_bScopeCanSlewAsync = GetCanSlewAsync();				// Can it slew asynchronously?
		_bScopeIsGEM = (GetAlignmentMode() == 2);				// Is it a GEM?

		//
		// Write the magic registry value that TheSky uses for TPOINT to 
		// tell if it is a GEM or not.
		//
		// Since this goes under HKCU, the installer can NOT make the keys.
		// We must do it here for the current user! And we can't tell whether 
		// we're running under TheSky 6 or X so we just write it into
		// both places. BEST EFFORTS, NO ERROR DETECTION.
		//
		dwGemVal = _bScopeIsGEM ? 1 : 0;
		dwSize = 4;
		if(CreateRegKeyStructure(HKEY_CURRENT_USER, TELEAPI_REGISTRY_AREA_6))
		{
			if(RegOpenKeyEx(HKEY_CURRENT_USER, 
					TELEAPI_REGISTRY_AREA_6,
					0,
					KEY_WRITE,
					&hKey) == ERROR_SUCCESS)
			{
				HRESULT hr = RegSetValueEx(hKey, 
						   "GEM",
						   NULL,
						   REG_DWORD,
						   (const BYTE *)&dwGemVal,
						   dwSize);
				RegCloseKey(hKey);
			}
		}
		
		if(CreateRegKeyStructure(HKEY_CURRENT_USER, TELEAPI_REGISTRY_AREA_X))
		{
			if(RegOpenKeyEx(HKEY_CURRENT_USER, 
					TELEAPI_REGISTRY_AREA_X,
					0,
					KEY_WRITE,
					&hKey) == ERROR_SUCCESS)
			{
				RegSetValueEx(hKey, 
						   "GEM",
						   NULL,
						   REG_DWORD,
						   (const BYTE *)&dwGemVal,
						   dwSize);
				RegCloseKey(hKey);
			}
		}

		//
		// Now we verify that there is a scope out there. We first try to
		// get RA, and if that's not implemented, try to get Az. If THAT's 
		// not implemented, we've had it. If either fail for other reasons,
		// we've also had it. We also use this to set a flag so that later
		// we don't unnecessarily call GetRA/Dec if the scope doesn't 
		// support it. 
		//
		__try {
			_bScopeHasEqu = false;								// Assume we cannot get RA/Dec
			GetRightAscension();
			_bScopeHasEqu = true;								// We can get RA/Dec
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			if(GetExceptionCode() != EXCEP_NOTIMPL)
				ABORT;											// Some real error, has been alerted
		}
		if(!_bScopeHasEqu)										// If we don't have Equatorial
		{
			__try {
				GetAzimuth();									// Then we must have Alt/Az!
			}
			__except(EXCEPTION_EXECUTE_HANDLER)
			{
				if(GetExceptionCode() == EXCEP_NOTIMPL)
					//
					// Neither RA/Dec nor Alt/Az implemented!
					//
					MessageBox(NULL,
						   "The selected telescope does not support either RA/Dec or Alt/Az readout. Cannot continue.",
						   _szAlertTitle,  (MB_OK | MB_ICONSTOP | MB_SETFOREGROUND));

				ABORT;											// We've had it in any case

			}
		}
		//
		// If the scope can be unparked, do it. The version of TeleAPI
		// that I have doesn't have unpark support, and without it the scope 
		// could be wedged at this point.
		//
		if(get_bool(L"CanUnpark"))
			UnparkScope();
		//
		// Finally, if the scope's tracking can be turned on and off, 
		// turn it on. Again, this version of TeleAPI doesn't appear
		// to support tracking control.
		//
		if(get_bool(L"CanSetTracking"))
			set_bool(L"Tracking", true);
		//
		// Done!
		//
		_bScopeActive = true;									// We're off and running!
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		_bScopeActive = false;
		iRes = -1;			// What are the errors?
	}

	return(iRes);
}

// ---------
// TermScope
// ---------
//
void TermScope(bool fatal)
{
	OLECHAR *name = L"Connected";
	DISPID dispid;
	DISPID didPut = DISPID_PROPERTYPUT;
	DISPPARAMS dispParms;
	VARIANTARG rgvarg[1];
	EXCEPINFO excep;
	VARIANT vRes;
	short iRes = 0;												// Assume success (our retval)
	HRESULT hr;

	if(_p_DrvDisp != NULL)										// Just in case! (see termPlugin())
	{
		switchThreadIf();

		//
		// We now need to unconnect the scope. To do this, we set the 
		// Connected property to FALSE. THe use of !fatal in the calls
		// to drvFail assues that drvFail() will not recursively call 
		// US because its call to TermScope() passses true, while all
		// others pass false.
		//
		if(FAILED(_p_DrvDisp->GetIDsOfNames(
			IID_NULL, 
			&name, 
			1, 
			LOCALE_USER_DEFAULT,
			&dispid)))
			drvFail(
				"The ASCOM scope driver is missing the Connected property.",
				NULL, !fatal);

		rgvarg[0].vt = VT_BOOL;
		rgvarg[0].boolVal = VARIANT_FALSE;
		dispParms.cArgs = 1;
		dispParms.rgvarg = rgvarg;
		dispParms.cNamedArgs = 1;								// PropPut kludge
		dispParms.rgdispidNamedArgs = &didPut;
		if(FAILED(hr = _p_DrvDisp->Invoke(
			dispid,
			IID_NULL, 
			LOCALE_USER_DEFAULT, 
			DISPATCH_PROPERTYPUT, 
			&dispParms, 
			&vRes,
			&excep, NULL)))
			drvFail("the Connected = False failed internally.", &excep, !fatal);

		_p_GIT->RevokeInterfaceFromGlobal(dwIntfcCookie);		// We're done with this driver/object

		_p_DrvDisp->Release();									// Release instance of the driver
		_p_DrvDisp = NULL;										// So won't happen again in PROCESS_DETACH
	}
	if(_szScopeName != NULL)
		delete[] _szScopeName;									// Free this string
	_szScopeName = NULL;										// [sentinel]
	_bScopeActive = false;

}

// ------------
// GetCanSlew()
// ------------
//
bool GetCanSlew(void)
{
	return(get_bool(L"CanSlew"));
}

// -----------------
// GetCanSlewAsync()
// -----------------
//
bool GetCanSlewAsync(void)
{
	return(get_bool(L"CanSlewAsync"));
}

// ------------
// GetCanSlew()
// ------------
//
bool GetCanSync(void)
{
	return(get_bool(L"CanSync"));
}

// ------------------
// GetAlignmentMode()
// ------------------
//
int GetAlignmentMode(void)
{
	return(get_integer(L"AlignmentMode"));
}

// -----------------
// GetRightAscension
// -----------------
//
double GetRightAscension(void)
{
	return(get_double(L"RightAscension"));
}

// --------------
// GetDeclination
// --------------
//
double GetDeclination(void)
{
	return(get_double(L"Declination"));
}

// ----------
// GetAzimuth
// ----------
//
double GetAzimuth(void)
{
	return(get_double(L"Azimuth"));
}

// -----------
// GetAltitude
// -----------
//
double GetAltitude(void)
{
	return(get_double(L"Altitude"));
}

// -----------
// GetLatitude
// -----------
//
double GetLatitude(void)
{
	return(get_double(L"SiteLatitude"));
}

// ------------
// GetLongitude
// ------------
//
double GetLongitude(void)
{
	return(get_double(L"SiteLongitude"));
}

// -------------
// GetJulianDate
// -------------
//
// The "Date" epoch is Julian date 2415021.5. At least I THOUGHT 
// so... it seems not to be... ?????
//
double GetJulianDate(void)
{
	return(get_double(L"UTCDate") + 2415018.5);
}

// -----------
// SetLatitude
// -----------
//
void SetLatitude(double lat)
{
	set_double(L"SiteLatitude", lat);
}

// ------------
// SetLongitude
// ------------
//
void SetLongitude(double lng)
{
	set_double(L"SiteLongitude", lng);
}

// -------
// GetName
// -------
//
// Retrns -> to new[]'ed string. Caller must release.
//
char *GetName(void)
{
	OLECHAR *name = L"Name";
	DISPID dispid;
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	VARIANT vRes;

	switchThreadIf();

	//
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name,
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
		drvFail(
			"The ASCOM scope driver is missing the Name property.",
			NULL, true);

	//
	// No dispatch parameters for propget
	//
	dispparms.cArgs = 0;
	dispparms.rgvarg = NULL;
	dispparms.cNamedArgs = 0;
	dispparms.rgdispidNamedArgs = NULL;
	//
	// Invoke the method
	//
	if(FAILED(_p_DrvDisp->Invoke(dispid, 
				     IID_NULL, 
				     LOCALE_USER_DEFAULT, 
				     DISPATCH_PROPERTYGET,
				     &dispparms, 
				     &vRes, 
				     &excep, 
				     NULL)))
		drvFail("Internal error getting Name property.", &excep, true);
		
	return(uni_to_ansi(vRes.bstrVal));
}

// ---------
// IsSlewing
// ---------
//
bool IsSlewing(void)
{
	OLECHAR *name = L"Slewing";
	DISPID dispid;
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	VARIANT vRes;

	if(!_bScopeActive)											// If scope not even active
		return(false);											// Can't be slewing

	if(!_bScopeCanSlewAsync)									// If can't do async slew, or never slewed
		return(bSyncSlewing);									// Use our sync slewing flag (never slewed = false)

	switchThreadIf();

	//
	// We can do async slews, assume driver supports Slewing.
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name,
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
		drvFail(
			"The ASCOM scope driver is missing the Slewing property.",
			NULL, true);

	//
	// No dispatch parameters for propget
	//
	dispparms.cArgs = 0;
	dispparms.rgvarg = NULL;
	dispparms.cNamedArgs = 0;
	dispparms.rgdispidNamedArgs = NULL;
	//
	// Invoke the method
	//
	if(FAILED(_p_DrvDisp->Invoke(dispid, 
				     IID_NULL, 
				     LOCALE_USER_DEFAULT, 
				     DISPATCH_PROPERTYGET,
				     &dispparms, 
				     &vRes, 
				     &excep, 
				     NULL)))
		drvFail("Internal error getting Slewing property.", &excep, true);
		
	return((vRes.boolVal != VARIANT_FALSE) ? true : false);
}

//
//	----
//	Slew
//	----
//
//	Slew the telescope to a specified RA/Dec position.
//
short SlewScope(double dRA, double dDec)
{
	OLECHAR *name;
	DISPID dispid;
	DISPPARAMS dispParms;
	VARIANTARG rgvarg[2];
	EXCEPINFO excep;
	VARIANT vRes;
	short iRes = 0;												// Assume success (our retval)
	HRESULT hr;

	if(!_bScopeActive)											// No scope hookup?
	{
		bSyncSlewing = false;									// Cannot be sync-slewing!
		return(-1);												// Forget this
	}

	//
	// Fall back to sync slewing if async not supported.
	//
	if(_bScopeCanSlewAsync)
		name = L"SlewToCoordinatesAsync";
	else
		name = L"SlewToCoordinates";

	__try
	{

		switchThreadIf();

		//
		// Get our dispatch ID
		//
		if(FAILED(_p_DrvDisp->GetIDsOfNames(
			IID_NULL, 
			&name, 
			1, 
			LOCALE_USER_DEFAULT,
			&dispid)))
		{
			char buf[256];

			wsprintf(buf, "The ASCOM scope driver is missing the %s method.", name);
			drvFail(buf, NULL, true);
		}
		//
		// Start the slew.
		//
		rgvarg[0].vt = VT_R8;									// Arg order is R->L
		rgvarg[0].dblVal = dDec;
		rgvarg[1].vt = VT_R8;
		rgvarg[1].dblVal = dRA;
		dispParms.cArgs = 2;
		dispParms.rgvarg = rgvarg;
		dispParms.cNamedArgs = 0;
		dispParms.rgdispidNamedArgs =NULL;
		if(!_bScopeCanSlewAsync) bSyncSlewing = true;			// Internal flag
		if(FAILED(hr = _p_DrvDisp->Invoke(
			dispid,
			IID_NULL, 
			LOCALE_USER_DEFAULT, 
			DISPATCH_METHOD, 
			&dispParms, 
			&vRes,
			&excep, 
			NULL)))
		{
			//
			// Most slew failures are not fatal (below horizon, etc.)
			// Report the error and let the driver continue.
			//
			drvFail("Slew to object failed internally.", &excep, false);
		}
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		iRes = -1;
	}

	bSyncSlewing = false;

	return(iRes);
}

// ---------
// AbortSlew
// ---------
//
void AbortSlew(void)
{
	OLECHAR *name = L"AbortSlew";
	DISPID dispid;
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	VARIANT vRes;

	switchThreadIf();

	//
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name,
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
		drvFail(
			"The ASCOM scope driver is missing the AbortSlew method.",
			NULL, true);

	//
	// No dispatch parameters for propget
	//
	dispparms.cArgs = 0;
	dispparms.rgvarg = NULL;
	dispparms.cNamedArgs = 0;
	dispparms.rgdispidNamedArgs = NULL;
	//
	// Invoke the method
	//
	if(FAILED(_p_DrvDisp->Invoke(dispid, 
				     IID_NULL, 
				     LOCALE_USER_DEFAULT, 
				     DISPATCH_METHOD,
				     &dispparms, 
				     &vRes, 
				     &excep, 
				     NULL)))
		drvFail("AbortSlew failed internally.", &excep, true);
		
}

//	---------
//	SyncScope
//	---------
//
//	Sync the telescope's position to the given coordinates
//
//
short SyncScope(double dRA, double dDec)
{
	OLECHAR *name = L"SyncToCoordinates";
	DISPID dispid;
	DISPPARAMS dispParms;
	VARIANTARG rgvarg[2];
	EXCEPINFO excep;
	VARIANT vRes;
	short iRes = 0;												// Assume success (our retval)
	HRESULT hr;

	if(!_bScopeActive)											// No scope hookup?
		return(-1);												// Forget this

	__try
		{
			switchThreadIf();

			//
			// Get our dispatch ID
			//
			if(FAILED(_p_DrvDisp->GetIDsOfNames(
				IID_NULL, 
				&name, 
				1, 
				LOCALE_USER_DEFAULT,
				&dispid)))
				drvFail(
					"The ASCOM scope driver is missing the SyncToCoordinates method.",
					NULL, true);

			//
			// Do the sync
			//
			rgvarg[0].vt = VT_R8;		// Arg order is R->L
			rgvarg[0].dblVal = dDec;
			rgvarg[1].vt = VT_R8;
			rgvarg[1].dblVal = dRA;
			dispParms.cArgs = 2;
			dispParms.rgvarg = rgvarg;
			dispParms.cNamedArgs = 0;
			dispParms.rgdispidNamedArgs =NULL;
			if(FAILED(hr = _p_DrvDisp->Invoke(
				dispid, 
				IID_NULL, 
				LOCALE_USER_DEFAULT, 
				DISPATCH_METHOD, 
				&dispParms, 
				&vRes,
				&excep, 
				NULL)))
			{
				//
				// All errors fatal. Should not call this if _bScopeCanSync is false!
				//
				drvFail("Sync to coordinates failed internally.", &excep, true);
			}
		}
	__except(EXCEPTION_EXECUTE_HANDLER)
		{
			iRes = -1;
		}

	return(iRes);
}	

// ------
// Unpark
// ------
//
void UnparkScope(void)
{
	OLECHAR *name = L"Unpark";
	DISPID dispid;
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	VARIANT vRes;

	switchThreadIf();

	//
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name,
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
		drvFail(
			"The ASCOM scope driver is missing the Unpark method.",
			NULL, true);

	//
	// No dispatch parameters for propget
	//
	dispparms.cArgs = 0;
	dispparms.rgvarg = NULL;
	dispparms.cNamedArgs = 0;
	dispparms.rgdispidNamedArgs = NULL;
	//
	// Invoke the method
	//
	if(FAILED(_p_DrvDisp->Invoke(dispid, 
				     IID_NULL, 
				     LOCALE_USER_DEFAULT, 
				     DISPATCH_METHOD,
				     &dispparms, 
				     &vRes, 
				     &excep, 
				     NULL)))
		drvFail("Unpark failed internally.", &excep, true);
		
}

// -----------
// ConfigScope
// -----------
//
// Use the ASCOM Scope Chooser to get the ProgID of the driver to use.
// The ProgID is stored in the registry and used by InitScope().
// The Chooser also provides a config button for the scope itself.
//
short ConfigScope(void)
{
	CLSID CLSID_chooser;
	OLECHAR *name = L"Choose";
	DISPID dispid;
	DISPPARAMS dispParms;
	EXCEPINFO excep;
	VARIANTARG rgvarg[1];										// Chooser.Choose(ProgID)
	VARIANT vRes;
	HRESULT hr;
	DWORD dwDisp, dwType, dwSize;
	char szProgID[256];
	short iRes = 0;												// Assume success (our retval)
	IDispatch *pChsrDsp = NULL;									// [sentinel]
	char *cp = NULL;											// [sentinel]
	HKEY hKey = (HKEY)INVALID_HANDLE_VALUE;						// [sentinel]
	BSTR bsProgID = NULL;										// [sentinel]

	__try {
		
		//
		// Create an instance of the ASCOM Scope Chooser.
		//
		if(FAILED(CLSIDFromProgID(L"DriverHelper.Chooser", &CLSID_chooser)))
			drvFail(
				"Failed to find the ASCOM Scope Chooser component. Is it installed?", 
				NULL, true);

		if(FAILED(CoCreateInstance(
			CLSID_chooser,
			NULL,
			CLSCTX_SERVER,
			IID_IDispatch,
			(LPVOID *)&pChsrDsp)))
			drvFail(
				"Failed to create an instance of the ASCOM Scope Chooser. Is it installed?", 
				NULL, true);

		//
		// Now just call the Choose() method. It returns a BSTR 
		// containing the ProgId to use for the selected scope 
		// driver.
		//
		if(FAILED(pChsrDsp->GetIDsOfNames(
			IID_NULL, 
			&name, 
			1, 
			LOCALE_USER_DEFAULT,
			&dispid)))
			drvFail(
				"The ASCOM Scope Chooser is missing the Choose method.",
				NULL, true);

		//
		// Retrieve the ProgID of the driver that is currently selected.
		// If there, call the chooser to start with that driver initially
		// selected in its list.
		//
		bsProgID = SysAllocString(L"");							// Assume no current ProgID
		if(RegOpenKeyEx(OUR_REGISTRY_BASE, 
				OUR_REGISTRY_AREA,
				0,
				KEY_READ,
				&hKey) == ERROR_SUCCESS)
		{
			dwSize = 255;
			if(RegQueryValueEx(hKey, 
					   OUR_DRIVER_SEL,
					   NULL,
					   &dwType,
					   (BYTE *)szProgID,
					   &dwSize) == ERROR_SUCCESS)
			{
				SysFreeString(bsProgID);
				bsProgID = ansi_to_bstr(szProgID);
			}
			RegCloseKey(hKey);
		}
		hKey = (HKEY)INVALID_HANDLE_VALUE;						// [sentinel]

			//
			// Call the Choose() method with our ProgID string
			//
		rgvarg[0].vt = VT_BSTR;
		rgvarg[0].bstrVal = bsProgID;
		dispParms.cArgs = 1;
		dispParms.rgvarg = rgvarg;
		dispParms.cNamedArgs = 0;
		dispParms.rgdispidNamedArgs = NULL;
		if(FAILED(hr = pChsrDsp->Invoke(
			dispid,
			IID_NULL, 
			LOCALE_USER_DEFAULT, 
			DISPATCH_METHOD, 
			&dispParms, 
			&vRes,
			&excep, NULL)))
			drvFail("The Choose() method failed internally.", &excep, true);

			//
			// At this point, the variant vRes contains a BSTR with the 
			// ProgID of the selected driver, or an empty BSTR. Fail if 
			// the method returned something other than a bstr!
			//
		if(vRes.vt != VT_BSTR)
			drvFail("The Chooser returned something other than a string.",
				NULL, true);

		//
		// Now write the chosen driver's ProgID into our registry area.
		// This creates the registry area if needed.
		//
		if(SysStringLen(vRes.bstrVal) > 0)						// Chooser dialog not cancelled
		{
			cp = uni_to_ansi(vRes.bstrVal);						// Get ProgID in ANSI

			if(RegCreateKeyEx(OUR_REGISTRY_BASE, 
					  OUR_REGISTRY_AREA,
					  NULL, NULL, 0,
					  KEY_WRITE,
					  NULL,
					  &hKey,
					  &dwDisp) != ERROR_SUCCESS)
				drvFail("Failed to create or open the plug-in's registry area.",
					NULL, true);

			if(RegSetValueEx(hKey, 
					 OUR_DRIVER_SEL, 
					 0,
					 REG_SZ, 
					 (BYTE *)cp, 
					 (strlen(cp) + 1)) != ERROR_SUCCESS)
				drvFail("Failed to store the driver name into the registry.",
					NULL, true);

			pChsrDsp->Release();
			delete[] cp;
			RegCloseKey(hKey);
			SysFreeString(bsProgID);
		}
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
		{
			if(pChsrDsp != NULL) pChsrDsp->Release();
			if(cp != NULL) delete[] cp;
			if(hKey != (HKEY)INVALID_HANDLE_VALUE) RegCloseKey(hKey);
			iRes = -1;
		}

	return(iRes);
}

// ===============
// LOCAL UTILITIES
// ===============

// BADLY NEEDS REFACTORING!

// -------------
// get_integer()
// -------------
//
// Get a named double property. If the property is not supported
// then this raises a NOTIMPL exception and lets upstream code
// decide what to do.
//
static int get_integer(OLECHAR *name)
{
	DISPID dispid;
	VARIANT result;
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	char *cp;
	char buf[256];

	switchThreadIf();

	//
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name, 
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
	{
		cp = uni_to_ansi(name);
		wsprintf(buf, 
			 "The selected telescope driver is missing the %s property.", cp);
		delete[] cp;
		drvFail(buf, NULL, true);
	}

	//
	// No dispatch parameters for propget
	//
	dispparms.cArgs = 0;
	dispparms.rgvarg = NULL;
	dispparms.cNamedArgs = 0;
	dispparms.rgdispidNamedArgs = NULL;
	//
	// Invoke the method
	//
	if(FAILED(_p_DrvDisp->Invoke(dispid, 
				     IID_NULL, 
				     LOCALE_USER_DEFAULT, 
				     DISPATCH_PROPERTYGET,
				     &dispparms, 
				     &result, 
				     &excep, 
				     NULL)))
	{
		if(excep.scode == EXCEP_NOTIMPL)						// Optional
			NOTIMPL;											// Resignal silently
		else
		{
			cp = uni_to_ansi(name);
			wsprintf(buf, 
				 "Internal error reading from the %s property.", cp);
			delete[] cp;
			drvFail(buf, &excep, true);
		}
	}

	return(result.intVal);										// Return integer result
}

// ------------
// get_double()
// ------------
//
// Get a named double property. If the property is not supported
// then this raises a NOTIMPL exception and lets upstream code
// decide what to do.
//
static double get_double(OLECHAR *name)
{
	DISPID dispid;
	VARIANT result;
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	char *cp;
	char buf[256];

	switchThreadIf();

	//
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name, 
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
	{
		cp = uni_to_ansi(name);
		wsprintf(buf, 
			 "The selected telescope driver is missing the %s property.", cp);
		delete[] cp;
		drvFail(buf, NULL, true);
	}

	//
	// No dispatch parameters for propget
	//
	dispparms.cArgs = 0;
	dispparms.rgvarg = NULL;
	dispparms.cNamedArgs = 0;
	dispparms.rgdispidNamedArgs = NULL;
	//
	// Invoke the method
	//
	if(FAILED(_p_DrvDisp->Invoke(dispid, 
				     IID_NULL, 
				     LOCALE_USER_DEFAULT, 
				     DISPATCH_PROPERTYGET,
				     &dispparms, 
				     &result, 
				     &excep, 
				     NULL)))
	{
		if(excep.scode == EXCEP_NOTIMPL)						// Optional
			NOTIMPL;											// Resignal silently
		else
		{
			cp = uni_to_ansi(name);
			wsprintf(buf, 
				 "Internal error reading from the %s property.", cp);
			delete[] cp;
			drvFail(buf, &excep, true);
		}
	}

	return(result.dblVal);										// Return long result
}

// ------------
// set_double()
// ------------
//
// Set a named double property. If the property is not supported
// then this raises a NOTIMPL exception and lets upstream code
// decide what to do.
//
static void set_double(OLECHAR *name, double val)
{
	DISPID dispid;
	VARIANTARG rgvarg[1];
	VARIANT result;
	DISPID ppdispid[1];
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	char *cp;
	char buf[256];

	switchThreadIf();

	//
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name, 
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
	{
		cp = uni_to_ansi(name);
		wsprintf(buf, 
			 "The selected telescope driver is missing the %s property.", cp);
		delete[] cp;
		drvFail(buf, NULL, true);
	}

	//
	// Special setup for propput (See MSKB Q175618)
	//
	rgvarg[0].vt = VT_R8;
	rgvarg[0].dblVal = val;
	dispparms.cArgs = 1;
	dispparms.rgvarg = rgvarg;
	ppdispid[0] = DISPID_PROPERTYPUT;
	dispparms.cNamedArgs = 1;
	dispparms.rgdispidNamedArgs = ppdispid;
	//
	// Invoke the method
	//
	if(FAILED(_p_DrvDisp->Invoke(dispid, 
				     IID_NULL, 
				     LOCALE_USER_DEFAULT, 
				     DISPATCH_PROPERTYPUT,
				     &dispparms, 
				     &result, 
				     &excep, 
				     NULL)))
	{
		if(excep.scode == EXCEP_NOTIMPL)						// Optional
			NOTIMPL;											// Resignal silently
		else
		{
			cp = uni_to_ansi(name);
			wsprintf(buf, 
				 "Internal error writing to the %s property.", cp);
			delete[] cp;
			drvFail(buf, &excep, true);
		}
	}
}

// ----------
// get_bool()
// ----------
//
// Get a named boolean property. If the property is not supported
// then this raises a NOTIMPL exception and lets upstream code
// decide what to do.
//
static bool get_bool(OLECHAR *name)
{
	DISPID dispid;
	VARIANT result;
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	char *cp;
	char buf[256];

	switchThreadIf();

	//
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name, 
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
	{
		cp = uni_to_ansi(name);
		wsprintf(buf, 
			 "The selected telescope driver is missing the %s property.", cp);
		delete[] cp;
		drvFail(buf, NULL, true);
	}

	//
	// No dispatch parameters for propget
	//
	dispparms.cArgs = 0;
	dispparms.rgvarg = NULL;
	dispparms.cNamedArgs = 0;
	dispparms.rgdispidNamedArgs = NULL;
	//
	// Invoke the method
	//
	if(FAILED(_p_DrvDisp->Invoke(dispid, 
				     IID_NULL, 
				     LOCALE_USER_DEFAULT, 
				     DISPATCH_PROPERTYGET,
				     &dispparms, 
				     &result, 
				     &excep, 
				     NULL)))
	{
		if(excep.scode == EXCEP_NOTIMPL)						// Optional
			NOTIMPL;											// Resignal silently
		else
		{
			cp = uni_to_ansi(name);
			wsprintf(buf, 
				 "Internal error reading from the %s property.", cp);
			delete[] cp;
			drvFail(buf, &excep, true);
		}
	}

	return(result.boolVal == VARIANT_TRUE);						// Return C++ bool result
}


// ----------
// set_bool()
// ----------
//
// Set a named boolean property. If the property is not supported
// then this raises a NOTIMPL exception and lets upstream code
// decide what to do.
//
static void set_bool(OLECHAR *name, bool val)
{
	DISPID dispid;
	VARIANTARG rgvarg[1];
	VARIANT result;
	DISPID ppdispid[1];
	DISPPARAMS dispparms;
	EXCEPINFO excep;
	char *cp;
	char buf[256];
	HRESULT hr;

	switchThreadIf();

	//
	// Get our dispatch ID
	//
	if(FAILED(hr = _p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name, 
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
	{
		cp = uni_to_ansi(name);
		wsprintf(buf, 
			 "The selected telescope driver is missing the %s property.", cp);
		delete[] cp;
		drvFail(buf, NULL, true);
	}

	rgvarg[0].vt = VT_BOOL;
	rgvarg[0].boolVal = (val ? VARIANT_TRUE : VARIANT_FALSE);	// Translate to Variant Bool
	dispparms.cArgs = 1;
	dispparms.rgvarg = rgvarg;
	ppdispid[0] = DISPID_PROPERTYPUT;
	dispparms.cNamedArgs = 1;									// PropPut kludge
	dispparms.rgdispidNamedArgs = ppdispid;
	if(FAILED(_p_DrvDisp->Invoke(
			dispid,
			IID_NULL, 
			LOCALE_USER_DEFAULT, 
			DISPATCH_PROPERTYPUT, 
			&dispparms, 
			&result,
			&excep, NULL)))
	{
		if(excep.scode == EXCEP_NOTIMPL)						// Optional
			NOTIMPL;											// Resignal silently
		else
		{
			cp = uni_to_ansi(name);
			wsprintf(buf, 
				 "Internal error writing to the %s property.", cp);
			delete[] cp;
			drvFail(buf, &excep, true);
		}
	}

}

//
// Gets the IDispatch interface on a new thread from a previously obtained
// marshal stream, then gets a new interface marshaling stream on the new
// thread (remembering the thread on which the interface is currently
// marshaled). This may be repeatedly called to wsitch the IDispatch to
// the scope from one thread to another. 
//
static void switchThreadIf()
{
	if (GetCurrentThreadId() != dCurrIntfcThreadId)
	{
		IDispatch *pTemp;

		dCurrIntfcThreadId = GetCurrentThreadId();
		if(FAILED(_p_GIT->GetInterfaceFromGlobal(dwIntfcCookie, 
						IID_IDispatch, 
						(void **)&pTemp)))
			drvFail("Failed to get interface from GIT in new thread", NULL, true);
		//_p_DrvDisp->Release();									// Required to prevent refcount buildup
		_p_DrvDisp = pTemp;
	}
}