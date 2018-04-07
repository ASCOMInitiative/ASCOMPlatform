#pragma once
#include "../../licensedinterfaces/sberrorx.h"
#include "../../licensedinterfaces/basicstringinterface.h"
#include "../../licensedinterfaces/basiciniutilinterface.h"
//#include "../../licensedinterfaces/driverinfointerface.h"
//#include "../../licensedinterfaces/deviceinfointerface.h"
#include "../../licensedinterfaces/focuserdriverinterface.h"
#include "../../licensedinterfaces/loggerinterface.h"
#include "../../licensedinterfaces/modalsettingsdialoginterface.h"
#include "../../licensedinterfaces/theskyxfacadefordriversinterface.h"
#include "../../licensedinterfaces/sleeperinterface.h"
#include "../../licensedinterfaces/focuser/focusergotointerface2.h"

// Forward declare the interfaces that the this driver is "given" by TheSkyX
class SerXInterface;
class TheSkyXFacadeForDriversInterface;
class SleeperInterface;
class BasicIniUtilInterface;
class LoggerInterface;
class MutexInterface;
class AbstractSyncMount;
class TickCountInterface;

class X2Focuser : public FocuserDriverInterface,
						//public HardwareInfoInterface,
						//public DriverInfoInterface,
						public ModalSettingsDialogInterface,
						public FocuserGotoInterface2
{
public:
	/*!Standard X2 constructor*/
	X2Focuser(const char* pszDriverSelection,
				const int& nInstanceIndex,
				SerXInterface						* pSerX, 
				TheSkyXFacadeForDriversInterface	* pTheSkyX, 
				SleeperInterface					* pSleeper,
				BasicIniUtilInterface				* pIniUtil,
				LoggerInterface						* pLogger,
				MutexInterface						* pIOMutex,
				TickCountInterface					* pTickCount);

	~X2Focuser();

// Operations
public:

	//DriverRootInterface
	virtual DeviceType							deviceType(void) { return DriverRootInterface::DT_FOCUSER; }
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

	//ModalSettingsDialogInterface
	virtual int									initModalSettingsDialog(void){return 0;}
	virtual int									execModalSettingsDialog(void);

	//FocuserGotoInterface2
	/*! Return the position of the focuser.  If the hardware doesn't have a digital read out, return a number that roughly corresponds to whatever units the focuser moves in (time, lenght, etc.)*/
	virtual int									focPosition(int& nPosition);
	/*! Return the focusers minimum limit.*/
	virtual int									focMinimumLimit(int& nMinLimit);
	/*! Return the focusers maximum limit.*/
	virtual int									focMaximumLimit(int& nMaxLimit);
	/*! Abort an operation in progress.*/
	virtual int									focAbort();

	/*! Initiate the focus goto operation.*/
	virtual int									startFocGoto(const int& nRelativeOffset);
	/*! Return if the goto is complete.*/
	virtual int									isCompleteFocGoto(bool& bComplete) const;
	/*! Called after the goto is complete.  This is called once for every corresponding startFocGoto() allowing software implementations of focuser gotos.*/
	virtual int									endFocGoto(void);

	/*! Return the number (count) of avaiable focuser gotos.*/
	virtual int									amountCountFocGoto(void) const;
	/*! Return a string along with the amount or size of the corresponding focuser goto.*/
	virtual int									amountNameFromIndexFocGoto(const int& nZeroBasedIndex, BasicStringInterface& strDisplayName, int& nAmount);
	/*! Return the current index of focuser goto selection. */
	virtual int									amountIndexFocGoto(void);

	/*! Coming soon to TheSkyX, a mount having an embedded focuser, via x2. */
	virtual void								embeddedFocuserInit(const char* psFilterWheelSelection);

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
extern char *_szFocuserName;									// These need to be delete[]ed
extern char *_szFocuserDescription;
extern char *_szFocuserDriverInfo;
extern char *_szFocuserDriverVersion;
extern char _szDriverID[256];
extern bool _bAbsolute;


extern bool InitDrivers(LoggerInterface *pLogger);
extern short InitFocuser(void);
extern void TermFocuser(bool);
extern short ConfigFocuser();
extern int GetPosition(void);
extern bool GetIsMoving();
extern void Move(int Value);
extern void Halt(void);
extern void SaveDriverID(char *id);

// -------------
// Utilities.cpp
// -------------

extern BSTR ansi_to_bstr(char *s);
extern OLECHAR *ansi_to_uni(char *s);
extern char *uni_to_ansi(OLECHAR *os);
extern void drvFail(char *msg, EXCEPINFO *ei, bool bFatal);
extern int CreateRegKeyStructure(HKEY hKey, const char *sPath);
