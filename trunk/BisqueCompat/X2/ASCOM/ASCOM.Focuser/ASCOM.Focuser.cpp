//========================================================================
//
// TITLE:		ASCOM.Focuser.cpp
//
// FACILITY:	X2 Plugin for TheSky X and ASCOM drivers
//
// ABSTRACT:	Implementation of the interfaces needed for the Bisque X2
//				focuser plugin API.
//
// ENVIRONMENT:	Microsoft Windows XP/Vista/7
//				Developed under Microsoft Visual C++ 9 (VS2008)
//
// AUTHOR:		Robert B. Denny
//
// Edit Log:
//
// When			Who		What
//----------	---		--------------------------------------------------
// 03-May-11	rbd		Initial edit
//========================================================================

#include "stdafx.h"

X2Focuser::X2Focuser(const char* pszDriverSelection,
				const int& nInstanceIndex,
				SerXInterface						* pSerX, 
				TheSkyXFacadeForDriversInterface	* pTheSkyX, 
				SleeperInterface					* pSleeper,
				BasicIniUtilInterface				* pIniUtil,
				LoggerInterface						* pLogger,
				MutexInterface						* pIOMutex,
				TickCountInterface					* pTickCount)
{
	m_nPrivateMultiInstanceIndex	= nInstanceIndex;
	m_pSerX							= pSerX;		
	m_pTheSkyXForDrivers			= pTheSkyX;
	m_pSleeper						= pSleeper;
	m_pIniUtil						= pIniUtil;
	m_pLogger						= pLogger;	
	m_pIOMutex						= pIOMutex;
	m_pTickCount					= pTickCount;

	m_szIniKey  = "ASCOM_FOCUSER";
	m_pszDriverInfoDetailedInfo		= "ASCOM focuser driver adapter for X2";
	m_dDriverInfoVersion			= 1.0;

	//
	// Because TheSky doesn't honor changes to X2 driver info and abstractions
	// after its initial call, we cache this stuff and read it out from the
	// 'ini' store here, on first call. This is not optimal because the info
	// is not correct immediately after a change to the selected ASCOM driver.
	// Maybe this will change in the future and we can remove this clap-trap.
	//
	m_pIniUtil->readString(m_szIniKey, "DeviceInfoNameShort", 
						"ASCOM_Focuser", 
						m_pszDeviceInfoNameShort, 255);
	m_pIniUtil->readString(m_szIniKey, "DeviceInfoNameLong", 
						"Any ASCOM-compliant focuser", 
						m_pszDeviceInfoNameLong, 255);
	m_pIniUtil->readString(m_szIniKey, "DeviceInfoDetailedDescription", 
						"Supports any focuser which has an ASCOM driver.", 
						m_pszDeviceInfoDetailedDescription, 255);
	m_pIniUtil->readString(m_szIniKey, "DeviceInfoFirmwareVersion", 
						"n/a", 
						m_pszDeviceInfoFirmwareVersion, 255);
	m_pIniUtil->readString(m_szIniKey, "DeviceInfoModel", 
						"n/a", 
						m_pszDeviceInfoModel, 255);
	//
	// Now the mount capabilities, which we also need before connecting
	// in order to return the appropriate set of abscraction interfaces.
	//
	//_bScopeCanSync = (m_pIniUtil->readInt(m_szIniKey, "MountCanSync", 0) == 1);
	//_bScopeIsGEM = (m_pIniUtil->readInt(m_szIniKey, "MountIsAsymEqu", 0) == 1);
	//_bScopeCanSetTracking = (m_pIniUtil->readInt(m_szIniKey, "MountCanTrackRates", 0) == 1);
	//_bScopeCanPark = (m_pIniUtil->readInt(m_szIniKey, "MountCanPark", 0) == 1);
	//_bScopeCanUnpark = (m_pIniUtil->readInt(m_szIniKey, "MountCanUnpark", 0) == 1);


	InitDrivers(m_pLogger);									// Initialize COM/ASCOM
}

