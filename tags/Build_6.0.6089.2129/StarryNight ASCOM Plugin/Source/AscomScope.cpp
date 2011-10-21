//========================================================================
//
// TITLE:		ASCOMSCOPE.CPP
//
// FACILITY:	StarryNight V3Plug-In DLL ASCOM Telescope Control
//
// ABSTRACT:	
//
// USING:		
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
// 26-Jul-00	rbd		Initial edit, after lx200snv3.cpp, my plugin for
//						Astronomer's Control Panel.
// 25-Aug-00	rbd		Hook up to new Scope Chooser, enable Configure
//						in the Telescope menu.
// 28-Aug-00	rbd		Much work, integration complete.
// 20-Sep-00	rbd		Fix magnification cross-talk between windows,
//						op_DrawLayer stuff only for our window! Move
//						Move Sync item on context menu
// 12-Nov-00	rbd		If scope driver doesn't support Lat or Long,
//						use home location. Same with time. Now a V7
//						plugin.
// 13-Nov-00	rbd		Major overhaul of exception handling, provision
//						for unimplemented functions. Much cleaner now!
//						Allow driver to provide either equatorial or 
//						alt/az coordinates. Feed driver Lat/Long.
// 14-Nov-00	rbd		Reduce update rate from 4Hz to 2Hz, for Autostar.
//						Even the LX200 is sort of swamped at 4Hz. 2Hz
//						should be OK.
// 11-Dec-00	rbd		Supress idle-time events when processing a request
//						received via DoOperation(). This prevents us from
//						re-entering the driver in doIdle when the driver
//						yields to pump events. 
// 22-Jan-01	rbd		Chooser moved into DriverHelper
// 26-Jan-01	rbd		New capability properties, dim or hide sync and/or
//						slew in menus if driver doesn't support the 
//						corresponding operation. Use op_PostDraw to 
//						allow switching back and forth between the 
//						crosshairs and user telrad/indicator.
// START V2
// --------
// 16-Sep-02	rbd		Add new status stuff for SNP V4, rev plugin version
//						for ASCOM Platform 2.1. Make this adaptive by
//						dynamically linking to new V4 status callback.
// 26-Jul-04	rbd		Hmm, fAdjustFOV was true, but that caused SN to zoom
//						in to 0 deg if tracking scoipe and the FOV is zoomed
//						in from the default. Possibly due to bogus _dGazeFOVRad
//						from SN itself??? Anyway, set fAdjustFOV to false
//						and completely remove _dGazeFOVRad. Dunno why it
//						was bad, but...
//========================================================================
#include "AscomScope.h"
#pragma hdrstop
#include <math.h>
#include <time.h>

#define FAST_UPDATE_MS 1000				// Update interval when scope is alive
#define SLOW_UPDATE_MS 5000				// Slow item update interval

//
// wID values for right-click menus we add
//
enum {
	do_SlewTo = 12000,
	do_SyncOn,
	};

//
// Add this so we can mimic a V7 plugin. This is not supported
// in the V8 API and we're using the V8 .H file...
//
//const sn_TimeFlowState kFlowReal			= 1104;	//??? Gone in V8 ???


// =======
// GLOBALS
// =======

//
// General global stuff
//
HINSTANCE _hInst;						// Our DLL module handle
HINSTANCE _hProc;						// Handle to Starry Night process that loaded us
HWND _hWndMain;							// Our main window handle

bool _bScopeCanSlew			= false;	// This is true if scope can slew at all
bool _bScopeCanSlewAsync	= false;	// This is true if scope can slew asynchronously
bool _bScopeCanSync			= false;	// This is true if scope can be sync'ed
bool _bScopeCanPark			= false;	// This is true if scope can be parked
bool _bScopeCanUnpark		= false;	// This is true if scope can be unparked
bool _bScopeCanSetPark		= false;	// This is true if scope can set park position
bool _bScopeCanFindHome		= false;	// This is true if scope can home
bool _bScopeHasEqu			= false;	// This is true if scope provides RA/Dec
bool _bScopeActive			= false;	// This is true if scope is active
bool _bScopeBusy			= false;	// This is true if we're currently talking to the scope
bool _bIsSlewing			= false;	// True if slewing
bool _bAutoTrack			= false;	// True for auto-tracking
bool _bStartCenter			= true;		// One-time flag to center on startup
bool _bOurIndicator			= true;		// We're drawing the indicator
bool _bFOVICalled			= false;	// do_FOVIndicator called this drawing cycle

char *_szScopeName = NULL;		// Name for crosshair labeling (delete[])
const char *_szAlertTitle = "ASCOM Telescope Plugin";

