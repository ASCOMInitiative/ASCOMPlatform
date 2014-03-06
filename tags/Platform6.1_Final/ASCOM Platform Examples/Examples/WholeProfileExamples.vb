Public Class WholeProfileExamples
    Sub Example()
        'Retrieve the whole profile as an ASCOMProfile class
        Dim ProfileContents As ASCOM.Utilities.ASCOMProfile
        Dim Prof As ASCOM.Utilities.Profile
        Dim Msg As String = "", Value As String

        Const MY_SCOPE As String = "ScopeSim.Telescope"

        'Create a profile object and set its device type
        Prof = New ASCOM.Utilities.Profile
        Prof.DeviceType = "Telescope"

        'Retrieve the whole profile in one go
        ProfileContents = Prof.GetProfile(MY_SCOPE)

        'Extract every value in every subkey
        For Each SubKey As String In ProfileContents.ProfileValues.Keys
            Msg = Msg + "Subkey: " & SubKey & vbCrLf
            For Each ValueName As String In ProfileContents.ProfileValues.Item(SubKey).Keys
                Value = ProfileContents.ProfileValues.Item(SubKey).Item(ValueName) 'Can also be written as: Value = ProfileContents.ProfileValues(SubKey)(ValueName)
                Msg = Msg + "  Value: """ & ValueName & """ = """ & Value & """" & vbCrLf
            Next
        Next
        MsgBox("The ScopeSim profile is: " & vbCrLf & Msg)

        'Extract a single value e.g.the default value of the profile root
        Msg = ProfileContents.GetValue("", "")
        MsgBox("The Scope Simulator's default value is: " & Msg)

        'Extract a single value e.g.the CanPulseGuide value of the Capabilities subkey
        Msg = ProfileContents.GetValue("CanPulseGuide", "Capabilities")
        MsgBox("The Scope Simulator'CanPulseGuide value is: " & Msg)

        'Set two single values in the class
        ProfileContents.SetValue("", "Telescope Simulator", "")
        ProfileContents.SetValue("CanPulseGuide", "False", "Capabilities")

        'Write the modified values back to the Profile store using Profile.SetProfile
        Prof.SetProfile(MY_SCOPE, ProfileContents)
    End Sub

    Sub ExampleXML()
        'Retrieve the whole profile as an XML string
        Dim ProfileContentsXML As String
        Dim Prof As ASCOM.Utilities.Profile
        Dim Msg As String = ""
        Const MY_SCOPE As String = "ScopeSim.Telescope"

        Prof = New ASCOM.Utilities.Profile 'Create a profile object and set its device type
        Prof.DeviceType = "Telescope"
        ProfileContentsXML = Prof.GetProfileXML(MY_SCOPE) 'Retrieve the whole profile in one go
        MsgBox("The ScopeSim profile in XML is: " & vbCrLf & ProfileContentsXML)

        'You can parse and manipulate the XML in any way you wish at this point and then write an updated version back with:
        Prof.SetProfileXML(MY_SCOPE, ProfileContentsXML)
    End Sub
End Class
