//========================================================================
//
// TITLE:		ASCOM.Telescope.cpp
//
// FACILITY:	X2 Plugin for TheSky X and ASCOM drivers
//
// ABSTRACT:	Implementation of the interfaces needed for the Bisque X2
//				mount plugin API.
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
// 22-Apr-11	rbd		Initial edit
// 27-Apr-11	rbd		Many changes over the last few days, still fluid.
// 03-May-11	rbd		Fix m_pszDriverInfoDetailedInfo. Add deletion of 
//						callback interface pointers.
//========================================================================

#include "StdAfx.h"

// TODO - How to get Communications Log item to show in Telescope/Tools menu?
//        When this is solved, log ASCOM calls.
// TODO - Why doesn't LinkFromUIThreadInterface remove the need for the 
//        CROSS_THREAD code?


X2Mount::X2Mount(const char* pszDriverSelection,
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

	m_szIniKey  = "ASCOM_MOUNT";
	m_pszDriverInfoDetailedInfo		= "ASCOM telescope driver adapter for X2";
	m_dDriverInfoVersion			= 1.0;

	//
	// Because TheSky doesn't honor changes to X2 driver info and abstractions
	// after its initial call, we cache this stuff and read it out from the
	// 'ini' store here, on first call. This is not optimal because the info
	// is not correct immediately after a change to the selected ASCOM driver.
	// Maybe this will change in the future and we can remove this clap-trap.
	//
	m_pIniUtil->readString(m_szIniKey, "DeviceInfoNameShort", 
						"ASCOM_Mount", 
						m_pszDeviceInfoNameShort, 255);
	m_pIniUtil->readString(m_szIniKey, "DeviceInfoNameLong", 
						"Any ASCOM-compliant mount", 
						m_pszDeviceInfoNameLong, 255);
	m_pIniUtil->readString(m_szIniKey, "DeviceInfoDetailedDescription", 
						"Supports any mount which has an ASCOM driver.", 
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
	_bScopeCanSync = (m_pIniUtil->readInt(m_szIniKey, "MountCanSync", 0) == 1);
	_bScopeIsGEM = (m_pIniUtil->readInt(m_szIniKey, "MountIsAsymEqu", 0) == 1);
	_bScopeCanSetTracking = (m_pIniUtil->readInt(m_szIniKey, "MountCanTrackRates", 0) == 1);
	_bScopeCanPark = (m_pIniUtil->readInt(m_szIniKey, "MountCanPark", 0) == 1);
	_bScopeCanUnpark = (m_pIniUtil->readInt(m_szIniKey, "MountCanUnpark", 0) == 1);

	InitDrivers(m_pLogger);									// Initialize COM/ASCOM
}

//
// Cannot call TermScope here because TheSky has already wound
// way too far down, the GIT is gone, we can't make COM calls.
//
X2Mount::~X2Mount()
{
	TermDrivers();
	//if (GetSerX())
	//	delete GetSerX();
	if (GetTheSkyXFacadeForDrivers())
		delete GetTheSkyXFacadeForDrivers();
	if (GetSleeper())
		delete GetSleeper();
	if (GetSimpleIniUtil())
		delete GetSimpleIniUtil();
	if (GetLogger())
		delete GetLogger();
	//if (GetMutex())
	//	delete GetMutex();
	//if (GetTickCountInterface())
	//	delete GetTickCountInterface();
}

int	X2Mount::queryAbstraction(const char* pszName, void** ppVal)
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
	if (!strcmp(pszName, SyncMountInterface_Name) && _bScopeCanSync)
		*ppVal = dynamic_cast<SyncMountInterface*>(this);
	else if (!strcmp(pszName, SlewToInterface_Name))
		*ppVal = dynamic_cast<SlewToInterface*>(this);
	else if (!strcmp(pszName, NeedsRefractionInterface_Name))
		*ppVal = dynamic_cast<NeedsRefractionInterface*>(this);
	else if (!strcmp(pszName, TrackingRatesInterface_Name) && _bScopeCanSetTracking)
		*ppVal = dynamic_cast<TrackingRatesInterface*>(this);
	else if (!strcmp(pszName, ModalSettingsDialogInterface_Name))
		*ppVal = dynamic_cast<ModalSettingsDialogInterface*>(this);
	else if (!strcmp(pszName, AsymmetricalEquatorialInterface_Name) && _bScopeIsGEM)
		*ppVal = dynamic_cast<AsymmetricalEquatorialInterface*>(this);
	else if (!strcmp(pszName, ParkInterface_Name) && _bScopeCanPark)
		*ppVal = dynamic_cast<ParkInterface*>(this);
	else if (!strcmp(pszName, UnparkInterface_Name) && _bScopeCanUnpark)
		*ppVal = dynamic_cast<UnparkInterface*>(this);
	else if (!strcmp(pszName, LinkFromUIThreadInterface_Name))
		*ppVal = dynamic_cast<LinkFromUIThreadInterface*>(this);

	return SB_OK;
}

