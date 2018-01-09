//========================================================================
//
// TITLE:		DriverInterface.CPP
//
// FACILITY:	StarryNight V5 Plug-In DLL ASCOM Focuser Control
//
// ABSTRACT:	
//
// USING:		
//
// ENVIRONMENT:	Microsoft Windows Windows 95/98/NT/2000/XP
//				Developed under Microsoft Visual C++ .NET
//
// AUTHOR:		Marko Kudjerski (based on ASCOM Telescope plugin by Robert B. Denny)
//
// Edit Log:
//
// When			Who		What
//----------	---		--------------------------------------------------
// 25-Nov-05	MK		Removed a few warnings
// 01-Aug-05	mk		Initial edit
//========================================================================
#include "AscomFocuser.h"
#pragma hdrstop

#define OUR_REGISTRY_BASE HKEY_LOCAL_MACHINE
#define OUR_REGISTRY_AREA "Software\\Imaginova Canada\\Starry Night Focuser Plugin"
#define OUR_DRIVER_SEL "ASCOM Driver ID"

static long get_long(OLECHAR *name);
static double get_double(OLECHAR *name);
static void set_double(OLECHAR *name, double val);
static bool get_bool(OLECHAR *name);
static void set_bool(OLECHAR *name, bool val);

static IDispatch *_p_DrvDisp = NULL;				// [sentinel]

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
	__try
	{
		SetMessageQueue(96);
		dwVer = CoBuildVersion();
		if(rmm != HIWORD(dwVer))
			drvFail("Wrong version of OLE", NULL, true);
		if(FAILED(CoInitialize(NULL)))
			drvFail("Failed to start OLE", NULL, true);
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		bResult = false;							// Failed to initialize
	}

	return bResult;
}

// ---------
// InitFocuser
// ---------
//
// Here is where we fire up COM and create an instance of the focuser driver.
//
short InitFocuser(void)
{
	CLSID CLSID_driver;
	OLECHAR *name = L"Link";
	DISPID didPut = DISPID_PROPERTYPUT;
	short iRes = 0;									// Assume success (our retval)
	HKEY hKey;
	DWORD dwSize;
	char szProgID[256];
	OLECHAR *ocProgID = NULL;
	DWORD dwType;

	__try
	{
		_bFocuserActive = false;						// Assume failure

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
"You have not yet configured your focuser type and settings.",
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

			wsprintf(buf, "Failed to find focuser driver %s.", szProgID);
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

			wsprintf(buf, "Failed to create an instance of the focuser driver %s.", szProgID);
			drvFail(buf, NULL, true);
		}

		//
		// We now need to connect the focuser. To do this, we set the 
		// Connected property to TRUE.
		//
		set_bool(L"Link", true);
		
		//
		// Now we verify that there is a focuser out there.
		// We try to get the value of TempCompAvailable
		//
		__try
		{
			_bFocuserHasTempComp = GetHasTempComp();			// We can use Temperature Compensation
			_bFocuserCanHalt = GetCanHalt();					// We can halt the focuser
			_bFocuserIsAbsolute = GetIsAbsolute();				// We can issue absolute goto to focuser
			if (_bFocuserIsAbsolute)							// If the focuser is absolute we can also show its position
				sn_SetFocuserPositionInfo(GetPosition());
			else
				sn_SetFocuserPositionInfo((long)coordinate_NotAvailable); // otherwise position is N/A

			sn_SetFocuserProperties(0, GetMaxStep(), GetMaxIncrement()); // set the limits shown in SN
			
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			MessageBox(_hWndMain,
				"The focuser failed to connect. Please check your connection.",
					_szAlertTitle,  (MB_OK | MB_ICONSTOP | MB_SETFOREGROUND));
			ABORT;							
		}

		//
		// Done!
		//
		_bFocuserActive = true;						// We're off and running!
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		_bFocuserActive = false;
		iRes = -1;									// What are the errors?
	}

	return(iRes);
}