double _dScopeRA;
double _dScopeDec;
double _dRAPrev		= coordinate_NotAvailable;	// Force center-point at startup
double _dDecPrev	= coordinate_NotAvailable;
double _dMinFOV;				// Best guess at MINIMUM Gaze FOV
//double _dGazeFOVRad;			// Gaze FOV for panning/centering RADIANS

PSelection *_pSelect	= NULL;	// Non-null -> PSelection of currently selected obj
sn_WindowRef _pWnd		= NULL;	// Non-null -> SN window for telescope
PLocation *_pHomeLoc	= NULL;	// Non-null -> PLocation for home location
bool _bV8Plugin			= false;// true if running under SNP V4
bool _bV10Plugin		= false;// true if running under SNP V5.0.2
bool _bOfflineMsgSent	= false;// Limits sending of "offline" scope messages to once

//
// Dynamic link to sn_SetPluginStatusText()
//
typedef void (*SPSPROC)(pStr255, pStr255, double, double);
SPSPROC _psn_SetPluginStatusText;

//
// Dynamic link to sn_GetAreCoordinatesWithinUserSlewLimits()
//
typedef void (*GCWSLPROC)(PGaze, pBoolean, pBoolean*);
GCWSLPROC _psn_GetAreCoordinatesWithinUserSlewLimits;

// ==========
// MAIN ENTRY
// ==========
//
// Auto C Runtime initialization when the name DllMain() is used.
//
BOOL WINAPI DllMain(HMODULE hMod, DWORD fReason, LPVOID pvRes)
{
	switch (fReason) 
	{
	case DLL_PROCESS_ATTACH:
		_hInst = (HINSTANCE)hMod;			// Needed for various things
		_hProc = (HINSTANCE)GetModuleHandle(NULL);
		DisableThreadLibraryCalls(hMod);	// No thread att/det calls
		break;

	case DLL_PROCESS_DETACH:
		TermScope();						// Just in case (see termPlugin)
		break;

	default:								// No need for thread att/det
		;
	}
	return TRUE;
}




// ===============
// ACTION ROUTINES
// ===============
//

// ----------
// initPlugin
// ----------
//
static short initPlugin(PInitialize* p)
{
	_psn_SetPluginStatusText = (SPSPROC)GetProcAddress(_hProc, "sn_SetPluginStatusText");
	_psn_GetAreCoordinatesWithinUserSlewLimits = (GCWSLPROC)GetProcAddress(_hProc, "sn_GetAreCoordinatesWithinUserSlewLimits");
	
	if(_psn_SetPluginStatusText == NULL)	// Running under SNP V3?
	{
		p->fPluginAPI = kVersion7Plugin;	// Act like a V7 plugin
		_bV8Plugin = false;					// We can NOT use sn_SetPluginStatusText
	}
	else
	{
		p->fPluginAPI = kVersion8Plugin;	// This is a V8 or later plugin (has new status string stuff)
		_bV8Plugin = true;					// We can use sn_SetPluginStatusText
	}
	
	if (_psn_GetAreCoordinatesWithinUserSlewLimits && _psn_SetPluginStatusText)
	{
		p->fPluginAPI = kVersion10Plugin;	// This is a V10 or later plugin (has new status string stuff)
		_bV10Plugin = true;					// We can use sn_GetAreCoordinatesWithinUserSlewLimits
	}

	p->fSuccess			= true;					// Assume success
	p->fWantsMenuItem	= true;			// if this plug-in wants a menu command in settings menu
	p->fWantsIdleTime	= true;			// if true then plug-in receives op_Idle regularly
	p->fWantsWindow		= false;			// true if the plug-in requires a window
	p->fWindowVisible	= false;			// true if window is initially visible
	p->fWindowID		= 0;					// id of WIND resource for this window
	p->fWindowFloats	= false;			// true if requested window should float
	p->fLayer			= Depth_Telrad;			// Draw FOV here
	p->fIsTelescopeController = true;	// I control a telescope!
	p->fAboutMenuItem[0]= 15;			// Length of ""ASCOM Telescope""
	lstrcpy((char *)(&p->fAboutMenuItem[1]), "ASCOM Telescope");
	p->fWindowMenuItem[0]	= 0;
	p->fOptionsMenuItem[0]	= 0;
//	p->fWantsAllEvents = true;			// Wa want mouse clix in window	(Abort on click, see below)

	InitDrivers();

	return(0);
}


// ----------
// termPlugin
// ----------
//
static void termPlugin(void)
{
	TermScope();
}