//
// Cannot call TermScope here because TheSky has already wound
// way too far down, the GIT is gone, we can't make COM calls.
//
X2Focuser::~X2Focuser()
{
	// I doubt that these are needed for the same reason as above
	//if (GetSerX())
	//	delete GetSerX();
	//if (GetTheSkyXFacadeForDrivers())
	//	delete GetTheSkyXFacadeForDrivers();
	//if (GetSleeper())
	//	delete GetSleeper();
	//if (GetSimpleIniUtil())
	//	delete GetSimpleIniUtil();
	//if (GetLogger())
	//	delete GetLogger();
	//if (GetMutex())
	//	delete GetMutex();
	//if (GetTickCountInterface())
	//	delete GetTickCountInterface();
}

int	X2Focuser::queryAbstraction(const char* pszName, void** ppVal)
{
	*ppVal = NULL;

	//
	// TODO - This does get called after establishLink() but the additional
	// abstractions that I return once I know the scope aren't reflected
	// in UI changes in THeSky. THe abstractions must be returned when the
	// plugin is first initialized or they never show. Hance the elaborate
	// 'ini' storage and connect-after-config junk here. I hope this can be
	// eliminated some day.
	//
	if (!strcmp(pszName, FocuserGotoInterface2_Name))
		*ppVal = dynamic_cast<FocuserGotoInterface2*>(this);

	return SB_OK;
}

// ModalSettingsDialog

int X2Focuser::execModalSettingsDialog(void)
{
	char prev_driver[256];
	char buf[256];

	strcpy (prev_driver, _szDriverID);
	if (ConfigScope() == SB_OK)
	{
		if (_stricmp(prev_driver, _szDriverID) == 0)
			return SB_OK;
		//
		// TheSky/X2 does not dynamically adjust itself in response to changes
		// in names and mount capabilities after the mount connects. So we need
		// to cache this stuff in X2 "ini" storage then grab it back next time
		// so at least afger the first change the data is right. I hope this
		// changes and we can eliminate this kludge.
		//
		_hWndMain = GetActiveWindow();
		int ans = MessageBox(_hWndMain, 
"In order to complete the change, we must connect to your mount now. Make sure your mount is connected and powered up.", 
			"Get Mount Info", (MB_OKCANCEL + MB_ICONINFORMATION));
		//
		// If user decided not to do this, restore previous driver
		// (before the Chooser was used)
		//
		if(ans == IDCANCEL)
		{
			SaveDriverID(prev_driver);
			MessageBox(_hWndMain, "Canceled - The ASCOM driver selection has not changed", 
				"Get Mount Info", (MB_OK + MB_ICONEXCLAMATION));
			return SB_OK;
		}
		//
		// If failed to connect, we can't get info, so restore previous driver
		// (before the Chooser was used)
		//
		if (InitScope() == -1)
		{
			SaveDriverID(prev_driver);
			MessageBox(_hWndMain, "Failed to connect to mount - The ASCOM driver selection has not changed", 
				"Get Mount Info", (MB_OK + MB_ICONEXCLAMATION));
			return SB_OK;
		}
		//
		// OK now copy the info into the 'ini' area for later
		//
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoNameShort", _szScopeName);
		sprintf(buf, "ASCOM %s", _szScopeName);
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoNameLong", buf);
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoDetailedDescription", _szScopeDriverInfo);
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoModel", _szScopeDescription);
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoFirmwareVersion", _szScopeDriverVersion);
		m_pIniUtil->writeInt(m_szIniKey, "MountCanSync", (_bScopeCanSync ? 1 : 0));
		m_pIniUtil->writeInt(m_szIniKey, "MountIsAsymEqu", (_bScopeIsGEM ? 1 : 0));
		m_pIniUtil->writeInt(m_szIniKey, "MountCanTrackRates", (_bScopeCanSetTracking ? 1 : 0));
		m_pIniUtil->writeInt(m_szIniKey, "MountCanPark", (_bScopeCanPark ? 1 : 0));
		m_pIniUtil->writeInt(m_szIniKey, "MountCanUnpark", (_bScopeCanUnpark ? 1 : 0));

		TermScope(false);
		
		MessageBox(_hWndMain, "The Hardware and Driver information, as well as available mount controls, will not reflect this change until TheSky X is restarted.", 
			"Get Mount Info", (MB_OK + MB_ICONINFORMATION));
	}
	return SB_OK;
}

