'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Telescope driver for $safeprojectname$
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM Telescope interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Telescope template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.$safeprojectname$.Telescope
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Telescope
' The ClassInterface/None addribute prevents an empty interface called
' _Telescope from being created and used as the [default] interface
'
<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Telescope
'		=====================
		Implements ITelescope	' Early-bind interface implemented by this driver
'		=====================
	'
	' Driver ID and descriptive string that shows in the Chooser
	'
	Private Shared s_csDriverID As String = "ASCOM.$safeprojectname$.Telescope"
	' TODO Change the descriptive string for your driver then remove this line
	Private Shared s_csDriverDescription As String = "$safeprojectname$ Telescope"

	'
	' Driver private data (rate collections)
	'
	Private m_AxisRates(2) As AxisRates
	Private m_TrackingRates As TrackingRates

	'
	' Constructor - Must be public for COM registration!
	'
	Public Sub New()
		m_AxisRates(0) = New AxisRates(TelescopeAxes.axisPrimary)
		m_AxisRates(1) = New AxisRates(TelescopeAxes.axisSecondary)
		m_AxisRates(2) = New AxisRates(TelescopeAxes.axisTertiary)
		m_TrackingRates = New TrackingRates()
		' TODO Implement your additional construction here
	End Sub

#Region "ASCOM Registration"

	Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

		Dim P As New Helper.Profile()
		P.DeviceTypeV = "Telescope"			'  Requires Helper 5.0.3 or later
		If bRegister Then
			P.Register(s_csDriverID, s_csDriverDescription)
		Else
			P.Unregister(s_csDriverID)
		End If
		Try									' In case Helper becomes native .NET
			Marshal.ReleaseComObject(P)
		Catch ex As Exception
			' Ignore exception
		End Try
		P = Nothing

	End Sub

	<ComRegisterFunction()> _
	Public Shared Sub RegisterASCOM(ByVal T As Type)

		RegUnregASCOM(True)

	End Sub

	<ComUnregisterFunction()> _
	Public Shared Sub UnregisterASCOM(ByVal T As Type)

		RegUnregASCOM(False)

	End Sub

#End Region
	'
	' PUBLIC COM INTERFACE ITelescope IMPLEMENTATION
	'

