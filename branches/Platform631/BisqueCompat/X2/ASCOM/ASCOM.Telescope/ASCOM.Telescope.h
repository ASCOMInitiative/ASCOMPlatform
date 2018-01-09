
#pragma once
#include "../../licensedinterfaces/sberrorx.h"
#include "../../licensedinterfaces/basicstringinterface.h"
#include "../../licensedinterfaces/basiciniutilinterface.h"
#include "../../licensedinterfaces/driverinfointerface.h"
#include "../../licensedinterfaces/deviceinfointerface.h"
#include "../../licensedinterfaces/mountdriverinterface.h"
#include "../../licensedinterfaces/loggerinterface.h"
#include "../../licensedinterfaces/modalsettingsdialoginterface.h"
#include "../../licensedinterfaces/theskyxfacadefordriversinterface.h"
#include "../../licensedinterfaces/sleeperinterface.h"
#include "../../licensedinterfaces/parkinterface.h"
#include "../../licensedinterfaces/unparkinterface.h"
#include "../../licensedinterfaces/mount/slewtointerface.h"
#include "../../licensedinterfaces/mount/syncmountinterface.h"
#include "../../licensedinterfaces/mount/needsrefractioninterface.h"
#include "../../licensedinterfaces/mount/trackingratesinterface.h"
#include "../../licensedinterfaces/mount/AsymmetricalEquatorialInterface.h"
#include "../../licensedinterfaces/mount/linkfromuithreadinterface.h"

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
						//These belong to MountDriverInterface
						//public HardwareInfoInterface,
						//public DriverInfoInterface,
						//public NeedsRefractionInterface,
						public ModalSettingsDialogInterface,
						public AsymmetricalEquatorialInterface,
						public SyncMountInterface,
						public SlewToInterface,
						public TrackingRatesInterface,
						public ParkInterface,
						public UnparkInterface,
						public LinkFromUIThreadInterface
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

	//DriverRootInterface
	virtual DeviceType							deviceType(void) { return DriverRootInterface::DT_MOUNT; }
	virtual int									queryAbstraction(const char* pszName, void** ppVal);

	//LinkInterface
	virtual int									establishLink(void);
	virtual int									terminateLink(void);
	virtual bool								isLinked(void) const;
	virtual bool								isEstablishLinkAbortable(void) const;

	//DriverInfoInterface
	virtual void								driverInfoDetailedInfo(BasicStringInterface& str) const;
	virtual double								driverInfoVersion(void) const;

	//HardwareInfoInterface
	virtual void								deviceInfoNameShort(BasicStringInterface& str) const;
	virtual void								deviceInfoNameLong(BasicStringInterface& str) const;
	virtual void								deviceInfoDetailedDescription(BasicStringInterface& str) const;
	virtual void								deviceInfoFirmwareVersion(BasicStringInterface& str);
	virtual void								deviceInfoModel(BasicStringInterface& str);

	virtual int									raDec(double& ra, double& dec, const bool& bCached = false);
	virtual int									abort(void);

	//SyncMountInterface
	virtual int									syncMount(const double& ra, const double& dec);
	virtual bool								isSynced();

	//AsymmetricalMountInterface
	virtual bool								knowsBeyondThePole();
	virtual int									beyondThePole(bool& bYes);
	virtual double								flipHourAngle();
	virtual int									gemLimits(double& dHoursEast, double& dHoursWest);

	//SlewToInterface
	virtual int									startSlewTo(const double& dRa, const double& dDec);
	virtual int									isCompleteSlewTo(bool& bComplete) const;
	virtual int									endSlewTo(void);
	
	//NeedsRefractionInterface
	virtual bool								needsRefactionAdjustments(void);

	//TrackingRatesInterface 
	virtual int setTrackingRates( const bool& bTrackingOn, const bool& bIgnoreRates, const double& dRaRateArcSecPerSec, const double& dDecRateArcSecPerSec);
	virtual int trackingRates( bool& bTrackingOn, double& dRaRateArcSecPerSec, double& dDecRateArcSecPerSec);

	//ParkInterface
	virtual bool								isParked(void);
	virtual int									startPark(const double& dAz, const double& dAlt);
	virtual int									isCompletePark(bool& bComplete) const;
	virtual int									endPark(void);

	//UnparkInterface
	virtual int									startUnpark(void);
	virtual int									isCompleteUnpark(bool& bComplete) const;
	virtual int									endUnpark(void);

	//ModalSettingsDialogInterface
	virtual int									initModalSettingsDialog(void){return 0;}
	virtual int									execModalSettingsDialog(void);

