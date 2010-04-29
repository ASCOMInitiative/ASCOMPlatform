Imports System.Runtime.InteropServices
Public Class ConnectForm

    Private Const DEFAULT_DEVICE_TYPE As String = "Telescope"
    Private Const DEFAULT_DEVICE As String = "ScopeSim.Telescope"

    Private CurrentDevice, CurrentDeviceType As String, Connected As Boolean, Device As Object, Util As ASCOM.Utilities.Util

    Private Sub ConnectForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DeviceTypes() As String, Profile As New ASCOM.Utilities.Profile

        AddHandler cmbDeviceType.SelectedIndexChanged, AddressOf DevicetypeChangedhandler
        Try
            Util = New ASCOM.Utilities.Util
            DeviceTypes = Profile.RegisteredDeviceTypes
            For Each DeviceType As String In DeviceTypes
                cmbDeviceType.Items.Add(DeviceType)
            Next
            CurrentDevice = DEFAULT_DEVICE
            CurrentDeviceType = DEFAULT_DEVICE_TYPE
            cmbDeviceType.SelectedItem = CurrentDeviceType
            btnProperties.Enabled = False
            txtDevice.Text = CurrentDevice
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub DevicetypeChangedhandler()
        CurrentDeviceType = cmbDeviceType.SelectedItem.ToString
        If CurrentDeviceType = "Telescope" Then 'Enable or disable the run script button as appropriate
            btnScript.Enabled = True
        Else
            btnScript.Enabled = False
        End If
    End Sub

    Private Sub btnChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChoose.Click
        Dim Chooser As ASCOM.Utilities.Chooser, NewDevice As String

        CurrentDeviceType = cmbDeviceType.SelectedItem
        Chooser = New ASCOM.Utilities.Chooser
        Chooser.DeviceType = CurrentDeviceType
        NewDevice = Chooser.Choose(CurrentDevice)
        If NewDevice <> "" Then CurrentDevice = NewDevice

        If CurrentDevice <> "" Then
            btnProperties.Enabled = True
        Else
            btnProperties.Enabled = False
        End If

        txtDevice.Text = CurrentDevice

        Chooser.Dispose()

    End Sub

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click

        If Connected Then ' Disconnect
            Try
                txtStatus.Text = "Disconnecting..."
                Application.DoEvents()
                Select Case CurrentDeviceType
                    Case "Focuser"
                        Device.Link = False
                    Case Else
                        Device.Connected = False
                End Select
                Connected = False
                txtStatus.Text = "Disconnected OK"
                Try : Marshal.ReleaseComObject(Device) : Catch : End Try
                Device = Nothing
                btnConnect.Text = "Connect"
                btnChoose.Enabled = True
                If CurrentDevice <> "" Then btnProperties.Enabled = True
                DevicetypeChangedhandler() 'Enable or disable script button according todevice type
            Catch ex As Exception
                txtStatus.Text = "Connect Failed..." & ex.Message & vbCrLf & vbCrLf & ex.ToString
            End Try

        Else 'Disconnected so connect
            Try
                txtStatus.Text = "Connecting..."
                Application.DoEvents()
                Device = CreateObject(CurrentDevice)
                Select Case CurrentDeviceType
                    Case "Focuser"
                        Device.Link = True
                    Case Else
                        Device.Connected = True
                End Select
                Connected = True
                txtStatus.Text = "Connected OK"
                btnConnect.Text = "Disconnect"
                btnChoose.Enabled = False
                btnProperties.Enabled = False
                btnScript.Enabled = False
            Catch ex As Exception
                txtStatus.Text = "Connect Failed..." & ex.Message & vbCrLf & vbCrLf & ex.ToString
            End Try

        End If

    End Sub

    Private Sub btnProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProperties.Click
        Device = CreateObject(CurrentDevice)
        Device.SetupDialog()
        Try : Marshal.ReleaseComObject(Device) : Catch : End Try
        Device = Nothing
    End Sub

    Private TL As TraceLogger, DeviceObject As Object

    Private Sub btnScript_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScript.Click
        txtStatus.Clear()
        TL = New TraceLogger("", "DiagnosticScript")
        TL.Enabled = True
        LogMsg("Script", "Diagnostic Script Started")
        ExecuteCommand("CreateObject")
        ExecuteCommand("Connect")
        ExecuteCommand("Description")
        ExecuteCommand("DriverInfo")
        ExecuteCommand("DriverVersion")
        For i As Integer = 1 To 3
            ExecuteCommand("RightAscension")
            ExecuteCommand("Declination")
        Next
        ExecuteCommand("Disconnect")
        ExecuteCommand("DestroyObject")
        LogMsg("Script", "Diagnostic Script Completed")
        TL.Enabled = False
        TL.Dispose()
    End Sub

    Sub ExecuteCommand(ByVal Command As String)
        Dim sw As New Stopwatch, StartTime As Date, Result As String = ""
        Try
            StartTime = Now
            sw.Start()
            LogMsg(Command, "Started")

            Select Case Command
                Case "CreateObject"
                    DeviceObject = CreateObject(CurrentDevice)
                Case "Connect"
                    DeviceObject.Connected = True
                Case "DriverInfo"
                    Result = DeviceObject.DriverInfo
                Case "Description"
                    Result = DeviceObject.Description
                Case "DriverVersion"
                    Result = DeviceObject.DriverVersion
                Case "RightAscension"
                    Result = Util.DegreesToHMS(DeviceObject.RightAscension)
                Case "Declination"
                    Result = Util.DegreesToDMS(DeviceObject.Declination, ":", ":", "")
                Case "Disconnect"
                    DeviceObject.Connected = False
                Case "DestroyObject"
                    Marshal.ReleaseComObject(DeviceObject)
                    DeviceObject = Nothing
                Case Else
                    LogMsg(Command, "***** Unknown command *****")
            End Select
            sw.Stop()

            LogMsg(Command, "Finished - Started: " & Format(StartTime, "HH:mm:ss.fff") & " duration: " & sw.ElapsedMilliseconds & " ms - " & Result)
        Catch ex As Exception
            LogMsg(Command, "Exception - Started: " & Format(StartTime, "HH:mm:ss.fff") & " duration: " & sw.ElapsedMilliseconds & " ms - Exception: " & ex.ToString)
        End Try
    End Sub
    Sub LogMsg(ByVal Command As String, ByVal Msg As String)
        TL.LogMessage(Command, Msg)
        txtStatus.Text = txtStatus.Text & Command & " " & Msg & vbCrLf
    End Sub

    Private Sub btnGetProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetProfile.Click
        Dim Prof As New Profile, Result As String = ""
        txtStatus.Clear()
        TL = New TraceLogger("", "DiagnosticScript")
        TL.Enabled = True

        Result = Prof.GetProfileXML(txtDevice.Text)
        LogMsg("GetProfile", Result)
        LogMsg("Script", "Diagnostic Script Completed")
        TL.Enabled = False
        TL.Dispose()

    End Sub
End Class