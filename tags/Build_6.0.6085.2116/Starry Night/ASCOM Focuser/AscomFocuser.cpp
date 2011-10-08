//========================================================================
//
// TITLE:		AscomFocuser.CPP
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
// 07-Oct-05	MK		Ready for official release. Now handles Absolute and relative scopes. v5.7.0
// 01-Aug-05	mk		Initial edit
//========================================================================
#include "AscomFocuser.h"
#pragma hdrstop
#include <math.h>
#include <time.h>

#define FAST_UPDATE_MS 1000				// Update interval when focuser is alive
#define SLOW_UPDATE_MS 5000				// Slow item update interval


// =======
// GLOBALS
// =======

//
// General global stuff
//
HINSTANCE _hInst;						// Our DLL module handle
HINSTANCE _hProc;						// Handle to Starry Night process that loaded us
HWND _hWndMain;							// Our main window handle

bool _bFocuserIsAbsolute		= false;	// This is true if focuser is absolute
bool _bFocuserHasTempComp		= false;	// This is true if focuser has temperature compensation
bool _bFocuserCanHalt			= false;	// This is true if focuser can stop
bool _bFocuserActive			= false;	// This is true if focuser is active
bool _bFocuserBusy				= false;	// This is true if we're currently talking to the focuser
static bool _bDoIdleBusy		= false;	// This is true if doIdle is currently in process
bool _bIsMoving					= false;	// True if moving

const char *_szAlertTitle = "ASCOM Focuser Plugin";

sn_WindowRef _pWnd		= NULL;	// Non-null -> SN window for focuser
PLocation *_pHomeLoc	= NULL;	// Non-null -> PLocation for home location

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
		TermFocuser();						// Just in case (see termPlugin)
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
	p->fSuccess			= true;					// Assume success
	p->fWantsMenuItem	= true;					// if this plug-in wants a menu command in settings menu
	p->fWantsIdleTime	= true;					// if true then plug-in receives op_Idle regularly
	p->fWantsWindow		= false;				// true if the plug-in requires a window
	p->fWindowVisible	= false;				// true if window is initially visible
	p->fWindowID		= 0;					// id of WIND resource for this window
	p->fWindowFloats	= false;				// true if requested window should float
	p->fIsFocuserController = true;				// I control a focuser!
	p->fIsTelescopeController = false;			// but I don't control the Telescope
	p->fAboutMenuItem[0]= 13;					// Length of ""ASCOM Focuser""
	lstrcpy((char *)(&p->fAboutMenuItem[1]), "ASCOM Focuser");
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
	TermFocuser();
}

// -------
// doAbout
// -------
//
static void doAbout(void)
{
	MessageBox(NULL, 
"Starry Night v5.7.0 plug-in for ASCOM focusers. For more information on ASCOM \
see http://ASCOM-Standards.org/", 
		_szAlertTitle, (MB_OK & MB_ICONINFORMATION | MB_SETFOREGROUND));
}