// -------
// doAbout
// -------
//
static void doAbout(void)
{
	MessageBox(NULL, 
"Starry Night V3/V4 plug-in for ASCOM telescopes. For more information on ASCOM \
see http://ASCOM-Standards.org/", 
		_szAlertTitle, (MB_OK & MB_ICONINFORMATION | MB_SETFOREGROUND));
}

// ----------------
// buildContextMenu
// ----------------
//
// Adds "Slew to <object>" and "Sync on <object>" to context menu
// The current (3.0) Starry Night context menu is:
//
//	0	Select <object>			<--- MAY BE MISSING
//	1	---------				<--- MAY BE MISSING
//	2	Centre/Lock
//	3	Magnify
//	4	Add Image...
//	5	Go There                <--- MAY BE MISSING
//	6	---------				<--- ADDED AFTER CALL TO THIS PROC
//	7	Live Sky Web Info...    <--- ADDED AFTER CALL TO THIS PROC
//								<--- May be other stuff added after this call
//
// If the scope can't do a sync or slew, then don't add the item.
//
static void buildContextMenu(PBuildContext *p)
{
	HMENU hMenu = (HMENU)(p->fMenuHandle);
	PSelection o = p->fObject;
	MENUITEMINFO mi;
	char buf1[256], buf2[256];

	if(!_bScopeActive)								// No scope hookup?
		return;										// Forget this

	//
	// If object name is same as constellation name, then
	// this is a call from the constellation selector. 
	// Don't offer slewing or synching in this case.
	//
	if(pStrCmp(o.fObjectName, o.fConstellationName))
		return;

	//
	// OK, build and insert the menu items. Use the 
	// "userFormatttedName" for the menu strings. If
	// not clicked on an object, still offer to slew
	// but "to this location".
	//
	pToCStr(o.fUserFormattedName, buf1);			// Get object name
	if(buf1[0] == '\0')								// Empty?
		lstrcpy(buf1, "this location");

	//
	// Slew to <object> (if supported by driver)
	//
	if(_bScopeCanSlew && !GetParked())
	{
		sprintf(buf2, "Slew to %s", buf1);				// Format menu string

		ZeroMemory(&mi, sizeof(mi));
		mi.cbSize = sizeof(mi);
		mi.fMask = (MIIM_DATA | MIIM_ID | MIIM_STATE | MIIM_TYPE);
		mi.fType = MFT_STRING;
		mi.fState = _bIsSlewing ? MFS_DISABLED : MFS_ENABLED;
		mi.wID = do_SlewTo;								// do_SlewTo identifier
		mi.dwTypeData = buf2;

		InsertMenuItem(hMenu, 0, TRUE, &mi);
	}


	//
	// Sync on <object> is added AFTER the Go There, and
	// after adding a separator before it. Thus, Sync will
	// be surrounded by separators, to minimize the chance
	// of an accidental sync.
	//
	if(_bScopeCanSync && (o.fObjectName[0] != 0))	// Only if we can, and if we're on an object
	{
		int n = GetMenuItemCount(hMenu);

		ZeroMemory(&mi, sizeof(mi));				// Append a separator
		mi.cbSize = sizeof(mi);
		mi.fMask = MIIM_TYPE;
		mi.fType = MFT_SEPARATOR;
		InsertMenuItem(hMenu, n, TRUE, &mi);

		sprintf(buf2, "Sync on %s", buf1);			// Format menu string
		ZeroMemory(&mi, sizeof(mi));
		mi.cbSize = sizeof(mi);
		mi.fMask = (MIIM_DATA | MIIM_ID | MIIM_STATE | MIIM_TYPE);
		mi.fType = MFT_STRING;
		mi.fState = _bIsSlewing ? MFS_DISABLED : MFS_ENABLED;
		mi.wID = do_SyncOn;							// do_SyncOn identifier
		mi.dwTypeData = buf2;
		InsertMenuItem(hMenu, n+1, TRUE, &mi);
	}
}