// Implementation
private:	

	SerXInterface*								m_pSerX;		
	TheSkyXFacadeForDriversInterface* 			m_pTheSkyXForDrivers;
	SleeperInterface*							m_pSleeper;
	BasicIniUtilInterface*						m_pIniUtil;
	LoggerInterface*							m_pLogger;
	MutexInterface*								m_pIOMutex;
	TickCountInterface*							m_pTickCount;

	SerXInterface 								*GetSerX() {return m_pSerX; }		
	TheSkyXFacadeForDriversInterface			*GetTheSkyXFacadeForDrivers() {return m_pTheSkyXForDrivers;}
	SleeperInterface							*GetSleeper() {return m_pSleeper; }
	BasicIniUtilInterface						*GetSimpleIniUtil() {return m_pIniUtil; }
	LoggerInterface								*GetLogger() {return m_pLogger; }
	MutexInterface								*GetMutex()  {return m_pIOMutex;}
	TickCountInterface							*GetTickCountInterface() {return m_pTickCount;}

	int m_nPrivateMultiInstanceIndex;
	char *m_szIniKey;

	char *m_pszDriverInfoDetailedInfo;			// About THIS (X2) driver
	double m_dDriverInfoVersion;

	char m_pszDeviceInfoNameShort[256];			// Dynamic values
	char m_pszDeviceInfoNameLong[256];
	char m_pszDeviceInfoDetailedDescription[256];
	char m_pszDeviceInfoFirmwareVersion[256];
	char m_pszDeviceInfoModel[256];

	HWND _hWndMain;
};

#define SIDRATE 0.9972695677								// UTC seconds per sidereal second
#define EXCEP_ABORT 0xE100000F
#define ABORT RaiseException(EXCEP_ABORT, 0, NULL, NULL)
#define EXCEP_NOTIMPL 0x80040400							// Drivers must raise this code!
#define NOTIMPL RaiseException(EXCEP_NOTIMPL, 0, NULL, NULL)

// -------------------
// DriverInterface.cpp
// -------------------

extern bool _bScopeActive;
extern bool _bDoingInit;
extern const char *_szAlertTitle;
extern HWND _hWndMain;
extern LoggerInterface *_pLogger;
extern char *_szScopeName;									// These need to be delete[]ed
extern char *_szScopeDescription;
extern char *_szScopeDriverInfo;
extern char *_szScopeDriverVersion;
extern char _szDriverID[256];
extern int  _iScopeInterfaceVersion;
extern bool _bScopeHasEqu;
extern bool _bScopeCanSlew;
extern bool _bScopeCanSlewAsync;
extern bool _bScopeCanSlewAltAz;
extern bool _bScopeCanSync;
extern bool _bScopeIsGEM;
extern bool _bScopeCanSideOfPier;
extern bool _bScopeCanSetTracking;
extern bool _bScopeCanSetTrackRates;
extern bool _bScopeCanPark;
extern bool _bScopeCanUnpark;
extern bool _bScopeCanSetPark;
extern bool _bScopeDoesRefraction;
extern bool _bScopeCanSideOfPier;

extern bool InitDrivers(LoggerInterface *pLogger);
extern void TermDrivers(void);
extern short InitScope(void);
extern void TermScope(bool);
extern short ConfigScope();
extern bool GetCanPierSide(void);
extern bool GetAtPark(void);
extern double GetRightAscension(void);
extern double GetRightAscensionRate(void);
extern double GetDeclination(void);
extern double GetDeclinationRate(void);
extern bool GetTracking(void);
extern void SetTracking(bool state);
extern void SetLatitude(double lat);
extern void SetLongitude(double lng);
extern void SetRightAscensionRate(double rate);
extern void SetDeclinationRate(double rate);
extern bool IsPierWest(void);
extern bool IsSlewing(void);
extern void SlewScope(double dRA, double dDec);
extern void AbortSlew(void);
extern void SyncScope(double dRA, double dDec);
extern void ParkScope(void);
extern void UnparkScope(void);
extern void SetParkScope(void);
extern void SaveDriverID(char *id);

// -------------
// Utilities.cpp
// -------------

extern BSTR ansi_to_bstr(char *s);
extern OLECHAR *ansi_to_uni(char *s);
extern char *uni_to_ansi(OLECHAR *os);
extern void drvFail(char *msg, EXCEPINFO *ei, bool bFatal);
extern int CreateRegKeyStructure(HKEY hKey, const char *sPath);
