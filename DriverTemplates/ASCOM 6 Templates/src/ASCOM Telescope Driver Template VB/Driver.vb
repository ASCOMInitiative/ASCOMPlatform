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
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

<Guid("$guid2$")> _
<ClassInterface(ClassInterfaceType.None)> _
<ComVisible(True)> _
Public Class Telescope
    '		=====================
    Implements ITelescopeV3   ' Early-bind interface implemented by this driver
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
    'Private m_TrackingRates As TrackingRates

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        m_AxisRates(0) = New AxisRates(TelescopeAxes.axisPrimary)
        m_AxisRates(1) = New AxisRates(TelescopeAxes.axisSecondary)
        m_AxisRates(2) = New AxisRates(TelescopeAxes.axisTertiary)
        'm_TrackingRates = New TrackingRates()
        ' TODO Implement your additional construction here
    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Using P As New Profile()
            P.DeviceType = "Telescope"
            If bRegister Then
                P.Register(s_csDriverID, s_csDriverDescription)
            Else
                P.Unregister(s_csDriverID)
            End If
        End Using
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

    Public Sub SetupDialog() Implements ITelescopeV3.SetupDialog
        Using f As New SetupDialogForm
            f.ShowDialog()
        End Using
    End Sub

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ITelescopeV3.Action
        Throw New ASCOM.MethodNotImplementedException("Action")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ITelescopeV3.CommandBlind
        Throw New ASCOM.MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements ITelescopeV3.CommandBool
        Throw New ASCOM.MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements ITelescopeV3.CommandString
        Throw New ASCOM.MethodNotImplementedException("CommandString")
    End Function

    Public Sub Dispose() Implements ITelescopeV3.Dispose
        Throw New System.NotImplementedException()
    End Sub

    Public Sub AbortSlew() Implements ITelescopeV3.AbortSlew
        Throw New System.NotImplementedException()
    End Sub

    Public Function AxisRates(ByVal Axis As TelescopeAxes) As IAxisRates Implements ITelescopeV3.AxisRates
        Throw New System.NotImplementedException()
    End Function

    Public Function CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean Implements ITelescopeV3.CanMoveAxis
        Throw New System.NotImplementedException()
    End Function

    Public Function DestinationSideOfPier(ByVal RightAscension As Double, ByVal Declination As Double) As PierSide Implements ITelescopeV3.DestinationSideOfPier
        Throw New System.NotImplementedException()
    End Function

    Public Sub FindHome() Implements ITelescopeV3.FindHome
        Throw New System.NotImplementedException()
    End Sub

    Public Sub MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double) Implements ITelescopeV3.MoveAxis
        Throw New System.NotImplementedException()
    End Sub

    Public Sub Park() Implements ITelescopeV3.Park
        Throw New System.NotImplementedException()
    End Sub

    Public Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer) Implements ITelescopeV3.PulseGuide
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SetPark() Implements ITelescopeV3.SetPark
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SlewToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ITelescopeV3.SlewToAltAz
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SlewToAltAzAsync(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ITelescopeV3.SlewToAltAzAsync
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SlewToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double) Implements ITelescopeV3.SlewToCoordinates
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, ByVal Declination As Double) Implements ITelescopeV3.SlewToCoordinatesAsync
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SlewToTarget() Implements ITelescopeV3.SlewToTarget
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SlewToTargetAsync() Implements ITelescopeV3.SlewToTargetAsync
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ITelescopeV3.SyncToAltAz
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SyncToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double) Implements ITelescopeV3.SyncToCoordinates
        Throw New System.NotImplementedException()
    End Sub

    Public Sub SyncToTarget() Implements ITelescopeV3.SyncToTarget
        Throw New System.NotImplementedException()
    End Sub

    Public Sub Unpark() Implements ITelescopeV3.Unpark
        Throw New System.NotImplementedException()
    End Sub

    Public Property Connected() As Boolean Implements ITelescopeV3.Connected
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ITelescopeV3.Description
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements ITelescopeV3.DriverInfo
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ITelescopeV3.DriverVersion
        Get
            Dim Ass As Reflection.Assembly

            Ass = Reflection.Assembly.GetExecutingAssembly 'Get our own assembly and report its version number
            Return Ass.GetName.Version.Major.ToString & "." & Ass.GetName.Version.Minor.ToString
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ITelescopeV3.InterfaceVersion
        Get
            Return 3
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements ITelescopeV3.Name
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property SupportedActions() As ArrayList Implements ITelescopeV3.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public ReadOnly Property AlignmentMode() As AlignmentModes Implements ITelescopeV3.AlignmentMode
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Altitude() As Double Implements ITelescopeV3.Altitude
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ApertureArea() As Double Implements ITelescopeV3.ApertureArea
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property ApertureDiameter() As Double Implements ITelescopeV3.ApertureDiameter
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements ITelescopeV3.AtHome
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements ITelescopeV3.AtPark
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Azimuth() As Double Implements ITelescopeV3.Azimuth
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanFindHome() As Boolean Implements ITelescopeV3.CanFindHome
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanPark() As Boolean Implements ITelescopeV3.CanPark
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanPulseGuide() As Boolean Implements ITelescopeV3.CanPulseGuide
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetDeclinationRate() As Boolean Implements ITelescopeV3.CanSetDeclinationRate
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetGuideRates() As Boolean Implements ITelescopeV3.CanSetGuideRates
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements ITelescopeV3.CanSetPark
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetPierSide() As Boolean Implements ITelescopeV3.CanSetPierSide
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetRightAscensionRate() As Boolean Implements ITelescopeV3.CanSetRightAscensionRate
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetTracking() As Boolean Implements ITelescopeV3.CanSetTracking
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSlew() As Boolean Implements ITelescopeV3.CanSlew
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSlewAltAz() As Boolean Implements ITelescopeV3.CanSlewAltAz
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSlewAltAzAsync() As Boolean Implements ITelescopeV3.CanSlewAltAzAsync
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSlewAsync() As Boolean Implements ITelescopeV3.CanSlewAsync
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSync() As Boolean Implements ITelescopeV3.CanSync
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSyncAltAz() As Boolean Implements ITelescopeV3.CanSyncAltAz
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanUnpark() As Boolean Implements ITelescopeV3.CanUnpark
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Declination() As Double Implements ITelescopeV3.Declination
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property DeclinationRate() As Double Implements ITelescopeV3.DeclinationRate
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property DoesRefraction() As Boolean Implements ITelescopeV3.DoesRefraction
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property EquatorialSystem() As EquatorialCoordinateType Implements ITelescopeV3.EquatorialSystem
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property FocalLength() As Double Implements ITelescopeV3.FocalLength
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property GuideRateDeclination() As Double Implements ITelescopeV3.GuideRateDeclination
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property GuideRateRightAscension() As Double Implements ITelescopeV3.GuideRateRightAscension
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property IsPulseGuiding() As Boolean Implements ITelescopeV3.IsPulseGuiding
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property RightAscension() As Double Implements ITelescopeV3.RightAscension
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property RightAscensionRate() As Double Implements ITelescopeV3.RightAscensionRate
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property SideOfPier() As PierSide Implements ITelescopeV3.SideOfPier
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As PierSide)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property SiderealTime() As Double Implements ITelescopeV3.SiderealTime
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property SiteElevation() As Double Implements ITelescopeV3.SiteElevation
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property SiteLatitude() As Double Implements ITelescopeV3.SiteLatitude
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property SiteLongitude() As Double Implements ITelescopeV3.SiteLongitude
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Slewing() As Boolean Implements ITelescopeV3.Slewing
        Get
            Throw New System.NotImplementedException()
        End Get
    End Property

    Public Property SlewSettleTime() As Short Implements ITelescopeV3.SlewSettleTime
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Short)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property TargetDeclination() As Double Implements ITelescopeV3.TargetDeclination
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property TargetRightAscension() As Double Implements ITelescopeV3.TargetRightAscension
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Double)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property Tracking() As Boolean Implements ITelescopeV3.Tracking
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Boolean)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public Property TrackingRate() As DriveRates Implements ITelescopeV3.TrackingRate
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As DriveRates)
            Throw New System.NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property TrackingRates() As ITrackingRates Implements ITelescopeV3.TrackingRates
        Get
            TrackingRates = New TrackingRates()
        End Get
    End Property

    Public Property UTCDate() As Date Implements ITelescopeV3.UTCDate
        Get
            Throw New System.NotImplementedException()
        End Get
        Set(ByVal value As Date)
            Throw New System.NotImplementedException()
        End Set
    End Property
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
<ComVisible(True)> _
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

    Public Sub Dispose() Implements IRate.Dispose
        Throw New System.NotImplementedException()
    End Sub

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
<ComVisible(True)> _
Public Class AxisRates
    '		======================
    Implements IAxisRates
    Implements IEnumerable
    '		======================

    Private m_Axis As TelescopeAxes
    Private m_Rates(-1) As Rate         ' Empty array, but an array nonetheless

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

    Public Sub Dispose() Implements IAxisRates.Dispose
        Throw New System.NotImplementedException()
    End Sub

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator, IAxisRates.GetEnumerator
        Return m_Rates.GetEnumerator()
    End Function

    Default Public ReadOnly Property Item(ByVal Index As Integer) As IRate Implements IAxisRates.Item
        Get
            Return CType(m_Rates(Index - 1), IRate)    ' 1-based
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
<ComVisible(True)> _
Public Class TrackingRates
    '		=========================
    Implements ITrackingRates
    Implements IEnumerable
    '		=========================

    Private m_TrackingRates(-1) As DriveRates           ' Empty array, but an array nonetheless

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

    Public Sub Dispose() Implements ITrackingRates.Dispose
        Throw New System.NotImplementedException()
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Integer) As DriveRates Implements ITrackingRates.Item
        Get
            Return m_TrackingRates(Index - 1)  ' 1-based
        End Get
    End Property

#End Region

End Class



