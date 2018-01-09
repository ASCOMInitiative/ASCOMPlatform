Public Class ChooserExamples
    Sub Example()
        Dim Ch As ASCOM.Utilities.Chooser
        Dim SelectedDevice As String

        'Create a new chooser and set the required device type
        Ch = New ASCOM.Utilities.Chooser ' Create a new chooser component and set its device type
        Ch.DeviceType = "Telescope"

        'Select the required device - use one of:
        SelectedDevice = Ch.Choose 'Show the chooser dialogue with no device or with your device pre-selected
        SelectedDevice = Ch.Choose("Simulator.Telescope")

        MsgBox("The selected device is: " & SelectedDevice)

        'Clean up chooser object
        Ch.Dispose()
        Ch = Nothing
    End Sub
End Class
