'tabs=4
' --------------------------------------------------------------------------------
'
' ASCOM FilterWheel driver for FilterWheelSimulator
'
' Description:	A port of the VB6 ASCOM Filterwheel simulator to VB.Net.
'               Converted and built in Visual Studio 2008.
'               The port leaves some messy code - it could really do with
'               a ground up re-write!
'
' Implements:	ASCOM FilterWheel interface version: 5.1.0
' Author:		Mark Crossley <mark@markcrossley.co.uk>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' 06-Jun-2009	mpc	1.0.0	Initial edit, from FilterWheel template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.FilterWheelSimulator.FilterWheel
'
' The Guid attribute sets the CLSID for ASCOM.FilterWheelSimulator.FilterWheel
' The ClassInterface/None addribute prevents an empty interface called
' _FilterWheel from being created and used as the [default] interface
'
<Guid("F9043C88-F6F2-101A-A3C9-08002B2F49FC")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class FilterWheel
    '	==========
    Implements IFilterWheel ' Early-bind interface implemented by this driver
    '	==========

    Private m_handBox As HandboxForm                     ' Hand box
    Private m_bImplementsNames As Boolean
    Private m_bImplementsOffsets As Boolean

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()

        Dim i As Integer
        Dim RegVer As String = "1"                      ' Registry version, use to change registry if required by new version

        m_handBox = New HandboxForm
        g_Profile = New ASCOM.Helper.Profile
        g_Profile.DeviceType = "FilterWheel"            ' We're a filter wheel driver


        '
        ' initialize variables that are not persistent
        '
        Dim rand As Random = New Random

        g_Profile.Register(g_csDriverID, g_csDriverDescription) ' Self reg (skips if already reg)

        '
        ' Persistent settings - Create on first start as determined by
        ' existence of the RegVer key, Increment RegVer if we need to change registry settings
        ' in a new version of the driver
        '
        If g_Profile.GetValue(g_csDriverID, "RegVer") <> RegVer Then
            ' Create some 'realistic' defaults
            Dim colours() As System.Drawing.Color = New Drawing.Color() {Drawing.Color.Red, Drawing.Color.Green, _
                                                                         Drawing.Color.Blue, Drawing.Color.Gray, _
                                                                         Drawing.Color.DarkRed, Drawing.Color.Teal, _
                                                                         Drawing.Color.Violet, Drawing.Color.Black}
            Dim names() As String = New String() {"Red", "Green", "Blue", "Clear", "Ha", "OIII", "LPR", "Dark"}

            g_Profile.WriteValue(g_csDriverID, "RegVer", RegVer)
            g_Profile.WriteValue(g_csDriverID, "Position", "0")
            g_Profile.WriteValue(g_csDriverID, "Slots", "4")
            g_Profile.WriteValue(g_csDriverID, "Time", "1000")
            g_Profile.WriteValue(g_csDriverID, "ImplementsNames", "True")
            g_Profile.WriteValue(g_csDriverID, "ImplementsOffsets", "True")
            g_Profile.WriteValue(g_csDriverID, "AlwaysOnTop", "True")
            g_Profile.WriteValue(g_csDriverID, "Left", "100")
            g_Profile.WriteValue(g_csDriverID, "Top", "100")
            For i = 0 To 7
                g_Profile.WriteValue(g_csDriverID, i.ToString, names(i), "FilterNames")
                g_Profile.WriteValue(g_csDriverID, i.ToString, rand.Next(10000).ToString, "FocusOffsets")
                g_Profile.WriteValue(g_csDriverID, i.ToString, System.Drawing.ColorTranslator.ToWin32(colours(i)).ToString, "FilterColours")
            Next i
        End If

        m_bImplementsNames = CBool(g_Profile.GetValue(g_csDriverID, "ImplementsNames"))
        m_bImplementsOffsets = CBool(g_Profile.GetValue(g_csDriverID, "ImplementsOffsets"))

        ' Now we have some default if required, update the handbox values from the registry
        m_handBox.UpdateConfig()

        ' Set handbox screen position
        m_handBox.Left = CInt(g_Profile.GetValue(g_csDriverID, "Left"))
        m_handBox.Top = CInt(g_Profile.GetValue(g_csDriverID, "Top"))

        ' Fix bad positions (which shouldn't ever happen, ha ha)
        If m_handBox.Left < 0 Then
            m_handBox.Left = 100
            g_Profile.WriteValue(g_csDriverID, "Left", m_handBox.Left.ToString)
        End If
        If m_handBox.Top < 0 Then
            m_handBox.Top = 100
            g_Profile.WriteValue(g_csDriverID, "Top", m_handBox.Top.ToString)
        End If

        ' Show the handbox now
        m_handBox.Show()
        m_handBox.Activate()
        ' And start the Timer
        m_handBox.Timer.Enabled = True

    End Sub

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Dim P As New Helper.Profile()
        P.DeviceTypeV = "FilterWheel"           '  Requires Helper 5.0.3 or later
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

    '
    ' PUBLIC COM INTERFACE IFilterWheel IMPLEMENTATION
    '

