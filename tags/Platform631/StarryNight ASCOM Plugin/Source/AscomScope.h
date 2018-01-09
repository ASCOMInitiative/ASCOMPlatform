#define WIN32_LEAN_AND_MEAN
#define NOSERVICE
#define NOMCX
#define NOIME
#define NOSOUND
#define NOKANJI
#define NOIMAGE
#define NOTAPE
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



extern bool _bScopeActive;
extern const char *_szAlertTitle;
extern HWND _hWndMain;
extern char *_szScopeName;
extern bool _bScopeHasEqu;
extern bool _bScopeCanSlew;
extern bool _bScopeCanSlewAsync;
extern bool _bScopeCanSync;
extern bool _bScopeCanPark;
extern bool _bScopeCanUnpark;
extern bool _bScopeCanSetPark;
extern bool _bScopeParked;
extern bool _bScopeCanFindHome;
extern bool _bStartCenter;
extern bool	_bScopeBusy	;
extern bool	_bIsSlewing;
extern bool _bAutoTrack;

extern bool InitDrivers(void);
extern short InitScope(void);
extern void TermScope(void);
extern short ConfigScope(void);
extern bool GetCanSlew(void);
extern bool GetCanSlewAsync(void);
extern bool GetCanSync(void);
extern bool GetCanPark(void);
extern bool GetCanUnpark(void);
extern bool GetCanSetPark(void);
extern bool GetParked(void);
extern bool GetCanFindHome(void);
extern double GetRightAscension(void);
extern double GetDeclination(void);
extern double GetAzimuth();
extern double GetAltitude();
extern double GetLatitude(void);
extern double GetLongitude(void);
extern double GetJulianDate(void);
extern void SetLatitude(double lat);
extern void SetLongitude(double lng);
extern char *GetName(void);
extern bool IsSlewing(void);
extern short SlewScope(double dRA, double dDec);
extern void AbortSlew(void);
extern short SyncScope(double dRA, double dDec);
extern void UnparkScope(void);
extern void ParkScope(void);
extern void SetParkScope(void);
extern void FindHomeScope(void);

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