// ---------
// TermFocuser
// ---------
//
void TermFocuser(void)
{
	OLECHAR *name = L"Link";
	DISPID dispid;
	DISPID didPut = DISPID_PROPERTYPUT;
	DISPPARAMS dispParms;
	VARIANTARG rgvarg[1];
	EXCEPINFO excep;
	VARIANT vRes;
	short iRes = 0;				// Assume success (our retval)
	HRESULT hr;
	if(_p_DrvDisp != NULL)				// Just in case! (see termPlugin())
	{
		//
		// We now need to connect the focuser. To do this, we set the 
		// Connected property to TRUE. Property-put calls employ a hack
		// as described in Brockschmidt. Don't ask...
		//
		if(FAILED(_p_DrvDisp->GetIDsOfNames(
			IID_NULL, 
			&name, 
			1, 
			LOCALE_USER_DEFAULT,
			&dispid)))
			drvFail(
				"The ASCOM Focuser driver is missing the Link property.",
				NULL, true);

		rgvarg[0].vt = VT_BOOL;
		rgvarg[0].boolVal = VARIANT_FALSE;
		dispParms.cArgs = 1;
		dispParms.rgvarg = rgvarg;
		dispParms.cNamedArgs = 1;					// PropPut kludge
		dispParms.rgdispidNamedArgs = &didPut;
		if(FAILED(hr = _p_DrvDisp->Invoke(
			dispid,
			IID_NULL, 
			LOCALE_USER_DEFAULT, 
			DISPATCH_PROPERTYPUT, 
			&dispParms, 
			&vRes,
			&excep, NULL)))
			drvFail("the Connected = False failed internally.", &excep, true);

		_p_DrvDisp->Release();			// Release instance of the Driver
		_p_DrvDisp = NULL;				// So won't happen again in PROCESS_DETACH
	}
	
	// need to reset globals
	_bFocuserIsAbsolute		= false;	// True if focuser provides exact position
	_bFocuserCanHalt		= false;	// True if focuser can be stopped
	_bFocuserHasTempComp	= false;	// This is true if focuser can apply temperature compensation
	_bFocuserBusy			= false;	// This is true if we're currently talking to the focuser
	_bIsMoving				= false;	// True if moving
	
	//set all status values to N/A
	sn_SetFocuserTempInfo(coordinate_NotAvailable);
	sn_SetFocuserPositionInfo((long)coordinate_NotAvailable);
	sn_SetFocuserProperties((long)coordinate_NotAvailable, (long)coordinate_NotAvailable, (long)coordinate_NotAvailable);

	// finally, set focuser to the inactive state
	_bFocuserActive		= false;

	sn_RefreshTelescopePanel();
}

// ------
// GetIsAbsolute()
// ------
//
bool GetIsAbsolute(void)
{
	return get_bool(L"Absolute");
}

// ------
// GetTempCompAvailable()
// ------
//
bool GetHasTempComp(void)
{
	return get_bool(L"TempCompAvailable");
}

// ------
// GetCanHalt()
// ------
//
bool GetCanHalt(void)
{
	OLECHAR *name = L"Halt";
	DISPID dispid;
    DISPPARAMS dispparms;
    EXCEPINFO excep;
	VARIANT vRes;

	//
	// Get our dispatch ID
	//
	if(FAILED(_p_DrvDisp->GetIDsOfNames(
									IID_NULL, 
									&name,
									1, 
									LOCALE_USER_DEFAULT,
									&dispid)))
		return false;

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
		return false;
	return true;
}

// ------------
// GetIsMoving
// ------------
//
bool GetIsMoving(void)
{
	return(get_bool(L"IsMoving"));
}

// -----------------
// GetMaxStep
// -----------------
//
long GetMaxStep(void)
{
	return(get_long(L"MaxStep"));
}

// -----------------
// GetMaxIncrement
// -----------------
//
long GetMaxIncrement(void)
{
	return(get_long(L"MaxIncrement"));
}

// -----------------
// GetStepSize
// -----------------
//
double GetStepSize(void)
{
	return(get_double(L"StepSize"));
}

// --------------
// GetPosition
// --------------
//
long GetPosition(void)
{
	return(get_long(L"Position"));
}

// ----------
// GetTemperature
// ----------
//
double GetTemperature(void)
{
	return(get_double(L"Temperature"));
}

// -----------
// SetTempComp
// -----------
//
void SetTempComp(bool tempcomp)
{
	set_bool(L"TempComp", tempcomp);
}