// -------------
// doContextMenu
// -------------
//
static void doContextMenu(PDoContext *p)
{
	char buf[256];

	pBoolean isWithinSlewLimits = true;
	PGaze targetCoords;
	targetCoords.fGazeRa	= (p->fObject.fRightAscension);
	targetCoords.fGazeDec	= (p->fObject.fDeclination);
	targetCoords.fGazeAltitude	= coordinate_NotAvailable;
	targetCoords.fGazeAzimuth	= coordinate_NotAvailable;

	switch(p->fMenuCommand)
	{
	case do_SlewTo:
		{
			if (_bV10Plugin)
				(*_psn_GetAreCoordinatesWithinUserSlewLimits)(targetCoords, true, &isWithinSlewLimits);
			
			if (isWithinSlewLimits)
			{
				pToCStr(p->fObject.fUserFormattedName, buf);
				SlewScope((p->fObject.fRightAscension * HR_PER_RAD), (p->fObject.fDeclination * DEG_PER_RAD));
			}
			
			p->fMenuHandled = true;
			break;
		}

	case do_SyncOn:
		{
			if (_bV10Plugin)
				(*_psn_GetAreCoordinatesWithinUserSlewLimits)(targetCoords, true, &isWithinSlewLimits);
			
			if (isWithinSlewLimits)
				SyncScope((p->fObject.fRightAscension * HR_PER_RAD), (p->fObject.fDeclination * DEG_PER_RAD));
			p->fMenuHandled = true;
			break;
		}

	default:
		p->fMenuHandled = false;
		break;
	}
}

// ------------
// doMenuStatus
// ------------
//
static void doMenuStatus(PMenuStatus *p)
{
	p->fIsEnabled = false;							// Default these now
	p->fHasCheck = false;

	pBoolean isParked = false;
	if (_bScopeCanPark || _bScopeCanUnpark)
		isParked = GetParked();

	switch(p->fMenuCommand)
	{
		case cmd_TelescopeTime:							// We use time and location from the scope
		case cmd_TelescopeLocation:
			p->fIsEnabled = FALSE;
			break;

		case cmd_TelescopeSlew:
			if (_bScopeCanPark && isParked)
				p->fIsEnabled = false; // disable if parked
			else
				p->fIsEnabled = _bScopeCanSlew && _bScopeActive && !_bIsSlewing;
			break;
		case cmd_TelescopeSync:
			if (_bScopeCanPark && isParked)
				p->fIsEnabled = false; // disable if parked
			else
				p->fIsEnabled = _bScopeCanSync && _bScopeActive && !_bIsSlewing;
			break;
		case cmd_TelescopeClose:
			p->fIsEnabled = _bScopeActive && !_bIsSlewing;
			break;
		case cmd_TelescopeConfigure:					// Allow when scope is not active
		case cmd_TelescopeOpen:
			p->fIsEnabled = !_bScopeActive;
			break;
		case cmd_TelescopeTrack:
			p->fIsEnabled = _bScopeActive;
			//p->fHasCheck = (_bScopeActive && _bAutoTrack);
			p->fHasCheck = (_bScopeActive && _bAutoTrack) ? TRUE : FALSE;
			break;
		case cmd_TelescopePark:
			if (_bScopeCanPark)
				p->fIsEnabled = (!isParked && _bScopeActive);
			break;
		case cmd_TelescopeUnpark:
			if(_bScopeCanUnpark)
				p->fIsEnabled = (isParked && _bScopeActive);
			break;
		case cmd_TelescopeSetParkPosition:
			if (_bScopeCanSetPark)
				p->fIsEnabled = (!isParked && _bScopeActive);
			break;
		case cmd_TelescopeGoHome:
			if (_bScopeCanFindHome)
			{
				if (_bScopeCanPark && isParked)
					p->fIsEnabled = false; // disable if parked
				else
					p->fIsEnabled = (!isParked && _bScopeActive);
			}
			break;
		default:
			break;
	}
}

// -----
// doFOV
// -----
//
// For now we don't use multiple layers. This gets called whenever
// Starry Night or us requests an update.
//
static void doFOV(PIndicator *p)
{
	p->fDrawIndicator = true;						// We're going to draw one
	p->fDoneDrawing = true;							// One figure only
	p->fCentreRA = _dScopeRA * RAD_PER_HR;			// RA and Dec location
	p->fCentreDec = _dScopeDec * RAD_PER_DEG;

	cToPstr(_szScopeName, p->fString);				// Indicator label
	p->fIndicatorStyle = Indicator_Circle;			// (actually irrelevant)
	p->fVertical = 0.0;								// 0 means crosshair
	p->fHorizontal = 0.0;
}