//LinkInterface

int	X2Focuser::establishLink(void)					
{
	char buf[256];

	int iRes = (int)InitScope();
	if (iRes == SB_OK)
	{
		//
		// Now that we're connected, refresh all of the descriptive info
		// and re-save to the INI store. This will catch changes in the 
		// ASCOM driver info without having to re-select it in the
		// execModelSettingsDialog() method above.
		//
		strcpy(m_pszDeviceInfoNameShort, _szScopeName);
		sprintf(buf, "ASCOM %s", _szScopeName);
		strcpy(m_pszDeviceInfoNameLong, buf);
		strcpy(m_pszDeviceInfoDetailedDescription, _szScopeDriverInfo);
		strcpy(m_pszDeviceInfoModel, _szScopeDescription);
		strcpy(m_pszDeviceInfoFirmwareVersion, _szScopeDriverVersion);

		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoNameShort", m_pszDeviceInfoNameShort);
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoNameLong", m_pszDeviceInfoNameLong);
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoDetailedDescription", m_pszDeviceInfoDetailedDescription);
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoModel", m_pszDeviceInfoModel);
		m_pIniUtil->writeString(m_szIniKey,"DeviceInfoFirmwareVersion", m_pszDeviceInfoFirmwareVersion);
		m_pIniUtil->writeInt(m_szIniKey, "MountCanSync", (_bScopeCanSync ? 1 : 0));
		m_pIniUtil->writeInt(m_szIniKey, "MountIsAsymEqu", (_bScopeIsGEM ? 1 : 0));
		m_pIniUtil->writeInt(m_szIniKey, "MountCanTrackRates", (_bScopeCanSetTracking ? 1 : 0));
		m_pIniUtil->writeInt(m_szIniKey, "MountCanPark", (_bScopeCanPark ? 1 : 0));
		m_pIniUtil->writeInt(m_szIniKey, "MountCanUnpark", (_bScopeCanUnpark ? 1 : 0));
	}
	return iRes;
}

int	X2Focuser::terminateLink(void)						
{
	TermScope(false);
	return SB_OK;
}

bool X2Focuser::isLinked(void) const					
{
	return _bScopeActive;
}

bool X2Focuser::isEstablishLinkAbortable(void) const	{return false;}

//DriverInfoInterface

void	X2Focuser::driverInfoDetailedInfo(BasicStringInterface& str) const
{
	str = m_pszDriverInfoDetailedInfo;
}

double	X2Focuser::driverInfoVersion(void) const				
{
	return m_dDriverInfoVersion;
}

//HardwareInfoInterface
// Retrieved when the Telescope Setup window opens, and after changing X2 drivers.

void X2Focuser::deviceInfoNameShort(BasicStringInterface& str) const				
{
	str = m_pszDeviceInfoNameShort;
}

void X2Focuser::deviceInfoNameLong(BasicStringInterface& str) const				
{
	str = m_pszDeviceInfoNameLong;
}

void X2Focuser::deviceInfoDetailedDescription(BasicStringInterface& str) const	
{
	str = m_pszDeviceInfoDetailedDescription;
}

void X2Focuser::deviceInfoFirmwareVersion(BasicStringInterface& str)		
{
	str = m_pszDeviceInfoFirmwareVersion;
}

void X2Focuser::deviceInfoModel(BasicStringInterface& str)				
{
	str = m_pszDeviceInfoModel;
}

