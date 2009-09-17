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



        g_Profile = New HelperNET.Profile
        g_Profile.DeviceType = "Dome"            ' Dome device type


        '
        ' initialize variables that are not persistent
        '

        g_Profile.Register(g_csDriverID, g_csDriverDescription) ' Self reg (skips if already reg)


        ' Set handbox screen position
        Try
            g_handBox.Left = CInt(g_Profile.GetValue(g_csDriverID, "Left"))
            g_handBox.Top = CInt(g_Profile.GetValue(g_csDriverID, "Top"))
        Catch ex As Exception

        End Try


        ' Fix bad positions (which shouldn't ever happen, ha ha)
        If g_handBox.Left < 0 Then
            g_handBox.Left = 100
            g_Profile.WriteValue(g_csDriverID, "Left", g_handBox.Left.ToString)
        End If
        If g_handBox.Top < 0 Then
            g_handBox.Top = 100
            g_Profile.WriteValue(g_csDriverID, "Top", g_handBox.Top.ToString)
        End If

        g_dOCProgress = 0
        g_dOCDelay = 0

        '
        ' Persistent settings - Create on first start
        '
        If g_Profile.GetValue(ID, "RegVer") <> RegVer Then
            g_Profile.WriteValue(ID, "RegVer", RegVer)

            g_Profile.WriteValue(ID, "OCDelay", "3")
            g_Profile.WriteValue(ID, "SetPark", "180")
            g_Profile.WriteValue(ID, "SetHome", "0")
            g_Profile.WriteValue(ID, "AltRate", "30")
            g_Profile.WriteValue(ID, "AzRate", "15")
            g_Profile.WriteValue(ID, "StepSize", "5")
            g_Profile.WriteValue(ID, "MaxAlt", "90")
            g_Profile.WriteValue(ID, "MinAlt", "0")
            g_Profile.WriteValue(ID, "StartShutterError", "False")
            g_Profile.WriteValue(ID, "SlewingOpenClose", "False")
            g_Profile.WriteValue(ID, "NonFragileAtHome", "False")
            g_Profile.WriteValue(ID, "NonFragileAtPark", "False")

            g_Profile.WriteValue(ID, "DomeAz", CStr(INVALID_COORDINATE), "State")
            g_Profile.WriteValue(ID, "DomeAlt", CStr(INVALID_COORDINATE), "State")
            g_Profile.WriteValue(ID, "ShutterState", "1", "State")       ' ShutterClosed

            g_Profile.WriteValue(ID, "Left", "100")
            g_Profile.WriteValue(ID, "Top", "100")

            g_Profile.WriteValue(ID, "CanFindHome", "True", "Capabilities")
            g_Profile.WriteValue(ID, "CanPark", "True", "Capabilities")
            g_Profile.WriteValue(ID, "CanSetAltitude", "True", "Capabilities")
            g_Profile.WriteValue(ID, "CanSetAzimuth", "True", "Capabilities")
            g_Profile.WriteValue(ID, "CanSetPark", "True", "Capabilities")
            g_Profile.WriteValue(ID, "CanSetShutter", "True", "Capabilities")
            g_Profile.WriteValue(ID, "CanSyncAzimuth", "True", "Capabilities")
        End If

        g_dOCDelay = CDbl(g_Profile.GetValue(ID, "OCDelay"))
        g_dSetPark = CDbl(g_Profile.GetValue(ID, "SetPark"))
        g_dSetHome = CDbl(g_Profile.GetValue(ID, "SetHome"))
        g_dAltRate = CDbl(g_Profile.GetValue(ID, "AltRate"))
        g_dAzRate = CDbl(g_Profile.GetValue(ID, "AzRate"))
        g_dStepSize = CDbl(g_Profile.GetValue(ID, "StepSize"))
        g_dMaxAlt = CDbl(g_Profile.GetValue(ID, "MaxAlt"))
        g_dMinAlt = CDbl(g_Profile.GetValue(ID, "MinAlt"))
        g_bStartShutterError = CBool(g_Profile.GetValue(ID, "StartShutterError"))
        g_bSlewingOpenClose = CBool(g_Profile.GetValue(ID, "SlewingOpenClose"))
        g_bStandardAtHome = Not CBool(g_Profile.GetValue(ID, "NonFragileAtHome"))
        g_bStandardAtPark = Not CBool(g_Profile.GetValue(ID, "NonFragileAtPark"))

        g_bCanFindHome = CBool(g_Profile.GetValue(ID, "CanFindHome", "Capabilities"))
        g_bCanPark = CBool(g_Profile.GetValue(ID, "CanPark", "Capabilities"))
        g_bCanSetAltitude = CBool(g_Profile.GetValue(ID, "CanSetAltitude", "Capabilities"))
        g_bCanSetAzimuth = CBool(g_Profile.GetValue(ID, "CanSetAzimuth", "Capabilities"))
        g_bCanSetPark = CBool(g_Profile.GetValue(ID, "CanSetPark", "Capabilities"))
        g_bCanSetShutter = CBool(g_Profile.GetValue(ID, "CanSetShutter", "Capabilities"))
        g_bCanSyncAzimuth = CBool(g_Profile.GetValue(ID, "CanSyncAzimuth", "Capabilities"))

        ' get and range dome state
        g_dDomeAz = CDbl(g_Profile.GetValue(ID, "DomeAz", "State"))
        g_dDomeAlt = CDbl(g_Profile.GetValue(ID, "DomeAlt", "State"))
        If g_dDomeAlt < g_dMinAlt Then _
            g_dDomeAlt = g_dMinAlt
        If g_dDomeAlt > g_dMaxAlt Then _
            g_dDomeAlt = g_dMaxAlt
        If g_dDomeAz < 0 Or g_dDomeAz >= 360 Then _
            g_dDomeAz = g_dSetPark
        g_dTargetAlt = g_dDomeAlt
        g_dTargetAz = g_dDomeAz

        If g_bStartShutterError Then
            g_eShutterState = ShutterState.shutterError
        Else
            g_eShutterState = CDbl(g_Profile.GetValue(ID, "ShutterState", "State"))
        End If

        g_eSlewing = Going.slewNowhere
        g_bAtPark = HW_AtPark                   ' its OK to wake up parked
        If g_bStandardAtHome Then
            g_bAtHome = False                   ' Standard: home is set by home() method, never wake up homed!
        Else
            g_bAtHome = HW_AtHome               ' Non standard, position, ok to wake up homed
        End If

        g_timer.Interval = TIMER_INTERVAL * 1000
        
        g_handBox.LabelButtons()
        g_handBox.RefreshLEDs()

        ' Show the handbox now
        g_handBox.Show()
        g_handBox.Activate()
    End Sub