// -----------
// GetTempComp
// -----------
//
bool GetTempComp(void)
{
	return(get_bool(L"TempComp"));
}

//
//	----
//	Move
//	----
//
//	Move the focuser to the specified position.
//
short MoveFocuser(long newposition)
{
	OLECHAR *name;
	DISPID dispid;
    DISPPARAMS dispParms;
    VARIANTARG rgvarg[1];
    EXCEPINFO excep;
	VARIANT vRes;
	short iRes = 0;									// Assume success (our retval)

	if(!_bFocuserActive)								// No focuser hookup?
	{
		return(-1);									// Forget this
	}

	name = L"Move";

	__try
	{

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

			wsprintf(buf, "The ASCOM Focuser driver is missing the %s method.", name);
			drvFail(buf, NULL, true);
		}

		_bIsMoving = true;
		sn_RefreshTelescopePanel();

		//
		// Start the move.
		//
		rgvarg[0].vt = VT_I4;
		rgvarg[0].lVal = newposition;
		dispParms.cArgs = 1;
		dispParms.rgvarg = rgvarg;
		dispParms.cNamedArgs = 0;
		dispParms.rgdispidNamedArgs =NULL;
		if(FAILED(_p_DrvDisp->Invoke(
									dispid,
									IID_NULL, 
									LOCALE_USER_DEFAULT, 
									DISPATCH_METHOD, 
									&dispParms, 
									&vRes,
									&excep, 
									NULL)))
		{
			drvFail("Move failed internally.", &excep, false);
		}
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		iRes = -1;
	}

	return(iRes);
}


// ---------
// AbortMove
// ---------
//
void AbortMove(void)
{
	OLECHAR *name = L"Halt";
	DISPID dispid;
    DISPPARAMS dispparms;
    EXCEPINFO excep;
	VARIANT vRes;

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
"The ASCOM Focuser driver is missing the Halt property.",
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
		drvFail("Halt failed internally.", &excep, true);

}


