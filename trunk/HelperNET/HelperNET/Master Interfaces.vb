Imports System.Runtime.InteropServices
Namespace Interfaces

#Region "Enums"
    <ComVisible(True)> _
    Public Enum DriveRates As Integer
        driveSidereal = 0
        driveLunar = 1
        driveSolar = 2
        driveKing = 3
    End Enum

    <ComVisible(True)> _
    Public Enum AlignmentModes
        algAltAz = 0
        algPolar = 1
        algGermanPolar = 2
    End Enum

    <ComVisible(True)> _
    Public Enum TelescopeAxes
        axisPrimary = 0
        axisSecondary = 1
        axisTertiary = 2
    End Enum

    <ComVisible(True)> _
    Public Enum EquatorialCoordinateType
        equOther = 0
        equLocalTopocentric = 1
        equJ2000 = 2
        equJ2050 = 3
        equB1950 = 4
    End Enum

    <ComVisible(True)> _
    Public Enum PierSide
        pierEast = 0
        pierWest = 1
        pierUnknown = -1
    End Enum

    <ComVisible(True)> _
    Public Enum GuideDirections
        guideNorth = 0
        guideSouth = 1
        guideEast = 2
        guideWest = 3
    End Enum
#End Region 'ASCOM Interface Enums

#Region "ITelescope"
    <ComVisible(True)> _
    Public Interface ITelescope
        Sub AbortSlew()
        ReadOnly Property AlignmentMode() As AlignmentModes
        ReadOnly Property Altitude() As Double
        ReadOnly Property ApertureArea() As Double
        ReadOnly Property ApertureDiameter() As Double
        ReadOnly Property AtHome() As Boolean
        ReadOnly Property AtPark() As Boolean
        Function AxisRates(ByVal Axis As TelescopeAxes) As IAxisRates
        ReadOnly Property Azimuth() As Double
        ReadOnly Property CanFindHome() As Boolean
        Function CanMoveAxis(ByVal Axis As TelescopeAxes) As Boolean
        ReadOnly Property CanPark() As Boolean
        ReadOnly Property CanPulseGuide() As Boolean
        ReadOnly Property CanSetDeclinationRate() As Boolean
        ReadOnly Property CanSetGuideRates() As Boolean
        ReadOnly Property CanSetPark() As Boolean
        ReadOnly Property CanSetPierSide() As Boolean
        ReadOnly Property CanSetRightAscensionRate() As Boolean
        ReadOnly Property CanSetTracking() As Boolean
        ReadOnly Property CanSlew() As Boolean
        ReadOnly Property CanSlewAltAz() As Boolean
        ReadOnly Property CanSlewAltAzAsync() As Boolean
        ReadOnly Property CanSlewAsync() As Boolean
        ReadOnly Property CanSync() As Boolean
        ReadOnly Property CanSyncAltAz() As Boolean
        ReadOnly Property CanUnpark() As Boolean
        Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False)
        Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean
        Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String
        Property Connected() As Boolean
        ReadOnly Property Declination() As Double
        Property DeclinationRate() As Double
        ReadOnly Property Description() As String
        Function DestinationSideOfPier(ByVal RightAscension As Double, ByVal Declination As Double) As PierSide
        Property DoesRefraction() As Boolean
        ReadOnly Property DriverInfo() As String
        ReadOnly Property DriverVersion() As String
        ReadOnly Property EquatorialSystem() As EquatorialCoordinateType
        Sub FindHome()
        ReadOnly Property FocalLength() As Double
        Property GuideRateDeclination() As Double
        Property GuideRateRightAscension() As Double
        ReadOnly Property InterfaceVersion() As Short
        ReadOnly Property IsPulseGuiding() As Boolean
        Sub MoveAxis(ByVal Axis As TelescopeAxes, ByVal Rate As Double)
        ReadOnly Property Name() As String
        Sub Park()
        Sub PulseGuide(ByVal Direction As GuideDirections, ByVal Duration As Integer)
        ReadOnly Property RightAscension() As Double
        Property RightAscensionRate() As Double
        Sub SetPark()
        Sub SetupDialog()
        Property SideOfPier() As PierSide
        ReadOnly Property SiderealTime() As Double
        Property SiteElevation() As Double
        Property SiteLatitude() As Double
        Property SiteLongitude() As Double
        ReadOnly Property Slewing() As Boolean
        Property SlewSettleTime() As Short
        Sub SlewToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
        Sub SlewToAltAzAsync(ByVal Azimuth As Double, ByVal Altitude As Double)
        Sub SlewToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double)
        Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, ByVal Declination As Double)
        Sub SlewToTarget()
        Sub SlewToTargetAsync()
        Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double)
        Sub SyncToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double)
        Sub SyncToTarget()
        Property TargetDeclination() As Double
        Property TargetRightAscension() As Double
        Property Tracking() As Boolean
        Property TrackingRate() As DriveRates
        ReadOnly Property TrackingRates() As ITrackingRates
        Sub Unpark()
        Property UTCDate() As Date
    End Interface

    Public Interface IAxisRates
        Inherits IEnumerable
        ReadOnly Property Count() As Integer
        Default ReadOnly Property Item(ByVal Index As Integer) As IRate
    End Interface

    Public Interface ITrackingRates
        Inherits IEnumerable
        ReadOnly Property Count() As Integer
        Default ReadOnly Property Item(ByVal Index As Integer) As DriveRates
    End Interface

    Public Interface IRate
        Property Maximum() As Double
        Property Minimum() As Double
    End Interface
#End Region ' ITelescope definition

End Namespace