// ------------
// doMenuStatus
// ------------
//
static void doMenuStatus(PMenuStatus *p)
{

	p->fIsEnabled = false;							// Default these now
	p->fHasCheck = false;

	switch(p->fMenuCommand)
	{
		//enable Close button if focuser is active and not moving
		case cmd_FocuserClose:
			p->fIsEnabled = _bFocuserActive && !_bIsMoving;
			break;
		//enable Stop button if focuser is active and can be stopped
		case cmd_FocuserStop:
			p->fIsEnabled = _bFocuserActive && _bFocuserCanHalt && _bIsMoving;
			break;
		//enable TempCompensation checkbox if focuser is active and has temp compensation
		//check this box if tempcomp is enabled
		case cmd_FocuserTempCompensation:
			p->fIsEnabled = _bFocuserActive && _bFocuserHasTempComp && !_bIsMoving;
			p->fHasCheck = _bFocuserActive && _bFocuserHasTempComp && GetTempComp();
			break;
		//enable Goto absolute position box if focuser is active and absolute
		case cmd_FocuserGoto:
			p->fIsEnabled = _bFocuserActive && _bFocuserIsAbsolute && !_bIsMoving;
			break;
		//enable move by increment box if focuser is active (ie. all focusers should support this)
		case cmd_FocuserMove:
			p->fIsEnabled = _bFocuserActive && !_bIsMoving;
			break;
		//enable Configure and Connect buttons if focuser is not active
		case cmd_FocuserConfigure:
		case cmd_FocuserOpen:
			p->fIsEnabled = !_bFocuserActive;
			break;
		default:
			break;
	}
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

	p->fOutOperation = out_DoNothing;				// Assume no-op
	p->fOutParams = NULL;				

	//
	// Open focuser in top window
	//
	if(_pWnd == NULL)
	{
		sn_GetTopDocumentRef(&_pWnd);

		if (_pWnd == NULL) {
			sn_NewWindow(&_pWnd);						// Make new window
			sn_BringDocumentWindowToFront(_pWnd);
		}

		sn_TurnHorizonOnOff(_pWnd, true, kHorizonOption_noChange);

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

			//If the focuser was moving and it's status changed then refresh the panel
			if (_bIsMoving && (!GetIsMoving()))
			{
				_bIsMoving = false;
				sn_RefreshTelescopePanel();
			}

			//Setting _bFocuserActive to false and true upon completing query is here in case connection with
			// the focuser breaks
			if (_bFocuserHasTempComp || _bFocuserIsAbsolute) {
				_bFocuserActive = false;
			
				if (_bFocuserHasTempComp)
					sn_SetFocuserTempInfo(GetTemperature());

				if (_bFocuserIsAbsolute)
					sn_SetFocuserPositionInfo(GetPosition());
			
				_bFocuserActive = true;
//				sn_RefreshTelescopePanel();
			}

			tn = t + FAST_UPDATE_MS;				// Next interval
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			iRes = -1;								// Note failure and bail
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

			case op_FocuserConfigure:				// NULL
				outErr = ConfigFocuser();
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_FocuserOpen:					// NULL
				_bFocuserBusy = true;				// Stop doIdle() from taking place now
				outErr = InitFocuser();
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_FocuserClose:					// NULL
				TermFocuser();
				SetForegroundWindow(_hWndMain);		// Bring ourself to the front
				break;

			case op_Idle:								// PEvent*
				if(_bFocuserActive && !_bFocuserBusy)	// run doIdle of if the scope is not busy
					if (!_bDoIdleBusy) {				// and doIdle is not already running (prevents inf. regression)
						_bDoIdleBusy = true;
						outErr = doIdle((PEvent *)ioParams);
						_bDoIdleBusy = false;
					}
				break;

			case op_MenuStatus:						// PMenuStatus*
				doMenuStatus((PMenuStatus *)ioParams);
				break;

			case op_FocuserMove:					// long*
				if (_bFocuserActive && !_bIsMoving) {   //if focuser is active and not already moving
					long* value = (long *)ioParams;
					if (_bFocuserIsAbsolute)			// then move it by the amount specified
						MoveFocuser(GetPosition() + *value); // but absolute focuser need the exact new position
					else
						MoveFocuser(*value);				// and relative focusers only need the increment
				}	
				break;

			case op_FocuserGoto:					// long*
				if (_bFocuserActive && !_bIsMoving && _bFocuserIsAbsolute) { // absolute position goto makes sense only
					long* value = (long *)ioParams;							// for absolute focusers
					MoveFocuser(*value);
				}	
				break;

			case op_FocuserStop:					// NULL
				if (_bFocuserActive && _bIsMoving && _bFocuserCanHalt) // initiate stop sequence if the focuser can be stopped
					AbortMove();
				break;

			case op_FocuserTemperatureCompensation:	// NULL
				if (_bFocuserActive && _bFocuserHasTempComp) //set temperature compensation if focuser supports that feature
					SetTempComp(!GetTempComp());
				break;



#ifdef JUST_FOR_REFERENCE
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

	_bFocuserBusy = false;							// Let doIdle run again
	return outErr;
}