// -----------
// ConfigFocuser
// -----------
//
// Use the ASCOM Focuser Chooser to get the ProgID of the driver to use.
// The ProgID is stored in the registry and used by InitFocuser().
// The Chooser also provides a config button for the focuser itself.
//
short ConfigFocuser(void)
{
	CLSID CLSID_chooser;
	OLECHAR *name = L"Choose";
	DISPID dispid;
    DISPPARAMS dispParms;
    EXCEPINFO excep;
	VARIANTARG rgvarg[1];							// Chooser.Choose(ProgID)
	VARIANT vRes;
	HRESULT hr;
	DWORD dwDisp, dwType, dwSize;
	char szProgID[256];
	short iRes = 0;									// Assume success (our retval)
	IDispatch *pChsrDsp = NULL;						// [sentinel]
	char *cp = NULL;								// [sentinel]
	HKEY hKey = (HKEY)INVALID_HANDLE_VALUE;			// [sentinel]
	BSTR bsProgID = NULL;							// [sentinel]

	__try
	{

		//
		// Create an instance of the ASCOM Focuser Chooser.
		//
		if(FAILED(CLSIDFromProgID(L"DriverHelper.Chooser", &CLSID_chooser)))
			drvFail(
				"Failed to find the ASCOM Focuser Chooser component. Is it installed?", 
						NULL, true);


		if(FAILED(CoCreateInstance(
							CLSID_chooser,
							NULL,
							CLSCTX_SERVER,
							IID_IDispatch,
							(LPVOID *)&pChsrDsp)))
			drvFail(
				"Failed to create an instance of the ASCOM Focuser Chooser. Is it installed?", 
						NULL, true);

		//Now set device type to Focuser

		static unsigned short *name2 = L"DeviceType";
	
		if(FAILED(pChsrDsp->GetIDsOfNames (
								IID_NULL, 
								&name2, 
								1, 
								LOCALE_USER_DEFAULT, 
								&dispid)))
			drvFail(
				"The ASCOM Focuser Chooser is missing the DeviceType property.",
						NULL, true);

		dispParms.cArgs=1;
		dispParms.cNamedArgs=1;
		dispParms.rgdispidNamedArgs = new DISPID(DISPID_PROPERTYPUT);;
		dispParms.rgvarg = new VARIANT;
		dispParms.rgvarg[0].vt = VT_BSTR;
		dispParms.rgvarg[0].bstrVal = ansi_to_bstr("Focuser");

		if(FAILED(pChsrDsp->Invoke(     dispid,
										IID_NULL,
										LOCALE_USER_DEFAULT,
										DISPATCH_PROPERTYPUT,
										&dispParms,
										&vRes,
										&excep,
										NULL)))
			drvFail(
			"Couldn't change device type",
						NULL, true);


		//
		// Now just call the Choose() method. It returns a BSTR 
		// containing the ProgId to use for the selected focuser 
		// driver.
		//
		if(FAILED(pChsrDsp->GetIDsOfNames(
										IID_NULL, 
										&name, 
										1, 
										LOCALE_USER_DEFAULT,
										&dispid)))
			drvFail(
				"The ASCOM Focuser Chooser is missing the Choose method.",
						NULL, true);

		//
		// Retrieve the ProgID of the driver that is currently selected.
		// If there, call the chooser to start with that driver initially
		// selected in its list.
		//
		bsProgID = SysAllocString(L"");				// Assume no current ProgID
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
		hKey = (HKEY)INVALID_HANDLE_VALUE;			// [sentinel]

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
		if(SysStringLen(vRes.bstrVal) > 0)		// Chooser dialog not cancelled
		{
			cp = uni_to_ansi(vRes.bstrVal);	// Get ProgID in ANSI

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

// ------------
// get_long()
// ------------
//
// Get a named long property. If the property is not supported
// then this raises a NOTIMPL exception and lets upstream code
// decide what to do.
//
static long get_long(OLECHAR *name)
{
	DISPID dispid;
	VARIANT result;
    DISPPARAMS dispparms;
    EXCEPINFO excep;
	char *cp;
	char buf[256];

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
"The selected focuser driver is missing the %s property.", cp);
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
		if(excep.scode == EXCEP_NOTIMPL)			// Optional
			NOTIMPL;								// Resignal silently
		else
		{
			cp = uni_to_ansi(name);
			wsprintf(buf, 
"Internal error reading from the %s property.", cp);
			delete[] cp;
			drvFail(buf, &excep, false);
		}
	}

	return(result.lVal);							// Return long result
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
"The selected focuser driver is missing the %s property.", cp);
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
		if(excep.scode == EXCEP_NOTIMPL)			// Optional
			NOTIMPL;								// Resignal silently
		else
		{
			cp = uni_to_ansi(name);
			wsprintf(buf, 
"Internal error reading from the %s property.", cp);
			delete[] cp;
			drvFail(buf, &excep, true);
		}
	}

	return(result.dblVal);							// Return long result
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
"The selected focuser driver is missing the %s property.", cp);
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
		if(excep.scode == EXCEP_NOTIMPL)			// Optional
			NOTIMPL;								// Resignal silently
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
"The selected focuser driver is missing the %s property.", cp);
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
		if(excep.scode == EXCEP_NOTIMPL)			// Optional
			NOTIMPL;								// Resignal silently
		else
		{
			cp = uni_to_ansi(name);
			wsprintf(buf, 
"Internal error reading from the %s property.", cp);
			delete[] cp;
			drvFail(buf, &excep, true);
		}
	}

	return(result.boolVal == VARIANT_TRUE);			// Return C++ bool result
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

	if(FAILED(_p_DrvDisp->GetIDsOfNames(
		IID_NULL, 
		&name, 
		1, 
		LOCALE_USER_DEFAULT,
		&dispid)))
	{
		cp = uni_to_ansi(name);
		wsprintf(buf, 
			 "The selected focuser driver is missing the %s property.", cp);
		delete[] cp;
		drvFail(buf, NULL, true);
	}

	rgvarg[0].vt = VT_BOOL;
	rgvarg[0].boolVal = (val ? VARIANT_TRUE : VARIANT_FALSE);	// Translate to Variant Bool
	dispparms.cArgs = 1;
	dispparms.rgvarg = rgvarg;
	ppdispid[0] = DISPID_PROPERTYPUT;
	dispparms.cNamedArgs = 1;						// PropPut kludge
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
		if(excep.scode == EXCEP_NOTIMPL)			// Optional
			NOTIMPL;								// Resignal silently
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