// ModalSettingsDialog

int X2Mount::execModalSettingsDialog(void)
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

int	X2Mount::establishLink(void)					
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

int	X2Mount::terminateLink(void)						
{
	TermScope(false);
	return SB_OK;
}

bool X2Mount::isLinked(void) const					
{
	return _bScopeActive;
}

bool X2Mount::isEstablishLinkAbortable(void) const	{return false;}

//DriverInfoInterface

void	X2Mount::driverInfoDetailedInfo(BasicStringInterface& str) const
{
	str = m_pszDriverInfoDetailedInfo;
}

double	X2Mount::driverInfoVersion(void) const				
{
	return m_dDriverInfoVersion;
}

//HardwareInfoInterface
// Retrieved when the Telescope Setup window opens, and after changing X2 drivers.

void X2Mount::deviceInfoNameShort(BasicStringInterface& str) const				
{
	str = m_pszDeviceInfoNameShort;
}

void X2Mount::deviceInfoNameLong(BasicStringInterface& str) const				
{
	str = m_pszDeviceInfoNameLong;
}

void X2Mount::deviceInfoDetailedDescription(BasicStringInterface& str) const	
{
	str = m_pszDeviceInfoDetailedDescription;
}

void X2Mount::deviceInfoFirmwareVersion(BasicStringInterface& str)		
{
	str = m_pszDeviceInfoFirmwareVersion;
}

void X2Mount::deviceInfoModel(BasicStringInterface& str)				
{
	str = m_pszDeviceInfoModel;
}

//Common Mount specifics

