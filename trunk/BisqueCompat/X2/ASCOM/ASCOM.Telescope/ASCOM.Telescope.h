
#pragma once
#include "../../licensedinterfaces/mountdriverinterface.h"
#include "../../licensedinterfaces/mount/slewtointerface.h"
#include "../../licensedinterfaces/mount/syncmountinterface.h"
#include "../../licensedinterfaces/mount/trackingratesinterface.h"

// Forward declare the interfaces that the this driver is "given" by TheSkyX
class SerXInterface;
class TheSkyXFacadeForDriversInterface;
class SleeperInterface;
class BasicIniUtilInterface;
class LoggerInterface;
class MutexInterface;
class AbstractSyncMount;
class TickCountInterface;

/*!
\brief The X2Mount example.

\ingroup Example

Use this example to write an X2Mount driver.
*/
class X2Mount : public MountDriverInterface,
						public SyncMountInterface,
						public SlewToInterface,
						public TrackingRatesInterface 
{
public:
	/*!Standard X2 constructor*/
	X2Mount(const char* pszDriverSelection,
				const int& nInstanceIndex,
				SerXInterface						* pSerX, 
				TheSkyXFacadeForDriversInterface	* pTheSkyX, 
				SleeperInterface					* pSleeper,
				BasicIniUtilInterface				* pIniUtil,
				LoggerInterface						* pLogger,
				MutexInterface						* pIOMutex,
				TickCountInterface					* pTickCount);

	~X2Mount();

// Operations
public:

	/*!\name DriverRootInterface Implementation
	See DriverRootInterface.*/
	//@{ 
	virtual DeviceType							deviceType(void) { return DriverRootInterface::DT_MOUNT; }
	virtual int									queryAbstraction(const char* pszName, void** ppVal);
	//@} 

	/*!\name LinkInterface Implementation
	See LinkInterface.*/
	//@{ 
	virtual int									establishLink(void);
	virtual int									terminateLink(void);
	virtual bool								isLinked(void) const;
	virtual bool								isEstablishLinkAbortable(void) const;
	//@} 

	/*!\name DriverInfoInterface Implementation
	See DriverInfoInterface.*/
	//@{ 
	virtual void								driverInfoDetailedInfo(BasicStringInterface& str) const;
	virtual double								driverInfoVersion(void) const;
	//@} 

	/*!\name HardwareInfoInterface Implementation
	See HardwareInfoInterface.*/
	//@{ 
	virtual void								deviceInfoNameShort(BasicStringInterface& str) const;
	virtual void								deviceInfoNameLong(BasicStringInterface& str) const;
	virtual void								deviceInfoDetailedDescription(BasicStringInterface& str) const;
	virtual void								deviceInfoFirmwareVersion(BasicStringInterface& str);
	virtual void								deviceInfoModel(BasicStringInterface& str);
	//@} 

	virtual int									raDec(double& ra, double& dec, const bool& bCached = false);
	virtual int									abort(void);

	//Optional interfaces, uncomment and implement as required.

	//SyncMountInterface
	virtual int									syncMount(const double& ra, const double& dec);
	virtual bool								isSynced();

	//SlewToInterface
	virtual int									startSlewTo(const double& dRa, const double& dDec);
	virtual int									isCompleteSlewTo(bool& bComplete) const;
	virtual int									endSlewTo(void);
	
	//NeedsRefractionInterface
	//virtual bool							needsRefactionAdjustments(void);

	//TrackingRatesInterface 
	virtual int setTrackingRates( const bool& bTrackingOn, const bool& bIgnoreRates, const double& dRaRateArcSecPerSec, const double& dDecRateArcSecPerSec);
	virtual int trackingRates( bool& bTrackingOn, double& dRaRateArcSecPerSec, double& dDecRateArcSecPerSec);

// Implementation
private:	

	SerXInterface 								*GetSerX() {return m_pSerX; }		
	TheSkyXFacadeForDriversInterface			*GetTheSkyXFacadeForMounts() {return m_pTheSkyXForMounts;}
	SleeperInterface							*GetSleeper() {return m_pSleeper; }
	BasicIniUtilInterface						*GetSimpleIniUtil() {return m_pIniUtil; }
	LoggerInterface								*GetLogger() {return m_pLogger; }
	MutexInterface								*GetMutex()  {return m_pIOMutex;}
	TickCountInterface							*GetTickCountInterface() {return m_pTickCount;}
	

	int m_nPrivateMultiInstanceIndex;
	SerXInterface*								m_pSerX;		
	TheSkyXFacadeForDriversInterface* 			m_pTheSkyXForMounts;
	SleeperInterface*							m_pSleeper;
	BasicIniUtilInterface*						m_pIniUtil;
	LoggerInterface*							m_pLogger;
	MutexInterface*								m_pIOMutex;
	TickCountInterface*							m_pTickCount;
};


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
