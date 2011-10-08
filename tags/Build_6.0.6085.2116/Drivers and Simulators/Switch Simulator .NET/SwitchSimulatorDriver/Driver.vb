Imports ASCOM.Utilities 'replaces HelperNet
Imports ASCOM.Interface
Imports System.Reflection 'Needed for Assembly informations
'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Switch driver for SwitchSimulator
'
' Description:	.
'
' Implements:	ASCOM Switch interface version: 1.0
' Author:		Pierre de Ponthiere <pierredeponthiere@gmail.com>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Switch template
' --------------------------------------------------------------------------------
'
' Your driver's ID is ASCOM.SwitchSimulator.Switch
'
' The Guid attribute sets the CLSID for ASCOM.SwitchSimulator.Switch
' The ClassInterface/None addribute prevents an empty interface called
' _Switch from being created and used as the [default] interface
'
<Guid("d6707f49-0be2-49fa-899b-1975d91ecff4")> _
<ClassInterface(ClassInterfaceType.None)> _
Public Class Switch
    '	==========
    Implements ISwitch ' Early-bind interface implemented by this driver
    '	==========

#Region "New and Dispose"
    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        Dim i As Integer
        Dim oldRegVer As Double
        Using Prof = New Profile()
            ' get the registry format version so we can only update whats changed
            Try
                Prof.DeviceType = "Switch"
                oldRegVer = CDbl(Prof.GetValue(s_csDriverID, "RegVer"))
            Catch ex As InvalidCastException 'Value has not been set yet
                oldRegVer = 0.0#
            Catch ex As Exception 'Something bad happened
                MsgBox("SwitchSimulator: New exception - " & ex.ToString)
            End Try
            ' Persistent settings - Create on first start, or update
            If oldRegVer < 1.1 Then 'First start
                Prof.WriteValue(s_csDriverID, "MaxSwitch", CStr(8))
                Prof.WriteValue(s_csDriverID, "Zero", "False")
                ' enable all switches
            For i = 0 To (NUM_SWITCHES - 1)
                Prof.WriteValue(s_csDriverID, CStr(i), "True", "CanGetSwitch")
                Prof.WriteValue(s_csDriverID, CStr(i), "True", "CanSetSwitch")
                Prof.WriteValue(s_csDriverID, CStr(i), "True", "SwitchState")
            Next i

                Prof.WriteValue(s_csDriverID, "Left", "100")
                Prof.WriteValue(s_csDriverID, "Top", "100")
            End If
            If oldRegVer < 1.2 Then
                ' initialize names as "1","2",...
            For i = 0 To (NUM_SWITCHES - 1)
                Prof.WriteValue(s_csDriverID, CStr(i), CStr(i), "SwitchName")
            Next i
            End If
            Prof.WriteValue(s_csDriverID, "RegVer", RegVer)
            g_iMaxSwitch = CShort(Prof.GetValue(s_csDriverID, "MaxSwitch"))
            g_bZero = CBool(Prof.GetValue(s_csDriverID, "Zero"))

        For i = 0 To (NUM_SWITCHES - 1)
            g_bCanGetSwitch(i) = CBool(Prof.GetValue(s_csDriverID, CStr(i), "CanGetSwitch"))
            g_bCanSetSwitch(i) = CBool(Prof.GetValue(s_csDriverID, CStr(i), "CanSetSwitch"))
            g_bSwitchState(i) = CBool(Prof.GetValue(s_csDriverID, CStr(i), "SwitchState"))
            g_sSwitchName(i) = Prof.GetValue(s_csDriverID, CStr(i), "SwitchName")
        Next i
        End Using
    End Sub
    Public Sub Dispose()
        
    End Sub
#End Region

#Region "ASCOM Registration"
    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)
        Dim P As ASCOM.Utilities.Interfaces.IProfile
        P = New ASCOM.Utilities.Profile
        P.DeviceType = "Switch"
        'Dim P As New Helper.Profile()
        'P.DeviceTypeV = "Switch"           '  Requires Helper 5.0.3 or later
        If bRegister Then
            P.Register(s_csDriverID, s_csDriverDescription)
        Else
            P.Unregister(s_csDriverID)
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
    ' PUBLIC COM INTERFACE ISwitch IMPLEMENTATION
    '

