Imports ASCOM.Interface

Public Class Class1
    Implements ASCOM.Interface.ITelescopeV4

    Public Property Configuration() As String Implements ASCOM.Interface.IAscomDriver.Configuration
        Get

        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Public Property Connected() As Boolean Implements ASCOM.Interface.IAscomDriver.Connected
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ASCOM.Interface.IAscomDriver.Description
        Get

        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements ASCOM.Interface.IAscomDriver.DriverInfo
        Get

        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ASCOM.Interface.IAscomDriver.DriverVersion
        Get

        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ASCOM.Interface.IAscomDriver.InterfaceVersion
        Get

        End Get
    End Property

    Public ReadOnly Property Name() As String Implements ASCOM.Interface.IAscomDriver.Name
        Get

        End Get
    End Property

    Public Sub SetupDialog() Implements ASCOM.Interface.IAscomDriver.SetupDialog

    End Sub

    Public ReadOnly Property SupportedConfigurations() As String() Implements ASCOM.Interface.IAscomDriver.SupportedConfigurations
        Get

        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ASCOM.Interface.IDeviceControl.Action

    End Function

    Public Sub CommandBlind(ByVal Command As String, ByVal Raw As Boolean) Implements ASCOM.Interface.IDeviceControl.CommandBlind

    End Sub

    Public Function CommandBool(ByVal Command As String, ByVal Raw As Boolean) As Boolean Implements ASCOM.Interface.IDeviceControl.CommandBool

    End Function

    Public Function CommandString(ByVal Command As String, ByVal Raw As Boolean) As String Implements ASCOM.Interface.IDeviceControl.CommandString

    End Function

    Public ReadOnly Property LastResult() As String Implements ASCOM.Interface.IDeviceControl.LastResult
        Get

        End Get
    End Property

    Public ReadOnly Property SupportedActions() As String() Implements ASCOM.Interface.IDeviceControl.SupportedActions
        Get

        End Get
    End Property

    Public Sub ADontKnowYetMethod() Implements ASCOM.Interface.IDontKnowYet.ADontKnowYetMethod

    End Sub

    Public ReadOnly Property ADontKnowYetProperty() As String Implements ASCOM.Interface.IDontKnowYet.ADontKnowYetProperty
        Get

        End Get
    End Property

    Public Sub AbortSlew() Implements ASCOM.Interface.ITelescope.AbortSlew

    End Sub

    Public ReadOnly Property AlignmentMode() As ASCOM.Interface.AlignmentModes Implements ASCOM.Interface.ITelescope.AlignmentMode
        Get

        End Get
    End Property

    Public ReadOnly Property Altitude() As Double Implements ASCOM.Interface.ITelescope.Altitude
        Get

        End Get
    End Property

    Public ReadOnly Property ApertureArea() As Double Implements ASCOM.Interface.ITelescope.ApertureArea
        Get

        End Get
    End Property

    Public ReadOnly Property ApertureDiameter() As Double Implements ASCOM.Interface.ITelescope.ApertureDiameter
        Get

        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements ASCOM.Interface.ITelescope.AtHome
        Get

        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements ASCOM.Interface.ITelescope.AtPark
        Get

        End Get
    End Property

    Public Function AxisRates(ByVal Axis As ASCOM.Interface.TelescopeAxes) As ASCOM.Interface.IAxisRates Implements ASCOM.Interface.ITelescope.AxisRates

    End Function

    Public ReadOnly Property Azimuth() As Double Implements ASCOM.Interface.ITelescope.Azimuth
        Get

        End Get
    End Property

    Public ReadOnly Property CanFindHome() As Boolean Implements ASCOM.Interface.ITelescope.CanFindHome
        Get

        End Get
    End Property

    Public Function CanMoveAxis(ByVal Axis As ASCOM.Interface.TelescopeAxes) As Boolean Implements ASCOM.Interface.ITelescope.CanMoveAxis

    End Function

    Public ReadOnly Property CanPark() As Boolean Implements ASCOM.Interface.ITelescope.CanPark
        Get

        End Get
    End Property

    Public ReadOnly Property CanPulseGuide() As Boolean Implements ASCOM.Interface.ITelescope.CanPulseGuide
        Get

        End Get
    End Property

    Public ReadOnly Property CanSetDeclinationRate() As Boolean Implements ASCOM.Interface.ITelescope.CanSetDeclinationRate
        Get

        End Get
    End Property

    Public ReadOnly Property CanSetGuideRates() As Boolean Implements ASCOM.Interface.ITelescope.CanSetGuideRates
        Get

        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements ASCOM.Interface.ITelescope.CanSetPark
        Get

        End Get
    End Property

    Public ReadOnly Property CanSetPierSide() As Boolean Implements ASCOM.Interface.ITelescope.CanSetPierSide
        Get

        End Get
    End Property

    Public ReadOnly Property CanSetRightAscensionRate() As Boolean Implements ASCOM.Interface.ITelescope.CanSetRightAscensionRate
        Get

        End Get
    End Property

    Public ReadOnly Property CanSetTracking() As Boolean Implements ASCOM.Interface.ITelescope.CanSetTracking
        Get

        End Get
    End Property

    Public ReadOnly Property CanSlew() As Boolean Implements ASCOM.Interface.ITelescope.CanSlew
        Get

        End Get
    End Property

    Public ReadOnly Property CanSlewAltAz() As Boolean Implements ASCOM.Interface.ITelescope.CanSlewAltAz
        Get

        End Get
    End Property

    Public ReadOnly Property CanSlewAltAzAsync() As Boolean Implements ASCOM.Interface.ITelescope.CanSlewAltAzAsync
        Get

        End Get
    End Property

    Public ReadOnly Property CanSlewAsync() As Boolean Implements ASCOM.Interface.ITelescope.CanSlewAsync
        Get

        End Get
    End Property

    Public ReadOnly Property CanSync() As Boolean Implements ASCOM.Interface.ITelescope.CanSync
        Get

        End Get
    End Property

    Public ReadOnly Property CanSyncAltAz() As Boolean Implements ASCOM.Interface.ITelescope.CanSyncAltAz
        Get

        End Get
    End Property

    Public ReadOnly Property CanUnpark() As Boolean Implements ASCOM.Interface.ITelescope.CanUnpark
        Get

        End Get
    End Property

    Public ReadOnly Property Declination() As Double Implements ASCOM.Interface.ITelescope.Declination
        Get

        End Get
    End Property

    Public Property DeclinationRate() As Double Implements ASCOM.Interface.ITelescope.DeclinationRate
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public Function DestinationSideOfPier(ByVal RightAscension As Double, ByVal Declination As Double) As ASCOM.Interface.PierSide Implements ASCOM.Interface.ITelescope.DestinationSideOfPier

    End Function

    Public Property DoesRefraction() As Boolean Implements ASCOM.Interface.ITelescope.DoesRefraction
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public ReadOnly Property EquatorialSystem() As ASCOM.Interface.EquatorialCoordinateType Implements ASCOM.Interface.ITelescope.EquatorialSystem
        Get

        End Get
    End Property

    Public Sub FindHome() Implements ASCOM.Interface.ITelescope.FindHome

    End Sub

    Public ReadOnly Property FocalLength() As Double Implements ASCOM.Interface.ITelescope.FocalLength
        Get

        End Get
    End Property

    Public Property GuideRateDeclination() As Double Implements ASCOM.Interface.ITelescope.GuideRateDeclination
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public Property GuideRateRightAscension() As Double Implements ASCOM.Interface.ITelescope.GuideRateRightAscension
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public ReadOnly Property IsPulseGuiding() As Boolean Implements ASCOM.Interface.ITelescope.IsPulseGuiding
        Get

        End Get
    End Property

    Public Sub MoveAxis(ByVal Axis As ASCOM.Interface.TelescopeAxes, ByVal Rate As Double) Implements ASCOM.Interface.ITelescope.MoveAxis

    End Sub

    Public Sub Park() Implements ASCOM.Interface.ITelescope.Park

    End Sub

    Public Sub PulseGuide(ByVal Direction As ASCOM.Interface.GuideDirections, ByVal Duration As Integer) Implements ASCOM.Interface.ITelescope.PulseGuide

    End Sub

    Public ReadOnly Property RightAscension() As Double Implements ASCOM.Interface.ITelescope.RightAscension
        Get

        End Get
    End Property

    Public Property RightAscensionRate() As Double Implements ASCOM.Interface.ITelescope.RightAscensionRate
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public Sub SetPark() Implements ASCOM.Interface.ITelescope.SetPark

    End Sub

    Public Property SideOfPier() As ASCOM.Interface.PierSide Implements ASCOM.Interface.ITelescope.SideOfPier
        Get

        End Get
        Set(ByVal value As ASCOM.Interface.PierSide)

        End Set
    End Property

    Public ReadOnly Property SiderealTime() As Double Implements ASCOM.Interface.ITelescope.SiderealTime
        Get

        End Get
    End Property

    Public Property SiteElevation() As Double Implements ASCOM.Interface.ITelescope.SiteElevation
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public Property SiteLatitude() As Double Implements ASCOM.Interface.ITelescope.SiteLatitude
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public Property SiteLongitude() As Double Implements ASCOM.Interface.ITelescope.SiteLongitude
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public ReadOnly Property Slewing() As Boolean Implements ASCOM.Interface.ITelescope.Slewing
        Get

        End Get
    End Property

    Public Property SlewSettleTime() As Short Implements ASCOM.Interface.ITelescope.SlewSettleTime
        Get

        End Get
        Set(ByVal value As Short)

        End Set
    End Property

    Public Sub SlewToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ASCOM.Interface.ITelescope.SlewToAltAz

    End Sub

    Public Sub SlewToAltAzAsync(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ASCOM.Interface.ITelescope.SlewToAltAzAsync

    End Sub

    Public Sub SlewToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double) Implements ASCOM.Interface.ITelescope.SlewToCoordinates

    End Sub

    Public Sub SlewToCoordinatesAsync(ByVal RightAscension As Double, ByVal Declination As Double) Implements ASCOM.Interface.ITelescope.SlewToCoordinatesAsync

    End Sub

    Public Sub SlewToTarget() Implements ASCOM.Interface.ITelescope.SlewToTarget

    End Sub

    Public Sub SlewToTargetAsync() Implements ASCOM.Interface.ITelescope.SlewToTargetAsync

    End Sub

    Public Sub SyncToAltAz(ByVal Azimuth As Double, ByVal Altitude As Double) Implements ASCOM.Interface.ITelescope.SyncToAltAz

    End Sub

    Public Sub SyncToCoordinates(ByVal RightAscension As Double, ByVal Declination As Double) Implements ASCOM.Interface.ITelescope.SyncToCoordinates

    End Sub

    Public Sub SyncToTarget() Implements ASCOM.Interface.ITelescope.SyncToTarget

    End Sub

    Public Property TargetDeclination() As Double Implements ASCOM.Interface.ITelescope.TargetDeclination
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public Property TargetRightAscension() As Double Implements ASCOM.Interface.ITelescope.TargetRightAscension
        Get

        End Get
        Set(ByVal value As Double)

        End Set
    End Property

    Public Property Tracking() As Boolean Implements ASCOM.Interface.ITelescope.Tracking
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property TrackingRate() As ASCOM.Interface.DriveRates Implements ASCOM.Interface.ITelescope.TrackingRate
        Get

        End Get
        Set(ByVal value As ASCOM.Interface.DriveRates)

        End Set
    End Property

    Public ReadOnly Property TrackingRates() As ASCOM.Interface.ITrackingRates Implements ASCOM.Interface.ITelescope.TrackingRates
        Get

        End Get
    End Property

    Public Sub Unpark() Implements ASCOM.Interface.ITelescope.Unpark

    End Sub

    Public Property UTCDate() As Date Implements ASCOM.Interface.ITelescope.UTCDate
        Get

        End Get
        Set(ByVal value As Date)

        End Set
    End Property

    Public Sub ANewTelescopeV3Method() Implements ASCOM.Interface.ITelescopeV3.ANewTelescopeV3Method

    End Sub

    Public ReadOnly Property ANewTelescopeV3Property() As String Implements ASCOM.Interface.ITelescopeV3.ANewTelescopeV3Property
        Get

        End Get
    End Property

    Public Sub ANewTelescopeV4Method(ByVal NewParameter1 As String, ByVal NewParameter2 As Double) Implements ASCOM.Interface.ITelescopeV4.ANewTelescopeV4Method

    End Sub

    Public ReadOnly Property ANewTelescopeV4Property() As String Implements ASCOM.Interface.ITelescopeV4.ANewTelescopeV4Property
        Get

        End Get
    End Property
End Class
