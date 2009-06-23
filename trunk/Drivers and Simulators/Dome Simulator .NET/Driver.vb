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

    ' Hand box

    Public Sub New()

        Dim RegVer As String = "1"                      ' Registry version, use to change registry if required by new version

        g_handBox = New HandboxForm
        g_Profile = New ASCOM.Helper.Profile
        g_Profile.DeviceType = "Dome"            ' Dome device type


        '
        ' initialize variables that are not persistent
        '

        g_Profile.Register(g_csDriverID, g_csDriverDescription) ' Self reg (skips if already reg)


        ' Now we have some default if required, update the handbox values from the registry
        g_handBox.UpdateConfig()

        ' Set handbox screen position
        g_handBox.Left = CInt(g_Profile.GetValue(g_csDriverID, "Left"))
        g_handBox.Top = CInt(g_Profile.GetValue(g_csDriverID, "Top"))

        ' Fix bad positions (which shouldn't ever happen, ha ha)
        If g_handBox.Left < 0 Then
            g_handBox.Left = 100
            g_Profile.WriteValue(g_csDriverID, "Left", g_handBox.Left.ToString)
        End If
        If g_handBox.Top < 0 Then
            g_handBox.Top = 100
            g_Profile.WriteValue(g_csDriverID, "Top", g_handBox.Top.ToString)
        End If

        ' Show the handbox now
        g_handBox.Show()
        g_handBox.Activate()
    End Sub


#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Dim P As New Helper.Profile()
        P.DeviceTypeV = "Dome"           '  Requires Helper 5.0.3 or later
        If bRegister Then
            P.Register(g_csDriverID, g_csDriverDescription)
        Else
            P.Unregister(g_csDriverID)
        End If
        Try                                 ' In case Helper becomes native .NET
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
            Connected = g_bConnected

            If Not g_show Is Nothing Then
                If g_show.chkConnected.Checked Then _
                    g_show.TrafficLine("Connected: " & Connected)
            End If
        End Get
        Set(ByVal value As Boolean)
            Dim out As String

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkOther.Checked Then _
                    g_trafficDialog.TrafficStart("Connected: " & g_handBox.Connected & " -> " & value)
            End If

            g_handBox.Connected = value
            out = " (done)"

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkOther.Checked Then _
                    g_trafficDialog.TrafficEnd(out)
            End If
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
        If Not g_show Is Nothing Then
            If g_show.chkOther.Checked Then _
                g_show.TrafficStart("SetupDialog")
        End If

        g_handBox.DoSetup()

        If Not g_show Is Nothing Then
            If g_show.chkOther.Checked Then _
                g_show.TrafficEnd(" (done)")
        End If
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
