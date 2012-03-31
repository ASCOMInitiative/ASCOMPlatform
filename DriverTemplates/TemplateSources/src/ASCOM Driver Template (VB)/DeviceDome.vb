' All lines from line 1 to the device interface implementation region will be discarded by the project wizard when the template is used
' Required code must lie within the device implementation region
' The //ENDOFINSERTEDFILE tag must be the last but one line in this file

Imports ASCOM.DeviceInterface
Imports ASCOM
Imports ASCOM.Utilities

Class DeviceDome
    Implements IDomeV2
    Private m_util As New Util()
    Private TL As New TraceLogger()

#Region "IDome Implementation"
    Public Sub AbortSlew() Implements IDomeV2.AbortSlew
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public ReadOnly Property Altitude() As Double Implements IDomeV2.Altitude
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements IDomeV2.AtHome
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements IDomeV2.AtPark
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Azimuth() As Double Implements IDomeV2.Azimuth
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanFindHome() As Boolean Implements IDomeV2.CanFindHome
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanPark() As Boolean Implements IDomeV2.CanPark
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetAltitude() As Boolean Implements IDomeV2.CanSetAltitude
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetAzimuth() As Boolean Implements IDomeV2.CanSetAzimuth
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements IDomeV2.CanSetPark
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSetShutter() As Boolean Implements IDomeV2.CanSetShutter
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSlave() As Boolean Implements IDomeV2.CanSlave
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public ReadOnly Property CanSyncAzimuth() As Boolean Implements IDomeV2.CanSyncAzimuth
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Sub CloseShutter() Implements IDomeV2.CloseShutter
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public Sub FindHome() Implements IDomeV2.FindHome
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public Sub OpenShutter() Implements IDomeV2.OpenShutter
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public Sub Park() Implements IDomeV2.Park
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public Sub SetPark() Implements IDomeV2.SetPark
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public ReadOnly Property ShutterStatus() As ShutterState Implements IDomeV2.ShutterStatus
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Property Slaved() As Boolean Implements IDomeV2.Slaved
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New ASCOM.PropertyNotImplementedException()
        End Set
    End Property

    Public Sub SlewToAltitude(Altitude As Double) Implements IDomeV2.SlewToAltitude
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public Sub SlewToAzimuth(Azimuth As Double) Implements IDomeV2.SlewToAzimuth
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

    Public ReadOnly Property Slewing() As Boolean Implements IDomeV2.Slewing
        Get
            Throw New ASCOM.PropertyNotImplementedException()
        End Get
    End Property

    Public Sub SyncToAzimuth(Azimuth As Double) Implements IDomeV2.SyncToAzimuth
        Throw New ASCOM.MethodNotImplementedException()
    End Sub

#End Region

    '//ENDOFINSERTEDFILE
End Class