// ------
// doIdle
// ------
//
static short doIdle(PEvent *p)
{
	static DWORD tn = 0;							// Tix for next update (0 = force update)
	static DWORD tnSlow = 0;						// Tix 
	DWORD t = GetTickCount();
	short iRes = 0;									// Assume success (our retval)
	char buf[256];
	char buf2[256];

	p->fOutOperation = out_DoNothing;				// Assume no-op
	p->fOutParams = NULL;				

	if(!_bScopeActive)								// If no scope
	{
		if(_bV8Plugin && !_bOfflineMsgSent)			// Fill in status box on SNP V4
		{
			cToPstr("Telescope is not connected", (unsigned char*)&buf);
			cToPstr("To use to your telescope, make sure it is physically connected to your computer, configure it using the Configure Connection... button above, then click the Connect button.", (unsigned char*)&buf2);
					(*_psn_SetPluginStatusText)((unsigned char*)&buf, 
					(unsigned char*)&buf2, 
					coordinate_NotAvailable, 
					coordinate_NotAvailable);

			_bOfflineMsgSent = true;				// We have sent the offline status now
		}
		if(_pWnd != NULL)							// Must close during idle
		{
//			sn_CloseWindow(_pWnd);
			_pWnd = NULL;
			_bStartCenter = true;					// Force center on next open
			_bOurIndicator = true;					// Assume we must draw indicator
			_dRAPrev = coordinate_NotAvailable;
			_dDecPrev = coordinate_NotAvailable;
		}
		return(0);									// DONE
	}
	_bOfflineMsgSent = false;						// Online now, need to send offline status again if go offline

	//
	// Open scope in top window
	//
	if(_pWnd == NULL)
	{
		double dLat;
		double dLong;

		sn_GetTopDocumentRef(&_pWnd);

		if (_pWnd == NULL) {
			sn_NewWindow(&_pWnd);						// Make new window
			sn_BringDocumentWindowToFront(_pWnd);
		}

		sn_TurnDaylightOnOff(_pWnd, false);
		sn_TurnHorizonOnOff(_pWnd, true, kHorizonOption_noChange);
		//
		// Try to get the Lat/Long from the scope. If either fails then use the
		// home location for Starry Night. Remember whatever we used for next
		// step.
		//
		__try
		{
			dLat = GetLatitude();
			dLong = GetLongitude();
			if(sn_SetViewerLocation(_pWnd, (dLat * RAD_PER_DEG), 
								(dLong * RAD_PER_DEG), (3.0/M_PER_AU), false) != snNoErr)
				drvFail(
"Failed to set viewer location. The selected telescope probably returned bad lat/long.", 
							NULL, true);
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			//
			// If failed because driver doesn't implement, use Starry Night's
			// home location. Otherwise this is a real failure and it has 
			// already been alerted. Just resignal in that case.
			//
			if(GetExceptionCode() == EXCEP_NOTIMPL)	
			{
				dLat = _pHomeLoc->fEarthLatitude * DEG_PER_RAD;
				dLong = _pHomeLoc->fEarthLongitude * DEG_PER_RAD;
				sn_SetViewerLocation(_pWnd, _pHomeLoc->fEarthLatitude, 
						_pHomeLoc->fEarthLongitude,  (3.0/M_PER_AU), false);
			}
			else
				ABORT;								// Signal an error
		}
		//
		// This is bizarre. Now we try to SET the Lat/long in the driver. We do this
		// because at least one known driver, the NexStar one, needs geodetic
		// position to convert RA/Dec to Alt/Az. The NexStar is badly broken for 
		// for equatorial slews. And it does not support RS-232 access to its
		// internal Lat/Long. In any case, this is harmless. If the driver doesn't
		// support setting Lat/Long, we don't worry. If it does, it eithr gets 
		// the same thing we just read from it, or the SN home location. 
		//
		__try
		{
			SetLatitude(dLat);
			SetLongitude(dLong);
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			if(GetExceptionCode() != EXCEP_NOTIMPL)	
				ABORT;								// Signal an error
		}

		//
		// Try to get the local time from the scope. If that fails, use the
		// time from the PC. This depends on the user setting the time zone
		// in the PC correctly. 
		//
		__try
		{
			sn_SetCurrentTime(_pWnd, GetJulianDate());
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			//
			// If failed because driver doesn't implement, use the PC's time()
			// function. Otherwise this is a failure (scope disappeared?)
			// and it has already been alerted. Just resignal in that case.
			// The C runtime time() fucnction returns a value whose Julian epoch
			// is 2440587.5.
			//
			if(GetExceptionCode() == EXCEP_NOTIMPL)	
				sn_SetCurrentTime(_pWnd, 2440587.5 + ((double)time(NULL)/86400.0));
			else
				ABORT;								// Signal an error
		}
		if(!_bV8Plugin) sn_SetTimeFlowState(_pWnd, kFlowForward);		// ??? Gone in V8 ???
		sn_SetOrientation(_pWnd, kOrientationLocal);
		//sn_SaveAsWindow(_pWnd, 0, 0, (unsigned char *)buf2);
		sn_ImmediateUpdateWindow(_pWnd);
		tn = 0;										// Force update stuff below
	}


	//
	// FAST UPDATE ITEMS
	//
	// We control the rate of updating here! If time stepping (or real time)
	// is on, Starry Night issues an update at the step rate. BUT if neither
	// is active, we need to force updates here in order to properly display
	// the telrad.
	//
	if((t > tn))	//&& (p->fInWindow == _pWnd))			// If update interval has elapsed
	{
		__try
		{
			_bIsSlewing = IsSlewing();				// Make sure this gets called during async slew.

			//
			// This set of coordinates is used here and by the 
			// telrad drawing code as well.
			//
			if (_bScopeCanPark && GetParked())
			{
				cToPstr("Telescope is parked.", (unsigned char*)&buf);
				cToPstr( "The telescope is currently parked. To move the telescope click 'Unpark'.", (unsigned char*)&buf2);
				
				(*_psn_SetPluginStatusText)((unsigned char*)&buf, (unsigned char*)&buf2, coordinate_NotAvailable, coordinate_NotAvailable);
			}
			else
			{
				if(_bScopeHasEqu)						// If we can get RA/Dec
				{
					_dScopeRA = GetRightAscension();	// Get it!
					_dScopeDec = GetDeclination();
				}
				else									// Can only get Alt/Az ...
				{										// Convert using the SN window location
					PSphericalCoordinates sphereCoords;
					PSphericalCoordinates raDec;
					PXYZCoordinates xyzAltAzCoords;
					PXYZCoordinates xyzRaDecCoords;

					if(sn_RealWorldAzimuth2Mathematical((GetAzimuth() * RAD_PER_DEG), 
														&sphereCoords.ra) != snNoErr)
						drvFail("Azimuth conversion failed", NULL, true);
					sphereCoords.dec = GetAltitude() * RAD_PER_DEG;
					sphereCoords.radius = 1.0;
					if(sn_SphericalToXYZ(sphereCoords, &xyzAltAzCoords) != snNoErr)
						drvFail("Spherical to XYZ conversion failed", NULL, true);
					if(sn_CoordinateConversion(_pWnd, 
													kSNAltAzSystem,
													kSNCelestialJNowSystem, 
													xyzAltAzCoords, 
													&xyzRaDecCoords) != snNoErr)
						drvFail("Local to Equatorial coordinate conversion failed", NULL, true);
					if(sn_XYZToSpherical(xyzRaDecCoords, &raDec) != snNoErr)
						drvFail("XYZ to spherical conversion failed", NULL, true);
					_dScopeRA = raDec.ra * HR_PER_RAD;
					_dScopeDec = raDec.dec * DEG_PER_RAD;
				}

				//
				// Refresh the V4 status window.
				//
				if(_bV8Plugin)								// Fill in status box on SNP V4
				{
					if(_bIsSlewing)
					{
						cToPstr("Telescope is moving...", (unsigned char*)&buf);
						cToPstr( "The telescope is currently moving to the new sky location. If the view is not centered on the location you intended, check the alignment, date/time, and location settings of your telescope.", (unsigned char*)&buf2);
					}
					else
					{
						cToPstr("Telescope is tracking the sky.", (unsigned char*)&buf);
						cToPstr("The telescope is currently tracking the motion of the Earth, keeping the current view of the sky still in the eyepiece or camera. If objects are moving in the field of view, check the alignment of your telescope.", (unsigned char*)&buf2);
					}
					(*_psn_SetPluginStatusText)((unsigned char*)&buf, (unsigned char*)&buf2, 
															_dScopeRA * RAD_PER_HR, 
															_dScopeDec * RAD_PER_DEG);
				}

				//
				// Auto-track logic (or start-up centering)
				//
				if(_bAutoTrack || _bStartCenter)
				{
					static PCentreAction pc;			// WARNING! Not thread safe!
					double dr;
					double dd;
					double d;

					//
					// Algorithm: If the cursor has moved more than 1/4
					// of the "minimum" FOV (in _dMinFOV, deg.) then
					// we recenter. NOTE: This is a POOR approximation
					// for wide gazes, where the mapping from RA/Dec to 
					// gaze alt/az is REALLY curved. Therefore, if the 
					// gazeFOV is > 30, we just step every 15 deg. The 
					// angular motion of RA is 15 deg/hr at the celestial 
					// equator, reduce by the cos(dec). Use the midpoint
					// of the old and new dec.
					//
					// Panning: If the delta position is "small" then pan,
					// otherwise jump.
					//
					// Always move to center on startup if _bStartCenter
					//
					dr = ((_dScopeRA - _dRAPrev) * 15.0) * 
									cos(((_dScopeDec + _dDecPrev) / 2.0) * RAD_PER_DEG);
					dd = (_dScopeDec - _dDecPrev);
					d = sqrt((dr * dr) + (dd * dd));	// Distance moved
					if(_bStartCenter || (d > (_dMinFOV / 4.0)))	 // If starting or moved to auto-center distance
					{									// ... then re-center
						_bStartCenter = false;			// (no more start centering)
						pc.fRightAscension = _dScopeRA * RAD_PER_HR;// New centre point coords
						pc.fDeclination = _dScopeDec * RAD_PER_DEG;
						pc.fUseCustomPan = true;		// Override SN ...
						if(_bStartCenter || (d > (_dMinFOV / 2.0)))	// If starting or large jump
							pc.fPan = false;			// No panning, jump
						else							// Small jump
							pc.fPan = true;				// pan for beauty
						pc.fAdjustFOV = false;			// Never change FOV
						//pc.fNewFOV = _dGazeFOVRad;
						p->fOutOperation = out_CentrePoint;
						p->fOutParams = &pc;
						_dRAPrev = _dScopeRA;
						_dDecPrev = _dScopeDec;
					}
					else if(d > 0.003)					// If moved more than 10 arcsec,
					{
						p->fOutOperation = out_Update;	// Update to draw telrad
					}
				}
				else									// Not auto-track, draw telrad
				{
					_dRAPrev	= coordinate_NotAvailable;	// (also reset auto-track tests)
					_dDecPrev	= coordinate_NotAvailable;
					p->fOutOperation = out_Update;		// Force screen update for telrad
				}
			}

			tn = t + FAST_UPDATE_MS;				// Next interval
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			p->fOutOperation = out_Update;			// Just do screen update
			iRes = -1;								// But note failure and bail
		}
	}
	return(iRes);
}