#Region "ITelescope Members"

	Public Sub AbortSlew() Implements ITelescope.AbortSlew
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("AbortSlew")
	End Sub

	Public ReadOnly Property AlignmentMode() As AlignmentModes Implements ITelescope.AlignmentMode
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("AlignmentMode", False)
		End Get
	End Property

	Public ReadOnly Property Altitude() As Double Implements ITelescope.Altitude
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Altitude", False)
		End Get
	End Property

	Public ReadOnly Property ApertureArea() As Double Implements ITelescope.ApertureArea
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("ApertureArea", False)
		End Get
	End Property

	Public ReadOnly Property ApertureDiameter() As Double Implements ITelescope.ApertureDiameter
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("ApertureDiameter", False)
		End Get
	End Property

	Public ReadOnly Property AtHome() As Boolean Implements ITelescope.AtHome
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("AtHome", False)
		End Get
	End Property

	Public ReadOnly Property AtPark() As Boolean Implements ITelescope.AtPark
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("AtPark", False)
		End Get
	End Property

	Public Function AxisRates(ByVal Axis As TelescopeAxes) As IAxisRates Implements ITelescope.AxisRates
		Select Case Axis
			Case TelescopeAxes.axisPrimary
				Return m_AxisRates(0)
			Case TelescopeAxes.axisSecondary
				Return m_AxisRates(1)
			Case TelescopeAxes.axisTertiary
				Return m_AxisRates(2)
			Case Else
				Return Nothing
		End Select
	End Function

	Public ReadOnly Property Azimuth() As Double Implements ITelescope.Azimuth
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Azimuth", False)
		End Get
	End Property

	Public ReadOnly Property CanFindHome() As Boolean Implements ITelescope.CanFindHome
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanFindHome", False)
		End Get
	End Property

	Public Function CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean Implements ITelescope.CanMoveAxis
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("CanMoveAxis")
	End Function

	Public ReadOnly Property CanPark() As Boolean Implements ITelescope.CanPark
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanPark", False)
		End Get
	End Property

	Public ReadOnly Property CanPulseGuide() As Boolean Implements ITelescope.CanPulseGuide
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanPulseGuide", False)
		End Get
	End Property

	Public ReadOnly Property CanSetDeclinationRate() As Boolean Implements ITelescope.CanSetDeclinationRate
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetDeclinationRate", False)
		End Get
	End Property

	Public ReadOnly Property CanSetGuideRates() As Boolean Implements ITelescope.CanSetGuideRates
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetGuideRates", False)
		End Get
	End Property

	Public ReadOnly Property CanSetPark() As Boolean Implements ITelescope.CanSetPark
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetPark", False)
		End Get
	End Property

	Public ReadOnly Property CanSetPierSide() As Boolean Implements ITelescope.CanSetPierSide
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetPierSide", False)
		End Get
	End Property

	Public ReadOnly Property CanSetRightAscensionRate() As Boolean Implements ITelescope.CanSetRightAscensionRate
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetRightAscensionRate", False)
		End Get
	End Property

	Public ReadOnly Property CanSetTracking() As Boolean Implements ITelescope.CanSetTracking
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSetTracking", False)
		End Get
	End Property

	Public ReadOnly Property CanSlew() As Boolean Implements ITelescope.CanSlew
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSlew", False)
		End Get
	End Property

	Public ReadOnly Property CanSlewAltAz() As Boolean Implements ITelescope.CanSlewAltAz
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSlewAltAz", False)
		End Get
	End Property

	Public ReadOnly Property CanSlewAltAzAsync() As Boolean Implements ITelescope.CanSlewAltAzAsync
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSlewAltAzAsync", False)
		End Get
	End Property

	Public ReadOnly Property CanSlewAsync() As Boolean Implements ITelescope.CanSlewAsync
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSlewAsync", False)
		End Get
	End Property

	Public ReadOnly Property CanSync() As Boolean Implements ITelescope.CanSync
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSync", False)
		End Get
	End Property

	Public ReadOnly Property CanSyncAltAz() As Boolean Implements ITelescope.CanSyncAltAz
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanSyncAltAz", False)
		End Get
	End Property

	Public ReadOnly Property CanUnpark() As Boolean Implements ITelescope.CanUnpark
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("CanUnpark", False)
		End Get
	End Property

	Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ITelescope.CommandBlind
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("CommandBlind")
	End Sub

	Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements ITelescope.CommandBool
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("CommandBool")
	End Function

	Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements ITelescope.CommandString
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("CommandString")
	End Function

	Public Property Connected() As Boolean Implements ITelescope.Connected
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Connected", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("Connected", True)
		End Set
	End Property

	Public ReadOnly Property Declination() As Double Implements ITelescope.Declination
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Declination", False)
		End Get
	End Property

	Public Property DeclinationRate() As Double Implements ITelescope.DeclinationRate
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("DeclinationRate", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("DeclinationRate", True)
		End Set
	End Property

	Public ReadOnly Property Description() As String Implements ITelescope.Description
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Description", False)
		End Get
	End Property

	Public Function DestinationSideOfPier(ByVal RightAscension As Double, ByVal Declination As Double) As PierSide Implements ITelescope.DestinationSideOfPier
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("DestinationSideOfPier")
	End Function

	Public Property DoesRefraction() As Boolean Implements ITelescope.DoesRefraction
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("DoesRefraction", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("DoesRefraction", True)
		End Set
	End Property

	Public ReadOnly Property DriverInfo() As String Implements ITelescope.DriverInfo
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("DriverInfo", False)
		End Get
	End Property

	Public ReadOnly Property DriverVersion() As String Implements ITelescope.DriverVersion
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("DriverVersion", False)
		End Get
	End Property

	Public ReadOnly Property EquatorialSystem() As EquatorialCoordinateType Implements ITelescope.EquatorialSystem
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("EquatorialCoordinateType", False)
		End Get
	End Property

	Public Sub FindHome() Implements ITelescope.FindHome
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("FindHome")
	End Sub

	Public ReadOnly Property FocalLength() As Double Implements ITelescope.FocalLength
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("FocalLength", False)
		End Get
	End Property

	Public Property GuideRateDeclination() As Double Implements ITelescope.GuideRateDeclination
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("GuideRateDeclination", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("GuideRateDeclination", True)
		End Set
	End Property

	Public Property GuideRateRightAscension() As Double Implements ITelescope.GuideRateRightAscension
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("GuideRateRightAscension", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("GuideRateRightAscension", True)
		End Set
	End Property

	Public ReadOnly Property InterfaceVersion() As Short Implements ITelescope.InterfaceVersion
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("InterfaceVersion", False)
		End Get
	End Property

	Public ReadOnly Property IsPulseGuiding() As Boolean Implements ITelescope.IsPulseGuiding
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("IsPulseGuiding", False)
		End Get
	End Property

	Public Sub MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double) Implements ITelescope.MoveAxis
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("MoveAxis")
	End Sub

	Public ReadOnly Property Name() As String Implements ITelescope.Name
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Name", False)
		End Get
	End Property

	Public Sub Park() Implements ITelescope.Park
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("Park")
	End Sub

	Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer) Implements ITelescope.PulseGuide
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("PulseGuide")
	End Sub

	Public ReadOnly Property RightAscension() As Double Implements ITelescope.RightAscension
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("RightAscension", False)
		End Get
	End Property

	Public Property RightAscensionRate() As Double Implements ITelescope.RightAscensionRate
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("RightAscensionRate", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("RightAscensionRate", True)
		End Set
	End Property

	Public Sub SetPark() Implements ITelescope.SetPark
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SetPark")
	End Sub

	Public Sub SetupDialog() Implements ITelescope.SetupDialog
		Dim F As SetupDialogForm = New SetupDialogForm()
		F.ShowDialog()
	End Sub

	Public Property SideOfPier() As PierSide Implements ITelescope.SideOfPier
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("SideOfPier", False)
		End Get
		Set(ByVal Value As PierSide)
			 Throw New PropertyNotImplementedException("SideOfPier", True)
		End Set
	End Property

	Public ReadOnly Property SiderealTime() As Double Implements ITelescope.SiderealTime
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("SiderealTime", False)
		End Get
	End Property

	Public Property SiteElevation() As Double Implements ITelescope.SiteElevation
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("SiteElevation", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("SiteElevation", True)
		End Set
	End Property

	Public Property SiteLatitude() As Double Implements ITelescope.SiteLatitude
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("SiteLatitude", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("SiteLatitude", True)
		End Set
	End Property

	Public Property SiteLongitude() As Double Implements ITelescope.SiteLongitude
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("SiteLongitude", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("SiteLongitude", True)
		End Set
	End Property

	Public Property SlewSettleTime() As Short Implements ITelescope.SlewSettleTime
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("SlewSettleTime", False)
		End Get
		Set(ByVal Value As Short)
			 Throw New PropertyNotImplementedException("SlewSettleTime", True)
		End Set
	End Property

	Public Sub SlewToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ITelescope.SlewToAltAz
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SlewToAltAz")
	End Sub

	Public Sub SlewToAltAzAsync(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ITelescope.SlewToAltAzAsync
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SlewToAltAzAsync")
	End Sub

	Public Sub SlewToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double) Implements ITelescope.SlewToCoordinates
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SlewToCoordinates")
	End Sub

	Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, ByVal Declination As Double) Implements ITelescope.SlewToCoordinatesAsync
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SlewToCoordinatesAsync")
	End Sub

	Public Sub SlewToTarget() Implements ITelescope.SlewToTarget
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SlewToTarget")
	End Sub

	Public Sub SlewToTargetAsync() Implements ITelescope.SlewToTargetAsync
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SlewToTargetAsync")
	End Sub

	Public ReadOnly Property Slewing() As Boolean Implements ITelescope.Slewing
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Slewing", False)
		End Get
	End Property

	Public Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ITelescope.SyncToAltAz
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SyncToAltAz")
	End Sub

	Public Sub SyncToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double) Implements ITelescope.SyncToCoordinates
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SyncToCoordinates")
	End Sub

	Public Sub SyncToTarget() Implements ITelescope.SyncToTarget
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("SyncToTarget")
	End Sub

	Public Property TargetDeclination() As Double Implements ITelescope.TargetDeclination
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("TargetDeclination", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("TargetDeclination", True)
		End Set
	End Property

	Public Property TargetRightAscension() As Double Implements ITelescope.TargetRightAscension
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("TargetRightAscension", False)
		End Get
		Set(ByVal Value As Double)
			 Throw New PropertyNotImplementedException("TargetRightAscension", True)
		End Set
	End Property

	Public Property Tracking() As Boolean Implements ITelescope.Tracking
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("Tracking", False)
		End Get
		Set(ByVal Value As Boolean)
			 Throw New PropertyNotImplementedException("Tracking", True)
		End Set
	End Property

	Public Property TrackingRate() As DriveRates Implements ITelescope.TrackingRate
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("TrackingRate", False)
		End Get
		Set(ByVal Value As DriveRates)
			 Throw New PropertyNotImplementedException("TrackingRate", True)
		End Set
	End Property

	Public ReadOnly Property TrackingRates() As ITrackingRates Implements ITelescope.TrackingRates
		' TODO Replace this with your implementation
		Get
			 Return m_TrackingRates
		End Get
	End Property

	Public Property UTCDate() As DateTime Implements ITelescope.UTCDate
		' TODO Replace this with your implementation
		Get
			 Throw New PropertyNotImplementedException("UTCDate", False)
		End Get
		Set(ByVal Value As DateTime)
			 Throw New PropertyNotImplementedException("UTCDate", True)
		End Set
	End Property

	Public Sub Unpark() Implements ITelescope.Unpark
		' TODO Replace this with your implementation
		Throw New MethodNotImplementedException("Unpark")
	End Sub

#End Region


End Class

'
' The Rate class implements IRate, and is used to hold values
' for AxisRates. You do not need to change this class.
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.Rate
' The ClassInterface/None addribute prevents an empty interface called
' _Rate from being created and used as the [default] interface
'
<Guid("$guid3$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Rate
'		================
		Implements IRate
'		================

	Private m_dMaximum As Double = 0
	Private m_dMinimum As Double = 0

	'
	' Default constructor - Internal prevents public creation
	' of instances. These are values for AxisRates.
	'
	Friend Sub New(ByVal Minimum As Double, ByVal Maximum As Double)
		m_dMaximum = Maximum
		m_dMinimum = Minimum
	End Sub

#Region "IRate Members"

	Public Property Maximum() As Double Implements IRate.Maximum
		Get
			 Return m_dMaximum
		End Get
		Set(ByVal Value As Double)
			 m_dMaximum = Value
		End Set
	End Property

	Public Property Minimum() As Double Implements IRate.Minimum
		Get
			 Return m_dMinimum
		End Get
		Set(ByVal Value As Double)
			 m_dMinimum = Value
		End Set
	End Property

#End Region
End Class

'
' AxisRates is a strongly-typed collection that must be enumerable by
' both COM and .NET. The IAxisRates and IEnumerable interfaces provide
' this polymorphism. 
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.AxisRates
' The ClassInterface/None addribute prevents an empty interface called
' _AxisRates from being created and used as the [default] interface
'
<Guid("$guid4$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class AxisRates
'		======================
		Implements IAxisRates
		Implements IEnumerable
'		======================

	Private m_Axis As TelescopeAxes
	Private m_Rates(-1) As Rate			' Empty array, but an array nonetheless

	'
	' Constructor - Friend prevents public creation
	' of instances. Returned by Telescope.AxisRates.
	'
	Friend Sub New(ByVal Axis As TelescopeAxes)
		m_Axis = Axis
		'
		' This collection must hold zero or more Rate objects describing the 
		' rates of motion ranges for the Telescope.MoveAxis() method
		' that are supported by your driver. It is OK to leave this 
		' array empty, indicating that MoveAxis() is not supported.
		'
		' Note that we are constructing a rate array for the axis passed
		' to the constructor. Thus we switch() below, and each case should 
		' initialize the array for the rate for the selected axis.
		'
		Select Case Axis
			Case TelescopeAxes.axisPrimary
				' TODO Initialize this array with any Primary axis rates that your driver may provide
				' Example: ReDim m_Rates(2)
				'          m_Rates(0) = New Rate(10.0, 35.0)
				'          m_Rates(1) = New Rate(60.1, 120.0)
				Exit Sub
			Case TelescopeAxes.axisSecondary
				' TODO Initialize this array with any Secondary axis rates that your driver may provide
				Exit Sub
			Case TelescopeAxes.axisTertiary
				' TODO Initialize this array with any Tertiary axis rates that your driver may provide
				Exit Sub
		End Select
	End Sub

#Region "IAxisRates Members"

	Public ReadOnly Property Count() As Integer Implements IAxisRates.Count
		Get
			 Return m_Rates.Length
		End Get
	End Property

	Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator, IAxisRates.GetEnumerator
		Return m_Rates.GetEnumerator()
	End Function

	Default Public ReadOnly Property Item(ByVal Index As Integer) As IRate Implements IAxisRates.Item
		Get
			 Return CType(m_Rates(Index - 1), IRate)	' 1-based
		End Get
	End Property

#End Region

End Class

'
' TrackingRates is a strongly-typed collection that must be enumerable by
' both COM and .NET. The ITrackingRates and IEnumerable interfaces provide
' this polymorphism. 
'
' The Guid attribute sets the CLSID for ASCOM.$safeprojectname$.TrackingRates
' The ClassInterface/None addribute prevents an empty interface called
' _TrackingRates from being created and used as the [default] interface
'
<Guid("$guid5$")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class TrackingRates
'		=========================
		Implements ITrackingRates
		Implements IEnumerable
'		=========================

	Private m_TrackingRates(-1) As DriveRates			' Empty array, but an array nonetheless

	'
	' Default constructor - Friend prevents public creation
	' of instances. Returned by Telescope.AxisRates.
	'
	Friend Sub New()
		'
		' This array must hold ONE or more DriveRates values, indicating
		' the tracking rates supported by your telescope. The one value
		' (tracking rate) that MUST be supported is driveSidereal!
		'
		' TODO Initialize this array with any additional tracking rates that your driver may provide
		' Example: ReDim m_TrackingRates(1)
		'          m_TrackingRates(0) = DriveRates.driveSidereal
		'          m_TrackingRates(1) = DriveRates.driveLunar
		'
		ReDim m_TrackingRates(0)
		m_TrackingRates(0) = DriveRates.driveSidereal

	End Sub

#Region "ITrackingRates Members"

	Public ReadOnly Property Count() As Integer Implements ITrackingRates.Count
		Get
			 Return m_TrackingRates.Length
		End Get
	End Property

	Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator, ITrackingRates.GetEnumerator
		Return m_TrackingRates.GetEnumerator()
	End Function

	Default Public ReadOnly Property Item(ByVal Index As Integer) As DriveRates Implements ITrackingRates.Item
		Get
			 Return m_TrackingRates(Index - 1)	' 1-based
		End Get
	End Property

#End Region

End Class



