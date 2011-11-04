#include <objbase.h>
#include <ole2ver.h>
#include <initguid.h>
#include <stdlib.h>

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
extern bool _bScopeIsGEM;
extern bool InitDrivers(void);
extern short InitScope(void);
extern void TermScope(bool);
extern short ConfigScope(void);
extern int GetAlignmentMode(void);
extern bool GetCanSlew(void);
extern bool GetCanSlewAsync(void);
extern bool GetCanSync(void);
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

// -------------
// Utilities.cpp
// -------------

extern BSTR ansi_to_bstr(char *s);
extern OLECHAR *ansi_to_uni(char *s);
extern char *uni_to_ansi(OLECHAR *os);
extern void drvFail(char *msg, EXCEPINFO *ei, bool bFatal);
extern int CreateRegKeyStructure(HKEY hKey, const char *sPath);
