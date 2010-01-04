Imports System.Runtime.InteropServices
Public Class ConnectForm

    Private Const DEFAULT_DEVICE_TYPE As String = "Telescope"
    Private Const DEFAULT_DEVICE As String = "ScopeSim.Telescope"

    Private CurrentDevice, CurrentDeviceType As String, Connected As Boolean, Device As Object

    Private Sub ConnectForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim DeviceTypes() As String, Profile As New ASCOM.Utilities.Profile


        DeviceTypes = Profile.RegisteredDeviceTypes
        For Each DeviceType As String In DeviceTypes
            cmbDeviceType.Items.Add(DeviceType)
        Next
        CurrentDevice = DEFAULT_DEVICE
        CurrentDeviceType = DEFAULT_DEVICE_TYPE
        cmbDeviceType.SelectedItem = CurrentDeviceType
        btnProperties.Enabled = False
        txtDevice.Text = CurrentDevice


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
End Class