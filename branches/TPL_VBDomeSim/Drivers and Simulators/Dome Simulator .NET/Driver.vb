'tabs=4
' --------------------------------------------------------------------------------
'
' ASCOM Dome driver for DomeSimulator
'
' Description:	A port of the VB6 ASCOM Dome simulator to VB.Net.
'               Converted and built in Visual Studio 2008.
'
'
'
' Implements:	ASCOM Dome interface version: 5.1.0
' Author:		Robert Turner <rbturner@charter.net>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' 22-Jun-2009	rbt	1.0.0	Initial edit, from Dome template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.FilterWheelSimulator.FilterWheel
'
' The Guid attribute sets the CLSID for ASCOM.DomeWheelSimulator.Dome
' The ClassInterface/None addribute prevents an empty interface called
' _Dome from being created and used as the [default] interface
'
<Guid("70896ae0-b6c4-4303-a945-01219bf40bb4")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Dome
    Implements IDome


    Public Sub AbortSlew() Implements IDome.AbortSlew

    End Sub

    Public ReadOnly Property Altitude() As Double Implements IDome.Altitude
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements IDome.AtHome
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements IDome.AtPark
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property Azimuth() As Double Implements IDome.Azimuth
        Get
            Return 0
        End Get
    End Property

    Public ReadOnly Property CanFindHome() As Boolean Implements IDome.CanFindHome
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanPark() As Boolean Implements IDome.CanPark
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSetAltitude() As Boolean Implements IDome.CanSetAltitude
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSetAzimuth() As Boolean Implements IDome.CanSetAzimuth
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements IDome.CanSetPark
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSetShutter() As Boolean Implements IDome.CanSetShutter
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSlave() As Boolean Implements IDome.CanSlave
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSyncAzimuth() As Boolean Implements IDome.CanSyncAzimuth
        Get
            Return True
        End Get
    End Property

    Public Sub CloseShutter() Implements IDome.CloseShutter

    End Sub

    Public Sub CommandBlind(ByVal Command As String) Implements IDome.CommandBlind

    End Sub

    Public Function CommandBool(ByVal Command As String) As Boolean Implements IDome.CommandBool
        Return True
    End Function

    Public Function CommandString(ByVal Command As String) As String Implements IDome.CommandString
        Return ""
    End Function

    Public Property Connected() As Boolean Implements IDome.Connected
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IDome.Description
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements IDome.DriverInfo
        Get
            Return ""
        End Get
    End Property

    Public Sub FindHome() Implements IDome.FindHome

    End Sub

    Public ReadOnly Property InterfaceVersion() As Short Implements IDome.InterfaceVersion
        Get

        End Get
    End Property

    Public ReadOnly Property Name() As String Implements IDome.Name
        Get
            Return ""
        End Get
    End Property

    Public Sub OpenShutter() Implements IDome.OpenShutter

    End Sub

    Public Sub Park() Implements IDome.Park

    End Sub

    Public Sub SetPark() Implements IDome.SetPark

    End Sub

    Public Sub SetupDialog() Implements IDome.SetupDialog

    End Sub

    Public ReadOnly Property ShutterStatus() As ShutterState Implements IDome.ShutterStatus
        Get
            Return ShutterState.shutterClosed
        End Get
    End Property

    Public Property Slaved() As Boolean Implements IDome.Slaved
        Get
            Return True
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public ReadOnly Property Slewing() As Boolean Implements IDome.Slewing
        Get
            Return True
        End Get
    End Property

    Public Sub SlewToAltitude(ByVal Altitude As Double) Implements IDome.SlewToAltitude

    End Sub

    Public Sub SlewToAzimuth(ByVal Azimuth As Double) Implements IDome.SlewToAzimuth

    End Sub

    Public Sub SyncToAzimuth(ByVal Azimuth As Double) Implements IDome.SyncToAzimuth

    End Sub
End Class
