' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities
Imports ASCOM.Astrometry.AstroUtils

Class DeviceTelescope
    Implements ITelescopeV3
    Private utilities As New Util()
    Private astroUtilities As New AstroUtils()
    Private TL As New TraceLogger()

#Region "ITelescope Implementation"

	''' <summary>
	''' Stops a slew in progress.
	''' </summary>
	Public Sub AbortSlew() Implements ITelescopeV3.AbortSlew
		TL.LogMessage("AbortSlew", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("AbortSlew")
	End Sub

	''' <summary>
	''' The alignment mode of the mount (Alt/Az, Polar, German Polar).
	''' </summary>
	Public ReadOnly Property AlignmentMode() As AlignmentModes Implements ITelescopeV3.AlignmentMode
		Get
			TL.LogMessage("AlignmentMode Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("AlignmentMode", False)
		End Get
	End Property

	''' <summary>
	''' The Altitude above the local horizon of the telescope's current position (degrees, positive up)
	''' </summary>
	Public ReadOnly Property Altitude() As Double Implements ITelescopeV3.Altitude
		Get
			TL.LogMessage("Altitude Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Altitude", False)
		End Get
	End Property

	''' <summary>
	''' The area of the telescope's aperture, taking into account any obstructions (square meters)
	''' </summary>
	Public ReadOnly Property ApertureArea() As Double Implements ITelescopeV3.ApertureArea
		Get
			TL.LogMessage("ApertureArea Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ApertureArea", False)
		End Get
	End Property

	''' <summary>
	''' The telescope's effective aperture diameter (meters)
	''' </summary>
	Public ReadOnly Property ApertureDiameter() As Double Implements ITelescopeV3.ApertureDiameter
		Get
			TL.LogMessage("ApertureDiameter Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("ApertureDiameter", False)
		End Get
	End Property

	''' <summary>
	''' True if the telescope is stopped in the Home position. Set only following a <see cref="FindHome"></see> operation,
	''' and reset with any slew operation. This property must be False if the telescope does not support homing.
	''' </summary>
	Public ReadOnly Property AtHome() As Boolean Implements ITelescopeV3.AtHome
		Get
			TL.LogMessage("AtHome", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if the telescope has been put into the parked state by the seee <see cref="Park" /> method. Set False by calling the Unpark() method.
	''' </summary>
	Public ReadOnly Property AtPark() As Boolean Implements ITelescopeV3.AtPark
		Get
			TL.LogMessage("AtPark", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' Determine the rates at which the telescope may be moved about the specified axis by the <see cref="MoveAxis" /> method.
	''' </summary>
	''' <param name="Axis">The axis about which rate information is desired (TelescopeAxes value)</param>
	''' <returns>Collection of <see cref="IRate" /> rate objects</returns>
	Public Function AxisRates(Axis As TelescopeAxes) As IAxisRates Implements ITelescopeV3.AxisRates
		TL.LogMessage("AxisRates", "Get - " & Axis.ToString())
		Return New AxisRates(Axis)
	End Function

	''' <summary>
	''' The azimuth at the local horizon of the telescope's current position (degrees, North-referenced, positive East/clockwise).
	''' </summary>
	Public ReadOnly Property Azimuth() As Double Implements ITelescopeV3.Azimuth
		Get
			TL.LogMessage("Azimuth Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Azimuth", False)
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed finding its home position (<see cref="FindHome" /> method).
	''' </summary>
	Public ReadOnly Property CanFindHome() As Boolean Implements ITelescopeV3.CanFindHome
		Get
			TL.LogMessage("CanFindHome", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope can move the requested axis
	''' </summary>
	Public Function CanMoveAxis(Axis As TelescopeAxes) As Boolean Implements ITelescopeV3.CanMoveAxis
		TL.LogMessage("CanMoveAxis", "Get - " & Axis.ToString())
		Select Case Axis
			Case TelescopeAxes.axisPrimary
				Return False
			Case TelescopeAxes.axisSecondary
				Return False
			Case TelescopeAxes.axisTertiary
				Return False
			Case Else
				Throw New InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2")
		End Select
	End Function

	''' <summary>
	''' True if this telescope is capable of programmed parking (<see cref="Park" />method)
	''' </summary>
	Public ReadOnly Property CanPark() As Boolean Implements ITelescopeV3.CanPark
		Get
			TL.LogMessage("CanPark", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of software-pulsed guiding (via the <see cref="PulseGuide" /> method)
	''' </summary>
	Public ReadOnly Property CanPulseGuide() As Boolean Implements ITelescopeV3.CanPulseGuide
		Get
			TL.LogMessage("CanPulseGuide", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if the <see cref="DeclinationRate" /> property can be changed to provide offset tracking in the declination axis.
	''' </summary>
	Public ReadOnly Property CanSetDeclinationRate() As Boolean Implements ITelescopeV3.CanSetDeclinationRate
		Get
			TL.LogMessage("CanSetDeclinationRate", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if the guide rate properties used for <see cref="PulseGuide" /> can ba adjusted.
	''' </summary>
	Public ReadOnly Property CanSetGuideRates() As Boolean Implements ITelescopeV3.CanSetGuideRates
		Get
			TL.LogMessage("CanSetGuideRates", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed setting of its park position (<see cref="SetPark" /> method)
	''' </summary>
	Public ReadOnly Property CanSetPark() As Boolean Implements ITelescopeV3.CanSetPark
		Get
			TL.LogMessage("CanSetPark", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if the <see cref="SideOfPier" /> property can be set, meaning that the mount can be forced to flip.
	''' </summary>
	Public ReadOnly Property CanSetPierSide() As Boolean Implements ITelescopeV3.CanSetPierSide
		Get
			TL.LogMessage("CanSetPierSide", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if the <see cref="RightAscensionRate" /> property can be changed to provide offset tracking in the right ascension axis.
	''' </summary>
	Public ReadOnly Property CanSetRightAscensionRate() As Boolean Implements ITelescopeV3.CanSetRightAscensionRate
		Get
			TL.LogMessage("CanSetRightAscensionRate", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if the <see cref="Tracking" /> property can be changed, turning telescope sidereal tracking on and off.
	''' </summary>
	Public ReadOnly Property CanSetTracking() As Boolean Implements ITelescopeV3.CanSetTracking
		Get
			TL.LogMessage("CanSetTracking", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed slewing (synchronous or asynchronous) to equatorial coordinates
	''' </summary>
	Public ReadOnly Property CanSlew() As Boolean Implements ITelescopeV3.CanSlew
		Get
			TL.LogMessage("CanSlew", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed slewing (synchronous or asynchronous) to local horizontal coordinates
	''' </summary>
	Public ReadOnly Property CanSlewAltAz() As Boolean Implements ITelescopeV3.CanSlewAltAz
		Get
			TL.LogMessage("CanSlewAltAz", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed asynchronous slewing to local horizontal coordinates
	''' </summary>
	Public ReadOnly Property CanSlewAltAzAsync() As Boolean Implements ITelescopeV3.CanSlewAltAzAsync
		Get
			TL.LogMessage("CanSlewAltAzAsync", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed asynchronous slewing to equatorial coordinates.
	''' </summary>
	Public ReadOnly Property CanSlewAsync() As Boolean Implements ITelescopeV3.CanSlewAsync
		Get
			TL.LogMessage("CanSlewAsync", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed synching to equatorial coordinates.
	''' </summary>
	Public ReadOnly Property CanSync() As Boolean Implements ITelescopeV3.CanSync
		Get
			TL.LogMessage("CanSync", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed synching to local horizontal coordinates
	''' </summary>
	Public ReadOnly Property CanSyncAltAz() As Boolean Implements ITelescopeV3.CanSyncAltAz
		Get
			TL.LogMessage("CanSyncAltAz", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' True if this telescope is capable of programmed unparking (<see cref="Unpark" /> method).
	''' </summary>
	Public ReadOnly Property CanUnpark() As Boolean Implements ITelescopeV3.CanUnpark
		Get
			TL.LogMessage("CanUnpark", "Get - " & False.ToString())
			Return False
		End Get
	End Property

	''' <summary>
	''' The declination (degrees) of the telescope's current equatorial coordinates, in the coordinate system given by the <see cref="EquatorialSystem" /> property.
	''' Reading the property will raise an error if the value is unavailable.
	''' </summary>
	Public ReadOnly Property Declination() As Double Implements ITelescopeV3.Declination
		Get
			Dim declination__1 As Double = 0.0
			TL.LogMessage("Declination", "Get - " & utilities.DegreesToDMS(declination__1, ":", ":"))
			Return declination__1
		End Get
	End Property

	''' <summary>
	''' The declination tracking rate (arcseconds per SI second, default = 0.0)
	''' </summary>
	Public Property DeclinationRate() As Double Implements ITelescopeV3.DeclinationRate
		Get
			Dim declination As Double = 0.0
			TL.LogMessage("DeclinationRate", "Get - " & declination.ToString())
			Return declination
		End Get
		Set(value As Double)
			TL.LogMessage("DeclinationRate Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("DeclinationRate", True)
		End Set
	End Property

	''' <summary>
	''' Predict side of pier for German equatorial mounts at the provided coordinates
	''' </summary>
	Public Function DestinationSideOfPier(RightAscension As Double, Declination As Double) As PierSide Implements ITelescopeV3.DestinationSideOfPier
		TL.LogMessage("DestinationSideOfPier Get", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("DestinationSideOfPier")
	End Function

	''' <summary>
	''' True if the telescope or driver applies atmospheric refraction to coordinates.
	''' </summary>
	Public Property DoesRefraction() As Boolean Implements ITelescopeV3.DoesRefraction
		Get
			TL.LogMessage("DoesRefraction Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("DoesRefraction", False)
		End Get
		Set(value As Boolean)
			TL.LogMessage("DoesRefraction Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("DoesRefraction", True)
		End Set
	End Property

	''' <summary>
	''' Equatorial coordinate system used by this telescope (e.g. Topocentric or J2000).
	''' </summary>
	Public ReadOnly Property EquatorialSystem() As EquatorialCoordinateType Implements ITelescopeV3.EquatorialSystem
		Get
			Dim equatorialSystem__1 As EquatorialCoordinateType = EquatorialCoordinateType.equTopocentric
			TL.LogMessage("DeclinationRate", "Get - " & equatorialSystem__1.ToString())
			Return equatorialSystem__1
		End Get
	End Property

	''' <summary>
	''' Locates the telescope's "home" position (synchronous)
	''' </summary>
	Public Sub FindHome() Implements ITelescopeV3.FindHome
		TL.LogMessage("FindHome", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("FindHome")
	End Sub

	''' <summary>
	''' The telescope's focal length, meters
	''' </summary>
	Public ReadOnly Property FocalLength() As Double Implements ITelescopeV3.FocalLength
		Get
			TL.LogMessage("FocalLength Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("FocalLength", False)
		End Get
	End Property

	''' <summary>
	''' The current Declination movement rate offset for telescope guiding (degrees/sec)
	''' </summary>
	Public Property GuideRateDeclination() As Double Implements ITelescopeV3.GuideRateDeclination
		Get
			TL.LogMessage("GuideRateDeclination Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("GuideRateDeclination", False)
		End Get
		Set(value As Double)
			TL.LogMessage("GuideRateDeclination Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("GuideRateDeclination", True)
		End Set
	End Property

	''' <summary>
	''' The current Right Ascension movement rate offset for telescope guiding (degrees/sec)
	''' </summary>
	Public Property GuideRateRightAscension() As Double Implements ITelescopeV3.GuideRateRightAscension
		Get
			TL.LogMessage("GuideRateRightAscension Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("GuideRateRightAscension", False)
		End Get
		Set(value As Double)
			TL.LogMessage("GuideRateRightAscension Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("GuideRateRightAscension", True)
		End Set
	End Property

	''' <summary>
	''' True if a <see cref="PulseGuide" /> command is in progress, False otherwise
	''' </summary>
	Public ReadOnly Property IsPulseGuiding() As Boolean Implements ITelescopeV3.IsPulseGuiding
		Get
			TL.LogMessage("IsPulseGuiding Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("IsPulseGuiding", False)
		End Get
	End Property

	''' <summary>
	''' Move the telescope in one axis at the given rate.
	''' </summary>
	''' <param name="Axis">The physical axis about which movement is desired</param>
	''' <param name="Rate">The rate of motion (deg/sec) about the specified axis</param>
	Public Sub MoveAxis(Axis As TelescopeAxes, Rate As Double) Implements ITelescopeV3.MoveAxis
		TL.LogMessage("MoveAxis", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("MoveAxis")
	End Sub

	''' <summary>
	''' Move the telescope to its park position, stop all motion (or restrict to a small safe range), and set <see cref="AtPark" /> to True.
	''' </summary>
	Public Sub Park() Implements ITelescopeV3.Park
		TL.LogMessage("Park", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("Park")
	End Sub

	''' <summary>
	''' Moves the scope in the given direction for the given interval or time at
	''' the rate given by the corresponding guide rate property
	''' </summary>
	''' <param name="Direction">The direction in which the guide-rate motion is to be made</param>
	''' <param name="Duration">The duration of the guide-rate motion (milliseconds)</param>
	Public Sub PulseGuide(Direction As GuideDirections, Duration As Integer) Implements ITelescopeV3.PulseGuide
		TL.LogMessage("PulseGuide", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("PulseGuide")
	End Sub

	''' <summary>
	''' The right ascension (hours) of the telescope's current equatorial coordinates,
	''' in the coordinate system given by the EquatorialSystem property
	''' </summary>
	Public ReadOnly Property RightAscension() As Double Implements ITelescopeV3.RightAscension
		Get
			Dim rightAscension__1 As Double = 0.0
			TL.LogMessage("RightAscension", "Get - " & utilities.HoursToHMS(rightAscension__1))
			Return rightAscension__1
		End Get
	End Property

	''' <summary>
	''' The right ascension tracking rate offset from sidereal (seconds per sidereal second, default = 0.0)
	''' </summary>
	Public Property RightAscensionRate() As Double Implements ITelescopeV3.RightAscensionRate
		Get
			Dim rightAscensionRate__1 As Double = 0.0
			TL.LogMessage("RightAscensionRate", "Get - " & rightAscensionRate__1.ToString())
			Return rightAscensionRate__1
		End Get
		Set(value As Double)
			TL.LogMessage("RightAscensionRate Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("RightAscensionRate", True)
		End Set
	End Property

	''' <summary>
	''' Sets the telescope's park position to be its current position.
	''' </summary>
	Public Sub SetPark() Implements ITelescopeV3.SetPark
		TL.LogMessage("SetPark", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SetPark")
	End Sub

	''' <summary>
	''' Indicates the pointing state of the mount. Read the articles installed with the ASCOM Developer
	''' Components for more detailed information.
	''' </summary>
	Public Property SideOfPier() As PierSide Implements ITelescopeV3.SideOfPier
		Get
			TL.LogMessage("SideOfPier Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SideOfPier", False)
		End Get
		Set(value As PierSide)
			TL.LogMessage("SideOfPier Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SideOfPier", True)
		End Set
	End Property

	''' <summary>
	''' The local apparent sidereal time from the telescope's internal clock (hours, sidereal)
	''' </summary>
	Public ReadOnly Property SiderealTime() As Double Implements ITelescopeV3.SiderealTime
		Get
			' now using novas 3.1
			Dim lst As Double = 0.0
			Using novas As New ASCOM.Astrometry.NOVAS.NOVAS31
				Dim jd As Double = utilities.DateUTCToJulian(DateTime.UtcNow)
				novas.SiderealTime(jd, 0, novas.DeltaT(jd),
								   Astrometry.GstType.GreenwichMeanSiderealTime,
								   Astrometry.Method.EquinoxBased,
								   Astrometry.Accuracy.Reduced,
								   lst)
			End Using

			' Allow for the longitude
			lst += SiteLongitude / 360.0 * 24.0

			' Reduce to the range 0 to 24 hours
			lst = astroUtilities.ConditionRA(lst)

			TL.LogMessage("SiderealTime", "Get - " & lst.ToString())
			Return lst
		End Get
	End Property

	''' <summary>
	''' The elevation above mean sea level (meters) of the site at which the telescope is located
	''' </summary>
	Public Property SiteElevation() As Double Implements ITelescopeV3.SiteElevation
		Get
			TL.LogMessage("SiteElevation Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SiteElevation", False)
		End Get
		Set(value As Double)
			TL.LogMessage("SiteElevation Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SiteElevation", True)
		End Set
	End Property

	''' <summary>
	''' The geodetic(map) latitude (degrees, positive North, WGS84) of the site at which the telescope is located.
	''' </summary>
	Public Property SiteLatitude() As Double Implements ITelescopeV3.SiteLatitude
		Get
			TL.LogMessage("SiteLatitude Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SiteLatitude", False)
		End Get
		Set(value As Double)
			TL.LogMessage("SiteLatitude Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SiteLatitude", True)
		End Set
	End Property

	''' <summary>
	''' The longitude (degrees, positive East, WGS84) of the site at which the telescope is located.
	''' </summary>
	Public Property SiteLongitude() As Double Implements ITelescopeV3.SiteLongitude
		Get
			TL.LogMessage("SiteLongitude Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SiteLongitude", False)
		End Get
		Set(value As Double)
			TL.LogMessage("SiteLongitude Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SiteLongitude", True)
		End Set
	End Property

	''' <summary>
	''' Specifies a post-slew settling time (sec.).
	''' </summary>
	Public Property SlewSettleTime() As Short Implements ITelescopeV3.SlewSettleTime
		Get
			TL.LogMessage("SlewSettleTime Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SlewSettleTime", False)
		End Get
		Set(value As Short)
			TL.LogMessage("SlewSettleTime Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("SlewSettleTime", True)
		End Set
	End Property

	''' <summary>
	''' Move the telescope to the given local horizontal coordinates, return when slew is complete
	''' </summary>
	Public Sub SlewToAltAz(Azimuth As Double, Altitude As Double) Implements ITelescopeV3.SlewToAltAz
		TL.LogMessage("SlewToAltAz", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SlewToAltAz")
	End Sub

	''' <summary>
	''' This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
	''' It returns immediately, with Slewing set to True
	''' </summary>
	''' <param name="Azimuth">Azimuth to which to move</param>
	''' <param name="Altitude">Altitude to which to move to</param>
	Public Sub SlewToAltAzAsync(Azimuth As Double, Altitude As Double) Implements ITelescopeV3.SlewToAltAzAsync
		TL.LogMessage("SlewToAltAzAsync", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SlewToAltAzAsync")
	End Sub

	''' <summary>
	''' This Method must be implemented if <see cref="CanSlewAltAzAsync" /> returns True.
	''' It does not return to the caller until the slew is complete.
	''' </summary>
	Public Sub SlewToCoordinates(RightAscension As Double, Declination As Double) Implements ITelescopeV3.SlewToCoordinates
		TL.LogMessage("SlewToCoordinates", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SlewToCoordinates")
	End Sub

	''' <summary>
	''' Move the telescope to the given equatorial coordinates, return with Slewing set to True immediately after starting the slew.
	''' </summary>
	Public Sub SlewToCoordinatesAsync(RightAscension As Double, Declination As Double) Implements ITelescopeV3.SlewToCoordinatesAsync
		TL.LogMessage("SlewToCoordinatesAsync", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SlewToCoordinatesAsync")
	End Sub

	''' <summary>
	''' Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" /> coordinates, return when slew complete.
	''' </summary>
	Public Sub SlewToTarget() Implements ITelescopeV3.SlewToTarget
		TL.LogMessage("SlewToTarget", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SlewToTarget")
	End Sub

	''' <summary>
	''' Move the telescope to the <see cref="TargetRightAscension" /> and <see cref="TargetDeclination" />  coordinates,
	''' returns immediately after starting the slew with Slewing set to True.
	''' </summary>
	Public Sub SlewToTargetAsync() Implements ITelescopeV3.SlewToTargetAsync
		TL.LogMessage("    Public Sub SlewToTargetAsync() Implements ITelescopeV3.SlewToTargetAsync", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SlewToTargetAsync")
	End Sub

	''' <summary>
	''' True if telescope is in the process of moving in response to one of the
	''' Slew methods or the <see cref="MoveAxis" /> method, False at all other times.
	''' </summary>
	Public ReadOnly Property Slewing() As Boolean Implements ITelescopeV3.Slewing
		Get
			TL.LogMessage("Slewing Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Slewing", False)
		End Get
	End Property

	''' <summary>
	''' Matches the scope's local horizontal coordinates to the given local horizontal coordinates.
	''' </summary>
	Public Sub SyncToAltAz(Azimuth As Double, Altitude As Double) Implements ITelescopeV3.SyncToAltAz
		TL.LogMessage("SyncToAltAz", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SyncToAltAz")
	End Sub

	''' <summary>
	''' Matches the scope's equatorial coordinates to the given equatorial coordinates.
	''' </summary>
	Public Sub SyncToCoordinates(RightAscension As Double, Declination As Double) Implements ITelescopeV3.SyncToCoordinates
		TL.LogMessage("SyncToCoordinates", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SyncToCoordinates")
	End Sub

	''' <summary>
	''' Matches the scope's equatorial coordinates to the target equatorial coordinates.
	''' </summary>
	Public Sub SyncToTarget() Implements ITelescopeV3.SyncToTarget
		TL.LogMessage("SyncToTarget", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("SyncToTarget")
	End Sub

	''' <summary>
	''' The declination (degrees, positive North) for the target of an equatorial slew or sync operation
	''' </summary>
	Public Property TargetDeclination() As Double Implements ITelescopeV3.TargetDeclination
		Get
			TL.LogMessage("TargetDeclination Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("TargetDeclination", False)
		End Get
		Set(value As Double)
			TL.LogMessage("TargetDeclination Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("TargetDeclination", True)
		End Set
	End Property

	''' <summary>
	''' The right ascension (hours) for the target of an equatorial slew or sync operation
	''' </summary>
	Public Property TargetRightAscension() As Double Implements ITelescopeV3.TargetRightAscension
		Get
			TL.LogMessage("TargetRightAscension Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("TargetRightAscension", False)
		End Get
		Set(value As Double)
			TL.LogMessage("TargetRightAscension Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("TargetRightAscension", True)
		End Set
	End Property

	''' <summary>
	''' The state of the telescope's sidereal tracking drive.
	''' </summary>
	Public Property Tracking() As Boolean Implements ITelescopeV3.Tracking
		Get
			Dim tracking__1 As Boolean = True
			TL.LogMessage("Tracking", "Get - " & tracking__1.ToString())
			Return tracking__1
		End Get
		Set(value As Boolean)
			TL.LogMessage("Tracking Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("Tracking", True)
		End Set
	End Property

	''' <summary>
	''' The current tracking rate of the telescope's sidereal drive
	''' </summary>
	Public Property TrackingRate() As DriveRates Implements ITelescopeV3.TrackingRate
		Get
			TL.LogMessage("TrackingRate Get", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("TrackingRate", False)
		End Get
		Set(value As DriveRates)
			TL.LogMessage("TrackingRate Set", "Not implemented")
			Throw New ASCOM.PropertyNotImplementedException("TrackingRate", True)
		End Set
	End Property

	''' <summary>
	''' Returns a collection of supported <see cref="DriveRates" /> values that describe the permissible
	''' values of the <see cref="TrackingRate" /> property for this telescope type.
	''' </summary>
	Public ReadOnly Property TrackingRates() As ITrackingRates Implements ITelescopeV3.TrackingRates
		Get
			Dim trackingRates__1 As ITrackingRates = New TrackingRates()
			TL.LogMessage("TrackingRates", "Get - ")
			For Each driveRate As DriveRates In trackingRates__1
				TL.LogMessage("TrackingRates", "Get - " & driveRate.ToString())
			Next
			Return trackingRates__1
		End Get
	End Property

	''' <summary>
	''' The UTC date/time of the telescope's internal clock
	''' </summary>
	Public Property UTCDate() As DateTime Implements ITelescopeV3.UTCDate
		Get
			Dim utcDate__1 As DateTime = DateTime.UtcNow
			TL.LogMessage("UTCDate", String.Format("Get - {0}", utcDate__1))
			Return utcDate__1
		End Get
		Set(value As DateTime)
			Throw New ASCOM.PropertyNotImplementedException("UTCDate", True)
		End Set
	End Property

	''' <summary>
	''' Takes telescope out of the Parked state.
	''' </summary>
	Public Sub Unpark() Implements ITelescopeV3.Unpark
		TL.LogMessage("Unpark", "Not implemented")
		Throw New ASCOM.MethodNotImplementedException("Unpark")
	End Sub

#End Region

	'//ENDOFINSERTEDFILE
End Class