#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Dim P As New HelperNET.Profile()
        P.DeviceType = "Dome"           '  Requires Helper 5.0.3 or later
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

#Region "Private Methods"
    Private Sub check_connected()

        If Not g_bConnected Then _
            Err.Raise(SCODE_NOT_CONNECTED, ERR_SOURCE, MSG_NOT_CONNECTED)

    End Sub
    Private Sub check_Az(ByVal Az As Double)

        If Az = INVALID_COORDINATE Then _
            Err.Raise(SCODE_NO_TARGET_COORDS, ERR_SOURCE, _
                "Azimuth " & MSG_NO_TARGET_COORDS)

        'pwgs changed next line from If Az > 360 Or Az < -360 Then
        If Az >= 360.0# Or Az < 0.0# Then _
            Err.Raise(SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
                "Azimuth " & MSG_VAL_OUTOFRANGE)

    End Sub

    
#End Region

#Region "IDome Impelementation"

    Public Sub AbortSlew() Implements IDome.AbortSlew
        If Not g_show Is Nothing Then
            If g_show.chkSlew.Checked Then _
                g_show.TrafficStart("AbortSlew")
        End If

        check_connected()
        HW_Halt()

        If Not g_show Is Nothing Then
            If g_show.chkSlew.Checked Then _
                g_show.TrafficEnd(" (done)")
        End If
    End Sub

    Public ReadOnly Property Altitude() As Double Implements IDome.Altitude
        Get
            If Not g_show Is Nothing Then
                If g_show.chkShutter.Checked Then _
                    g_show.TrafficStart("Altitude: ")
            End If

            If Not g_bCanSetAltitude Then _
                Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Property Altitude" & MSG_NOT_IMPLEMENTED)

            check_connected()

            If g_eShutterState = ShutterState.shutterError Then _
                Err.Raise(SCODE_SHUTTER_ERROR, ERR_SOURCE, _
                    "Property Altitude: " & MSG_SHUTTER_ERROR)

            If g_eShutterState <> ShutterState.shutterOpen Then _
                Err.Raise(SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, _
                    "Property Altitude: " & MSG_SHUTTER_NOT_OPEN)

            Altitude = g_dDomeAlt

            If Not g_show Is Nothing Then
                If g_show.chkShutter.Checked Then _
                    g_show.TrafficEnd(Format$(Altitude, "0.0"))
            End If
        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements IDome.AtHome
        Get
            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficStart("AtHome: ")
            End If

            check_connected()
            AtHome = g_bAtHome

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficEnd(CStr(AtHome))
            End If
        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements IDome.AtPark
        Get
            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficStart("AtPark: ")
            End If

            check_connected()
            AtPark = g_bAtPark

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficEnd(CStr(AtPark))
            End If
        End Get
    End Property

    Public ReadOnly Property Azimuth() As Double Implements IDome.Azimuth
        Get
            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficStart("Azimuth: ")
            End If

            If Not g_bCanSetAzimuth Then _
                Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Property Azimuth" & MSG_NOT_IMPLEMENTED)

            check_connected()
            Azimuth = g_dDomeAz

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficEnd(Format$(Azimuth, "0.0"))
            End If
        End Get
    End Property

    Public ReadOnly Property CanFindHome() As Boolean Implements IDome.CanFindHome
        Get
            CanFindHome = g_bCanFindHome

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("CanFindHome: " & CanFindHome)
            End If
        End Get
    End Property

    Public ReadOnly Property CanPark() As Boolean Implements IDome.CanPark
        Get
            CanPark = g_bCanPark

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("CanPark: " & CanPark)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSetAltitude() As Boolean Implements IDome.CanSetAltitude
        Get
            CanSetAltitude = g_bCanSetAltitude

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("CanSetAltitude: " & CanSetAltitude)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSetAzimuth() As Boolean Implements IDome.CanSetAzimuth
        Get
            CanSetAzimuth = g_bCanSetAzimuth

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("CanSetAzimuth: " & CanSetAzimuth)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements IDome.CanSetPark
        Get
            CanSetPark = g_bCanSetPark

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("CanSetPark: " & CanSetPark)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSetShutter() As Boolean Implements IDome.CanSetShutter
        Get
            CanSetShutter = g_bCanSetShutter

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("CanSetShutter: " & CanSetShutter)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSlave() As Boolean Implements IDome.CanSlave
        Get
            CanSlave = False

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("CanSlave: " & CanSlave)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSyncAzimuth() As Boolean Implements IDome.CanSyncAzimuth
        Get
            CanSyncAzimuth = g_bCanSyncAzimuth

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("CanSyncAzimuth: " & CanSyncAzimuth)
            End If
        End Get
    End Property

    Public Sub CloseShutter() Implements IDome.CloseShutter
        If Not g_show Is Nothing Then
            If g_show.chkShutter.Checked Then _
                g_show.TrafficStart("CloseShutter")
        End If

        If Not g_bCanSetShutter Then _
            Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method CloseShutter" & MSG_NOT_IMPLEMENTED)

        check_connected()
        HW_CloseShutter()

    End Sub

    Public Sub CommandBlind(ByVal Command As String) Implements IDome.CommandBlind
        Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandBlind" & MSG_NOT_IMPLEMENTED)
    End Sub

    Public Function CommandBool(ByVal Command As String) As Boolean Implements IDome.CommandBool
        Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandBool" & MSG_NOT_IMPLEMENTED)
    End Function

    Public Function CommandString(ByVal Command As String) As String Implements IDome.CommandString
        Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
            "Method CommandString" & MSG_NOT_IMPLEMENTED)
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
                    g_trafficDialog.TrafficStart("Connected: " & g_bConnected & " -> " & value)
            End If

            g_bConnected = value
            g_timer.Enabled = value

            out = " (done)"

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkOther.Checked Then _
                    g_trafficDialog.TrafficEnd(out)
            End If
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IDome.Description
        Get
            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficStart("Description")
            End If

            Description = INSTRUMENT_DESCRIPTION

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficEnd(" (done)")
            End If
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements IDome.DriverInfo
        Get
            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficStart("DriverInfo")
            End If
            DriverInfo = ""
            '
            ' Use the Project/Properties sheet, Make tab, to set these
            ' items. That way they will show in the Version tab of the
            ' Explorer property sheet, and the exact same data will
            ' show in Dome.DriverInfo.
            '
            'DriverInfo = application.FileDescription & " " & _
            '            App.Major & "." & App.Minor & "." & App.Revision
            'If App.CompanyName <> "" Then _
            '    DriverInfo = DriverInfo & vbCrLf & App.CompanyName
            'If App.LegalCopyright <> "" Then _
            '    DriverInfo = DriverInfo & vbCrLf & App.LegalCopyright
            'If App.Comments <> "" Then _
            '    DriverInfo = DriverInfo & vbCrLf & App.Comments

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficEnd(" (done)")
            End If
        End Get
    End Property

    Public Sub FindHome() Implements IDome.FindHome
        If Not g_show Is Nothing Then
            If g_show.chkSlew.Checked Then _
                g_show.TrafficStart("FindHome")
        End If

        If Not g_bCanFindHome Then _
            Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method FindHome" & MSG_NOT_IMPLEMENTED)

        check_connected()
        If Not g_bAtHome Then _
            HW_FindHome()
    End Sub

    Public ReadOnly Property InterfaceVersion() As Short Implements IDome.InterfaceVersion
        Get
            InterfaceVersion = 1

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("InterfaceVersion: " & InterfaceVersion)
            End If
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements IDome.Name
        Get
            Name = INSTRUMENT_NAME

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("Name: " & Name)
            End If
        End Get
    End Property

    Public Sub OpenShutter() Implements IDome.OpenShutter
        If Not g_show Is Nothing Then
            If g_show.chkShutter.Checked Then _
                g_show.TrafficStart("OpenShutter")
        End If

        If Not g_bCanSetShutter Then _
            Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method OpenShutter" & MSG_NOT_IMPLEMENTED)

        check_connected()

        If g_eShutterState = ShutterState.shutterError Then _
            Err.Raise(SCODE_SHUTTER_ERROR, ERR_SOURCE, _
                "Method OpenShutter: " & MSG_SHUTTER_ERROR)

        HW_OpenShutter()
    End Sub

    Public Sub Park() Implements IDome.Park
        If Not g_show Is Nothing Then
            If g_show.chkSlew.Checked Then _
                g_show.TrafficStart("Park")
        End If

        If Not g_bCanPark Then _
            Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method Park" & MSG_NOT_IMPLEMENTED)

        check_connected()
        If Not g_bAtPark Then _
            HW_Park()
    End Sub

    Public Sub SetPark() Implements IDome.SetPark
        If Not g_show Is Nothing Then
            If g_show.chkOther.Checked Then _
                g_show.TrafficStart("SetPark: " & Format$(g_dDomeAz, "0.0"))
        End If

        If Not g_bCanSetPark Then _
            Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method Park" & MSG_NOT_IMPLEMENTED)

        check_connected()
        g_dSetPark = g_dDomeAz

        If Not g_bStandardAtPark Then               ' Non-standard, position
            g_bAtPark = True
            g_handBox.RefreshLEDs()
        End If

        g_handBox.LabelButtons()

        If Not g_show Is Nothing Then
            If g_show.chkOther.Checked Then _
                g_show.TrafficEnd(" (done)")
        End If
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
            Dim out As String

            If Not g_show Is Nothing Then
                If g_show.chkShutter.Checked Then _
                    g_show.TrafficStart("ShutterStatus: ")
            End If

            If Not g_bCanSetShutter Then _
                Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Property ShutterStatus" & MSG_NOT_IMPLEMENTED)

            check_connected()
            ShutterStatus = g_eShutterState

            If Not g_show Is Nothing Then
                If g_show.chkShutter.Checked Then
                    out = "unknown"
                    Select Case ShutterStatus
                        Case ShutterState.shutterOpen : out = "Open"
                        Case ShutterState.shutterClosed : out = "Close"
                        Case ShutterState.shutterOpening : out = "Opening"
                        Case ShutterState.shutterClosing : out = "Closing"
                        Case ShutterState.shutterError : out = "Error"
                    End Select
                    g_show.TrafficEnd(out)
                End If
            End If
        End Get
    End Property

    Public Property Slaved() As Boolean Implements IDome.Slaved
        Get
            Slaved = False

            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("Slaved: " & Slaved)
            End If
        End Get
        Set(ByVal value As Boolean)
            If Not g_show Is Nothing Then
                If g_show.chkOther.Checked Then _
                    g_show.TrafficLine("Slaved: " & False & " -> " & value)
            End If

            If value Then
                Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                    "Slaving Dome" & MSG_NOT_IMPLEMENTED)
            End If
        End Set
    End Property

    Public ReadOnly Property Slewing() As Boolean Implements IDome.Slewing
        Get
            check_connected()

            Slewing = HW_Slewing

            If Not g_show Is Nothing Then
                If g_show.chkSlewing.Checked Then _
                    g_show.TrafficChar( _
                        IIf(Slewing, "Slewing: True", "Slewing: False"))
            End If
        End Get
    End Property

    Public Sub SlewToAltitude(ByVal Altitude As Double) Implements IDome.SlewToAltitude
        If Not g_show Is Nothing Then
            If g_show.chkShutter.Checked Then _
                g_show.TrafficLine("SlewToAltitude:" & Format$(Altitude, "0.0"))
        End If

        If Not g_bCanSetAltitude Then _
            Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method SlewToAltitude" & MSG_NOT_IMPLEMENTED)

        check_connected()

        If g_eShutterState = ShutterState.shutterError Then _
            Err.Raise(SCODE_SHUTTER_ERROR, ERR_SOURCE, _
                "Method SlewToAltitude " & MSG_SHUTTER_ERROR)

        If g_eShutterState <> ShutterState.shutterOpen Then _
            Err.Raise(SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, _
                "Method SlewToAltitude " & MSG_SHUTTER_NOT_OPEN)

        If Altitude < g_dMinAlt Or Altitude > g_dMaxAlt Then _
             Err.Raise(SCODE_VAL_OUTOFRANGE, ERR_SOURCE, _
                "Altitude " & MSG_VAL_OUTOFRANGE)

        HW_MoveShutter(Altitude)
    End Sub

    Public Sub SlewToAzimuth(ByVal Azimuth As Double) Implements IDome.SlewToAzimuth
        If Not g_show Is Nothing Then
            If g_show.chkSlew.Checked Then _
                g_show.TrafficLine("SlewToAzimuth: " & Format$(Azimuth, "0.0"))
        End If

        If Not g_bCanSetAzimuth Then _
            Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method SlewToAzimuth" & MSG_NOT_IMPLEMENTED)

        check_connected()
        check_Az(Azimuth)
        HW_Move(Azimuth)
    End Sub

    Public Sub SyncToAzimuth(ByVal Azimuth As Double) Implements IDome.SyncToAzimuth
        If Not g_show Is Nothing Then
            If g_show.chkSlew.Checked Then _
                g_show.TrafficLine("SyncToAzimuth: " & Format$(Azimuth, "0.0"))
        End If

        If Not g_bCanSyncAzimuth Then _
            Err.Raise(SCODE_NOT_IMPLEMENTED, ERR_SOURCE, _
                "Method SyncToAzimuth" & MSG_NOT_IMPLEMENTED)

        check_connected()
        check_Az(Azimuth)
        HW_Sync(Azimuth)

        If Not g_show Is Nothing Then
            If g_show.chkSlew.Checked Then _
                g_show.TrafficEnd(" (done)")
        End If
    End Sub
#End Region
End Class
