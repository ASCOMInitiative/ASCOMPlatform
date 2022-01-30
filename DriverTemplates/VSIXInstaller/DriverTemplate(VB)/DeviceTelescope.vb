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
    Public Sub AbortSlew() Implements ITelescopeV3.AbortSlew
        TL.LogMessage("AbortSlew", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("AbortSlew")
    End Sub

    Public ReadOnly Property AlignmentMode() As AlignmentModes Implements ITelescopeV3.AlignmentMode
        Get
            TL.LogMessage("AlignmentMode Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("AlignmentMode", False)
        End Get
    End Property

    Public ReadOnly Property Altitude() As Double Implements ITelescopeV3.Altitude
        Get
            TL.LogMessage("Altitude", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Altitude", False)
        End Get
    End Property

    Public ReadOnly Property ApertureArea() As Double Implements ITelescopeV3.ApertureArea
        Get
            TL.LogMessage("ApertureArea Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ApertureArea", False)
        End Get
    End Property

    Public ReadOnly Property ApertureDiameter() As Double Implements ITelescopeV3.ApertureDiameter
        Get
            TL.LogMessage("ApertureDiameter Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ApertureDiameter", False)
        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements ITelescopeV3.AtHome
        Get
            TL.LogMessage("AtHome", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements ITelescopeV3.AtPark
        Get
            TL.LogMessage("AtPark", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public Function AxisRates(Axis As TelescopeAxes) As IAxisRates Implements ITelescopeV3.AxisRates
        TL.LogMessage("AxisRates", "Get - " & Axis.ToString())
        Return New AxisRates(Axis)
    End Function

    Public ReadOnly Property Azimuth() As Double Implements ITelescopeV3.Azimuth
        Get
            TL.LogMessage("Azimuth Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Azimuth", False)
        End Get
    End Property

    Public ReadOnly Property CanFindHome() As Boolean Implements ITelescopeV3.CanFindHome
        Get
            TL.LogMessage("CanFindHome", "Get - " & False.ToString())
            Return False
        End Get
    End Property

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

    Public ReadOnly Property CanPark() As Boolean Implements ITelescopeV3.CanPark
        Get
            TL.LogMessage("CanPark", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanPulseGuide() As Boolean Implements ITelescopeV3.CanPulseGuide
        Get
            TL.LogMessage("CanPulseGuide", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetDeclinationRate() As Boolean Implements ITelescopeV3.CanSetDeclinationRate
        Get
            TL.LogMessage("CanSetDeclinationRate", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetGuideRates() As Boolean Implements ITelescopeV3.CanSetGuideRates
        Get
            TL.LogMessage("CanSetGuideRates", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements ITelescopeV3.CanSetPark
        Get
            TL.LogMessage("CanSetPark", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetPierSide() As Boolean Implements ITelescopeV3.CanSetPierSide
        Get
            TL.LogMessage("CanSetPierSide", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetRightAscensionRate() As Boolean Implements ITelescopeV3.CanSetRightAscensionRate
        Get
            TL.LogMessage("CanSetRightAscensionRate", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetTracking() As Boolean Implements ITelescopeV3.CanSetTracking
        Get
            TL.LogMessage("CanSetTracking", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSlew() As Boolean Implements ITelescopeV3.CanSlew
        Get
            TL.LogMessage("CanSlew", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSlewAltAz() As Boolean Implements ITelescopeV3.CanSlewAltAz
        Get
            TL.LogMessage("CanSlewAltAz", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSlewAltAzAsync() As Boolean Implements ITelescopeV3.CanSlewAltAzAsync
        Get
            TL.LogMessage("CanSlewAltAzAsync", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSlewAsync() As Boolean Implements ITelescopeV3.CanSlewAsync
        Get
            TL.LogMessage("CanSlewAsync", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSync() As Boolean Implements ITelescopeV3.CanSync
        Get
            TL.LogMessage("CanSync", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSyncAltAz() As Boolean Implements ITelescopeV3.CanSyncAltAz
        Get
            TL.LogMessage("CanSyncAltAz", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanUnpark() As Boolean Implements ITelescopeV3.CanUnpark
        Get
            TL.LogMessage("CanUnpark", "Get - " & False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property Declination() As Double Implements ITelescopeV3.Declination
        Get
            Dim declination__1 As Double = 0.0
            TL.LogMessage("Declination", "Get - " & utilities.DegreesToDMS(declination__1, ":", ":"))
            Return declination__1
        End Get
    End Property

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

    Public Function DestinationSideOfPier(RightAscension As Double, Declination As Double) As PierSide Implements ITelescopeV3.DestinationSideOfPier
        TL.LogMessage("DestinationSideOfPier Get", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("DestinationSideOfPier")
    End Function

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

    Public ReadOnly Property EquatorialSystem() As EquatorialCoordinateType Implements ITelescopeV3.EquatorialSystem
        Get
            Dim equatorialSystem__1 As EquatorialCoordinateType = EquatorialCoordinateType.equTopocentric
            TL.LogMessage("DeclinationRate", "Get - " & equatorialSystem__1.ToString())
            Return equatorialSystem__1
        End Get
    End Property

    Public Sub FindHome() Implements ITelescopeV3.FindHome
        TL.LogMessage("FindHome", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("FindHome")
    End Sub

    Public ReadOnly Property FocalLength() As Double Implements ITelescopeV3.FocalLength
        Get
            TL.LogMessage("FocalLength Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("FocalLength", False)
        End Get
    End Property

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

    Public ReadOnly Property IsPulseGuiding() As Boolean Implements ITelescopeV3.IsPulseGuiding
        Get
            TL.LogMessage("IsPulseGuiding Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("IsPulseGuiding", False)
        End Get
    End Property

    Public Sub MoveAxis(Axis As TelescopeAxes, Rate As Double) Implements ITelescopeV3.MoveAxis
        TL.LogMessage("MoveAxis", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("MoveAxis")
    End Sub

    Public Sub Park() Implements ITelescopeV3.Park
        TL.LogMessage("Park", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("Park")
    End Sub

    Public Sub PulseGuide(Direction As GuideDirections, Duration As Integer) Implements ITelescopeV3.PulseGuide
        TL.LogMessage("PulseGuide", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("PulseGuide")
    End Sub

    Public ReadOnly Property RightAscension() As Double Implements ITelescopeV3.RightAscension
        Get
            Dim rightAscension__1 As Double = 0.0
            TL.LogMessage("RightAscension", "Get - " & utilities.HoursToHMS(rightAscension__1))
            Return rightAscension__1
        End Get
    End Property

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

    Public Sub SetPark() Implements ITelescopeV3.SetPark
        TL.LogMessage("SetPark", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SetPark")
    End Sub

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

    Public Sub SlewToAltAz(Azimuth As Double, Altitude As Double) Implements ITelescopeV3.SlewToAltAz
        TL.LogMessage("SlewToAltAz", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SlewToAltAz")
    End Sub

    Public Sub SlewToAltAzAsync(Azimuth As Double, Altitude As Double) Implements ITelescopeV3.SlewToAltAzAsync
        TL.LogMessage("SlewToAltAzAsync", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SlewToAltAzAsync")
    End Sub

    Public Sub SlewToCoordinates(RightAscension As Double, Declination As Double) Implements ITelescopeV3.SlewToCoordinates
        TL.LogMessage("SlewToCoordinates", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SlewToCoordinates")
    End Sub

    Public Sub SlewToCoordinatesAsync(RightAscension As Double, Declination As Double) Implements ITelescopeV3.SlewToCoordinatesAsync
        TL.LogMessage("SlewToCoordinatesAsync", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SlewToCoordinatesAsync")
    End Sub

    Public Sub SlewToTarget() Implements ITelescopeV3.SlewToTarget
        TL.LogMessage("SlewToTarget", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SlewToTarget")
    End Sub

    Public Sub SlewToTargetAsync() Implements ITelescopeV3.SlewToTargetAsync
        TL.LogMessage("    Public Sub SlewToTargetAsync() Implements ITelescopeV3.SlewToTargetAsync", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SlewToTargetAsync")
    End Sub

    Public ReadOnly Property Slewing() As Boolean Implements ITelescopeV3.Slewing
        Get
            TL.LogMessage("Slewing Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Slewing", False)
        End Get
    End Property

    Public Sub SyncToAltAz(Azimuth As Double, Altitude As Double) Implements ITelescopeV3.SyncToAltAz
        TL.LogMessage("SyncToAltAz", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SyncToAltAz")
    End Sub

    Public Sub SyncToCoordinates(RightAscension As Double, Declination As Double) Implements ITelescopeV3.SyncToCoordinates
        TL.LogMessage("SyncToCoordinates", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SyncToCoordinates")
    End Sub

    Public Sub SyncToTarget() Implements ITelescopeV3.SyncToTarget
        TL.LogMessage("SyncToTarget", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SyncToTarget")
    End Sub

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

    Public Sub Unpark() Implements ITelescopeV3.Unpark
        TL.LogMessage("Unpark", "Not implemented")
        Throw New ASCOM.MethodNotImplementedException("Unpark")
    End Sub

#End Region

    '//ENDOFINSERTEDFILE
End Class