int	X2Mount::raDec(double& ra, double& dec, const bool& bCached)
{
	int iRes = SB_OK;

	if(!_bScopeActive)	{								// No scope hookup?
		ra = dec = 0.0;			
		return(ERR_COMMNOLINK);							// Forget this
	}

	__try {
		ra = GetRightAscension();
		dec = GetDeclination();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

int	X2Mount::abort(void)												
{
	int iRes = SB_OK;								// Assume success

	if(!_bScopeActive)								// No scope hookup?
		return(ERR_COMMNOLINK);						// Forget this
	if(isParked())									// Illegal op when parked
		return SB_OK;								// (happens quick-closing TheSky while parked and connected)

	__try {
		AbortSlew();
		SetForegroundWindow(_hWndMain);				// Bring TheSky to front
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

//SyncMountInterface

int X2Mount::syncMount(const double& ra, const double& dec)
{
	int iRes = SB_OK;

	if(!_bScopeActive)									// No scope hookup?
		return(ERR_COMMNOLINK);							// Forget this
	if(!_bScopeCanSync)
		return(ERR_NOT_IMPL);

	__try {
		SyncScope(ra, dec);
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

bool X2Mount::isSynced()
{
	return true;									// No way to tell, so just return true per docs
}

//SlewToInterface

/*!Initiate the slew.*/
int X2Mount::startSlewTo(const double& ra, const double& dec)
{
	int iRes = SB_OK;

	if(!_bScopeActive)								// No scope hookup?
		return(ERR_COMMNOLINK);						// Forget this
	if(!_bScopeCanSlew && !_bScopeCanSlewAsync)
		return(ERR_NOT_IMPL);

	if(_bScopeCanSetTracking && !GetTracking())		// Prevent "Wrong tracking state"
		SetTracking(true);

	_hWndMain = GetActiveWindow();					// For later restore
	__try {
		SlewScope(ra, dec);							// Smart about sync/async capabilities
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

/*!Called to monitor the slew process. \param bComplete Set to true if the slew is complete, otherwise return false.*/
int X2Mount::isCompleteSlewTo(bool& bComplete) const
{
	int iRes = SB_OK;								// Assume success

	if(!_bScopeActive)								// No scope hookup?
		return(ERR_COMMNOLINK);						// Forget this

	__try {
		if(!_bScopeCanSlew && !_bScopeCanSlewAsync)
			bComplete = true;
		else
			bComplete = !IsSlewing();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

/*!Called once the slew is complete. This is called once for every corresponding startSlewTo() allowing software implementations of gotos.*/
int X2Mount::endSlewTo(void)
{
	return SB_OK;
}

//TrackingRatesInterface

/*!Set the tracking rates.*/
int X2Mount::setTrackingRates( const bool& bTrackingOn, const bool& bIgnoreRates, const double& dRaRateArcSecPerSec, const double& dDecRateArcSecPerSec)
{
	int iRes = SB_OK;								// Assume success

	if(!_bScopeActive)								// No scope hookup?
		return(ERR_COMMNOLINK);						// Forget this
	if(!_bScopeCanSetTracking)
		return(ERR_NOT_IMPL);

	// Assuming that if it can set rates, can turn tracking on and off for sure
	__try {
		SetTracking(bTrackingOn);
		//
		// This is a hack for TheSky X. If the rates are 0,0 and yet bIgnoreRates
		// is true, still set the 0,0 rates. We do this because choosing No for 
		// the set rate doesn't even call us. The only way to get rid of an offset 
		// is to choose a star and select SetTracking Rates for 0,0 and then click
		// Yes.
		//
		if (!bIgnoreRates || (dRaRateArcSecPerSec == 0.0 && dDecRateArcSecPerSec == 0.0))
		{
			if(_bScopeCanSetTrackRates)
			{
				SetRightAscensionRate((dRaRateArcSecPerSec * SIDRATE) / 15.0);	// Arc sec to time sec
				SetDeclinationRate(dDecRateArcSecPerSec);
			}
			else if (!bIgnoreRates)
			{
				//
				// This is needed because TSX's TrackingRatesInterface must be implemented just for 
				// tracking on-off control. Thus, the Set Track Rates button will appear. If the mount 
				// supports tracking on-off but not offsets, we have to catch clicks on Set Track Rates 
				// and return the SB ERR_NOT_IMPL error.
				//
				iRes = ERR_NOT_IMPL;
			}
		}

	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

/*!Return the current tracking rates.  A special case for mounts that can set rates, but not read them...
   So the TheSkyX's user interface can know this, set bTrackSidereal=false and both rates to -1000.0*/
int X2Mount::trackingRates( bool& bTrackingOn, double& dRaRateArcSecPerSec, double& dDecRateArcSecPerSec)
{
	int iRes = SB_OK;								// Assume success

	if(!_bScopeActive)								// No scope hookup?
		return(ERR_COMMNOLINK);						// Forget this

	bTrackingOn = true;
	dRaRateArcSecPerSec = dDecRateArcSecPerSec = 0.0;

	__try {
		if(_bScopeCanSetTracking)
		{
			bTrackingOn = GetTracking();
			if(_bScopeCanSetTrackRates)
			{
				dRaRateArcSecPerSec = GetRightAscensionRate() / SIDRATE;	// Convert to SidSec/UTCSec
				dDecRateArcSecPerSec = GetDeclinationRate();
			}
		}
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

//ParkInterface

/*!Return true if the device is parked.*/
//WTF Why this and isCompletePark?
bool X2Mount::isParked(void)
{
	if(_bScopeActive && _bScopeCanPark)
		return GetAtPark();
	else
		return false;
}

/*!Initiate the park process. */
int X2Mount::startPark(const double& az, const double& alt)
{
	int iRes = SB_OK;

	if(!_bScopeActive)									// No scope hookup?
		return(ERR_COMMNOLINK);							// Forget this
	if(!_bScopeCanPark)
		return(ERR_NOT_IMPL);
	
	if(_bScopeCanSetTracking && !GetTracking())			// Prevent "Wrong tracking state"
		SetTracking(true);

	_hWndMain = GetActiveWindow();						// For later restore
	__try {
		// NB: TheSky first slews the scope to the "set park" position, 
		//     then it calls this. SO this time through, there will be
		//     a "small" slew. See below, we really should do the direct
		//     alt/az slew if we can anyway!
		if(_bScopeCanSetPark)
		{
			double ra, dec;
			// TODO - Do it right for alt/az slew capable scopes.
			m_pTheSkyXForDrivers->HzToEq(az, alt, ra, dec);
			startSlewTo(ra, dec);
			while(true)
			{
				bool fini;
				if(!isCompleteSlewTo(fini)) break;
				if(fini) break;
				m_pSleeper->sleep(200);
			}
			m_pSleeper->sleep(200);
			SetParkScope();
			m_pSleeper->sleep(200);
		}
		ParkScope();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

/*!Called to monitor the park process.  \param bComplete Set to true if the park is complete, otherwise set to false.*/
int X2Mount::isCompletePark(bool& bComplete) const
{
	int iRes = SB_OK;								// Assume success

	if(!_bScopeActive)								// No scope hookup?
		return(ERR_COMMNOLINK);						// Forget this

	__try {
		if(!_bScopeCanPark)
			bComplete = true;
		else
			bComplete = GetAtPark();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

/*!Called once the park is complete.  This is called once for every corresponding startPark() allowing software implementations of park.*/
int X2Mount::endPark(void)
{
	return SB_OK;
}

// UnparkInterface

/*!Initiate the unpark process. */
int X2Mount::startUnpark(void)
{
	int iRes = SB_OK;

	if(!_bScopeActive)									// No scope hookup?
		return(ERR_COMMNOLINK);							// Forget this
	if(!_bScopeCanUnpark)
		return(ERR_NOT_IMPL);

	_hWndMain = GetActiveWindow();						// For later restore
	__try {
		UnparkScope();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

/*!Called to monitor the unpark process.  \param bComplete Set to true if the unpark is complete, otherwise set to false.*/
int X2Mount::isCompleteUnpark(bool& bComplete) const
{
	int iRes = SB_OK;								// Assume success

	if(!_bScopeActive)								// No scope hookup?
		return(ERR_COMMNOLINK);						// Forget this

	__try {
		if(!_bScopeCanUnpark)
			bComplete = true;
		else
			bComplete = !GetAtPark();
	} __except(EXCEPTION_EXECUTE_HANDLER) {
		iRes = ERR_COMMNOLINK;
	}
	return iRes;
}

/*!Called once the unpark is complete.  This is called once for every corresponding startUnpark() allowing software implementations of unpark.*/
int X2Mount::endUnpark(void)
{
	//
	// For some reason this is absolutely required for mounts that have 
	// tracking control. Without it, X2 gets stuck in Unparking.
	//
	if (_bScopeCanSetTracking)
		return setTrackingRates(true, true, 0.0, 0.0);
	else
		return SB_OK;
}

//NeedsRefractionInterface

bool X2Mount::needsRefactionAdjustments(void)
{
	return !_bScopeDoesRefraction;
}

//AsymmetricalMountInterface

/*!
If knowsBeyondThePole() returns false, the mount
cannot distinguish unambiguosly if the OTA end of the declination axis 
is either east or west of the pier. This somewhat restricts use of the 
mount with TPoint - the mount must always have the OTA end of the declination 
axis higher than the counterweights. In other words, the mount should not slew past the meridian.
*/
bool X2Mount::knowsBeyondThePole()
{
	return (GetCanPierSide());
}

/*!
If knowsBeyondThePole() returns true,
then beyondThePole() tells TheSkyX unambiguously 
if the OTA end of the declination axis 
is either east (0) or west of the pier (1).
Note, the return value must be correct even
for cases where the OTA end of the Dec axis 
is lower than the counterweights.
*/
int X2Mount::beyondThePole(bool& bYes)
{
	if(!_bScopeActive)									// No scope hookup?
		return(ERR_COMMNOLINK);							// Forget this
	if (!GetCanPierSide())
		return(ERR_NOT_IMPL);							// Safety valve, should not happen

	bYes = (IsPierWest());
	return 0;
}

/*!
Return the hour angle at which the mount automatically flips.
*/
double X2Mount::flipHourAngle()
{
	return 0;
}

/*!
Return the east and west hour angle limits.
*/
int X2Mount::gemLimits(double& dHoursEast, double& dHoursWest)
{
	dHoursEast = dHoursWest = 0;
	return 0;
}
