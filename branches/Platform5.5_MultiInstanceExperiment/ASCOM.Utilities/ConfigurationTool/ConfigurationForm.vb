Public Class ConfigurationForm
    Private DEFAULT_DEVICE_TYPE As String = "Telescope"
    Private Profile As ASCOM.Utilities.Profile
    Private Sub ConfigurationForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Profile = New ASCOM.Utilities.Profile
        For Each DeviceType As String In Profile.RegisteredDeviceTypes
            cmbDeviceType.Items.Add(DeviceType)
            cmbDeviceType.SelectedItem = DEFAULT_DEVICE_TYPE
        Next
        cmbDeviceType_SelectedIndexChanged(New System.Object, New System.EventArgs)
        lstAliases.Items.Add("Item 1", False)
        lstAliases.Items.Add("Item 2", True)
        lstAliases.Items.Add("Item 3", False)
    End Sub

    Private Sub cmbDeviceType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDeviceType.SelectedIndexChanged
        cmbDevice.Items.Clear()
        For Each Device As ASCOM.Utilities.KeyValuePair In Profile.RegisteredDevices(cmbDeviceType.SelectedItem)
            cmbDevice.Items.Add(Device.Value & " (" & Device.Key & ")")
            cmbDevice.SelectedIndex = 0
        Next

    End Sub
End Class
