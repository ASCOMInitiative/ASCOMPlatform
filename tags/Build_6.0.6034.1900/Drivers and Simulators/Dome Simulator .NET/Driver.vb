

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
' Your driver's ID is ASCOM.Simulator.Dome
'
' The Guid attribute sets the CLSID for ASCOM.DomeWheelSimulator.Dome
' The ClassInterface/None addribute prevents an empty interface called
' _Dome from being created and used as the [default] interface
'
Imports ASCOM.DeviceInterface
Imports System.Globalization

<Guid("70896ae0-b6c4-4303-a945-01219bf40bb4")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Dome
    Implements IDomeV2, IDisposable

#Region "New and IDisposable Support"

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")> _
    Public Sub New()

        Dim RegVer As String = "1"                      ' Registry version, use to change registry if required by new version

        If g_timer Is Nothing Then g_timer = New Windows.Forms.Timer
        If g_handBox Is Nothing Then g_handBox = New HandboxForm
        If g_Profile Is Nothing Then g_Profile = New Utilities.Profile
        g_Profile.DeviceType = "Dome"            ' Dome device type

        ' initialize variables that are not persistent
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
            g_Profile.WriteValue(g_csDriverID, "Left", g_handBox.Left.ToString(CultureInfo.InvariantCulture))
        End If
        If g_handBox.Top < 0 Then
            g_handBox.Top = 100
            g_Profile.WriteValue(g_csDriverID, "Top", g_handBox.Top.ToString(CultureInfo.InvariantCulture))
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
        g_handBox.RefreshLeds()

        ' Show the handbox now
        g_handBox.Show()
        g_handBox.Activate()
    End Sub

    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If Not g_TrafficForm Is Nothing Then
                    Try : g_TrafficForm.Close() : Catch : End Try
                    Try : g_TrafficForm.Dispose() : Catch : End Try
                    g_TrafficForm = Nothing
                End If
                If Not g_handBox Is Nothing Then
                    g_handBox.BeginInvoke(New Action(AddressOf g_handBox.Close))
                    g_handBox.BeginInvoke(New Action(AddressOf g_handBox.Dispose))
                    'Try : g_handBox.Close() : Catch : End Try
                    'Try : g_handBox.Dispose() : Catch : End Try
                    g_handBox = Nothing
                End If
                If Not g_timer Is Nothing Then
                    Try : g_timer.Enabled = False : Catch : End Try
                    Try : g_timer.Dispose() : Catch : End Try
                    g_timer = Nothing
                End If
                If Not g_Profile Is Nothing Then
                    Try : g_Profile.Dispose() : Catch : End Try
                    g_Profile = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose, IDomeV2.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Using P As New Utilities.Profile()
            P.DeviceType = "Dome"           '  Requires Helper 5.0.3 or later
            If bRegister Then
                P.Register(g_csDriverID, g_csDriverDescription)
            Else
                P.Unregister(g_csDriverID)
            End If
            P.Dispose()
            Try : Marshal.ReleaseComObject(P) : Catch : End Try ' In case Helper becomes native .NET
        End Using

    End Sub

    <ComRegisterFunction()> _
    Private Shared Sub RegisterASCOM(ByVal T As Type)
        RegUnregASCOM(True)
    End Sub

    <ComUnregisterFunction()> _
    Private Shared Sub UnregisterASCOM(ByVal T As Type)
        RegUnregASCOM(False)
    End Sub

#End Region

#Region "Private Methods"
    Private Shared Sub check_connected()
        If Not g_bConnected Then Throw New NotConnectedException("Dome simulator is not connected")
    End Sub

    Private Shared Sub check_Az(ByVal Az As Double)
        If Az = INVALID_COORDINATE Then Throw New InvalidValueException("Azimuth", "Invalid Coordinate", "0 to 360 degrees")
        If Az >= 360.0# Or Az < 0.0# Then Throw New InvalidValueException("Azimuth", Az.ToString, "0 to 360.0 degrees")
    End Sub

#End Region

