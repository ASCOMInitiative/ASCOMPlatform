#define WIN32_LEAN_AND_MEAN
#define NOSERVICE
#define NOMCX
#define NOIME
#define NOSOUND
#define NOKANJI
#define NOIMAGE
#define NOTAPE

//#include <afxdisp.h>        // MFC Automation classes

#include <objbase.h>

#include <ole2ver.h>
#include <initguid.h>
#include <stdio.h>
#include <stdlib.h>
#include <climits>

#define TARGET_OS_MAC 0
#define TARGET_OS_WIN32 1

#include "StarryNightPlugins.h"

#define PI 3.14159265359
#define DEG_PER_RAD 57.2957795131
#define HR_PER_RAD (DEG_PER_RAD / 15.0)
#define RAD_PER_DEG 0.0174532925199
#define RAD_PER_HR (RAD_PER_DEG * 15.0)
#define M_PER_AU 1.4959787066E11

#define EXCEP_ABORT 0xE100000F
#define ABORT RaiseException(EXCEP_ABORT, 0, NULL, NULL)
#define EXCEP_NOTIMPL 0x80040400							// Drivers must raise this code!
#define NOTIMPL RaiseException(EXCEP_NOTIMPL, 0, NULL, NULL)

// -------------------
// DriverInterface.cpp
// -------------------

extern bool _bFocuserActive;
extern const char *_szAlertTitle;
extern HWND _hWndMain;
extern bool _bFocuserIsAbsolute;
extern bool	_bFocuserCanHalt;
extern bool _bFocuserHasTempComp;
extern bool	_bFocuserBusy	;
extern bool	_bIsMoving;

extern bool InitDrivers(void);
extern short InitFocuser(void);
extern void TermFocuser(void);
extern short ConfigFocuser(void);
extern bool GetIsAbsolute(void);
extern bool GetHasTempComp(void);
extern bool	GetCanHalt(void);
extern double GetTemperature(void);
extern long GetPosition(void);
extern long GetMaxStep();
extern long GetMaxIncrement();
extern double GetStepSize();
extern void SetTempComp(bool tempcomp);
extern bool GetTempComp(void);
extern bool GetIsMoving(void);
extern short MoveFocuser(long newpos);
extern void AbortMove(void);

// -------------
// Utilities.cpp
// -------------

extern void pToCStr(pStr255 p, char *c);
extern void cToPstr(char *c, pStr255 p);
extern bool pStrCmp(pStr255 s1, pStr255 s2);
extern BSTR ansi_to_bstr(char *s);
extern OLECHAR *ansi_to_uni(char *s);
extern char *uni_to_ansi(OLECHAR *os);
extern void drvFail(char *msg, EXCEPINFO *ei, bool bFatal);
