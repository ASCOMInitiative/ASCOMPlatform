Public Class ProfileExamples
    Sub Example()
        'Define variables for ASCOM Profile, Subkeys and values
        Dim Prof As ASCOM.Utilities.Profile
        Dim SubKeys, Values As ArrayList

        Const MOUNT_PROGID As String = "MyTestMount.Telescope"
        Const TELESCOPE_1 As String = "Telescope 1"
        Const TELESCOPE_2 As String = "Telescope 2"

        'Set up the profile object
        Prof = New ASCOM.Utilities.Profile 'Create a profile object and set its device type
        Prof.DeviceType = "Telescope"

        'Check whether we are registered, if not then register out test mount
        If Not Prof.IsRegistered(MOUNT_PROGID) Then
            Prof.Register(MOUNT_PROGID, "MyTestMount really is a great mount!")
        End If

        'Set and read some values
        Prof.WriteValue(MOUNT_PROGID, "Elevation", CStr(100)) ' Write a value in the root
        Prof.CreateSubKey(MOUNT_PROGID, TELESCOPE_1) 'Create a Telescope1 sub-key
        Prof.WriteValue(MOUNT_PROGID, "Focal Length", TELESCOPE_1, CStr(1445)) ' Write a value in Telescope1 key
        Prof.WriteValue(MOUNT_PROGID, "Aperture", TELESCOPE_1, CStr(280)) ' Write a value in Telescope1 key
        Prof.CreateSubKey(MOUNT_PROGID, TELESCOPE_2)
        Prof.WriteValue(MOUNT_PROGID, "Focal Length", TELESCOPE_2, CStr(600)) ' Write a value in the Telescope2 key
        Prof.WriteValue(MOUNT_PROGID, "Aperture", TELESCOPE_2, CStr(80)) ' Write a value in Telescope1 key
        MsgBox("Elevation is: " & Prof.GetValue(MOUNT_PROGID, "Elevation")) 'Read values back
        MsgBox("Telescope2 focal length is: " & Prof.GetValue(MOUNT_PROGID, "Focal Length", "Telescope2"))

        'Get a list of subkeys
        SubKeys = Prof.SubKeys(MOUNT_PROGID, TELESCOPE_1)
        For Each kvp As ASCOM.Utilities.KeyValuePair In SubKeys
            MsgBox("SubKey: " & kvp.Key & " default value: " & kvp.Value)
        Next

        'Get a list of values held in the "Telescope 1" subkey
        Values = Prof.Values(MOUNT_PROGID, TELESCOPE_1)
        For Each kvp As ASCOM.Utilities.KeyValuePair In Values
            MsgBox("Name: " & kvp.Key & " value: " & kvp.Value)
        Next

        'Enumerate the installed Device Types and devices installed
        For Each DeviceType As String In Prof.RegisteredDeviceTypes 'Get each device type in turn
            MsgBox("Listing " & DeviceType & " devices")
            For Each Device As ASCOM.Utilities.KeyValuePair In Prof.RegisteredDevices(DeviceType) 'List devices
                MsgBox("Found ProgID: " & Device.Key & ", Device: " & Device.Value)
            Next
        Next

        'Clean up and dispose of the profile object
        Prof.Unregister(MOUNT_PROGID) ' Unregister our test mount
        Prof.Dispose()
        Prof = Nothing
    End Sub
End Class