#Region "IFilterWheel Members"

    Public Property Connected() As Boolean Implements IFilterWheel.Connected
        Get
            Connected = m_handBox.Connected

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkOther.Checked Then _
                    g_trafficDialog.TrafficLine("Connected: " & m_handBox.Connected)
            End If
        End Get
        Set(ByVal value As Boolean)
            Dim out As String

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkOther.Checked Then _
                    g_trafficDialog.TrafficStart("Connected: " & m_handBox.Connected & " -> " & value)
            End If

            m_handBox.Connected = value
            out = " (done)"

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkOther.Checked Then _
                    g_trafficDialog.TrafficEnd(out)
            End If
        End Set
    End Property

    Public Property Position() As Short Implements IFilterWheel.Position
        Get
            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkPosition.Checked Then _
                    g_trafficDialog.TrafficStart("Position: ")
            End If

            check_connected()

            If m_handBox.Moving Then   ' Spec. says we must return -1 is position not determined
                Position = -1
            Else
                Position = m_handBox.Position
            End If

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkPosition.Checked Then _
                    g_trafficDialog.TrafficEnd(Position.ToString)
            End If
        End Get
        Set(ByVal value As Short)

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkPosition.Checked Then _
                    g_trafficDialog.TrafficStart("Position: " & value)
            End If

            check_connected()

            ' check if we are already there!
            If value = m_handBox.Position Then
                If Not g_trafficDialog Is Nothing Then
                    If g_trafficDialog.chkPosition.Checked Then _
                        g_trafficDialog.TrafficEnd(" (no move required)")
                End If
                Exit Property
            End If
            ' Range checking is done in the Handbox
            m_handBox.Position = value

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkMoving.Checked Then _
                    g_trafficDialog.TrafficEnd(" (started)")
            End If
        End Set
    End Property

    Public ReadOnly Property FocusOffsets() As Integer() Implements IFilterWheel.FocusOffsets
        Get

            check_connected()

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkName.Checked Then _
                    g_trafficDialog.TrafficLine("FocusOffsets: ")
            End If

            If m_bImplementsOffsets Then
                FocusOffsets = m_handBox.FocusOffsets
            Else
                Throw New PropertyNotImplementedException("FocusOffsets", False)
            End If

        End Get
    End Property

    Public ReadOnly Property Names() As String() Implements IFilterWheel.Names
        Get
            Dim i As Integer
            Dim temp(7) As String

            temp = m_handBox.FilterNames

            check_connected()

            If Not g_trafficDialog Is Nothing Then
                If g_trafficDialog.chkName.Checked Then _
                    g_trafficDialog.TrafficLine("Names: ")
            End If

            'If m_bImplementsNames Then
            For i = 0 To m_handBox.Slots - 1
                If Not m_bImplementsNames Then
                    temp(i) = "Filter " & i + 1         ' Spec. says we return "Filter 1" etc if names not supported
                End If
                If Not g_trafficDialog Is Nothing Then
                    If g_trafficDialog.chkName.Checked Then _
                        g_trafficDialog.TrafficLine("  Filter(" & i & ") = " & temp(i))
                End If
            Next

            Names = temp
            ' Else
            'Throw New PropertyNotImplementedException("Names", False)
            'End If

        End Get
    End Property

    Public Sub SetupDialog() Implements IFilterWheel.SetupDialog

        ' Do we need to log this?
        If Not g_trafficDialog Is Nothing Then
            If g_trafficDialog.chkOther.Checked = True Then _
                g_trafficDialog.TrafficStart("SetupDialog")
        End If

        m_handBox.DoSetup()

        ' Do we need to log this?
        If Not g_trafficDialog Is Nothing Then
            If g_trafficDialog.chkOther.Checked = True Then _
                g_trafficDialog.TrafficEnd(" (done)")
        End If

    End Sub
#End Region


    '---------------------------------------------------------------------
    '
    ' check_connected() - Raise an error if the focuser is not connected
    '
    '---------------------------------------------------------------------
    Private Sub check_connected()

        If Not m_handBox.Connected Then _
        Throw New DriverException(MSG_NOT_CONNECTED, SCODE_NOT_CONNECTED)

    End Sub

  
End Class