// =============================
// MAIN FUNCTION FOR STARRYNIGHT
// =============================

SNCall short DoOperation(OperationT inOperation, void* ioParams)
{
	PIndicator *p = (PIndicator *)ioParams;
	short outErr = 0;
	HWND hW;

	//
	// Get the current active window. Sometimes this is 0.
	//
	hW = GetActiveWindow();
	if(hW != 0)
		_hWndMain = hW;

	__try {
		switch (inOperation)
		{
			// =============
			// op_Initialize	called once at begining of program when
			// =============	the plugin is first initilialized. The
			//					current resource file will be the plug-in's
			//					file when called.
			//
			case op_Initialize:						// PInitialize*
				outErr = initPlugin((PInitialize*)ioParams);
				break;

			case op_Terminate:						// NULL
				termPlugin();
				break;

			case op_About:							// NULL
				doAbout();
				break;

			case op_TelescopeConfigure:				// NULL
				outErr = ConfigScope();
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_TelescopeOpen:					// NULL
				_bScopeBusy = true;					// Stop doIdle()
				outErr = InitScope();
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_TelescopeClose:					// NULL
				TermScope();
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_Idle:							// PEvent*
				if(!_bScopeBusy)					// Unless we're not supposed to run
					outErr = doIdle((PEvent *)ioParams);
				break;

			case op_MenuCommand:					// NULL
				MessageBox(NULL, "Requires no setup.", _szAlertTitle, 
					(MB_OK | MB_ICONINFORMATION | MB_SETFOREGROUND));
				break;

			case op_BuildContextualPopup:			// PBuildContext*
				buildContextMenu((PBuildContext *)ioParams);
				break;

			case op_DoContextualPopup:				// PDoContext*
				_bScopeBusy = true;					// Stop doIdle() (for Slew and Sync)
				doContextMenu((PDoContext *)ioParams);
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_MenuStatus:						// PMenuStatus*
				doMenuStatus((PMenuStatus *)ioParams);
				break;

			case op_HomeLocation:					// PLocation*
				if(_pHomeLoc == NULL)				// 1st call
					_pHomeLoc = new PLocation;		// Make local PLocation
				CopyMemory(_pHomeLoc, ioParams, sizeof(PLocation));
				break;

			case op_DrawLayer:						// PDraw* (just to get FOV)
				{									// Localize declared vars
					//
					// This gets the FOV in the smaller dimension, or the 
					// MINIMUM FOV.
					//
					PDraw* p = ((PDraw*)ioParams);
					//if(_pWnd == p->fDestinationWindowRef)
					//{
						short w = p->fSceneBounds.right - p->fSceneBounds.left;
						short h = p->fSceneBounds.bottom - p->fSceneBounds.top;
//						_dGazeFOVRad = w /  p->fPixelsPerRadian;
						_dMinFOV = ((h < w) ? h : w) / p->fPixelsPerRadian;
						_dMinFOV *= DEG_PER_RAD;		// FOV in degrees.
						//
						// TRICK! Move the reference point for auto center
						// if we're hand-panning. This will force the 
						// auto-center logic to jerk the telrad back
						// to the center.  DO WE WANT THIS???
						//
						_dRAPrev = p->fGazeRa * HR_PER_RAD;
						_dDecPrev = p->fGazeDec * DEG_PER_RAD;
					//}
				}
				break;

			case op_DrawIndicator:					// i/o PIndicator*
				p->fDrawIndicator = false;			// Assume no FOV drawn
				if(_bScopeActive &&	_bOurIndicator)// &&
							//(_pWnd == p->fWindowThatIsDrawing))
					doFOV((PIndicator *)ioParams);
				break;

			case op_FOVIndicatorPosition:			// i/o PIndicator*
				//if(_pWnd == p->fWindowThatIsDrawing)	// Do this only for our scope window
				//{
					_bFOVICalled = true;					// Note that we're doing FOVIndicator
					p->fCentreRA = _dScopeRA * RAD_PER_HR;	// Force its RA and Dec location
					p->fCentreDec = _dScopeDec * RAD_PER_DEG;
				//}
				break;

			case op_PostDraw:						// Flag whether FOVIndicator was active
				_bOurIndicator = !_bFOVICalled;		// for this pass through DoOperation
				_bFOVICalled = false;
				break;

			case op_TelescopeSlew:					// PGaze*
				{
					_bScopeBusy = true;				// Stop doIdle()
					PGaze *p = (PGaze*)ioParams;
					outErr = SlewScope((p->fGazeRa * HR_PER_RAD), // Could be async or sync
										(p->fGazeDec * DEG_PER_RAD));
				}
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_TelescopePark:					// 
				ParkScope();
				break;

			case op_TelescopeUnpark:				// 
				UnparkScope();
				break;

			case op_TelescopeSetParkPosition:		// 
				SetParkScope();
				break;

			case op_TelescopeGoHome:				// 
				FindHomeScope();
				break;

			case op_TelescopeSync:					// PGaze*
				{
					_bScopeBusy = true;				// Stop doIdle()
					PGaze *p = (PGaze*)ioParams;
					outErr = SyncScope((p->fGazeRa * HR_PER_RAD), 
											(p->fGazeDec * DEG_PER_RAD));
				}
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_TelescopeTrack:					// NULL (should toggle tracking on/off
				if(_bScopeActive)
					//_bAutoTrack = !_bAutoTrack;
					_bAutoTrack = (_bAutoTrack) ? false : true;
				else
					outErr = -1;					// Failed
				break;

			
			case op_SceneClick:						// Used to click-stop slewing
				if(_bScopeCanSlewAsync && IsSlewing())	// (but only for async-capable scopes)
				{
					MessageBeep(-1);
					AbortSlew();
					while(_bIsSlewing = IsSlewing())
						;
					inOperation = out_HandledByPlugin;
				}
				break;

#ifdef JUST_FOR_REFERENCE
			case op_TelescopeLocation:				// PLocation*
			case op_TelescopeTime:					// PTime*
			case op_SetupWindow:					// in WindowPtr
			case op_UpdateWindow,					// in	RgnHandle (update region)
			case op_Centre:							// in PGaze*
			case op_NewScene,						// in	PGaze* (or NULL if no new scene)
			case op_CursorPosition,					// in	PSelection*
			case op_Location,						// in	PLocation*
			case op_ReceiveDrag,					// in	PDragDrop
			case op_FindObjectInDataBase,			// i/o 	PFindAction*
			case op_ReintializeFind					// NULL
			case op_MenuStatus,						// out	PMenuStatus*

			case op_DragDropAcceptable:		// i/o PDragDropAcceptable
				{
				PDragDropAcceptable *p = (PDragDropAcceptable *)ioParams;

				p->fOutIsAcceptable = false;
				p->fOutIsAcceptableForObjects = false;
				}
				break;

			case op_GetDrawTimeFuncPtr:		// out	PDrawTimeFuncPtrs*
				{
				PDrawTimeFuncPtrs *p = (PDrawTimeFuncPtrs *)ioParams;

				p->fPPCFuncPtr = NULL;
				p->f68kFuncPtr = NULL;
				p->fPPCSetupFuncPtr = NULL;
				p->f68kSetupFuncPtr = NULL;
				}
				break;
#endif
			//
			// UNKNOWN CALL
			//
			default:
				;
		}
	}
	__except(EXCEPTION_EXECUTE_HANDLER)
	{
		outErr = -1;
	}

	_bScopeBusy = false;							// Let doIdle run again
	return outErr;
}