#Region "IDomeV2 Implementation"
    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IDomeV2.Action
        Throw New MethodNotImplementedException("Action")
    End Function

    Public ReadOnly Property SupportedActions As ArrayList Implements IDomeV2.SupportedActions
        Get
            Return New ArrayList()
        End Get
    End Property

    Public ReadOnly Property DriverVersion As String Implements DeviceInterface.IDomeV2.DriverVersion
        Get
            Dim Ass As Reflection.Assembly

            Ass = Reflection.Assembly.GetExecutingAssembly 'Get our own assembly and report its version number
            Return Ass.GetName.Version.Major.ToString & "." & Ass.GetName.Version.Minor.ToString
        End Get
    End Property

#End Region

#Region "IDome Impelementation"

    Public Sub AbortSlew() Implements IDomeV2.AbortSlew
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkSlew.Checked Then _
                g_TrafficForm.TrafficStart("AbortSlew")
        End If

        check_connected()
        HW_Halt()

        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkSlew.Checked Then _
                g_TrafficForm.TrafficEnd(" (done)")
        End If
    End Sub

    Public ReadOnly Property Altitude() As Double Implements IDomeV2.Altitude
        Get
            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkShutter.Checked Then _
                    g_TrafficForm.TrafficStart("Altitude: ")
            End If

            If Not g_bCanSetAltitude Then Throw New PropertyNotImplementedException("Altitude", False)

            check_connected()

            If g_eShutterState = ShutterState.shutterError Then Err.Raise(SCODE_SHUTTER_ERROR, ERR_SOURCE, "Property Altitude: " & MSG_SHUTTER_ERROR)
            If g_eShutterState <> ShutterState.shutterOpen Then Err.Raise(SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, "Property Altitude: " & MSG_SHUTTER_NOT_OPEN)

            Altitude = g_dDomeAlt

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkShutter.Checked Then _
                    g_TrafficForm.TrafficEnd(Format$(Altitude, "0.0"))
            End If
        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements IDomeV2.AtHome
        Get
            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficStart("AtHome: ")
            End If

            check_connected()
            AtHome = g_bAtHome

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficEnd(CStr(AtHome))
            End If
        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements IDomeV2.AtPark
        Get
            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then _
                    g_TrafficForm.TrafficStart("AtPark: ")
            End If

            check_connected()
            AtPark = g_bAtPark

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficEnd(CStr(AtPark))
            End If
        End Get
    End Property

    Public ReadOnly Property Azimuth() As Double Implements IDomeV2.Azimuth
        Get
            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then _
                    g_TrafficForm.TrafficStart("Azimuth: ")
            End If

            If Not g_bCanSetAzimuth Then Throw New PropertyNotImplementedException("Azimuth", False)

            check_connected()
            Azimuth = g_dDomeAz

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficEnd(Format$(Azimuth, "0.0"))
            End If
        End Get
    End Property

    Public ReadOnly Property CanFindHome() As Boolean Implements IDomeV2.CanFindHome
        Get
            CanFindHome = g_bCanFindHome

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("CanFindHome: " & CanFindHome)
            End If
        End Get
    End Property

    Public ReadOnly Property CanPark() As Boolean Implements IDomeV2.CanPark
        Get
            CanPark = g_bCanPark

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("CanPark: " & CanPark)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSetAltitude() As Boolean Implements IDomeV2.CanSetAltitude
        Get
            CanSetAltitude = g_bCanSetAltitude

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("CanSetAltitude: " & CanSetAltitude)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSetAzimuth() As Boolean Implements IDomeV2.CanSetAzimuth
        Get
            CanSetAzimuth = g_bCanSetAzimuth

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("CanSetAzimuth: " & CanSetAzimuth)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements IDomeV2.CanSetPark
        Get
            CanSetPark = g_bCanSetPark

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("CanSetPark: " & CanSetPark)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSetShutter() As Boolean Implements IDomeV2.CanSetShutter
        Get
            CanSetShutter = g_bCanSetShutter

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("CanSetShutter: " & CanSetShutter)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSlave() As Boolean Implements IDomeV2.CanSlave
        Get
            CanSlave = False

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("CanSlave: " & CanSlave)
            End If
        End Get
    End Property

    Public ReadOnly Property CanSyncAzimuth() As Boolean Implements IDomeV2.CanSyncAzimuth
        Get
            CanSyncAzimuth = g_bCanSyncAzimuth

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("CanSyncAzimuth: " & CanSyncAzimuth)
            End If
        End Get
    End Property

    Public Sub CloseShutter() Implements IDomeV2.CloseShutter
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkShutter.Checked Then g_TrafficForm.TrafficStart("CloseShutter")
        End If

        If Not g_bCanSetShutter Then Throw New MethodNotImplementedException("CloseShutter")

        check_connected()
        HW_CloseShutter()

    End Sub

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IDomeV2.CommandBlind
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean Implements IDomeV2.CommandBool
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String Implements IDomeV2.CommandString
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements IDomeV2.Connected
        Get
            Connected = g_bConnected

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkConnected.Checked Then g_TrafficForm.TrafficLine("Connected: " & Connected)
            End If
        End Get

        Set(ByVal value As Boolean)
            Dim out As String

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficStart("Connected: " & g_bConnected & " -> " & value)
            End If

            g_bConnected = value
            g_timer.Enabled = value

            out = " (done)"

            If value Then
                g_handBox.Show()
                If Not g_TrafficForm Is Nothing Then g_TrafficForm.Show()
            Else
                If Not g_TrafficForm Is Nothing Then g_TrafficForm.Hide()
                g_handBox.Hide()
            End If


            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficEnd(out)
            End If
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IDomeV2.Description
        Get
            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficStart("Description")
            End If

            Description = INSTRUMENT_DESCRIPTION

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficEnd(" (done)")
            End If
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements IDomeV2.DriverInfo
        Get
            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficStart("DriverInfo")
            End If
            DriverInfo = "ASCOM Platform 6 Dome Simulator in VB.NET"
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

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficEnd(" (done)")
            End If
        End Get
    End Property

    Public Sub FindHome() Implements IDomeV2.FindHome
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkSlew.Checked Then g_TrafficForm.TrafficStart("FindHome")
        End If

        If Not g_bCanFindHome Then Throw New MethodNotImplementedException("FindHome")

        check_connected()
        If Not g_bAtHome Then _
            HW_FindHome()
    End Sub

    Public ReadOnly Property InterfaceVersion() As Short Implements IDomeV2.InterfaceVersion
        Get
            InterfaceVersion = 2

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("InterfaceVersion: " & InterfaceVersion)
            End If
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements IDomeV2.Name
        Get
            Name = INSTRUMENT_NAME

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("Name: " & Name)
            End If
        End Get
    End Property

    Public Sub OpenShutter() Implements IDomeV2.OpenShutter
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkShutter.Checked Then g_TrafficForm.TrafficStart("OpenShutter")
        End If

        If Not g_bCanSetShutter Then Throw New MethodNotImplementedException("OpenShutter")

        check_connected()

        If g_eShutterState = ShutterState.shutterError Then Err.Raise(SCODE_SHUTTER_ERROR, ERR_SOURCE, "Method OpenShutter: " & MSG_SHUTTER_ERROR)

        HW_OpenShutter()
    End Sub

    Public Sub Park() Implements IDomeV2.Park
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkSlew.Checked Then g_TrafficForm.TrafficStart("Park")
        End If

        If Not g_bCanPark Then Throw New MethodNotImplementedException("Park")

        check_connected()
        If Not g_bAtPark Then _
            HW_Park()
    End Sub

    Public Sub SetPark() Implements IDomeV2.SetPark
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficStart("SetPark: " & Format$(g_dDomeAz, "0.0"))
        End If

        If Not g_bCanSetPark Then Throw New MethodNotImplementedException("SetPark")

        check_connected()
        g_dSetPark = g_dDomeAz

        If Not g_bStandardAtPark Then               ' Non-standard, position
            g_bAtPark = True
            g_handBox.RefreshLeds()
        End If

        g_handBox.LabelButtons()

        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficEnd(" (done)")
        End If
    End Sub

    Public Sub SetupDialog() Implements IDomeV2.SetupDialog
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficStart("SetupDialog")
        End If

        g_handBox.DoSetup()

        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficEnd(" (done)")
        End If
    End Sub

    Public ReadOnly Property ShutterStatus() As ShutterState Implements IDomeV2.ShutterStatus
        Get
            Dim out As String

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkShutter.Checked Then g_TrafficForm.TrafficStart("ShutterStatus: ")
            End If

            If Not g_bCanSetShutter Then Throw New PropertyNotImplementedException("ShutterStatus", False)

            check_connected()
            ShutterStatus = g_eShutterState

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkShutter.Checked Then
                    out = "unknown"
                    Select Case ShutterStatus
                        Case ShutterState.shutterOpen : out = "Open"
                        Case ShutterState.shutterClosed : out = "Close"
                        Case ShutterState.shutterOpening : out = "Opening"
                        Case ShutterState.shutterClosing : out = "Closing"
                        Case ShutterState.shutterError : out = "Error"
                    End Select
                    g_TrafficForm.TrafficEnd(out)
                End If
            End If
        End Get
    End Property

    Public Property Slaved() As Boolean Implements IDomeV2.Slaved
        Get
            Slaved = False

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("Slaved: " & Slaved)
            End If
        End Get
        Set(ByVal value As Boolean)
            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkOther.Checked Then g_TrafficForm.TrafficLine("Slaved: " & False & " -> " & value)
            End If

            If value Then Throw New PropertyNotImplementedException("Slaved", True)

        End Set
    End Property

    Public ReadOnly Property Slewing() As Boolean Implements IDomeV2.Slewing
        Get
            check_connected()

            Slewing = HW_Slewing

            If Not g_TrafficForm Is Nothing Then
                If g_TrafficForm.chkSlewing.Checked Then g_TrafficForm.TrafficChar(IIf(Slewing, "Slewing: True", "Slewing: False"))
            End If
        End Get
    End Property

    Public Sub SlewToAltitude(ByVal Altitude As Double) Implements IDomeV2.SlewToAltitude
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkShutter.Checked Then g_TrafficForm.TrafficLine("SlewToAltitude:" & Format$(Altitude, "0.0"))
        End If

        If Not g_bCanSetAltitude Then Throw New MethodNotImplementedException("SlewToAltitude")
        
        check_connected()

        If g_eShutterState = ShutterState.shutterError Then Err.Raise(SCODE_SHUTTER_ERROR, ERR_SOURCE, "Method SlewToAltitude " & MSG_SHUTTER_ERROR)
        If g_eShutterState <> ShutterState.shutterOpen Then Err.Raise(SCODE_SHUTTER_NOT_OPEN, ERR_SOURCE, "Method SlewToAltitude " & MSG_SHUTTER_NOT_OPEN)
        If Altitude < g_dMinAlt Or Altitude > g_dMaxAlt Then Throw New InvalidValueException("SlewToAltitude", Altitude.ToString, g_dMinAlt.ToString & " to " & g_dMaxAlt.ToString)

        HW_MoveShutter(Altitude)
    End Sub

    Public Sub SlewToAzimuth(ByVal Azimuth As Double) Implements IDomeV2.SlewToAzimuth
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkSlew.Checked Then g_TrafficForm.TrafficLine("SlewToAzimuth: " & Format$(Azimuth, "0.0"))
        End If

        If Not g_bCanSetAzimuth Then Throw New MethodNotImplementedException("SlewToAzimuth")

        check_connected()
        check_Az(Azimuth)
        HW_Move(Azimuth)
    End Sub

    Public Sub SyncToAzimuth(ByVal Azimuth As Double) Implements IDomeV2.SyncToAzimuth
        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkSlew.Checked Then g_TrafficForm.TrafficLine("SyncToAzimuth: " & Format$(Azimuth, "0.0"))
        End If

        If Not g_bCanSyncAzimuth Then Throw New MethodNotImplementedException("SyncToAzimuth")

        check_connected()
        check_Az(Azimuth)
        HW_Sync(Azimuth)

        If Not g_TrafficForm Is Nothing Then
            If g_TrafficForm.chkSlew.Checked Then _
                g_TrafficForm.TrafficEnd(" (done)")
        End If
    End Sub
#End Region

End Class