#Region "ISwitch Members"
    Public Property Connected() As Boolean Implements ISwitch.Connected
        Get
            Return g_bConnected
        End Get
        Set(ByVal value As Boolean)
            If value Then
                If Not g_bConnected And bUseHandbox Then
                    frmHandbox = New HandBoxForm()
                    frmHandbox.Show()
                End If
            Else
                frmHandbox = Nothing
            End If
            g_bConnected = value
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ISwitch.Description
        Get
            Return s_csDriverDescription
        End Get
    End Property

    Public ReadOnly Property DriverInfo() As String Implements ISwitch.DriverInfo
        Get
            Return "ASCOM Safety Monitor simulator driver by Pierre de Ponthière" & vbCrLf & "Version " & Assembly.GetExecutingAssembly.GetName.Version.ToString
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ISwitch.DriverVersion
        Get
            Dim Ver As Version = Assembly.GetExecutingAssembly.GetName.Version 'Get this assembly's version
            Return Ver.Major & "." & Ver.Minor 'Display the relevant portions of the version
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ISwitch.InterfaceVersion
        Get
            Return 1
        End Get
    End Property

    Public ReadOnly Property MaxSwitch() As Short Implements ISwitch.MaxSwitch
        Get
            Return g_iMaxSwitch
        End Get
    End Property

    Public ReadOnly Property Name() As String Implements ISwitch.Name
        Get
            Return "Switch Simulator"
        End Get
    End Property

    Public Function GetSwitch(ByVal SID As Short) As Boolean Implements ISwitch.GetSwitch
        Dim s As String
        Dim ex As Exception
        s = SIDRange()
        check_connected()
        If SID < CShort(IIf(g_bZero, 0, 1)) Or SID > g_iMaxSwitch Then


            Throw New ASCOM.DriverException("Switch is out of range", &H80040406)
            'ex = Nothing
            'Throw New ASCOM.Utilities.Exceptions.InvalidValueException(" GetSwitch Switch" & CStr(SID) & " is out of range. Range is " & SIDRange(), ex)
            'Throw New ASCOM.InvalidValueException("GetSwitch Switch", CStr(SID), SIDRange())
        End If
        If Not g_bCanGetSwitch(SID) Then
            Throw New ASCOM.DriverException("Switch not enabled", &H80040405)
        End If
        Return g_bSwitchState(SID)
    End Function

    Public Function GetSwitchName(ByVal SID As Short) As String Implements ISwitch.GetSwitchName
        check_connected()
        If SID < CShort(IIf(g_bZero, 0, 1)) Or SID > g_iMaxSwitch Then
            Throw New ASCOM.DriverException("Switch is out of range", &H80040406)
        End If
        If Len(g_sSwitchName(SID)) <= 0 Then
            Return CStr(SID)
        Else
            Return g_sSwitchName(SID)
        End If
    End Function

    Public Sub SetSwitch(ByVal SID As Short, ByVal State As Boolean) Implements ISwitch.SetSwitch
        Dim Prof As Interfaces.IProfile = New ASCOM.Utilities.Profile
        check_connected()
        If SID < CShort(IIf(g_bZero, 0, 1)) Or SID > g_iMaxSwitch Then
            Throw New ASCOM.DriverException("Switch is out of range", &H80040406)
        End If
        If Not g_bCanSetSwitch(SID) Then
            Throw New ASCOM.DriverException("Switch not enabled", &H80040405)
        End If
        g_bSwitchState(SID) = State
        If bUseHandbox Then frmHandbox.UpdateHandboxWindow()
        Prof.DeviceType = "Switch"
        Prof.WriteValue(s_csDriverID, CStr(SID), CStr(g_bSwitchState(SID)), "SwitchState")
    End Sub

    Public Sub SetSwitchName(ByVal SID As Short, ByVal Name As String) Implements ISwitch.SetSwitchName
        Dim Prof As Interfaces.IProfile = New ASCOM.Utilities.Profile
        If SID < CShort(IIf(g_bZero, 0, 1)) Or SID > g_iMaxSwitch Or Len(Name) > 20 Then
            'Throw New ASCOM.InvalidValueException("SetSwitchName", CStr(SID), SIDRange())
            Throw New ASCOM.DriverException("Switch not enabled", &H80040403)
        End If
        g_sSwitchName(SID) = Name
        If bUseHandbox Then frmHandbox.UpdateHandboxWindow()
        Prof.DeviceType = "Switch"
        Prof.WriteValue(s_csDriverID, CStr(SID), g_sSwitchName(SID), "SwitchName")
    End Sub

    Public Sub SetupDialog() Implements ISwitch.SetupDialog
        Dim F As SetupDialogForm = New SetupDialogForm()
        F.ShowDialog()
    End Sub

#End Region
    Private Function SIDRange() As String
        Dim sRange As String
        If g_bZero Then
            sRange = "0 to " & CStr(g_iMaxSwitch)
        Else
            sRange = "1 to " & CStr(g_iMaxSwitch)
        End If
        Return sRange
    End Function
    Private Sub check_connected()
        If Not g_bConnected Then
            Throw New ASCOM.DriverException("Switch Simulator is not connected", &H80040401)
        End If
    End Sub
End Class
